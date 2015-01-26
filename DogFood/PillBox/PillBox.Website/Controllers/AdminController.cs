using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PillBox.Model.Entities;
using PillBox.Services;
using PillBox.Website.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PillBox.Website.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            AdminHomeViewModel model = new AdminHomeViewModel();
            model.Users = UserManager.Users.ToList();
            model.CreatePatientModel = new CreatePatientModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(AdminHomeViewModel model)
        {
            if(ModelState.IsValid)
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

                Medicine med = null;

                if(!string.IsNullOrEmpty(model.CreatePatientModel.Medicine))
                {
                    med = new Medicine(){Name = model.CreatePatientModel.Medicine};
                }

                if(med != null && model.CreatePatientModel.RemindTime != null)
                {
                    med.RemindTime = model.CreatePatientModel.RemindTime;
                    user.Medicines.Add(med);
                }

                IdentityResult result = await UserManager.CreateAsync(user, user.PhoneNumber);

                if(result.Succeeded) {
                    return RedirectToAction("Index");
                }else{
                    AddErrorsFromResult(result);
                }
            }

            return View("Index");
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach(string error in result.Errors)
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