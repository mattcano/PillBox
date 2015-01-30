using PillBox.DAL;
using PillBox.DAL.Entities;
using PillBox.Services.DI;
using PillBox.Website.DI;
using PillBox.Website.ScheduledTasks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace PillBox.Website
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Start()
        {
            if (log.IsInfoEnabled) log.Info("Starting PillBox Website");
            NinjectBootstrapper.Initialize();
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(NinjectBootstrapper.Kernel));

            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //JobScheduler.EnqueueOnRemindTimes();
            JobScheduler.ScheduleCurrentMedicineReminders();
            JobScheduler.Start();
        }
    }
}