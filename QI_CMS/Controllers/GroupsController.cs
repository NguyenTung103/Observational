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
    public class GroupsController : BaseController
    {
        //
        // GET: /Point/
        private readonly GroupService groupService;
        private readonly UserProfileService userProfileService;
        public GroupsController()
        {
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
        public ActionResult GroupsManagement(int page = 1, int pageSize = 50, string title = "")
        {
            ViewBag.Title = "";
            ViewBag.MessageStatus = TempData["MessageStatus"];
            ViewBag.Message = TempData["Message"];
            if (pageSize == 1)
            {
                pageSize = CMSHelper.pageSizes[0];
            }
            @ViewBag.PageSizes = CMSHelper.pageSizes;
            string UserName = User.Identity.Name;
            int skip = (page - 1) * pageSize;

            #region Lấy dữ liệu
            List<GroupModel> list = groupService.GetAll(skip, pageSize, out int totalRow, title).AsEnumerable().Select(item => new GroupModel
            {
                Id = item.Id,
                Name = item.Name,
                Contact = item.Contact,
                CreateDay = item.CreateDay,
                NguoiTao = userProfileService.userProfileResponsitory.Single(item.CreateBy).FullName,
                Email=item.Email
            }).ToList();
            #endregion

            #region Hiển thị dữ liệu và phân trang
            GroupsViewModel viewModel = new GroupsViewModel
            {
                Groups = new StaticPagedList<GroupModel>(list, page, pageSize, totalRow),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = totalRow
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
            Group pts = groupService.groupResponsitory.Single(Id);
            bool checkDelete = false;
            if (pts != null)
            {
                checkDelete = groupService.groupResponsitory.Delete(pts);
            }
            TempData["MessageStatus"] = checkDelete;
            TempData["Message"] = $"Xóa nhóm {(checkDelete ? "" : "không")} thành công";
            return RedirectToAction("GroupsManagement", new { page = Request.Params["page"], pageSize = Request.Params["pageSize"] });
        }

        [HttpPost]
        public ActionResult DeleteSelected(List<int> ids)
        {
            bool checkDelete = false;
            foreach (var i in ids)
            {
                Group dp = groupService.groupResponsitory.Single(i);
                if (dp != null)
                {
                    checkDelete = groupService.groupResponsitory.Delete(dp);
                }
            }
            TempData["MessageStatus"] = checkDelete;
            TempData["Message"] = $"Xóa nhóm {(checkDelete ? "" : "không")} thành công";
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

            Group pts = groupService.groupResponsitory.Single(id);
            if (pts == null)
            {
                return RedirectToAction("GroupsManagement");
            }

            GroupModel model = pts.ToModel();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GroupModel model)
        {
            if (ModelState.IsValid)
            {
                Group pts = groupService.groupResponsitory.Single(model.Id);
                if (pts == null)
                {
                    return RedirectToAction("GroupsManagement");
                }
                bool checkSave = false;   
                pts = model.ToEntity(pts);
                checkSave = groupService.groupResponsitory.Update(pts);
                TempData["MessageStatus"] = checkSave;
                TempData["Message"] = $"Cập nhật nhóm {(checkSave ? "" : "không")} thành công";

                return RedirectToAction("GroupsManagement");
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
            GroupModel model = new GroupModel();
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GroupModel model)
        {
            if (ModelState.IsValid)
            {
                Group pts = new Group();
                bool checkSave = false;
                pts.CreateDay = DateTime.Now;
                int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
                pts.CreateBy = CurrentUserId;
                pts = model.ToEntity(pts);
                checkSave = groupService.groupResponsitory.Insert(pts);
                TempData["MessageStatus"] = checkSave;
                TempData["Message"] = $"Thêm mới nhóm {(checkSave ? "" : "không")} thành công";
                return RedirectToAction("GroupsManagement");
            }
            return View(model);
        }
        #endregion



    }
}
