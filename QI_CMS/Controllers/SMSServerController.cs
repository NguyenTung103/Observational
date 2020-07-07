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
    public class SMSServerController : Controller
    {
        //
        // GET: /SMSServer/
        private readonly RegisterSMSService registerSMSService;
        private readonly SitesService sitesService;
        private readonly GroupService groupService;
        private readonly UserProfileService userProfileService;
        private readonly SMSServerService sMSServerService;
        private readonly ObservationService observationService;
        public SMSServerController()
        {
            registerSMSService = new RegisterSMSService();
            sitesService = new SitesService();
            groupService = new GroupService();
            userProfileService = new UserProfileService();
            sMSServerService = new SMSServerService();
            observationService = new ObservationService();
        }
        public ActionResult Index(int page = 1, int pageSize = 50, string title = "", int? areaId = null)
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
            List<RegisterSMSModel> list = new List<RegisterSMSModel>();

            #region Lấy dữ liệu
            if (userName == "administrator")
            {
                list = registerSMSService.GetAll(skip, pageSize, out int totalRow, title, null, areaId).AsEnumerable().Select(item => new RegisterSMSModel
                {
                    Id = item.Id,
                    DanhSachSDT = item.DanhSachSDT,
                    CodeObservation = item.CodeObservation,
                    DateCreate = item.DateCreate,
                    SMSServer = sMSServerService.smsServerResponsitory.Single(item.SMSServerId).AddressIP,
                    IsAll = item.IsAll,
                    NameSite = sitesService.sitesResponsitory.GetAll().Where(i=>i.DeviceId==item.DeviceId).FirstOrDefault().Name,                    
                    GroupName = groupService.groupResponsitory.Single(item.GroupId).Name,                    
                }).ToList();
                totalRows = totalRow;
            }
            else
            {
                int groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id.Value;
                list = registerSMSService.GetAll(skip, pageSize, out int totalRow, title, groupId, areaId).AsEnumerable().Select(item => new RegisterSMSModel
                {
                    Id = item.Id,
                    DanhSachSDT = item.DanhSachSDT,
                    CodeObservation = item.CodeObservation,
                    DateCreate = item.DateCreate,
                    SMSServer = sMSServerService.smsServerResponsitory.Single(item.SMSServerId).AddressIP,
                    IsAll = item.IsAll,
                    NameSite = sitesService.sitesResponsitory.GetAll().Where(i => i.DeviceId == item.DeviceId).FirstOrDefault().Name,
                    GroupName = groupService.groupResponsitory.Single(item.GroupId).Name,
                }).ToList();
                totalRows = totalRow;
            }
            #endregion

            #region Hiển thị dữ liệu và phân trang
            RegisterSMSViewModel viewModel = new RegisterSMSViewModel
            {
                RG = new StaticPagedList<RegisterSMSModel>(list, page, pageSize, totalRows),
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
        #region Xóa đăng ký
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id)
        {
            RegisterSMS pts = registerSMSService.registerResponsitory.Single(Id);
            bool checkDelete = false;
            if (pts != null)
            {
                checkDelete = registerSMSService.registerResponsitory.Delete(pts);
            }
            TempData["MessageStatus"] = checkDelete;
            TempData["Message"] = $"Xóa đăng ký nhận tin {(checkDelete ? "" : "không")} thành công";
            return RedirectToAction("Index", new { page = Request.Params["page"], pageSize = Request.Params["pageSize"], areaId = Request.Params["areaId"] });
        }

        [HttpPost]
        public ActionResult DeleteSelected(List<int> ids)
        {
            bool checkDelete = false;
            foreach (var i in ids)
            {
                RegisterSMS pts = registerSMSService.registerResponsitory.Single(i);
                if (pts != null)
                {
                    checkDelete = registerSMSService.registerResponsitory.Delete(pts);
                }
            }
            TempData["MessageStatus"] = checkDelete;
            TempData["Message"] = $"Xóa đăng ký nhận tin {(checkDelete ? "" : "không")} thành công";
            return Json(new { Result = checkDelete });
        }
        #endregion

        #region update đăng ký
        [AuthorizeRoles]
        public ActionResult Update(int id = 0)
        {
            CMSHelper help = new CMSHelper();
            @ViewBag.Title = "";
            @ViewBag.MessageStatus = TempData["MessageStatus"];
            @ViewBag.Message = TempData["Message"];
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            string userName = User.Identity.Name;
            List<Site> sites = new List<Site>();
            List<Group> groups = new List<Group>();
            if (userName == "administrator")
            {
                groups = groupService.groupResponsitory.GetAll().ToList();
                sites = sitesService.sitesResponsitory.GetAll().ToList();
            }
            else
            {
                int groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id.Value;
                Group group = groupService.groupResponsitory.Single(groupId);
                sites = sitesService.GetBygroupId(groupId).ToList();
                groups.Add(group);
            }
            RegisterSMS pts = registerSMSService.registerResponsitory.Single(id);
            if (pts == null)
            {
                return RedirectToAction("index");
            }
            ViewBag.listGroup = groups;
            ViewBag.listObservation = observationService.observationResponsitory.GetAll();
            ViewBag.listSite = sites;
            ViewBag.listSmsServer = sMSServerService.smsServerResponsitory.GetAll();
            RegisterSMSModel model = pts.ToModel();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Update(RegisterSMSModel model, string[] CodeObservation)
        {
            if (ModelState.IsValid)
            {
                int i = 1;
                string listCode = "";
                foreach (var item in CodeObservation)
                {
                    if (i == 1)
                    {
                        listCode = listCode + item.Trim();
                    }
                    else
                    {
                        listCode = listCode + "," + item.Trim();
                    }
                    i++;

                }
                RegisterSMS pts = registerSMSService.registerResponsitory.Single(model.Id);
                if (pts == null)
                {
                    return RedirectToAction("index");
                }
                bool checkSave = false;
                pts = model.ToEntity(pts);
                pts.CodeObservation = listCode;
                checkSave = registerSMSService.registerResponsitory.Update(pts);
                TempData["MessageStatus"] = checkSave;
                TempData["Message"] = $"Cập nhật đăng ký nhận tin {(checkSave ? "" : "không")} thành công";

                return RedirectToAction("Index");
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
            RegisterSMSModel model = new RegisterSMSModel();
            List<Group> groups = new List<Group>();
            List<Site> sites = new List<Site>();
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            string userName = User.Identity.Name;
            if (userName == "administrator")
            {
                groups = groupService.groupResponsitory.GetAll().ToList();
                sites = sitesService.sitesResponsitory.GetAll().ToList();
            }
            else
            {
                int groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id.Value;
                Group group = groupService.groupResponsitory.Single(groupId);
                sites = sitesService.GetBygroupId(groupId).ToList();
                groups.Add(group);                
            }
            
            ViewBag.listGroup = groups;
            ViewBag.listObservation = observationService.observationResponsitory.GetAll();
            ViewBag.listSite = sites;
            ViewBag.listSmsServer = sMSServerService.smsServerResponsitory.GetAll() ;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterSMSModel model, string[] CodeObservation)
        {
            string listCode = "";
            int i = 1;
            foreach(var item in CodeObservation)
            {
                if(i==1)
                {
                    listCode = listCode + item.Trim();
                }
                else
                {
                    listCode = listCode + ","+ item.Trim();
                }
                i++;
                
            }
            if (ModelState.IsValid)
            {
                RegisterSMS pts = new RegisterSMS();
                bool checkSave = false;
                int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
                pts = model.ToEntity(pts);
                pts.CodeObservation = listCode;
                pts.DateCreate = DateTime.Now;
                checkSave = registerSMSService.registerResponsitory.Insert(pts);
                TempData["MessageStatus"] = checkSave;
                TempData["Message"] = $"Thêm mới đăng ký nhận tin {(checkSave ? "" : "không")} thành công";
                return RedirectToAction("index");
            }
            return View(model);
        }       
        #endregion
    }
}
