using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.Entity;
using System;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web.Mvc;
using WatsonTruckV2.Models;

namespace WatsonTruckV2.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private ApplicationDbContext context = new ApplicationDbContext();

        public AdminRolesController()
        {

        }
       

        public AdminRolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        //====================================
        // GET: Index 
        //==================================== 

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                ViewBag.displayMenu = "No";

                if (isAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                }
                return View();
            }
            else
            {
                ViewBag.Name = "Not Logged IN";
            }
            return View();

        }

        public bool isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        //====================================
        // GET: ListRoles 
        //==================================== 
        [HttpGet]
        public ActionResult ListRoles()
        {
            var roleList = roleManager.Roles;

            return View(roleList);
        }

        //====================================
        // GET: CreateRole 
        //====================================  
        [HttpGet]
        public ActionResult CreateRole()
        {
            return View();
        }

        //====================================
        // Post: CreateRole 
        //==================================== 
        [HttpPost]
        public async Task<ActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.UserRole
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", result.Errors.First());
                        //var errors = string.Join(",", result.Errors);
                        //ModelState.AddModelError("", errors);
                    }
                    return View(model);
                }

            }

            return View(model);
        }

        //====================================
        // GET: EditRole 
        //==================================== 
        public async Task<ActionResult> EditRole(string RoleId)
        {
            if (RoleId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var role = await roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {RoleId} cannot be found";
                return View("NotFound");
                //return HttpNotFound();
            }

            EditRoleViewModel editRoleViewModel = new EditRoleViewModel
            {
                RoleId = role.Id,
                UserRole = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(role.Id, role.Name))
                {
                    editRoleViewModel.Users.Add(user.UserName);
                }
            }

            return View(editRoleViewModel);
        }


        //====================================
        // Post: EditRole 
        //==================================== 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRole([Bind(Include = "Name,RoleId")] EditRoleViewModel editRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(editRoleViewModel.RoleId);

                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Role with Id = {editRoleViewModel.RoleId} cannot be found";
                    return View("NotFound");
                }
                else
                {
                    role.Name = editRoleViewModel.UserRole;
                    var result = await roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", result.Errors.First());

                    }
                    return View(editRoleViewModel);
                }
            }
            return View(editRoleViewModel);
        }

        //====================================
        // GET: EditUsersInRole 
        //==================================== 
        [HttpGet]
        public async Task<ActionResult> EditUsersInRole(string RoleId)
        {
            ViewBag.RoleId = RoleId;
            //ViewBag.userId = userId;

            var role = await roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {RoleId} cannot be found";
                return View("NotFound");
                //return HttpNotFound();
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    userId = user.Id,
                    UserName = user.UserName

                };

                if (await userManager.IsInRoleAsync(role.Id, role.Name))
                {
                    userRoleViewModel.isSelected = true;
                }
                else
                {
                    userRoleViewModel.isSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        //====================================
        // Post: EditUsersInRole 
        //==================================== 
        [HttpPost]
        public async Task<ActionResult> EditUsersInRole(List<UserRoleViewModel> model, string RoleId)
        {
            var role = await roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {RoleId} cannot be found";
                return View("NotFound");
                //return HttpNotFound();
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].userId);

                IdentityResult result = null;

                if (model[i].isSelected && !(await userManager.IsInRoleAsync(role.Id, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(role.Id, role.Name);

                }
                else if (!model[i].isSelected && await userManager.IsInRoleAsync(role.Id, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(role.Id, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = RoleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = RoleId });
        }

        //====================================
        // Get: AdminRoles/DeleteRole/RoleId
        //==================================== 
        public async Task<ActionResult> DeleteRole(string RoleId)
        {
            if (RoleId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var role = await roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {RoleId} cannot be found";
                return View("NotFound");
                //return HttpNotFound();
            }
            //else
            //{
                //try
                //{
                    //throw new Exception("Test Exception");
                    //    var result = await roleManager.DeleteAsync(role);

                    //    if (result.Succeeded)
                    //    {
                    //        return RedirectToAction("ListRoles");
                    //    }
                    //    foreach (var error in result.Errors)
                    //    {
                    //        ModelState.AddModelError(string.Empty, result.Errors.First());
                    //    }

                    //    return View("ListRoles");
                //}
                //catch(DbUpdateException)
                //{
                   
                //    ViewBag.ErrorTitle = $"{role.Name} role is in use";
                //    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users in this role." + 
                //        $"If you want to delete this role, please remove the users from the role and then try to delete";
                //    return View("Error");
                //}
            //}
    

            return View(role);

        }

        //====================================
        // Post: AdminRoles/DeleteRole/RoleId
        //==================================== 
        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRoleConfirmed(string RoleId, string User)
        {
            if (RoleId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var role = await roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {RoleId} cannot be found";
                return View("NotFound");
                //return HttpNotFound();
            }

            IdentityResult result;
            if (User != null)
            {
                result = await roleManager.DeleteAsync(role);
            }
            else
            {
                result = await roleManager.DeleteAsync(role);
            }

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", result.Errors.First());

            }
            return View("ListRoles");
        }
    }
}