using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PillBox.Model.Entities;
using PillBox.Services;
using PillBox.Website.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace PillBox.Website.Controllers
{
    [Authorize(Roles="Admin")]
    public class RoleAdminController : Controller
    {
        //
        // GET: /RoleAdmin/
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([Required]string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result
                    = await RoleManager.CreateAsync(new PillBoxRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            PillBoxRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] { "Role Not Found" });
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            PillBoxRole role = await RoleManager.FindByIdAsync(id);
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();
            IEnumerable<PillBoxUser> members
                = UserManager.Users.Where(x => memberIDs.Any(y => y == x.Id));
            IEnumerable<PillBoxUser> nonMembers = UserManager.Users.Except(members);
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    result = await UserManager.AddToRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    result = await UserManager.RemoveFromRoleAsync(userId,
                        model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                return RedirectToAction("Index");
            }
            return View("Error", new string[] { "Role Not Found" });
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

        private PillBoxRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<PillBoxRoleManager>();
            }
        }
	}
}