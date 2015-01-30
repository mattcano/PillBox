using PillBox.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PillBox.Website.Controllers
{
    public class HomeController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HomeController()
        {

        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            string ipaddress;
            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipaddress == "" || ipaddress == null)
                ipaddress = Request.ServerVariables["REMOTE_ADDR"];

            log.Info("Visit from:  " + ipaddress);
            
            //return View(_patientService.GetAllUsers());
            return View();
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public void DatabaseMe(FormCollection form)
        {
            //_patientService.AddPatient("New Guy", "newguy@gmail.com");
        }
    }
}
