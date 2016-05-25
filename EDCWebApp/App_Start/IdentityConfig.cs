using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using EDCWebApp.Models;
using SendGrid;
using System.Configuration;
using System.Net;
using System.Data.Entity;
using System.Web;
using EDCWebApp.Extensions;

namespace EDCWebApp
{

    //configure email service
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return ConfigSendGridAsync(message);
        }

        private Task ConfigSendGridAsync(IdentityMessage message)
        {
            var myMessage = new SendGrid.SendGridMessage();
            var emailOrigin = ConfigurationManager.AppSettings["mailAddress"];
            var emailDisplayName = ConfigurationManager.AppSettings["mailDisplayName"];
            myMessage.From = new System.Net.Mail.MailAddress(emailOrigin, emailDisplayName);
            myMessage.AddTo(message.Destination);
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(
                    ConfigurationManager.AppSettings["mailAccount"],
                    ConfigurationManager.AppSettings["mailPassword"]
                    );

            var transWeb = new Web(credentials);

            if (transWeb != null)
            {
                return transWeb.DeliverAsync(myMessage);
            }
            else
            {
                return Task.FromResult(0);
            }
        }
    }


    //configure the application role manager
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {

        }
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
             //   RequireNonLetterOrDigit = true,
             //   RequireDigit = true,
             //   RequireLowercase = true,
            //    RequireUppercase = true,
            };
            manager.EmailService = new EmailService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    //configure the application db initializer used for administer test
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        private void InitializeIdentityForEF(ApplicationDbContext context)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();

            var name = ConfigurationManager.AppSettings["managerName"];
            var pwd = ConfigurationManager.AppSettings["managerPassword"];

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                var result = userManager.Create(user, pwd);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }
            HttpContext.Current.GetOwinContext().AddUserRole(user.Id, "Admin");

            var teacherName = ConfigurationManager.AppSettings["teacherName"];
            var teacherPwd = ConfigurationManager.AppSettings["teacherPassword"];
            var teacher = userManager.FindByName(teacherName);
            if (teacher == null)
            {
                teacher = new ApplicationUser() { Email = teacherName, UserName = teacherName };
                var result = userManager.Create(teacher, teacherPwd);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }
            HttpContext.Current.GetOwinContext().AddUserRole(teacher.Id, "Teacher");
        }
    }

}
