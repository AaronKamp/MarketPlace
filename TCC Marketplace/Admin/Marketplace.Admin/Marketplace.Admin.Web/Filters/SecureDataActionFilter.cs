using System;
using System.Text;
using System.Linq;
using System.Web.Mvc;
using System.Diagnostics;
using System.Collections.Generic;
using Logger;
using Newtonsoft.Json;
using Marketplace.Admin.Entites;

namespace Marketplace.Admin.Filters
{
    public class SecureDataActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecuting method overridden to register login hit.
        /// </summary>
        /// <param name="loginContext" cref="ActionExecutingContext"></param>
        public override void OnActionExecuting(ActionExecutingContext loginContext)
        {
            //Notes Request timestamp in Request Items and actionparameters
            loginContext.HttpContext.Items[loginContext.ActionDescriptor.ActionName] = Stopwatch.StartNew();
            loginContext.HttpContext.Items["RequestTimeStamp"] = loginContext.HttpContext.Timestamp.ToUniversalTime();
        }

        /// <summary>
        /// OnActionExecuted method overridden to register action response.
        /// </summary>
        /// <param name="loginExecutedContext" cref="ActionExecutedContext"></param>
        public override void OnActionExecuted(ActionExecutedContext loginExecutedContext)
        {
            //Notes Request timestamp in Request Items and actionparameters
            Stopwatch stopWatch = (Stopwatch)loginExecutedContext.HttpContext.Items[loginExecutedContext.ActionDescriptor.ActionName];
            var executionTime = stopWatch.ElapsedMilliseconds;
            Log(loginExecutedContext, executionTime);
        }

        /// <summary>
        /// Generate the Information logger entity on action execution completion and calls the logger.
        /// </summary>
        /// <param name="actionContext" cref="ActionExecutedContext"></param>
        /// <param name="executionTime"> Action Execution time.</param>
        private void Log(ActionExecutedContext actionExecutedContext, long executionTime)
        {
            var logger = new LoggerEntity
            {
                Id = Guid.NewGuid(),
                Message = $"Request - {actionExecutedContext.HttpContext.Request.Url.Host}/{actionExecutedContext.ActionDescriptor.ControllerDescriptor.ControllerName}/{actionExecutedContext.ActionDescriptor.ActionName}",
                Severity = LogLevelEnum.Information,
                RequestTimeStamp = (DateTime)actionExecutedContext.HttpContext.Items["RequestTimeStamp"],
                ResponseTimeStamp = DateTime.UtcNow,
                RequestDetails = new Request
                {
                    Controller = actionExecutedContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    Action = actionExecutedContext.ActionDescriptor.ActionName,
                    URL = actionExecutedContext.HttpContext.Request.Url.ToString(),
                    UserName = actionExecutedContext.HttpContext.User.Identity.Name
                },
                ExecutionTime = executionTime
            };

            LogManager.Instance.Log(ComposeInformationLog(logger), LogLevelEnum.Information);
        }

        /// <summary>
        /// Composes detailed information log.
        /// </summary>
        /// <param name="logger" cref="LoggerEntity">Information logger entity</param>
        /// <returns>Request information log. </returns>
        private string ComposeInformationLog(LoggerEntity logger)
        {
            const string Seperator = "*************************************************************************************************";

            var sbInfoString = new StringBuilder();
            sbInfoString.AppendLine();
            sbInfoString.AppendLine(Seperator);
            sbInfoString.AppendLine(logger.Id.ToString());
            sbInfoString.AppendLine("Message : " + logger.Message);
            sbInfoString.AppendLine("Request details");
            sbInfoString.AppendLine("User Name : " + logger.RequestDetails.UserName);
            sbInfoString.AppendLine("Controller " + logger.RequestDetails.Controller);
            sbInfoString.AppendLine("Action : " + logger.RequestDetails.Action);
            sbInfoString.AppendLine("URL :" + logger.RequestDetails.URL);
            sbInfoString.AppendLine("Request TimeStamp : " + logger.RequestTimeStamp);
            sbInfoString.AppendLine("Response TimeStamp : " + logger.ResponseTimeStamp);
            sbInfoString.AppendLine("Execution Time : " + logger.ExecutionTime + " MilliSeconds.");
            sbInfoString.AppendLine(Seperator);
            return sbInfoString.ToString();
        }
    }
}