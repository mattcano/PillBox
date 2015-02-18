using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PillBox.DAL.Entities;
using PillBox.Model.Entities;
using PillBox.Services;
using PillBox.Website.Models;
using PillBox.Website.ScheduledTasks;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            PillBoxUser user = await UserManager.FindByIdAsync(id);

            if (user == null)
                return RedirectToAction("Dashboard");

            return View(user);
        }

        bool PropertyChanged(string curValue, string newValue)
        {
            return newValue != curValue;
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserName,FirstName,LastName,Email,PhoneNumber,Gender,AgeGroup")]PillBoxUser formUser)
        {
            if (ModelState.IsValid)
            {
                PillBoxUser user = await UserManager.FindByIdAsync(formUser.Id);
                if (user != null)
                {
                    if (PropertyChanged(user.FirstName, formUser.FirstName)) user.FirstName = formUser.FirstName;
                    if (PropertyChanged(user.LastName, formUser.LastName)) user.LastName = formUser.LastName;
                    if (PropertyChanged(user.Email, formUser.Email)) user.Email = formUser.Email;
                    if (PropertyChanged(user.PhoneNumber, formUser.PhoneNumber)) user.PhoneNumber = formUser.PhoneNumber;
                    if (PropertyChanged(user.Gender, formUser.Gender)) user.Gender = formUser.Gender;
                    if (PropertyChanged(user.AgeGroup, formUser.AgeGroup)) user.AgeGroup = formUser.AgeGroup;

                    IdentityResult validUser = await UserManager.UserValidator.ValidateAsync(user);

                    if (!validUser.Succeeded)
                        AddErrorsFromResult(validUser);

                    IdentityResult result = await UserManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                    return View("Error", result.Errors);
                }
            }

            var errorList = ModelState.Values.SelectMany(m => m.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList();

            return View("Error", errorList);
        }

        public async Task<ActionResult> DeleteMed(int id)
        {

            Medicine med = db.Medicines.Include("User").Single(m => m.Id == id);
            log.Info("Begin deleting " + med.Name + " for " + med.User.FirstName + " MedId: " + id);

            if (med != null)
            {
                //TODO Figure out how to do this with Cascade Deletes
                var userReminders = db.Reminders.Where(m => m.MedicineId == id);
                log.Info("Deleting " + userReminders.Count() + " reminders.");
                foreach (var reminder in userReminders)
                {
                    log.Info("Delete ReminderId: " + reminder.Id);
                    db.Reminders.Remove(reminder);
                }

                await db.SaveChangesAsync();

                //TODO Figure out how to do this with Cascade Deletes & Identity
                // First Delete User Meds
                log.Info("Deleting " + med.Name + " from DB");

                log.Info("Delete MedicineId: " + med.Id);
                JobScheduler.RemoveJob(med.Id);
                db.Medicines.Remove(med);

                await db.SaveChangesAsync();

                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                log.Info("Med with id: " + id + " not found to delete");
                return View("Error", new string[] { "Medicine Not Found" });
            }
        }

        public async Task<ActionResult> EditMed(int id)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Medicine med = await db.Medicines.Include("User").SingleAsync(m => m.Id == id);

            if (med == null)
                return View("Error", new string[] { "User Not Found" });

            //TODO finish edit view
            return View(med);
        }

        [HttpPost]
        public async Task<ActionResult> EditMed(Medicine formMed)
        {
            if (ModelState.IsValid)
            {
                Medicine med = db.Medicines.Include("User").Single(m => m.Id == formMed.Id);

                if (med != null)
                {
                    if (PropertyChanged(med.Name, formMed.Name)) med.Name = formMed.Name;
                    if (PropertyChanged(med.RemindTime.ToString(), formMed.RemindTime.ToString())) med.RemindTime = formMed.RemindTime;

                    db.Medicines.Attach(med);
                    db.Entry(med).State = EntityState.Modified;

                    try
                    {
                        log.Info("Editing MedicineId: " + med.Id+" with values "+med.Name+" "+ med.RemindTime.Value.ToShortTimeString());
                        await db.SaveChangesAsync();
                        JobScheduler.RemoveJob(med.Id);
                        JobScheduler.ScheduleMedicineReminder(med);
                        return RedirectToAction("Edit", "Admin", new { id = med.User.Id });
                    }
                    catch
                    {
                        log.Info("Error while editing Med with id: " + formMed.Id);
                        return View("Error", new string[] { "Medicine Edit Error" });
                    }
                }
            }

            var errorList = ModelState.Values.SelectMany(m => m.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList();

            return View("Error", errorList);
        }

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