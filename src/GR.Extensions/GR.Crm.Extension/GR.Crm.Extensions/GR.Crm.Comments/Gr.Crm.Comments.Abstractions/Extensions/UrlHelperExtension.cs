using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gr.Crm.Comments.Abstractions.Extensions
{
    public static class UrlHelperExtension
    {
        public static string CommentNotificationCallBack(this IUrlHelper url,
                string ActionName,
                string ControllerName,
                string id)
        {
            string scheme = url.ActionContext.HttpContext.Request.Scheme;
            return url.Action(ActionName, ControllerName, new { id }, scheme);
        }
    }
}
