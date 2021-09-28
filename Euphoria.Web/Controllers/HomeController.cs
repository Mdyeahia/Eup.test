using Euphoria.Web.Models;
using Euphoria.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Euphoria.Web.Controllers
{ 
    
    public class HomeController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            ViewBag.Name = new SelectList(context.Roless.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {

                var exists = context.UserMasters.Any(x => x.UserName ==model.UserName);
                if (!exists)
                {
                    context.Configuration.ValidateOnSaveEnabled = false;
                    var newUser = new UserMaster();
                    newUser.Name = model.Name;
                    newUser.Address = model.Address;
                    newUser.UserName = model.UserName;
                    newUser.Password = Passhashing(model.Password);
                    newUser.roleId = model.roleId;
                    context.UserMasters.Add(newUser);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "UserName alerady exist!!!";
                    return View();
                }
            }
            return View();

        }
        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string userName, string password)
        {
            if (ModelState.IsValid)
            {


                var logPassword =Passhashing(password);
                var user = context.UserMasters.Where(s => s.UserName.Equals(userName) && s.Password.Equals(logPassword)).ToList();
                if (user.Count() > 0)
                {
                    
                    
                    Session["UserName"] = user.FirstOrDefault().UserName;
                    Session["UserId"] = user.FirstOrDefault().UserId;
                    var logindetails = user.First();
                    this.SignInUser(logindetails.UserName, false);

                    return RedirectToAction("Details",new { uName = userName });
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        private void SignInUser(string username, bool isPersistent)
        {
            // Initialization.
            var claims = new List<Claim>();

            try
            {
                // Setting
                claims.Add(new Claim(ClaimTypes.Name, username));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                // Sign In.
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }
        }

        private void ClaimIdentities(string username, bool isPersistent)
        {
            // Initialization.
            var claims = new List<Claim>();
            try
            {
                // Setting
                claims.Add(new Claim(ClaimTypes.Name, username));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }
        }


        
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Details(string uName)
        {

            var user = context.UserMasters.Where(m=>m.UserName==uName).First();


            detailsViewModel model = new detailsViewModel();
            model.Name = user.Name;
            model.Address = user.Address;
            model.UserName = user.UserName;
            model.roleId = user.roleId;
            model.roleName =context.Roless.Where(m => m.Id == user.roleId).First();

            return PartialView(model);
        }
        [CustomAuthorize("Admin")]
        public ActionResult UserDetailsTable(int Id)
        {
            
            if (Id==1)
            {
                ListViewModel model = new ListViewModel();
                model.listuser = context.UserMasters.ToList();


                return PartialView(model);
            }
            return RedirectToAction("Login");
        }
        public static string Passhashing(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}