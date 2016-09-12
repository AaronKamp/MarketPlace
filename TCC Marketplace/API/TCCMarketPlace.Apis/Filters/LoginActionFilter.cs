using System;
using System.Diagnostics;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Text;
using TCCMarketPlace.Model;
using TCCMarketPlace.Model.Logger;

namespace TCCMarketPlace.Apis.Filters
{
    /// <summary>
    /// Handles information logging on login request.
    /// </summary>
    public class LoginActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecuting method overridden to register login hit.
        /// </summary>
        /// <param name="loginContext" cref="HttpActionContext"></param>
        public override void OnActionExecuting(HttpActionContext loginContext)
        {
            //Notes Request Timestamp in Request Properties.
            loginContext.Request.Properties["RequestTimeStamp"] = DateTime.UtcNow;

            //Starts a stopwatch in request's Properties.
            loginContext.Request.Properties["StopWatch"] = Stopwatch.StartNew();
        }

        /// <summary>
        /// OnActionExecuted method overridden to register login response.
        /// </summary>
        /// <param name="loginExecutedContext" cref="HttpActionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext loginExecutedContext)
        {
            //Notes Response timestamp in Request Properties
            loginExecutedContext.Request.Properties["ResponseTimeStamp"] = DateTime.UtcNow;
            Stopwatch stopWatch = (Stopwatch)loginExecutedContext.Request.Properties["StopWatch"];

            //get execution time retrieved from stopwatch
            var executionTime = stopWatch.ElapsedMilliseconds;

            Log(loginExecutedContext.ActionContext, executionTime);
        }

        /// <summary>
        /// Generate the Information logger entity on action execution completion and calls the logger.
        /// </summary>
        /// <param name="actionContext" cref="HttpActionContext"></param>
        /// <param name="executionTime"> Action Execution time.</param>
        private void Log(HttpActionContext actionContext, long executionTime)
        {
            //Information logger entity initialization.
            var logger = new LoggerEntity
            {
                Id = Guid.NewGuid(),
                Message = $"Request - {actionContext.Request.RequestUri.Host}/{actionContext.ControllerContext.ControllerDescriptor.ControllerName}/{actionContext.ActionDescriptor.ActionName}",
                Severity = LogLevelEnum.Information,
                RequestTimeStamp = (DateTime)actionContext.Request.Properties["RequestTimeStamp"],
                ResponseTimeStamp = (DateTime)actionContext.Request.Properties["ResponseTimeStamp"],
                RequestDetails = new Request
                {
                    Controller = actionContext.ControllerContext.ControllerDescriptor.ControllerName,
                    Action = actionContext.ActionDescriptor.ActionName,
                    URL = actionContext.Request.RequestUri.ToString()
                },
                ExecutionTime = executionTime,
            };

            // Write to application log file.
            new Log4NetLogger().Log(ComposeInformationLog(logger), LogLevelEnum.Information);
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
            sbInfoString.AppendLine("Message :" + logger.Message);
            sbInfoString.AppendLine("Request details");
            sbInfoString.AppendLine("Controller :" + logger.RequestDetails.Controller);
            sbInfoString.AppendLine("Action : " + logger.RequestDetails.Action);
            sbInfoString.AppendLine("URL :" + logger.RequestDetails.URL);
            sbInfoString.AppendLine("Request Timestamp : " + logger.RequestTimeStamp);
            sbInfoString.AppendLine("Response Timestamp : " + logger.ResponseTimeStamp);
            sbInfoString.AppendLine("Execution Time : " + logger.ExecutionTime + " MilliSeconds.");
            sbInfoString.AppendLine(Seperator);
            return sbInfoString.ToString();
        }
    }
}