using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PillBox.DAL.Entities;
using PillBox.Model.Entities;
using PillBox.Services;
using PillBox.Website.Models;
using PillBox.Website.ScheduledTasks;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PillBox.Website.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        PillBoxDbContext db;

        public AdminController()
        {
            db = new PillBoxDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult DataFeed(string sortOrder)
        //{
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

        //    var 

        //    switch (sortOrder)
        //    {

        //    }
        //}

        public ActionResult Dashboard()
        {
            AdminHomeViewModel model = new AdminHomeViewModel();
            model.Users = UserManager.Users.ToList();

            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            PillBoxUser user = await UserManager.FindByIdAsync(id);

            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Dashboard");
            }
        }

        //TODO Finish
        //[HttpPost]
        //public async Task<ActionResult> Edit(EditPatientModel model)
        //{
        //    PillBoxUser user = await UserManager.FindByIdAsync(model.id);

        //    if (user != null)
        //    {
        //        if(!string.IsNullOrEmpty(model.CurrentPassword) && 
        //            !string.IsNullOrEmpty(model.NewPassword))
        //        {
        //            IdentityResult validPass = await UserManager.ChangePasswordAsync(user.Id, model.CurrentPassword, model.NewPassword);
        //            if (validPass.Succeeded)
        //            {
        //                user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.NewPassword);
        //            }
        //            else
        //            {
        //                AddErrorsFromResult(validPass);
        //            }
        //        }

        //        if
        //    }
        //}

        [HttpPost]
        public async Task<ActionResult> AddMed(AdminHomeViewModel model)
        {
            PillBoxUser user = await UserManager.FindByIdAsync(model.CreateMedicineModel.UserId);

            if (user != null)
            {
                Medicine med = null;

                if (!string.IsNullOrEmpty(model.CreateMedicineModel.MedicineName))
                {
                    med = new Medicine() { Name = model.CreateMedicineModel.MedicineName };
                }

                if (med != null && model.CreateMedicineModel.RemindTime != null)
                {
                    string temp = null;
                    try
                    {
                        temp = model.CreateMedicineModel.RemindTime.Value.ToString();
                    }
                    catch
                    {

                    }
                    med.RemindTime = string.IsNullOrEmpty(temp) ? (DateTime?)null : DateTime.Parse(temp);
                    user.Medicines.Add(med);

                }

                IdentityResult result = await UserManager.UpdateAsync(user);

                if (result.Succeeded)
                    JobScheduler.ScheduleMedicineReminder(med);

                return RedirectToAction("Dashboard");
            }

            return View("Error", new string[] { "User Not Found" });
        }

        [HttpPost]
        public async Task<ActionResult> Create(AdminHomeViewModel model)
        {
            IdentityResult result = null;

            if (ModelState.IsValid)
            {
                PillBoxUser user = new PillBoxUser()
                {
                    UserName = model.CreatePatientModel.PhoneNumber,
                    FirstName = model.CreatePatientModel.FirstName,
                    LastName = model.CreatePatientModel.LastName,
                    Gender = model.CreatePatientModel.Gender,
                    AgeGroup = model.CreatePatientModel.AgeGroup,
                    PhoneNumber = model.CreatePatientModel.PhoneNumber,
                };

                result = await UserManager.CreateAsync(user, user.PhoneNumber);

                if (result.Succeeded) // Successful user creation
                {
                    Medicine med = null;

                    if (!string.IsNullOrEmpty(model.CreatePatientModel.Medicine))
                    {
                        med = new Medicine() { Name = model.CreatePatientModel.Medicine };
                    }

                    if (med != null && model.CreatePatientModel.RemindTime != null)
                    {
                        string temp = null;
                        try
                        {
                            temp = model.CreatePatientModel.RemindTime.Value.ToString();
                            //temp = model.CreatePatientModel.RemindTime.Value.ToUniversalTime().ToString(); 
                            med.RemindTime = string.IsNullOrEmpty(temp) ? (DateTime?)DateTime.Now : DateTime.Parse(temp);
                            user.Medicines.Add(med);

                            IdentityResult medResult = await UserManager.UpdateAsync(user);

                            if (medResult.Succeeded)
                                JobScheduler.ScheduleMedicineReminder(med);
                        }
                        catch
                        {

                        }
                    }
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    AddErrorsFromResult(result);
                    return View("Error", result.Errors);
                }
            }

            var errorList = ModelState.Values.SelectMany(m => m.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList();

            return View("Error", errorList);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {

            PillBoxUser user = await UserManager.FindByIdAsync(id);
            log.Info("Begin deleting " + user.FullName + " UserId: " + id);

            if (user != null)
            {
                //TODO Figure out how to do this with Cascade Deletes
                var userReminders = db.Reminders.Where(m => m.UserId == id);
                log.Info("Deleting " + userReminders.Count() + " reminders.");
                foreach (var reminder in userReminders)
                {
                    log.Info("Delete ReminderId: " + reminder.Id);
                    db.Reminders.Remove(reminder);
                }

                await db.SaveChangesAsync();

                //TODO Figure out how to do this with Cascade Deletes & Identity
                // First Delete User Meds
                var userMedicines = db.Medicines.Where(m => m.UserId == id);
                log.Info("Deleting " + userMedicines.Count() + " medicines.");
                foreach (var med in userMedicines)
                {
                    log.Info("Delete MedicineId: " + med.Id);
                    JobScheduler.RemoveJob(med.Id);
                    db.Medicines.Remove(med);
                }

                await db.SaveChangesAsync();

                string userName = user.FullName;

                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    log.Info("End user delete successful for " + userName);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    log.Info("End user delete un-successful for " + userName);
                    return View("Error", result.Errors);
                }
            }
            else
            {
                log.Info("User with id: " + id + " not found to delete");
                return View("Error", new string[] { "User Not Found" });
            }
        }


        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private PillBoxUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<PillBoxUserManager>();
            }
        }
    }
}