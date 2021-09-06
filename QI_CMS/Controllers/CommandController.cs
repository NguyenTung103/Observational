using Administrator;
using ES_CapDien.AppCode;
using ES_CapDien.Helpers;
using ES_CapDien.Models;
using HelperLibrary;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ES_CapDien.Controllers
{
    public class CommandController : BaseController
    {
        //
        // GET: /Point/
        private readonly CommandService commandService;
        private readonly GroupService groupService;
        private readonly SitesService sitesService;
        private readonly UserProfileService userProfileService;
        public CommandController()
        {
            commandService = new CommandService();
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
        public ActionResult Management(int page = 1, int pageSize = 50, string title = "", int? deviceId = null)
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
            List<CommandModel> list = new List<CommandModel>();

            #region Lấy dữ liệu
            if (userName == "administrator")
            {
                list = commandService.GetAll(skip, pageSize, out int totalRow, deviceId, null).AsEnumerable().Select(item => new CommandModel
                {
                    Id = item.Id,
                    Command_Content = item.Command_Content,
                    Device_Id = item.Device_Id,                  
                    CreateDay = item.CreateDay,
                    UserName = userProfileService.userProfileResponsitory.Single(item.CreateBy).FullName,
                    Status = item.Status,
                    GroupName = groupService.groupResponsitory.Single(item.GroupId).Name,
                    SiteName = sitesService.GetByDeviceId(item.Device_Id).FirstOrDefault().Name
                }).ToList();
                totalRows = totalRow;
            }
            else
            {
                int groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id.Value;
                list = commandService.GetAll(skip, pageSize, out int totalRow, deviceId, groupId).AsEnumerable().Select(item => new CommandModel
                {
                    Id = item.Id,
                    Command_Content = item.Command_Content,
                    Device_Id = item.Device_Id,
                    CreateDay = item.CreateDay,
                    UserName = userProfileService.userProfileResponsitory.Single(item.CreateBy).FullName,
                    Status = item.Status,
                    GroupName = groupService.groupResponsitory.Single(item.GroupId).Name,
                    SiteName = sitesService.GetByDeviceId(item.Device_Id).FirstOrDefault().Name
                }).ToList();
                totalRows = totalRow;
            }
            #endregion

            #region Hiển thị dữ liệu và phân trang
            CommandViewModel viewModel = new CommandViewModel
            {
                Command = new StaticPagedList<CommandModel>(list, page, pageSize, totalRows),
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
            Command pts = commandService.commandResponsitory.Single(Id);
            bool checkDelete = false;
            if (pts != null)
            {
                checkDelete = commandService.commandResponsitory.Delete(pts);
            }
            TempData["MessageStatus"] = checkDelete;
            TempData["Message"] = $"Xóa lệnh {(checkDelete ? "" : "không")} thành công";
            return RedirectToAction("Management", new { page = Request.Params["page"], pageSize = Request.Params["pageSize"], areaId = Request.Params["areaId"] });
        }

        [HttpPost]
        public ActionResult DeleteSelected(List<int> ids)
        {
            bool checkDelete = false;
            foreach (var i in ids)
            {
                Command pts = commandService.commandResponsitory.Single(i);
                if (pts != null)
                {
                    checkDelete = commandService.commandResponsitory.Delete(pts);
                }
            }
            TempData["MessageStatus"] = checkDelete;
            TempData["Message"] = $"Xóa điểm {(checkDelete ? "" : "không")} thành công";
            return Json(new { Result = checkDelete });
        }
        #endregion

        //#region update điểm
        //[AuthorizeRoles]
        //public ActionResult Update(int id = 0)
        //{
        //    CMSHelper help = new CMSHelper();
        //    @ViewBag.Title = "";
        //    @ViewBag.MessageStatus = TempData["MessageStatus"];
        //    @ViewBag.Message = TempData["Message"];
        //    int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
        //    string userName = User.Identity.Name;
        //    List<RegionalGroup> groups = new List<RegionalGroup>();
        //    List<Area> areas = new List<Area>();
        //    if (userName == "administrator")
        //    {
        //        groups = groupService.groupResponsitory.GetAll().ToList();
        //        areas = areasService.areaResponsitory.GetAll().ToList();
        //    }
        //    else
        //    {
        //        int groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id.Value;
        //        RegionalGroup group = groupService.groupResponsitory.Single(groupId);
        //        groups.Add(group);
        //        areas = areasService.areaResponsitory.GetAll().Where(i => i.Group_Id == groupId).ToList();
        //    }
        //    ViewBag.listGroup = groups;
        //    ViewBag.lstAreas = areas;
        //    Site pts = sitesService.sitesResponsitory.Single(id);
        //    if (pts == null)
        //    {
        //        return RedirectToAction("SitesManagement");
        //    }

        //    SiteModel model = pts.ToModel();
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        //public ActionResult Update(SiteModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Site pts = sitesService.sitesResponsitory.Single(model.Id);
        //        if (pts == null)
        //        {
        //            return RedirectToAction("SitesManagement");
        //        }
        //        bool checkSave = false;
        //        pts.Name = model.Name;
        //        pts.UpdateDay = DateTime.Now;
        //        int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
        //        pts.UpdateBy = CurrentUserId;
        //        pts.Group_Id = model.Group_Id;
        //        pts.Area_Id = model.Area_Id;
        //        pts.Address = model.Address;
        //        pts.TimeZone = model.TimeZone;
        //        pts.Longtitude = model.Longtitude;
        //        pts.Latitude = model.Latitude;
        //        checkSave = sitesService.sitesResponsitory.Update(pts);
        //        TempData["MessageStatus"] = checkSave;
        //        TempData["Message"] = $"Cập nhật điểm {(checkSave ? "" : "không")} thành công";

        //        return RedirectToAction("SitesManagement");
        //    }
        //    return View(model);
        //}
        //#endregion

        #region Create điểm
        [AuthorizeRoles]
        public ActionResult Create()
        {
            CMSHelper help = new CMSHelper();
            @ViewBag.Title = "";
            @ViewBag.MessageStatus = TempData["MessageStatus"];
            @ViewBag.Message = TempData["Message"];
            CommandModel model = new CommandModel();
            List<Site> lstSite = new List<Site>();            
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            string userName = User.Identity.Name;
            if (userName == "administrator")
            {
                lstSite = sitesService.sitesResponsitory.GetAll().ToList();                
            }
            else
            {
                int groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id.Value;
                lstSite = sitesService.GetBygroupId(groupId).ToList();                   
            }
            ViewBag.listSite = lstSite;            
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommandModel model)
        {
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            int groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id.Value;
            if (ModelState.IsValid)
            {
                Command pts = new Command();
                bool checkSave = false;                
                pts = model.ToEntity(pts);
                pts.CreateBy = CurrentUserId;
                pts.Device_Id = model.Device_Id;
                pts.GroupId = groupId;
                pts.CreateDay = DateTime.Now;
                pts.Status = null;
                checkSave = commandService.commandResponsitory.Insert(pts);
                TempData["MessageStatus"] = checkSave;
                TempData["Message"] = $"Thêm mới lệnh {(checkSave ? "" : "không")} thành công";
                return RedirectToAction("Management");
            }
            return View(model);
        }
        public ActionResult GetSiteByGroupId(int idGroup)
        {
            List<Site> lstSite = new List<Site>();
            lstSite = sitesService.GetBygroupId(idGroup).ToList();
            return PartialView("_SitePartialView", lstSite);
        }
        #endregion

    }
}
