using System.Diagnostics;
using System.Web.Mvc;
using System.Linq;
using Marketplace.Admin.Utils;
using System;
using Logger;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Marketplace.Admin.Filters
{
    public class LogActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecuting method overridden to register action hit.
        /// </summary>
        /// <param name="actionContext" cref="ActionExecutingContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes(typeof(SecureDataActionFilter),false).Any())
            {
                return;
            }

            //Notes Request timestamp in Request Items and actionparameters
            actionContext.HttpContext.Items[actionContext.ActionDescriptor.ActionName] = Stopwatch.StartNew();
            actionContext.HttpContext.Items["RequestTimeStamp"] = actionContext.HttpContext.Timestamp.ToUniversalTime();
            actionContext.HttpContext.Items["ActionParameters"] = actionContext.ActionParameters;
        }

        /// <summary>
        /// OnActionExecuted method overridden to register action response.
        /// </summary>
        /// <param name="actionExecutedContext" cref="ActionExecutedContext"></param>
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.ActionDescriptor.GetCustomAttributes(typeof(SecureDataActionFilter), false).Any())
            {
                return;
            }

            //Notes Request timestamp in Request Items and actionparameters
            Stopwatch stopWatch = (Stopwatch)actionExecutedContext.HttpContext.Items[actionExecutedContext.ActionDescriptor.ActionName];
            var executionTime = stopWatch.ElapsedMilliseconds;
            Log(actionExecutedContext, executionTime);
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
                    Arguments = (Dictionary<string, object>)actionExecutedContext.HttpContext.Items["ActionParameters"],
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
            sbInfoString.AppendLine("Arguments :" + JsonConvert.SerializeObject(logger.RequestDetails.Arguments));
            sbInfoString.AppendLine("Request TimeStamp : " + logger.RequestTimeStamp);
            sbInfoString.AppendLine("Response TimeStamp : " + logger.ResponseTimeStamp);
            sbInfoString.AppendLine("Execution Time : " + logger.ExecutionTime + " MilliSeconds.");
            sbInfoString.AppendLine(Seperator);
            return sbInfoString.ToString();
        }
    }
}