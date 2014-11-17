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

        IPatientService _patientService;

        public HomeController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(_patientService.GetAllUsers());
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public void DatabaseMe(FormCollection form)
        {
            _patientService.AddPatient("New Guy", "newguy@gmail.com");
        }
    }
}
