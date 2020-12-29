using Administrator;
using ES_CapDien.AppCode;
using ES_CapDien.AppCode.Interface;
using ES_CapDien.Helpers;
using ES_CapDien.Models;
using ES_CapDien.Models.Entity;
using ES_CapDien.MongoDb.Entity;
using ES_CapDien.MongoDb.Service;
using HelperLibrary;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ES_CapDien.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Data/
        private readonly DataObservationMongoService dataObservationMongoService;
        private readonly DataAlarmMongoService dataAlarmMongoService;
        private readonly AreasService areasService;
        private readonly SitesService sitesService;
        private readonly GroupService groupService;
        private readonly ObservationService observationService;
        private readonly UserProfileService userProfileService;
        private readonly CategoryTypeSiteService categoryTypeSiteService;
        public HomeController()
        {
            dataObservationMongoService = new DataObservationMongoService();
            dataAlarmMongoService = new DataAlarmMongoService();
            areasService = new AreasService();
            sitesService = new SitesService();
            groupService = new GroupService();
            observationService = new ObservationService();
            userProfileService = new UserProfileService();
            categoryTypeSiteService = new CategoryTypeSiteService();
        }
        #region Home
        public ActionResult Index()
        {
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            string userName = WebMatrix.WebData.WebSecurity.CurrentUserName;
            int? groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id;
            if (userName == "administrator")
            {
                ViewBag.listSite = sitesService.sitesResponsitory.GetAll().ToList();
            }
            else
            {
                ViewBag.listSite = sitesService.GetBygroupId(groupId).ToList();
            }
            ViewBag.lstBieuDo = observationService.observationResponsitory.GetAll().ToList();
            HomeModel model = new HomeModel();
            List<Site> sitesInGroup = sitesService.GetBygroupId(groupId).ToList();
            model.ThietBiHoatDong = sitesInGroup.Where(i => i.IsActive == true).Count();
            model.ThietBiKhongHoatDong = sitesInGroup.Where(i => i.IsActive == false).Count();
            ViewBag.listArea = areasService.GetAreasByGroupId(groupId).ToList();
            List<CategoryTypeSite> lstCategoryTypeSites = categoryTypeSiteService.categoryTypeSiteRepository.GetAll().ToList();
            ViewBag.LstCategoryTypeSite = lstCategoryTypeSites;
            return View(model);
        }
        public ActionResult GetSite(int idArea, int? type = null)
        {
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;            
            int? groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id;
            List<Site> sites = sitesService.GetByAreaId(idArea, type, groupId).ToList();
            List<CategoryTypeSite> lstCategoryTypeSites = categoryTypeSiteService.categoryTypeSiteRepository.GetAll().ToList();
            ViewBag.LstCategoryTypeSite = lstCategoryTypeSites;
            return PartialView("_ListSitePartialView", sites);
        }
        public JsonResult GetInfoSite(int siteId)
        {
            Site site = sitesService.sitesResponsitory.Single(siteId);
            return Json(new { listData = site }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataAlarm(int siteId)
        {
            int? deviceId = sitesService.sitesResponsitory.Single(siteId).DeviceId;
            List<DataAlarmMongo> dataAlarms = new List<DataAlarmMongo>();
            if (deviceId.HasValue)
            {
                DateTime from = DateTime.Now;
                DateTime to = DateTime.Today;
                dataAlarms = dataAlarmMongoService.GetDataPaging(from, to, 0, 15, deviceId.Value, out int total).Select(i => new DataAlarmMongo
                {
                    Id = i._id,
                    AMAFR = i.AMAFR == null ? "" : i.AMAFR,
                    AMADR = i.AMADR == null ? "" : i.AMADR,
                    AMATI = i.AMATI == null ? "" : i.AMATI,
                    AMIAC = i.AMIAC == null ? "" : i.AMIAC,
                    TimeSend = i.DateCreate
                }).ToList();
            }
            return PartialView("_DataAlarmPartialView", dataAlarms);
        }
        public ActionResult GetDataObservation(int deviceId)
        {
            List<DataObservationModel> data = new List<DataObservationModel>();
            DateTime from = DateTime.Today;
            DateTime to = DateTime.Now;
            Site site = sitesService.GetByDeviceId(deviceId).FirstOrDefault();
            data = dataObservationMongoService.GetDataPagingByDeviceId(from, to, deviceId, 0, 50, out int total).Select(i => new DataObservationModel
            {
                NameSite = sitesService.sitesResponsitory.GetAll().Where(j => j.DeviceId == i.Device_Id).FirstOrDefault().Name,
                DateCreate = i.DateCreate,
                BTI = i.BTI,
                BTO = i.BTO,
                BHU = i.BHU,
                BWS = i.BWS,
                BAP = i.BAP,
                BAV = i.BAV,
                BAF = i.BAF,
                BAC = i.BAC,
                BVC = i.BVC,
                BFR = i.BFR,
            }).ToList();
            if (data.Count() == 0)
            {
                data = dataObservationMongoService.GetOffline(deviceId, 0, 50, out int total1).Select(i => new DataObservationModel
                {
                    NameSite = sitesService.sitesResponsitory.GetAll().Where(j => j.DeviceId == i.Device_Id).FirstOrDefault().Name,
                    DateCreate = i.DateCreate,
                    BTI = i.BTI,
                    BTO = i.BTO,
                    BHU = i.BHU,
                    BWS = i.BWS,
                    BAP = i.BAP,
                    BAV = i.BAV,
                    BAF = i.BAF,
                    BAC = i.BAC,
                    BVC = i.BVC,
                    BFR = i.BFR,
                }).ToList();               
            }            
                if (site.TypeSiteId == 1)
                    return PartialView("_DataTableKhiTuongPartialView", data);
                else
                    return PartialView("_DataTableThuyVanPartialView", data);

        }
        public ActionResult GetDataObservationBieuDo(int siteId, int take)
        {
            Site site = sitesService.sitesResponsitory.Single(siteId);
            int? deviceId = site.DeviceId;
            List<DataObservationModel> data = new List<DataObservationModel>();
            if (deviceId.HasValue)
            {
                site.IsActive = true;
                sitesService.sitesResponsitory.Update(site);
                DateTime from = DateTime.Today;
                DateTime to = DateTime.Now;
                data = dataObservationMongoService.GetDataPagingByDeviceId(from, to, deviceId.Value, 0, take, out int total).Select(i => new DataObservationModel
                {
                    NameSite = sitesService.sitesResponsitory.GetAll().Where(j => j.DeviceId == i.Device_Id).FirstOrDefault().Name,
                    DateCreate = i.DateCreate,
                    BTI = i.BTI,
                    BTO = i.BTO,
                    BHU = i.BHU,
                    BWS = i.BWS,
                    BAP = i.BAP,
                    BAV = i.BAV,
                    BAF = i.BAF,
                    BAC = i.BAC,
                    BVC = i.BVC,
                }).OrderBy(i => i.DateCreate).ToList();
            }
            return Json(new { listData = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDetailDataAlarm(string alerId)
        {
            ES_CapDien.MongoDb.Entity.DataAlarm dataAlarm = new ES_CapDien.MongoDb.Entity.DataAlarm();
            dataAlarm = dataAlarmMongoService.FindByKey(alerId);
            dataAlarm.AMATI = dataAlarm.AMATI == null ? "" : dataAlarm.AMATI;
            dataAlarm.AMIHU = dataAlarm.AMIHU == null ? "" : dataAlarm.AMIHU;
            dataAlarm.AMADR = dataAlarm.AMADR == null ? "" : dataAlarm.AMADR;
            dataAlarm.AMAFL = dataAlarm.AMAFL == null ? "" : dataAlarm.AMAFL;
            dataAlarm.AMAFR = dataAlarm.AMAFR == null ? "" : dataAlarm.AMAFR;
            dataAlarm.AMIPS = dataAlarm.AMIPS == null ? "" : dataAlarm.AMIPS;
            dataAlarm.AMIAL = dataAlarm.AMIAL == null ? "" : dataAlarm.AMIAL;
            dataAlarm.AMIAH = dataAlarm.AMIAH == null ? "" : dataAlarm.AMIAH;
            dataAlarm.AMIAP = dataAlarm.AMIAP == null ? "" : dataAlarm.AMIAP;
            dataAlarm.AMIAC = dataAlarm.AMIAC == null ? "" : dataAlarm.AMIAC;
            dataAlarm.AMIGN = dataAlarm.AMIGN == null ? "" : dataAlarm.AMIGN;
            dataAlarm.AMIAR = dataAlarm.AMIAR == null ? "" : dataAlarm.AMIAR;
            dataAlarm.AMIL1 = dataAlarm.AMIL1 == null ? "" : dataAlarm.AMIL1;
            dataAlarm.AMIH1 = dataAlarm.AMIH1 == null ? "" : dataAlarm.AMIH1;
            dataAlarm.AMIT1 = dataAlarm.AMIT1 == null ? "" : dataAlarm.AMIT1;
            dataAlarm.AMIT2 = dataAlarm.AMIT2 == null ? "" : dataAlarm.AMIT2;
            dataAlarm.AMIL2 = dataAlarm.AMIL2 == null ? "" : dataAlarm.AMIL2;
            dataAlarm.AMIH2 = dataAlarm.AMIH2 == null ? "" : dataAlarm.AMIH2;
            return PartialView("_DetailAlarm", dataAlarm);
        }

        #endregion

        #region Dữ liệu
        public ActionResult GetDataTable(int type)
        {
            DateTime from = DateTime.Today;
            DateTime to = DateTime.Now;
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            string userName = User.Identity.Name;
            List<Site> sites = new List<Site>();
            List<DataObservationModel> list = new List<DataObservationModel>();
            if (userName == "administrator")
            {
                sites = sitesService.sitesResponsitory.GetAll().ToList();
                foreach (var site in sites)
                {
                    if (site.DeviceId.HasValue)
                    {
                        Models.Entity.Data data = dataObservationMongoService.GetDataPagingByDeviceId(from, to, site.DeviceId.Value, 0, 1, out int total).FirstOrDefault();
                        if (data != null)
                        {
                            DataObservationModel model = data.ToModel();
                            model.NameSite = site.Name;
                            list.Add(model);
                        }
                    }

                }
            }
            else
            {
                int? groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id;
                sites = sitesService.GetBygroupId(groupId, type).ToList();
                foreach (var site in sites)
                {
                    Models.Entity.Data data = dataObservationMongoService.GetDataPagingByDeviceId(from, to, site.DeviceId.Value, 0, 1, out int total).FirstOrDefault();
                    if (data != null)
                    {
                        DataObservationModel model = data.ToModel();
                        model.NameSite = site.Name;
                        list.Add(model);
                    }
                }
            }
            if (type == 1)
                return PartialView("_DataTableKhiTuongPartialView", list);
            else
                return PartialView("_DataTableThuyVanPartialView", list);
        }
        public ActionResult QuanLyKhiTuong(int page = 1, int pageSize = 50, int type = 0, string title = "", int? areaId = null, int? siteId = null, string fromDate = "", string toDate = "")
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
            int? groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id;
            ViewBag.lstTram = sitesService.GetBygroupId(groupId, type).ToList();
            ViewBag.lstArea = areasService.GetAreasByGroupId(groupId).ToList();
            string userName = User.Identity.Name;
            int skip = (page - 1) * pageSize;
            DateTime from = DateTime.Today;
            DateTime to = DateTime.Now;
            List<CategoryTypeSite> lstCategoryTypeSites = categoryTypeSiteService.categoryTypeSiteRepository.GetAll().ToList();
            ViewBag.LstCategoryTypeSite = lstCategoryTypeSites;
            if (fromDate != "" && toDate != null)
            {
                try
                {
                    from = Convert.ToDateTime(fromDate);
                    to = from.AddDays(1);
                }
                catch { }
            }
            List<Site> sites = new List<Site>();
            List<DataObservationModel> list = new List<DataObservationModel>();
            if (userName == "administrator")
            {
                if (siteId.HasValue && siteId != 0)
                {
                    var tram = sitesService.sitesResponsitory.Single(siteId);
                    list = dataObservationMongoService.GetDataPagingByDeviceId(from, to, tram.DeviceId.Value, skip, pageSize, out int total).Select(item => new DataObservationModel
                    {
                        DateCreate = item.DateCreate,
                        BTI = item.BTI,
                        BTO = item.BTO,
                        BHU = item.BHU,
                        BWS = item.BWS,
                        BAP = item.BAP,
                        BAV = item.BAV,
                        BAF = item.BAF,
                        BAC = item.BAC,
                        BA1 = item.BA1,
                        BA2 = item.BA2,
                        BA3 = item.BA3,
                        BA4 = item.BA4,
                        BB1 = item.BB1,
                        BB2 = item.BB2,
                        BB3 = item.BB3,
                        BB4 = item.BB4,
                        BC1 = item.BC1,
                        BC2 = item.BC2,
                        BDR = item.BDR,
                        BFA = item.BFA,
                        BFD = item.BFD,
                        BFL = item.BFL,
                        BFR = item.BFR,
                        BPS = item.BPS,
                        BPW = item.BPW,
                        BSE = item.BSE,
                        BT1 = item.BT1,
                        BT2 = item.BT2,
                        BV1 = item.BV1,
                        BVC = item.BVC,
                        BV2 = item.BV2,
                        Device_Id = item.Device_Id,
                        IsSEQ = item.IsSEQ,
                        NameSite = tram.Name
                    }).ToList();
                }
                else if (areaId.HasValue && areaId != 0)
                {
                    sites = sitesService.GetByAreaId(areaId, type).ToList();
                    foreach (var site in sites)
                    {
                        Models.Entity.Data data = dataObservationMongoService.GetDataPagingByDeviceId(from, to, site.DeviceId.Value, 0, 1, out int total).FirstOrDefault();
                        if (data != null)
                        {
                            DataObservationModel model = data.ToModel();
                            model.NameSite = site.Name;
                            list.Add(model);
                        }
                    }
                }
                else
                {
                    sites = sitesService.sitesResponsitory.GetAll().ToList();
                    foreach (var site in sites)
                    {
                        if (site.DeviceId.HasValue)
                        {
                            Models.Entity.Data data = dataObservationMongoService.GetDataPagingByDeviceId(from, to, site.DeviceId.Value, 0, 1, out int total).FirstOrDefault();
                            if (data != null)
                            {
                                DataObservationModel model = data.ToModel();
                                model.NameSite = site.Name;
                                list.Add(model);
                            }
                        }
                    }
                }

            }
            else
            {
                if (siteId.HasValue && siteId != 0)
                {
                    var tram = sitesService.sitesResponsitory.Single(siteId);
                    list = dataObservationMongoService.GetDataPagingByDeviceId(from, to, tram.DeviceId.Value, skip, pageSize, out int total).Select(item => new DataObservationModel
                    {
                        DateCreate = item.DateCreate,
                        BTI = item.BTI,
                        BTO = item.BTO,
                        BHU = item.BHU,
                        BWS = item.BWS,
                        BAP = item.BAP,
                        BAV = item.BAV,
                        BAF = item.BAF,
                        BAC = item.BAC,
                        BA1 = item.BA1,
                        BA2 = item.BA2,
                        BA3 = item.BA3,
                        BA4 = item.BA4,
                        BB1 = item.BB1,
                        BB2 = item.BB2,
                        BB3 = item.BB3,
                        BB4 = item.BB4,
                        BC1 = item.BC1,
                        BC2 = item.BC2,
                        BDR = item.BDR,
                        BFA = item.BFA,
                        BFD = item.BFD,
                        BFL = item.BFL,
                        BFR = item.BFR,
                        BPS = item.BPS,
                        BPW = item.BPW,
                        BSE = item.BSE,
                        BT1 = item.BT1,
                        BT2 = item.BT2,
                        BV1 = item.BV1,
                        BVC = item.BVC,
                        BV2 = item.BV2,
                        Device_Id = item.Device_Id,
                        IsSEQ = item.IsSEQ,
                        NameSite = tram.Name
                    }).ToList();
                }
                else
                {
                    sites = sitesService.GetBygroupId(groupId, type).ToList();
                    foreach (var site in sites)
                    {
                        Models.Entity.Data data = dataObservationMongoService.GetDataPagingByDeviceId(from, to, site.DeviceId.Value, 0, 1, out int total).FirstOrDefault();
                        if (data != null)
                        {
                            DataObservationModel model = data.ToModel();
                            model.NameSite = site.Name;
                            list.Add(model);
                        }
                    }
                }

            }
            #region Hiển thị dữ liệu và phân trang
            DataObservationViewModel viewModel = new DataObservationViewModel
            {
                DataO = new StaticPagedList<DataObservationModel>(list, page, pageSize, list.Count()),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = list.Count()
                },
                From = from,
                To = to,
            };
            #endregion
            return View(viewModel);
        }
        public ActionResult QuanLyThuyVan(int page = 1, int pageSize = 50, int type = 0, string title = "", int? areaId = null, int? siteId = null, string fromDate = "", string toDate = "")
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
            int? groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id;
            ViewBag.lstTram = sitesService.GetBygroupId(groupId, type).ToList();
            ViewBag.lstArea = areasService.GetAreasByGroupId(groupId).ToList();
            string userName = User.Identity.Name;
            int skip = (page - 1) * pageSize;
            DateTime from = DateTime.Today;
            DateTime to = DateTime.Now;
            List<CategoryTypeSite> lstCategoryTypeSites = categoryTypeSiteService.categoryTypeSiteRepository.GetAll().ToList();
            ViewBag.LstCategoryTypeSite = lstCategoryTypeSites;
            if (fromDate != "" && toDate != null)
            {
                try
                {
                    from = Convert.ToDateTime(fromDate);
                    to = from.AddDays(1);
                }
                catch { }
            }
            List<Site> sites = new List<Site>();
            List<DataObservationModel> list = new List<DataObservationModel>();
            if (userName == "administrator")
            {
                if (siteId.HasValue && siteId != 0)
                {
                    var tram = sitesService.sitesResponsitory.Single(siteId);
                    list = dataObservationMongoService.GetDataPagingByDeviceId(from, to, tram.DeviceId.Value, skip, pageSize, out int total).Select(item => new DataObservationModel
                    {
                        DateCreate = item.DateCreate,
                        BTI = item.BTI,
                        BTO = item.BTO,
                        BHU = item.BHU,
                        BWS = item.BWS,
                        BAP = item.BAP,
                        BAV = item.BAV,
                        BAF = item.BAF,
                        BAC = item.BAC,
                        BA1 = item.BA1,
                        BA2 = item.BA2,
                        BA3 = item.BA3,
                        BA4 = item.BA4,
                        BB1 = item.BB1,
                        BB2 = item.BB2,
                        BB3 = item.BB3,
                        BB4 = item.BB4,
                        BC1 = item.BC1,
                        BC2 = item.BC2,
                        BDR = item.BDR,
                        BFA = item.BFA,
                        BFD = item.BFD,
                        BFL = item.BFL,
                        BFR = item.BFR,
                        BPS = item.BPS,
                        BPW = item.BPW,
                        BSE = item.BSE,
                        BT1 = item.BT1,
                        BT2 = item.BT2,
                        BV1 = item.BV1,
                        BVC = item.BVC,
                        BV2 = item.BV2,
                        Device_Id = item.Device_Id,
                        IsSEQ = item.IsSEQ,
                        NameSite = tram.Name
                    }).ToList();
                }
                else if (areaId.HasValue && areaId != 0)
                {
                    sites = sitesService.GetByAreaId(areaId, type).ToList();
                    foreach (var site in sites)
                    {
                        Models.Entity.Data data = dataObservationMongoService.GetDataPagingByDeviceId(from, to, site.DeviceId.Value, 0, 1, out int total).FirstOrDefault();
                        if (data != null)
                        {
                            DataObservationModel model = data.ToModel();
                            model.NameSite = site.Name;
                            list.Add(model);
                        }
                    }
                }
                else
                {
                    sites = sitesService.sitesResponsitory.GetAll().ToList();
                    foreach (var site in sites)
                    {
                        if (site.DeviceId.HasValue)
                        {
                            Models.Entity.Data data = dataObservationMongoService.GetDataPagingByDeviceId(from, to, site.DeviceId.Value, 0, 1, out int total).FirstOrDefault();
                            if (data != null)
                            {
                                DataObservationModel model = data.ToModel();
                                model.NameSite = site.Name;
                                list.Add(model);
                            }
                        }
                    }
                }

            }
            else
            {
                if (siteId.HasValue && siteId != 0)
                {
                    var tram = sitesService.sitesResponsitory.Single(siteId);
                    list = dataObservationMongoService.GetDataPagingByDeviceId(from, to, tram.DeviceId.Value, skip, pageSize, out int total).Select(item => new DataObservationModel
                    {
                        DateCreate = item.DateCreate,
                        BTI = item.BTI,
                        BTO = item.BTO,
                        BHU = item.BHU,
                        BWS = item.BWS,
                        BAP = item.BAP,
                        BAV = item.BAV,
                        BAF = item.BAF,
                        BAC = item.BAC,
                        BA1 = item.BA1,
                        BA2 = item.BA2,
                        BA3 = item.BA3,
                        BA4 = item.BA4,
                        BB1 = item.BB1,
                        BB2 = item.BB2,
                        BB3 = item.BB3,
                        BB4 = item.BB4,
                        BC1 = item.BC1,
                        BC2 = item.BC2,
                        BDR = item.BDR,
                        BFA = item.BFA,
                        BFD = item.BFD,
                        BFL = item.BFL,
                        BFR = item.BFR,
                        BPS = item.BPS,
                        BPW = item.BPW,
                        BSE = item.BSE,
                        BT1 = item.BT1,
                        BT2 = item.BT2,
                        BV1 = item.BV1,
                        BVC = item.BVC,
                        BV2 = item.BV2,
                        Device_Id = item.Device_Id,
                        IsSEQ = item.IsSEQ,
                        NameSite = tram.Name
                    }).ToList();
                }
                else
                {
                    sites = sitesService.GetBygroupId(groupId, type).ToList();
                    foreach (var site in sites)
                    {
                        Models.Entity.Data data = dataObservationMongoService.GetDataPagingByDeviceId(from, to, site.DeviceId.Value, 0, 1, out int total).FirstOrDefault();
                        if (data != null)
                        {
                            DataObservationModel model = data.ToModel();
                            model.NameSite = site.Name;
                            list.Add(model);
                        }
                    }
                }

            }
            #region Hiển thị dữ liệu và phân trang
            DataObservationViewModel viewModel = new DataObservationViewModel
            {
                DataO = new StaticPagedList<DataObservationModel>(list, page, pageSize, list.Count()),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = list.Count()
                },
                From = from,
                To = to,
            };
            #endregion
            return View(viewModel);
        }
        #endregion


    }
}
