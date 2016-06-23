using System.Web.Mvc;
using System;
using System.Text;
using Logger;

namespace Marketplace.Admin.Filters
{
    public class CustomExceptionFilterAttribute : HandleErrorAttribute, IExceptionFilter
    {
        private const string ExceptionSeperator = "*************************************************************************************************";

        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.Exception != null)
            {
                var controller = filterContext.RouteData.Values["controller"].ToString();
                var action = filterContext.RouteData.Values["action"].ToString();
                var execptionIdentifier = Guid.NewGuid();

                LogManager.Instance.Log(ComposeExceptionLog(controller, action, filterContext.Exception, execptionIdentifier),
                                        filterContext.Exception, LogLevelEnum.Error);
            }
        }

        public static string ComposeExceptionLog(string controller, string action, Exception exception, Guid exceptionIdentifier)
        {
            var sbErrorLog = new StringBuilder();

            sbErrorLog.AppendLine(ExceptionSeperator);
            sbErrorLog.AppendLine(DateTime.Now + " : " + exceptionIdentifier);
            sbErrorLog.Append("Error occurred for request - ").Append(controller).Append("/").AppendLine(action);
            sbErrorLog.AppendLine("Exception - " + exception.Message);
            sbErrorLog.AppendLine(ExceptionSeperator);
            return sbErrorLog.ToString();
        }
    }
}