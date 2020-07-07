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
    public class AreasController : BaseController
    {
        //
        // GET: /Point/
        private readonly AreasService areasService;
        private readonly GroupService groupService;
        private readonly UserProfileService userProfileService;
        public AreasController()
        {
            areasService = new AreasService();
            groupService = new GroupService();
            userProfileService = new UserProfileService();
        }

        /// <summary>
        /// Hiển thị dữ liệu
        /// </summary>
        /// <param name="page">trang hiện tại</param>
        /// <param name="pageSize">tổng số trang</param>
        /// <param name="title">title trang web</param>
        /// <returns></returns>
        [AuthorizeRoles]
        public ActionResult AreasManagement(int page = 1, int pageSize = 50, string title = "")
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
            List<AreaModel> list = new List<AreaModel>();
            if (userName=="administrator")
            {
                list = areasService.GetAll(skip, pageSize, out int totalRow, title).AsEnumerable().Select(item => new AreaModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Latitude = item.Latitude,
                    Longtitude = item.Longtitude,
                    CreateDay = item.CreateDay,
                    NguoiTao = userProfileService.userProfileResponsitory.Single(item.CreateBy).FullName,
                    IsActive = item.IsActive,
                    GroupsName = groupService.groupResponsitory.Single(item.Group_Id).Name
                }).ToList();
                totalRows = totalRow;
            }
            else
            {
                int groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id.Value;
                list = areasService.GetAll(skip, pageSize, out int totalRow, title, groupId).AsEnumerable().Select(item => new AreaModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Latitude = item.Latitude,
                    Longtitude = item.Longtitude,
                    CreateDay = item.CreateDay,
                    NguoiTao = userProfileService.userProfileResponsitory.Single(item.CreateBy).FullName,
                    IsActive = item.IsActive,
                    GroupsName = groupService.groupResponsitory.Single(item.Group_Id).Name
                }).ToList();
                totalRows = totalRow;
            }
           

            #region Lấy dữ liệu
            
            #endregion

            #region Hiển thị dữ liệu và phân trang
            AreaViewModel viewModel = new AreaViewModel
            {
                Areas = new StaticPagedList<AreaModel>(list, page, pageSize, totalRows),
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
            Area pts = areasService.areaResponsitory.Single(Id);
            bool checkDelete = false;
            if (pts != null)
            {
                checkDelete = areasService.areaResponsitory.Delete(pts);
            }
            TempData["MessageStatus"] = checkDelete;
            TempData["Message"] = $"Xóa khu vực {(checkDelete ? "" : "không")} thành công";
            return RedirectToAction("AreasManagement", new { page = Request.Params["page"], pageSize = Request.Params["pageSize"] });
        }

        [HttpPost]
        public ActionResult DeleteSelected(List<int> ids)
        {
            bool checkDelete = false;
            foreach (var i in ids)
            {
                Area pts = areasService.areaResponsitory.Single(i);
                if (pts != null)
                {
                    checkDelete = areasService.areaResponsitory.Delete(pts);
                }
            }
            TempData["MessageStatus"] = checkDelete;
            TempData["Message"] = $"Xóa khu vực {(checkDelete ? "" : "không")} thành công";
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

            Area pts = areasService.areaResponsitory.Single(id);
            if (pts == null)
            {
                return RedirectToAction("AreasManagement");
            }

            AreaModel model = pts.ToModel();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Update(AreaModel model)
        {
            if (ModelState.IsValid)
            {
                Area pts = areasService.areaResponsitory.Single(model.Id);
                if (pts == null)
                {
                    return RedirectToAction("AreasManagement");
                }
                bool checkSave = false;   
                pts = model.ToEntity(pts);
                checkSave = areasService.areaResponsitory.Update(pts);
                TempData["MessageStatus"] = checkSave;
                TempData["Message"] = $"Cập nhật khu vực {(checkSave ? "" : "không")} thành công";

                return RedirectToAction("AreasManagement");
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
            AreaModel model = new AreaModel();
            List<Group> groups = new List<Group>();
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            string userName = User.Identity.Name;
            if (userName == "administrator")
            {
                groups = groupService.groupResponsitory.GetAll().ToList();
            }
            else
            {
                int groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id.Value;
                Group group = groupService.groupResponsitory.Single(groupId);
                groups.Add(group);
            }
            ViewBag.listGroup = groups;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AreaModel model)
        {
            if (ModelState.IsValid)
            {
                Area pts = new Area();
                bool checkSave = false;
                pts.CreateDay = DateTime.Now;
                int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
                pts.CreateBy = CurrentUserId;
                pts = model.ToEntity(pts);
                checkSave = areasService.areaResponsitory.Insert(pts);
                TempData["MessageStatus"] = checkSave;
                TempData["Message"] = $"Thêm mới khu vực {(checkSave ? "" : "không")} thành công";
                return RedirectToAction("AreasManagement");
            }
            return View(model);
        }
        #endregion
    }
}
