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
    public class ObservationalController : Controller
    {
        //
        // GET: /Observational/
        private readonly ObservationService observationService;
        private readonly UserProfileService userProfileService;
        public ObservationalController()
        {
            observationService = new ObservationService();
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
        public ActionResult ElementManagement(int page = 1, int pageSize = 50, string title = "")
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
            List<ObservationsModel> list = new List<ObservationsModel>();
            list = observationService.GetAll(skip, pageSize, out int totalRow, title).AsEnumerable().Select(item => new ObservationsModel
            {
                Id = item.Id,
                Name = item.Name,
                Noti_Alarm = item.Noti_Alarm,
                Code = item.Code,
                CreateDay = item.CreateDay,
                UpdateDay = item.UpdateDay,
                Low_Value = item.Low_Value,
                Hight_Value = item.Code,
                IsBieuDo = item.IsBieuDo,
            }).ToList();
            totalRows = totalRow;
            #region Lấy dữ liệu

            #endregion

            #region Hiển thị dữ liệu và phân trang
            ObservationsViewModel viewModel = new ObservationsViewModel
            {
                Observations = new StaticPagedList<ObservationsModel>(list, page, pageSize, totalRows),
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
            Observation pts = observationService.observationResponsitory.Single(Id);
            bool checkDelete = false;
            if (pts != null)
            {
                checkDelete = observationService.observationResponsitory.Delete(pts);
            }
            TempData["MessageStatus"] = checkDelete;
            TempData["Message"] = $"Xóa trạm {(checkDelete ? "" : "không")} thành công";
            return RedirectToAction("ElementManagement", new { page = Request.Params["page"], pageSize = Request.Params["pageSize"] });
        }

        [HttpPost]
        public ActionResult DeleteSelected(List<int> ids)
        {
            bool checkDelete = false;
            foreach (var i in ids)
            {
                Observation pts = observationService.observationResponsitory.Single(i);
                if (pts != null)
                {
                    checkDelete = observationService.observationResponsitory.Delete(pts);
                }
            }
            TempData["MessageStatus"] = checkDelete;
            TempData["Message"] = $"Xóa trạm {(checkDelete ? "" : "không")} thành công";
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

            Observation pts = observationService.observationResponsitory.Single(id);
            if (pts == null)
            {
                return RedirectToAction("ElementManagement");
            }

            ObservationsModel model = pts.ToModel();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ObservationsModel model)
        {
            if (ModelState.IsValid)
            {
                Observation pts = observationService.observationResponsitory.Single(model.Id);
                if (pts == null)
                {
                    return RedirectToAction("ElementManagement");
                }
                bool checkSave = false;
                pts = model.ToEntity(pts);
                checkSave = observationService.observationResponsitory.Update(pts);
                TempData["MessageStatus"] = checkSave;
                TempData["Message"] = $"Cập nhật trạm {(checkSave ? "" : "không")} thành công";

                return RedirectToAction("ElementManagement");
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
            ObservationsModel model = new ObservationsModel();
            List<Observation> groups = new List<Observation>();
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            string userName = User.Identity.Name;
            groups = observationService.observationResponsitory.GetAll().ToList();
            ViewBag.listGroup = groups;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ObservationsModel model)
        {
            if (ModelState.IsValid)
            {
                Observation pts = new Observation();
                bool checkSave = false;
                pts.CreateDay = DateTime.Now;
                int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;              
                pts = model.ToEntity(pts);
                checkSave = observationService.observationResponsitory.Insert(pts);
                TempData["MessageStatus"] = checkSave;
                TempData["Message"] = $"Thêm mới khu vực {(checkSave ? "" : "không")} thành công";
                return RedirectToAction("AreasManagement");
            }
            return View(model);
        }
        #endregion

    }
}
