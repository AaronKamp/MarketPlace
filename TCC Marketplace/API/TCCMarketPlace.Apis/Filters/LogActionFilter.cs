using System;
using System.Web;
using System.Linq;
using System.Web.Http.Filters;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Web.Http.Controllers;
using System.Security.Claims;
using TCCMarketPlace.Model;
using TCCMarketPlace.Model.Logger;
using System.Text;

namespace TCCMarketPlace.Apis.Filters
{
    /// <summary>
    /// Handles information logging on every request.
    /// </summary>
    public class LogActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecuting method overridden to register action hit.
        /// </summary>
        /// <param name="actionContext" cref="HttpActionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //Notes Request Timestamp in Request Properties.
            actionContext.Request.Properties["RequestTimeStamp"] = DateTime.UtcNow;
            //Starts a stopwatch in request's Properties.
            actionContext.Request.Properties["StopWatch"] = Stopwatch.StartNew();
        }

        /// <summary>
        /// OnActionExecuted method overridden to register action response.
        /// </summary>
        /// <param name="actionExecutedContext" cref="HttpActionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //Notes Response timestamp in Request Properties
            actionExecutedContext.Request.Properties["ResponseTimeStamp"] = DateTime.UtcNow;
            Stopwatch stopWatch = (Stopwatch)actionExecutedContext.Request.Properties["StopWatch"];

            //get execution time retrieved from stopwatch
            var executionTime = stopWatch.ElapsedMilliseconds;

            Log(actionExecutedContext.ActionContext, executionTime);
        }

        /// <summary>
        /// Generate the Information logger entity on action execution completion and calls the logger.
        /// </summary>
        /// <param name="actionContext" cref="HttpActionContext"></param>
        /// <param name="executionTime"> Action Execution time.</param>
        private void Log( HttpActionContext actionContext, long executionTime)
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
                    Arguments = actionContext.ActionArguments,
                    URL = actionContext.Request.RequestUri.ToString()
                },
                ExecutionTime = executionTime,
                User = (ClaimsIdentity)HttpContext.Current.User.Identity
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
            sbInfoString.AppendLine("Arguments : " + JsonConvert.SerializeObject(logger.RequestDetails.Arguments));
            sbInfoString.AppendLine("User details : " + JsonConvert.SerializeObject(new
            {
                UserId = Convert.ToInt32(logger.User.Claims.Where(c => c.Type == ClaimTypes.UserData)
                                                                                .Select(c => c.Value).FirstOrDefault()),
                UserName = logger.User.Claims.Where(c => c.Type == ClaimTypes.Email)
                                                                               .Select(c => c.Value).FirstOrDefault(),
                ThermostatId = Convert.ToInt32(logger.User.Claims.Where(c => c.Type == "thermostatId")
                                                                          .Select(c => c.Value).FirstOrDefault())

            }));
            sbInfoString.AppendLine("Request Timestamp : " + logger.RequestTimeStamp);
            sbInfoString.AppendLine("Response Timestamp : " + logger.ResponseTimeStamp);
            sbInfoString.AppendLine("Execution Time : " + logger.ExecutionTime + " MilliSeconds.");
            sbInfoString.AppendLine(Seperator);
            return sbInfoString.ToString();
        }
    }
}