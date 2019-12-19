using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Web.Hosting;
using WatsonTruckV2.Models;

[assembly: OwinStartupAttribute(typeof(WatsonTruckV2.Startup))]
namespace WatsonTruckV2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        // This method will create default User roles and Admin user for login 
        private void CreateRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // In Startup, creating first Admin Role and two default Admin Users
            if (!roleManager.RoleExists("Admin"))
            {
                //Create Admin Role
                if (!roleManager.RoleExists("Admin"))
                {
                    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                    role.Name = "Admin";
                    roleManager.Create(role);
                }


                //Create Admin- Users who will maintain the website
                var user1 = new ApplicationUser();
                //user1.UserRole = "Admin";
                user1.UserName = "vePape";
                user1.Email = "v.pape3@gmail.com";
                user1.EmailConfirmed = true;
                user1.PhoneNumber = "575-318-8115";
                user1.PhoneNumberConfirmed = true;
                //user1.EmployeeNumber = "0000";
                
                string user1Password = "!Example@1234";

                var checkUser1 = userManager.Create(user1, user1Password);

                //Add default User to Role Admin
                if (checkUser1.Succeeded)
                {
                    var result1 = userManager.AddToRole(user1.Id, "Admin");
                }

                var user2 = new ApplicationUser();
                //user1.UserRole = "Admin";
                user2.UserName = "lyRichards";
                user2.Email = "Lynetta.Richards@WatsonTruck.com";
                user2.EmailConfirmed = true;
                user2.PhoneNumber = "575-390-3886";
                user2.PhoneNumberConfirmed = true;
                //user1.EmployeeNumber = "1111";

                string user2Password = "!Example@5678";

                var checkUser2 = userManager.Create(user2, user2Password);

                //Add default User to Role Admin
                if (checkUser2.Succeeded)
                {
                    var result2 = userManager.AddToRole(user2.Id, "Admin");
                }
            }

            //Create Manager Role
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);
            }          

            //Create Employee Role
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);
            }
            
        }
    }
}
