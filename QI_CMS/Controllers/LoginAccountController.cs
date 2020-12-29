using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Administrator.Library.Filters;
using ES_CapDien.Models;
using CaptchaMvc.HtmlHelpers;
using System.Configuration;
using ES_CapDien.AppCode;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ES_CapDien.Controllers
{
    [Authorize]
    [InitializeAdministrator]
    public class LoginAccountController : Controller
    {        
        private readonly UsersProfileService usersProfileService;       
        public LoginAccountController()
        {
            usersProfileService = new UsersProfileService();            
        }
        //
        // GET: /Account/
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {            
            ViewBag.ReturnUrl = returnUrl;            
            return View();
        }        

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl, string searchDviqly)
        {
            TempData["MessageStatus"] = null;
            TempData["Message"] = "";
            @ViewBag.MessageStatus = TempData["MessageStatus"];
            @ViewBag.Message = TempData["Message"];
            using (var db = new ES_CapDien.ObservationEntities())
            {
                if (this.IsCaptchaValid("Captcha is not valid"))
                {
                    if (model.UserName.ToLower() != "administrator")
                    {
                        if (ModelState.IsValid && WebSecurity.Login(model.UserName.ToLower(), model.Password, model.RememberMe))
                        {
                            string passwordSalt = MD5Hash(model.Password);
                            usersProfileService.InsertPasswordSalt(passwordSalt, WebSecurity.GetUserId(model.UserName.ToLower()));
                            if (returnUrl != null)
                                return RedirectToLocal(returnUrl);
                            else
                                return RedirectToAction("Index", "Home", new { area = "" });
                        }
                        else
                        {                           
                            TempData["MessageStatus"] = false;
                            TempData["Message"] = "Sai tên đăng nhập hoặc mật khẩu.";
                        }
                    }
                    else
                    {
                        if (ModelState.IsValid && WebSecurity.Login(model.UserName.ToLower(), model.Password, model.RememberMe))
                        {                           
                            if (returnUrl != null)
                                return RedirectToLocal(returnUrl);
                               
                            else
                                return RedirectToAction("Index", "Home", new { area = "" });
                                
                        }
                        else
                        {                            
                            // If we got this far, something failed, redisplay form
                            TempData["MessageStatus"] = false;
                            TempData["Message"] = "Sai tên đơn vị, tên đăng nhập hoặc mật khẩu.";
                        }
                    }
                }
                else
                {
                    TempData["MessageStatus"] = false;
                    TempData["Message"] = "Mã xác nhận đã sai.";                    
                    return View(model);
                }


            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult LogOff(string returnUrl)
        {
            WebSecurity.Logout();
            return RedirectToAction("Login");
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        private string GetLocalIPAddress()
        {
            string VisitorsIPAddr = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (VisitorsIPAddr != null || VisitorsIPAddr != String.Empty)
            {
                VisitorsIPAddr = Request.ServerVariables["REMOTE_ADDR"];
            }
            return VisitorsIPAddr;
        }
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

    }
}
