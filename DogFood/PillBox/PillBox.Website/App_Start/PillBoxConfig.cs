using Microsoft.AspNet.Identity;
using Owin;
using PillBox.DAL.Entities;
using PillBox.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PillBox
{
    public class PillBoxConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<PillBoxDbContext>(PillBoxDbContext.Create);
            app.CreatePerOwinContext<PillBoxUserManager>(PillBoxUserManager.Create);

            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions{
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new Microsoft.Owin.PathString("/Account/Login"),
            });
        }
    }
}