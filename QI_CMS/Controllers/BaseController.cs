using System.Linq;
using System.Web.Mvc;

namespace ES_CapDien.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public BaseController()
        {

        }

        /// <summary>
        /// Lấy user đăng nhập hiện tại trong hệ thống
        /// </summary>
        protected UserProfile CurrenUser
        {
            get
            {
                string identityUserName = User?.Identity?.Name;
                if (!string.IsNullOrEmpty(identityUserName))
                {
                    ObservationEntities db = new ObservationEntities();
                    return db.UserProfiles.FirstOrDefault(i => i.UserName == identityUserName);
                }
                return null;
            }
        }
    }
}