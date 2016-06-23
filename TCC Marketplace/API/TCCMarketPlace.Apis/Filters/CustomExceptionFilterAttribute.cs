using System.Net;
using System.Web.Http.Filters;
using System.Net.Http;
using System;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using TCCMarketPlace.Apis.Models;
using TCCMarketPlace.Model.ExceptionHandlers;
using TCCMarketPlace.Model.Logger;

namespace TCCMarketPlace.Apis.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute, IExceptionFilter
    {
        const string UnauthorizedErrorMessage = "You don’t have sufficient permission to perform this operation. Kindly contact your system administrator for more details.";
        const string UnhandledErrorMessage = "Something went wrong!!! Please contact your system administrator with below identifier - {0}";
        const string ExceptionSeperator = "*************************************************************************************************";

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //string exceptionMessage = actionExecutedContext.Exception.Message;
            ApiResponse<bool> response = null;

            if (actionExecutedContext.Exception is BusinessException)
            {
                response = HandleBusinessException(actionExecutedContext);
            }            
            else
            {
                response = HandleSystemException(actionExecutedContext);
            }
            
        }

        private ApiResponse<bool> HandleSystemException(HttpActionExecutedContext actionExecutedContext)
        {
            ApiResponse<bool> response;
            Guid execptionIdentifier = Guid.NewGuid();

            //Log exception
            new Log4NetLogger().Log(ComposeExceptionLog(actionExecutedContext, execptionIdentifier), actionExecutedContext.Exception, LogLevelEnum.Error);

            //response = new ApiResponse<bool> { ErrorMessage = string.Format(UnhandledErrorMessage, execptionIdentifier), HasError = true, IsBusinessValidation = false };
            response = new ApiResponse<bool> { ErrorMessage = ComposeExceptionLog(actionExecutedContext, execptionIdentifier), HasError = true, IsBusinessValidation = false };
            response.Status = Constants.FAIL;
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, response);
            return response;
        }

        private static ApiResponse<bool> HandleBusinessException(HttpActionExecutedContext actionExecutedContext)
        {
            var businessException = actionExecutedContext.Exception as BusinessException;
            var errorMessageBuilder = new StringBuilder();

            if (businessException.Errors.Count > 1)
            {
                foreach (var exception in businessException.Errors)
                {
                    errorMessageBuilder.Append(exception + "newline");
                }
            }
            else
            {
                errorMessageBuilder.Append(businessException.Errors.First());
            }

            ApiResponse<bool> response = new ApiResponse<bool> { ErrorMessage = errorMessageBuilder.ToString(), HasError = true, IsBusinessValidation = true };
            response.Status = Constants.FAIL;
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, response);
            return response;
        }
        
        private static string ComposeExceptionLog(HttpActionExecutedContext actionExecutedContext, Guid execptionIdentifier)
        {
            var arguments = actionExecutedContext.ActionContext.ActionArguments;
            var sbErrorLog = new StringBuilder();

            sbErrorLog.AppendLine(ExceptionSeperator);
            sbErrorLog.AppendLine(execptionIdentifier.ToString());
            sbErrorLog.AppendLine("Error occurred for request- " + actionExecutedContext.Request.RequestUri.ToString());
            sbErrorLog.AppendLine("Arguments - " + JsonConvert.SerializeObject(arguments));
            sbErrorLog.AppendLine("Exception- " + actionExecutedContext.Exception.Message);
            //sbErrorLog.AppendLine(actionExecutedContext.Exception.StackTrace);            
            sbErrorLog.AppendLine(ExceptionSeperator);

            return sbErrorLog.ToString();
        }
    }
}
