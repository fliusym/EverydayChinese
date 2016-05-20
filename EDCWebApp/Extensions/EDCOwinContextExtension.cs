using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EDCWebApp.Extensions
{
    public static class EDCOwinContextExtension
    {
        public static void AddUserRole(this IOwinContext context, string userId, string roleName)
        {
            if (context != null)
            {
                var roleManager = context.Get<ApplicationRoleManager>();
                var userManager = context.GetUserManager<ApplicationUserManager>();
                if (roleManager != null && userManager != null)
                {

                    var role = roleManager.FindByName(roleName);
                    if (role == null)
                    {
                        role = new IdentityRole(roleName);
                        var roleresult = roleManager.Create(role);
                    }
                    var rolesForUser = userManager.GetRoles(userId);
                    if (!rolesForUser.Contains(role.Name))
                    {
                        var addResult = userManager.AddToRole(userId, role.Name);
                    }
                }
            }

        }

    }
}