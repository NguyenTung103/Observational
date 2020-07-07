using Administrator;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ES_CapDien.Controllers
{    
    public class AdminController : Controller
    {        
        public ActionResult DangNhap()
        {
            return Redirect("Administrator/Account/Login");
        }
        [AllowAnonymous]
        public ActionResult LogOff(string returnUrl)
        {
            WebSecurity.Logout();
            return Redirect("/Administrator/Account/Login");
        }
    }
}
