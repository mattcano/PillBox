using PillBox.DAL.Entities;
using PillBox.Model.Entities;
using PillBox.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PillBox.Website.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReminderController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private PillBoxDbContext db = new PillBoxDbContext();

        public ActionResult Index()
        {
            var reminders = db.Set<Reminder>().OrderByDescending(r => r.RemindTimeSent).ToList();
            List<DataFeedViewModel> dataFeedRows = 
                new List<DataFeedViewModel>(reminders.Select(r => new DataFeedViewModel(r)));

            return View(dataFeedRows);
        }
	}
}