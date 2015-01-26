using PillBox.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace PillBox.Website.Helpers
{
    public static class IdentityHelpers
    {
        public static MvcHtmlString GetUserName(this HtmlHelper html, string id)
        {
            PillBoxUserManager mgr
                = HttpContext.Current.GetOwinContext().GetUserManager<PillBoxUserManager>();
            return new MvcHtmlString(mgr.FindByIdAsync(id).Result.FullName);
        }
    }
}