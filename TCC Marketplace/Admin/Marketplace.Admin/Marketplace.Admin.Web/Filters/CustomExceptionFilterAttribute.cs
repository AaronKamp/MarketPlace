﻿using System.Web.Mvc;
using System;
using System.Text;
using Logger;

namespace Marketplace.Admin.Filters
{
    /// <summary>
    /// Custom exception Handler
    /// </summary>
    public class CustomExceptionFilterAttribute : HandleErrorAttribute, IExceptionFilter
    {
        private const string ExceptionSeperator = "*************************************************************************************************";

        /// <summary>
        /// On exception the control is transferred here.
        /// </summary>
        /// <param name="filterContext"></param>
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

        /// <summary>
        /// Compose detailed exception log.
        /// </summary>
        /// <param name="controller"> Controller where exception is thrown </param>
        /// <param name="action"> Action on which exception is thrown.</param>
        /// <param name="exception"> The Exception.</param>
        /// <param name="exceptionIdentifier"> Guid.</param>
        /// <returns></returns>
        public static string ComposeExceptionLog(string controller, string action, Exception exception, Guid exceptionIdentifier)
        {
            var sbErrorLog = new StringBuilder();

            sbErrorLog.AppendLine(ExceptionSeperator);
            sbErrorLog.AppendLine(DateTime.UtcNow + " : " + exceptionIdentifier);
            sbErrorLog.Append("Error occurred for request - ").Append(controller).Append("/").AppendLine(action);
            sbErrorLog.AppendLine("Exception - " + exception.Message);
            sbErrorLog.AppendLine(ExceptionSeperator);
            return sbErrorLog.ToString();
        }
    }
}