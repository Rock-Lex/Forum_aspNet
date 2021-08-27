using Microsoft.VisualStudio.TestTools.UnitTesting;
using Forum_netFramework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forum_netFramework.Models;
using System.Web.Mvc;

namespace Forum_netFramework.Controllers.UnitTests
{
    [TestClass()]
    public class HomeControllerUnitTests
    {
        private HomeController _homeController = new HomeController();
        private ForumDBEntities _dataBase = new ForumDBEntities();

        [TestMethod()]
        public void IndexUnitTest()
        {
            var result = _homeController.Index() as ViewResult;
            
            Assert.AreNotEqual(result.ViewName, "Index");
        }

        [TestMethod()]
        public void HomePageUnitTest()
        {
            var result = _homeController.HomePage() as ViewResult;

            Assert.AreNotEqual("HomePage", result.ViewName);
        }
    }
}