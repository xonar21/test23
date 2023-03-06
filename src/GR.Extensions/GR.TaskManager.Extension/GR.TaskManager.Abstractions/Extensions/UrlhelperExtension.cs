using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.TaskManager.Abstractions.Extensions
{
    public static class UrlhelperExtension
    {
        public static string TaskNotificationCallBack(this IUrlHelper url,
                string ActionName,
                string ControllerName,
                string id)
        {
            string scheme = url.ActionContext.HttpContext.Request.Scheme;
            return url.Action(ActionName, ControllerName, new { id }, scheme);
        }
    }
}
