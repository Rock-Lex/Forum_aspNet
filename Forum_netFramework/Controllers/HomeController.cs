using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Forum_netFramework.Models;
using Microsoft.AspNetCore.Identity;

namespace Forum_netFramework.Controllers
{
    public class HomeController : Controller
    {
        private ForumDBEntities _dataBase = new ForumDBEntities();

        public ActionResult Index()
        {
            var Items = _dataBase.Topics;
            return View(Items);
        }

        public ActionResult HomePage()              
        {
            return View(_dataBase.Topics);
        }

        public ActionResult TopicPage(int topicId)
        {
            string userEmail = Session["currentUserEmail"] as string;
            string userPassword = Session["currentUserPassword"] as string;

            var passwordHash = userPassword.GetHashCode();

            var user = _dataBase.Users.First(x => x.Email == userEmail && x.PasswordHash == passwordHash);

            var topic = _dataBase.Topics.First(x => x.Id == topicId);

            Topic.ReturnTopic returnTopic = new Topic.ReturnTopic()
            {
                userId = user.Id,
                topic = topic
            };

            return View(returnTopic);
        }

        [HttpPost]
        public ActionResult Login(string password, string email)
        {
            var passwordHash = password.GetHashCode();
            var user = _dataBase.Users.First(x => x.Email == email && x.PasswordHash == passwordHash && x.IsConfirmed == true); // должно выбить ошибку если не нашло такого пользователя

            Session["currentUserEmail"] = email;
            Session["currentUserPassword"] = password;

            return RedirectToAction("HomePage", "Home");
        }

        [HttpPost]
        public void SendComment(string commentText, int topicId)
        {
            string userEmail = Session["currentUserEmail"] as string;
            string userPassword = Session["currentUserPassword"] as string;

            var passwordHash = userPassword.GetHashCode();

            var user = _dataBase.Users.First(x => x.Email == userEmail && x.PasswordHash == passwordHash);

            Comment newComment = new Comment()
            {
                Text = commentText,
                TopicId = topicId,
                UserId = user.Id
            };

            _dataBase.Comments.Add(newComment);
            _dataBase.SaveChanges();
        }

        [HttpPost]
        public void ChangeComment(int commentId, string newCommentText)
        {
            string userEmail = Session["currentUserEmail"] as string;
            string userPassword = Session["currentUserPassword"] as string;

            var passwordHash = userPassword.GetHashCode();

            var user = _dataBase.Users.First(x => x.Email == userEmail && x.PasswordHash == passwordHash);

            var oldComment = _dataBase.Comments.First(x => x.Id == commentId);

            oldComment.Text = newCommentText;

            _dataBase.SaveChanges();
        }

        [HttpPost]
        public void CreateNewTopic(string topicName, string topicDescription, string topicText)
        {
            string userEmail = Session["currentUserEmail"] as string;
            string userPassword = Session["currentUserPassword"] as string;

            var passwordHash = userPassword.GetHashCode();

            var user = _dataBase.Users.First(x => x.Email == userEmail && x.PasswordHash == passwordHash);

            Topic newTopic = new Topic()
            {
                Name = topicName,
                Description = topicDescription,
                Text = topicText,
                UsersId = user.Id,
                CreationTime = DateTime.Now
            };

            _dataBase.Topics.Add(newTopic);
            _dataBase.SaveChanges();
        }

        public ActionResult NewTopic()
        {
            return View();
        }

        /*PartialView*/

        [ChildActionOnly]
        public PartialViewResult HeaderAuthorised()
        {
            string userEmail = Session["currentUserEmail"] as string;
            string userPass = Session["currentUserPassword"] as string;

            var passwordHash = userPass.GetHashCode();

            var user = _dataBase.Users.First(x => x.Email == userEmail && x.PasswordHash == passwordHash);

            return PartialView("HeaderAuthorised", user);
        }

        [ChildActionOnly]
        public PartialViewResult HeaderStart()
        {
            return PartialView("HeaderStart");
        }
    }
}