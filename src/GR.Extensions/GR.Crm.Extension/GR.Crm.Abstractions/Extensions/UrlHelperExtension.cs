using Microsoft.AspNetCore.Mvc;

namespace GR.Crm.Abstractions.Extensions
{
    public static class UrlHelperExtension
    {
        public static string NotificationCallBack(this IUrlHelper url,
               string ActionName,
               string ControllerName,
               string id)
        {
            string scheme = url.ActionContext.HttpContext.Request.Scheme;
            return url.Action(ActionName, ControllerName, new { id }, scheme);
        }
    }
}
