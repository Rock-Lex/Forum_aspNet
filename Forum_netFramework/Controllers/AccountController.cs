using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Forum_netFramework.Models;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace Forum_netFramework.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        private ForumDBEntities _dataBase = new ForumDBEntities();

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        /*[ValidateAntiForgeryToken]*/
        public async Task<ActionResult> Register(string username, string password, string email)
        {
            var passwordHash = password.GetHashCode();

            User user = new User
            {
                Name = username,
                PasswordHash = passwordHash,
                Email = email,
            };

            _dataBase.Users.Add(user);
            _dataBase.SaveChanges();

            var code = GenetateCode(username);

            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                        protocol: Request.Url.Scheme);

            IdentityMessage message = new IdentityMessage()
            {
                Destination = email,
                Subject = "Подтверждение электронной почты",
                Body = username + ", для завершения регистрации перейдите по ссылке " + callbackUrl
            };
            await SendAsync(message);

            return View();
        }

        private int GenetateCode(string username)
        {
            var usernameHash = username.GetHashCode();
            return usernameHash;
        }

        public Task SendAsync(IdentityMessage message)
        {
            SmtpClient client = new SmtpClient()
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("techforum.1111@gmail.com", "techforum12")
            };

            return client.SendMailAsync("techforum.1111@gmail.com", message.Destination, message.Subject, message.Body);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, int code)
        {
            var user = _dataBase.Users.First(x => x.Id == userId);

            if (code == GenetateCode(user.Name))
            {
                user.IsConfirmed = true;

                _dataBase.SaveChanges();

                return View();
            }

            return View("Error");
        }
    }
}