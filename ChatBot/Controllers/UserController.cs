using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Security;
using System.Web.Mvc;
using ChatBot.Models;
using System.Data;
using System.Data.SqlClient;

namespace ChatBot.Controllers
{
    public class UserController : Controller
    {
        public SqlConnectionStringBuilder strr = new SqlConnectionStringBuilder();

        [HttpGet]
        public ActionResult SignUp()
        {
            UserTable userModel = new UserTable();

            return View(userModel);
        }
        [HttpPost]
        public ActionResult SignUp(UserTable user)
        {
            int userId = user.UserID;
            user.IsAdmin = false;
            using (ChatBotEntities db = new ChatBotEntities())
            {
                if (db.UserTables.Any(x => x.UserName == user.UserName))
                {
                    ViewBag.Duplicate = "User is duplicated";
                    return View("SignUp", user);
                }
                db.UserTables.Add(user);
                db.SaveChanges();

            }
            ModelState.Clear();
            ViewBag.messageSuccess = "User registred successfully";
            return View("SignUp", new UserTable { UserID = userId++ });
        }
        [HttpGet]
        public ActionResult LogIn()
        {
            ViewBag.logedIn = false;
            
            return View("LogIn");
        }

        [HttpPost]
        public ActionResult LogIn(UserTable user)
        {
            //ViewBag.logedInError = false;
            using (ChatBotEntities db = new ChatBotEntities())
            {
                if (db.UserTables.Any(x => x.UserName == user.UserName && x.Password == user.Password))
                {
                    ViewBag.logedIn = true;
                    return View("~/Views/Home/Index.cshtml");
                    //return RedirectToAction("index", "Home");
                }
                else
                {
                    ViewBag.logedInError = true;
                    return View("LogIn");
                }
            }
            
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            ViewBag.logedIn = null;
            ViewBag.logedInError = null;
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("index","Home");
            
            //return View("~/Views/Home/Index.cshtml");
        }
    }
}