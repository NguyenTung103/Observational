using System;
using System.Web.Mvc;

#pragma warning disable 1591

namespace ES_CapDien.Repository
{
    /// <summary>
    /// Kế thừa Lớp Controller, bổ sung UnitOfWork sử dụng cho Repository.
    /// </summary>
    public class BaseController : Controller
    {
        protected UserActionLogInfo UserActionLogInfo;

        protected BaseController()
        {
            Uow = new UnitOfWork();
        }

        /// <summary>
        /// Xử lý Lấy thông tin người dùng - thông tin form phục vụ mục đích log user acction
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (Uow == null) return;
            var request = System.Web.HttpContext.Current.Request;
            Uow.UserActionLogInfo = new UserActionLogInfo
            {   
                //todo: lấy actor là người dùng hiện tại.
                Time = DateTime.Now,
                Url = request.Url.PathAndQuery
            };
        }
        
        protected IUnitOfWork Uow { get; set; }
        
    }
}