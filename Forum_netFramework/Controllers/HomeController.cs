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
            var items = _dataBase.Topics;
            
            if (items != null)
            {
                return View(items);
            }

            return View("Index");
        }

        public ActionResult HomePage()              
        {
            return View(_dataBase.Topics);
        }

        public ActionResult TopicPage(int topicId)
        {
            if (ValidateData(topicId))
            {
                var user = FindCurrentUser();

                var topic = _dataBase.Topics.First(x => x.Id == topicId);

                Topic.ReturnTopic returnTopic = new Topic.ReturnTopic()
                {
                    userId = user.Id,
                    topic = topic
                };

                return View(returnTopic);
            }

            return View("TopicPage");
        }

        [HttpPost]
        public ActionResult Login(string password, string email)
        {
            if (ValidateData(password, email))
            {
                var passwordHash = password.GetHashCode();
                var user = _dataBase.Users.First(x => x.Email == email && x.PasswordHash == passwordHash && x.IsConfirmed == true); // должно выбить ошибку если не нашло такого пользователя

                Session["currentUserEmail"] = email;
                Session["currentUserPassword"] = password;

                return RedirectToAction("HomePage", "Home");
            }

            return View("Login");
        }

        [HttpPost]
        public void SendComment(string commentText, int topicId)
        {
            if (ValidateData(commentText, topicId))
            {
                var user = FindCurrentUser();

                Comment newComment = new Comment()
                {
                    Text = commentText,
                    TopicId = topicId,
                    UserId = user.Id
                };

                _dataBase.Comments.Add(newComment);
                _dataBase.SaveChanges();
            }
        }

        [HttpPost]
        public void ChangeComment(string newCommentText, int commentId)
        {
            if (ValidateData(newCommentText, commentId))
            {
                var user = FindCurrentUser();

                var oldComment = _dataBase.Comments.First(x => x.Id == commentId);

                oldComment.Text = newCommentText;

                _dataBase.SaveChanges();
            }
        }

        [HttpPost]
        public void CreateNewTopic(string topicName, string topicDescription, string topicText)
        {
            if (ValidateData(topicName, topicDescription, topicText))
            {
                var user = FindCurrentUser();

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
        }

        public ActionResult NewTopic()
        {
            return View();
        }

        private User FindCurrentUser()
        {
            string userEmail = Session["currentUserEmail"] as string;
            string userPass = Session["currentUserPassword"] as string;

            var passwordHash = userPass.GetHashCode();

            var user = _dataBase.Users.First(x => x.Email == userEmail && x.PasswordHash == passwordHash);

            return user;
        }

        [ChildActionOnly]
        public PartialViewResult HeaderAuthorised()
        {
            var user = FindCurrentUser();

            return PartialView("HeaderAuthorised", user);
        }

        [ChildActionOnly]
        public PartialViewResult HeaderStart()
        {
            return PartialView("HeaderStart");
        }

        private bool ValidateData(string password, string email)
        {
            if(password.Length > 0 && email.Length > 0)
            {
                return true;
            }
         
            return false;
        }

        private bool ValidateData(string commentText, int topicId)
        {
            if (commentText.Length > 0 && topicId != null)
            {
                return true;
            }
    
            return false;
        }

        private bool ValidateData(string topicName, string topicDescription, string topicText)
        {
            if (topicName.Length > 0 && topicDescription.Length > 0 && topicText.Length > 0)
            {
                return true;
            }
            
            return false;
        }

        private bool ValidateData(int topicId)
        {
            if (topicId != null)
            {
                return true;
            }
    
            return false;
        }
    }
}