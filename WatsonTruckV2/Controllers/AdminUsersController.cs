using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.Entity;
using System;
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
    public class AdminUsersController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private ApplicationDbContext context = new ApplicationDbContext();

        public AdminUsersController()
        {
       
        }

        public AdminUsersController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            context = new ApplicationDbContext();
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

                if (!isAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            var roles = context.Roles.ToList();
            return View(roles);
        }

        public bool isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = userManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Employee")
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
        // GET: AdminUsers/ListUsers 
        //==================================== 
        [HttpGet]
        public ActionResult ListUsers()
        {
            var userList = userManager.Users;

            return View(userList);
        }

        //====================================
        // GET: AdminUsers/CreateUser 
        //====================================  
        [HttpGet]
        public ActionResult CreateUser()
        {
            return View();
        }

        //====================================
        // Post: AdminUsers/CreateUser 
        //==================================== 
        [HttpPost]
        public async Task<ActionResult> CreateUser(UserRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser
                {
                    UserName = model.UserName
                };

                IdentityResult result = await userManager.CreateAsync(applicationUser);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
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
        // GET: AdminUsers/EditUser 
        //==================================== 
        [HttpGet]
        public async Task<ActionResult> EditUser(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {Id} cannot be found";
                return View("NotFound");
                //return HttpNotFound();
            }

            //var userClaims = await userManager.GetClaimsAsync(user.Id);
            var userRoles = await userManager.GetRolesAsync(user.Id);

            var model = new EditUserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                //Claims = userClaims.Select(c => c.Value).ToList(),
                //Roles = userRoles
            };

            return View(model);
        }


        //====================================
        // Post: AdminUsers/EditUser 
        //==================================== 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser([Bind(Include = "Id,UserName,Email")] EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                    return View("NotFound");
                }
                else
                {
                    user.Id = model.Id;
                    user.UserName = model.UserName;
                    user.Email = model.Email;

                    var applicationUser = await userManager.FindByIdAsync(User.Identity.GetUserId());

                    var result = await userManager.UpdateAsync(applicationUser);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListUsers");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.First());

                    }
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
                //return HttpNotFound();
            }

            var model = new List<UserRoleViewModel>();

            foreach(var role in roleManager.Roles)
            {
                var UserRoleViewModel = new UserRoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if(await userManager.IsInRoleAsync(user.Id, role.Name))
                {
                    UserRoleViewModel.isSelected = true;
                }
                else
                {
                    UserRoleViewModel.isSelected = false;
                }

                model.Add(UserRoleViewModel);

            }
  
            return View(model);
        }

        //issue with  await userManager.AddToRoleAsync 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageUserRoles(List<UserRoleViewModel> model, string userId, params string[] selectedRoles)
        {

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var userRoles = await userManager.GetRolesAsync(user.Id);

            selectedRoles = selectedRoles ?? new string[] { };

            var result = await userManager.AddToRolesAsync(user.Id, selectedRoles.Except(userRoles).ToArray<string>());
            for (int i = 0; i < model.Count;)
            {
                //replace UserName with UserRole or Name
                if (model[i].isSelected && await userManager.IsInRoleAsync(user.Id, user.UserName))
                {
                    //replace UserName with UserRole or Name
                    result = await userManager.RemoveFromRolesAsync(user.Id, user.UserName);

                }
                //replace UserName with UserRole or Name
                else if (!model[i].isSelected && await userManager.IsInRoleAsync(user.Id, user.UserName))
                {
                    //replace UserName with UserRole or Name
                    result = await userManager.RemoveFromRoleAsync(user.Id, user.UserName);
                }

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Cannot remove user existing roles");
                    return View(model);
                }

                result = await userManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRoles).ToArray<string>());
                //result = await userManager.AddToRoleAsync(user.Id, model.Where(x => x.isSelected).Select(y => y.RoleName));
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Cannot add selected roles to user");
                    return View(model);
                }
                return RedirectToAction("EditUser", new { Id = userId });
            }

            return View(model);
        }

        //====================================
        // Get: AdminUsers/DeleteUser/UserId
        //==================================== 
        public async Task<ActionResult> DeleteUser(string UserId)
        {
            if (UserId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {UserId} cannot be found";
                return View("NotFound");
                //return HttpNotFound();
            }
            //else
            //{
            //    var result = await userManager.DeleteAsync(user);

            //    if (result.Succeeded)
            //    {
            //        return RedirectToAction("ListUsers");
            //    }
            //    foreach(var error in result.Errors)
            //    {
            //        ModelState.AddModelError(string.Empty, result.Errors.First());
            //    }

            //    return View("ListUsers");
            //}

            return View("ListUsers");
        }

        //====================================
        // Post: AdminUsers/DeleteUser/UserId
        //==================================== 
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUserConfirmed(string UserId)
        {
            if (UserId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {UserId} cannot be found";
                return View("NotFound");
                //return HttpNotFound();
            }
            
            var result = await userManager.DeleteAsync(user);
            if (result != null)
            {
                result = await userManager.DeleteAsync(user);
            }
            else
            {
                result = await userManager.DeleteAsync(user);
            }

            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, result.Errors.First());

            }
            return View("ListUsers");
        }
    }
}