using Administrator;
using ES_CapDien.AppCode;
using ES_CapDien.Helpers;
using ES_CapDien.Models;
using HelperLibrary;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ES_CapDien.Controllers
{
    //Có quyền đăng nhập mới được truy cập
    [Authorize]
    public class SitesController : BaseController
    {
        //
        // GET: /Point/
        private readonly AreasService areasService;
        private readonly GroupService groupService;
        private readonly SitesService sitesService;
        private readonly UserProfileService userProfileService;
        public SitesController()
        {
            areasService = new AreasService();
            groupService = new GroupService();
            userProfileService = new UserProfileService();
            sitesService = new SitesService();
        }

        /// <summary>
        /// Hiển thị dữ liệu
        /// </summary>
        /// <param name="page">trang hiện tại</param>
        /// <param name="pageSize">tổng số trang</param>
        /// <param name="title">title trang web</param>
        /// <returns></returns>
        [AuthorizeRoles]
        public ActionResult SitesManagement(int page = 1, int pageSize = 50, string title = "", int? areaId=null)
        {
            ViewBag.Title = "";
            ViewBag.MessageStatus = TempData["MessageStatus"];
            ViewBag.Message = TempData["Message"];
            if (pageSize == 1)
            {
                pageSize = CMSHelper.pageSizes[0];
            }
            @ViewBag.PageSizes = CMSHelper.pageSizes;
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            string userName = User.Identity.Name;
            int skip = (page - 1) * pageSize;
            int totalRows = 0;
            List<SiteModel> list = new List<SiteModel>();

            #region Lấy dữ liệu
            if (userName == "administrator")
            {
                list = sitesService.GetAll(skip, pageSize, out int totalRow, title, areaId, null).AsEnumerable().Select(item => new SiteModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    DeviceId = item.DeviceId,
                    Latitude = item.Latitude,
                    Longtitude = item.Longtitude,
                    CreateDay = item.CreateDay,
                    NguoiTao = userProfileService.userProfileResponsitory.Single(item.CreateBy).FullName,
                    IsActive = item.IsActive,
                    GroupsName = groupService.groupResponsitory.Single(item.Group_Id).Name,
                    AreasName = areasService.areaResponsitory.Single(item.Area_Id).Name
                }).ToList();
                totalRows = totalRow;
            }
            else
            {
                int groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id.Value;
                list = sitesService.GetAll(skip, pageSize, out int totalRow, title, areaId, groupId).AsEnumerable().Select(item => new SiteModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Latitude = item.Latitude,
                    DeviceId = item.DeviceId,
                    Longtitude = item.Longtitude,
                    CreateDay = item.CreateDay,
                    NguoiTao = userProfileService.userProfileResponsitory.Single(item.CreateBy).FullName,
                    IsActive = item.IsActive,
                    GroupsName = groupService.groupResponsitory.Single(item.Group_Id).Name,
                    AreasName = areasService.areaResponsitory.Single(item.Area_Id).Name
                }).ToList();
                totalRows = totalRow;
            }           
            #endregion

            #region Hiển thị dữ liệu và phân trang
            SitesViewModel viewModel = new SitesViewModel
            {
                Sites = new StaticPagedList<SiteModel>(list, page, pageSize, totalRows),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = totalRows
                }
            };
            #endregion

            return View(viewModel);
        }

        #region Xóa điểm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id)
        {
            Site pts = sitesService.sitesResponsitory.Single(Id);
            bool checkDelete = false;
            if (pts != null)
            {
                checkDelete = sitesService.sitesResponsitory.Delete(pts);
            }
            TempData["MessageStatus"] = checkDelete;
            TempData["Message"] = $"Xóa điểm {(checkDelete ? "" : "không")} thành công";
            return RedirectToAction("SitesManagement", new { page = Request.Params["page"], pageSize = Request.Params["pageSize"], areaId = Request.Params["areaId"] });
        }

        [HttpPost]
        public ActionResult DeleteSelected(List<int> ids)
        {
            bool checkDelete = false;
            foreach (var i in ids)
            {
                Site pts = sitesService.sitesResponsitory.Single(i);
                if (pts != null)
                {
                    checkDelete = sitesService.sitesResponsitory.Delete(pts);
                }
            }
            TempData["MessageStatus"] = checkDelete;
            TempData["Message"] = $"Xóa điểm {(checkDelete ? "" : "không")} thành công";
            return Json(new { Result = checkDelete });
        }
        #endregion

        #region update điểm
        [AuthorizeRoles]
        public ActionResult Update(int id = 0)
        {
            CMSHelper help = new CMSHelper();
            @ViewBag.Title = "";
            @ViewBag.MessageStatus = TempData["MessageStatus"];
            @ViewBag.Message = TempData["Message"];

            Site pts = sitesService.sitesResponsitory.Single(id);
            if (pts == null)
            {
                return RedirectToAction("SitesManagement");
            }

            SiteModel model = pts.ToModel();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Update(SiteModel model)
        {
            if (ModelState.IsValid)
            {
                Site pts = sitesService.sitesResponsitory.Single(model.Id);
                if (pts == null)
                {
                    return RedirectToAction("SitesManagement");
                }
                bool checkSave = false;   
                pts = model.ToEntity(pts);
                checkSave = sitesService.sitesResponsitory.Update(pts);
                TempData["MessageStatus"] = checkSave;
                TempData["Message"] = $"Cập nhật điểm {(checkSave ? "" : "không")} thành công";

                return RedirectToAction("SitesManagement");
            }
            return View(model);
        }
        #endregion

        #region Create điểm
        [AuthorizeRoles]
        public ActionResult Create()
        {
            CMSHelper help = new CMSHelper();
            @ViewBag.Title = "";
            @ViewBag.MessageStatus = TempData["MessageStatus"];
            @ViewBag.Message = TempData["Message"];
            SiteModel model = new SiteModel();
            List<Group> groups = new List<Group>();
            List<Area> areas = new List<Area>();
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            string userName = User.Identity.Name;
            if (userName == "administrator")
            {
                groups = groupService.groupResponsitory.GetAll().ToList();
                areas = areasService.areaResponsitory.GetAll().ToList();
            }
            else
            {
                int groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id.Value;
                Group group = groupService.groupResponsitory.Single(groupId);
                groups.Add(group);
                areas = areasService.areaResponsitory.GetAll().Where(i => i.Group_Id == groupId).ToList();
            }
            ViewBag.listGroup = groups;
            ViewBag.lstAreas = areas;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SiteModel model)
        {
            if (ModelState.IsValid)
            {
                Site pts = new Site();
                bool checkSave = false;               
                int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;                
                pts = model.ToEntity(pts);
                pts.CreateBy = CurrentUserId;                
                pts.CreateDay = DateTime.Now;
                checkSave = sitesService.sitesResponsitory.Insert(pts);
                TempData["MessageStatus"] = checkSave;
                TempData["Message"] = $"Thêm mới điểm {(checkSave ? "" : "không")} thành công";
                return RedirectToAction("SitesManagement");
            }
            return View(model);
        }
        public ActionResult GetAreasByGroupId(int idGroup)
        {
            List<Area> areas = new List<Area>();
            areas = areasService.GetAreasByGroupId(idGroup).ToList();
            return PartialView("_AreasPartialView", areas);
        }
        #endregion
    }
}
