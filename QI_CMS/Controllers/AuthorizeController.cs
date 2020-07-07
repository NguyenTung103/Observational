using Administrator.Library.Models;
using ES_CapDien.AppCode;
using ES_CapDien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ES_CapDien.Controllers
{
    public class AuthorizeController : Controller
    {
        //
        // GET: /Roles/
        private readonly AuthorizeService authorizeService;
        public AuthorizeController()
        {
            authorizeService = new AuthorizeService();
        }
        public ActionResult Index(int editing)
        {
            ViewBag.editing = editing;
            List<int> dsIscheck = new List<int>();
            webpages_RolesModel model = new webpages_RolesModel();
            List<webpages_UsersInRoles> lstUserInRoles = authorizeService.userInRolesResponsitory.GetAll().Where(i => i.UserId == editing).ToList();
            foreach(var j in lstUserInRoles)
            {
                dsIscheck.Add(j.RoleId);
            }
            ViewBag.IsCheck = dsIscheck;
            List<webpages_RolesModel> list = authorizeService.GetAll().AsEnumerable().Select(item => new webpages_RolesModel
            {
                RoleId= item.RoleId,
                RoleName= item.RoleName,
                Description=item.Description,
                
            }).ToList();
            return View(list); 
        }
        [HttpPost]
        public ActionResult AddRoles(List<int> idselects, int userId, List<int> idNotSelect)
        {
            bool check = false;
            if(idselects.Count()>0)
            {
                foreach (var i in idselects)
                {
                    webpages_UsersInRoles item = new webpages_UsersInRoles();
                    item.UserId = userId;
                    item.RoleId = i;
                    item.Description = authorizeService.roleResponsitory.Single(i).Description.ToString();
                    if (authorizeService.userInRolesResponsitory.GetAll().Where(k => k.RoleId == i && k.UserId == userId).ToList().Count() == 0)
                    {
                        check = authorizeService.userInRolesResponsitory.Insert(item);
                    }
                }
            }            
            if (idNotSelect.Count() > 0)
            {
                foreach (var i in idNotSelect)
                {
                    var cate = authorizeService.userInRolesResponsitory.GetAll().Where(q => q.UserId == userId && q.RoleId == i).ToList();
                    if (cate.Count() > 0)
                    {
                        var a = cate.FirstOrDefault();
                        check = authorizeService.userInRolesResponsitory.Delete(a);
                    }
                }
            }
            TempData["MessageStatus"] = check;
            TempData["Message"] = $"Chỉnh sửa quyền {(check ? "" : "không")} thành công";
            return Json(new { Result = check });
        }
        [HttpPost]
        public ActionResult EditRoles(List<int> ids, int userId)
        {
            bool check = false;
            if (ids.Count() > 0)
            {
                foreach (var i in ids)
                {
                    var cate = authorizeService.userInRolesResponsitory.GetAll().Where(q => q.UserId == userId && q.RoleId == i).ToList();
                    if (cate.Count() > 0)
                    {
                        var a = cate.FirstOrDefault();
                        check = authorizeService.userInRolesResponsitory.Delete(a);
                    }
                }
            }
            TempData["MessageStatus"] = check;
            TempData["Message"] = $"Chỉnh sửa quyền {(check ? "" : "không")} thành công";
            return Json(new { Result = check });        
        }                                           
    }
}
