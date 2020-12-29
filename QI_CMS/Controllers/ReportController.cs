using ES_CapDien.AppCode;
using ES_CapDien.Helpers;
using ES_CapDien.Models;
using ES_CapDien.MongoDb.Service;
using HelperLibrary;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ES_CapDien.Controllers
{
    public class ReportController : Controller
    {
        Cache_BO cacheBO = new Cache_BO();
        private readonly DataObservationMongoService dataObservationMongoService;
        private readonly DataAlarmMongoService dataAlarmMongoService;
        private readonly AreasService areasService;
        private readonly SitesService sitesService;
        private readonly GroupService groupService;
        private readonly ObservationService observationService;
        private readonly UserProfileService userProfileService;
        private readonly ReportTypeService reportTypeService;
        private readonly ReportDailyNhietDoService reportDailyNhietDoService;
        private readonly ReportDailyMucNuocService reportDailyMucNuocService;
        private readonly ReportDailyLuongMuaService reportDailyLuongMuaService;
        private readonly ReportDailyApSuatService reportDailyApSuatService;
        private readonly ReportDailyLuuLuongDongChayService reportDailyLuuLuongDongChayService;
        private readonly ReportDailyTocDoDongChayService reportDailyTocDoDongChayService;
        private readonly ReportDailyDoAmService reportDailyDoAmService;
        private readonly ReportDailyTocDoGioService reportDailyTocDoGioService;
        private readonly ReportDailyHuongGioService reportDailyHuongGioService;

        public ReportController()
        {
            dataObservationMongoService = new DataObservationMongoService();
            dataAlarmMongoService = new DataAlarmMongoService();
            areasService = new AreasService();
            sitesService = new SitesService();
            groupService = new GroupService();
            observationService = new ObservationService();
            userProfileService = new UserProfileService();
            reportTypeService = new ReportTypeService();
            reportDailyNhietDoService = new ReportDailyNhietDoService();
            reportDailyMucNuocService = new ReportDailyMucNuocService();
            reportDailyLuongMuaService = new ReportDailyLuongMuaService();
            reportDailyApSuatService = new ReportDailyApSuatService();
            reportDailyDoAmService = new ReportDailyDoAmService();
            reportDailyTocDoGioService = new ReportDailyTocDoGioService();
            reportDailyLuuLuongDongChayService = new ReportDailyLuuLuongDongChayService();
            reportDailyTocDoDongChayService = new ReportDailyTocDoDongChayService();
            reportDailyHuongGioService = new ReportDailyHuongGioService();
        }
        /// <summary>
        /// Báo cáo thường ngày
        /// </summary>
        /// <param name="day"></param>
        /// <param name="areaId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public ActionResult Daily(string Date = "", int? areaId = null, int? deviceId = null, int? groupId = null)
        {
            ViewBag.Title = "";
            ViewBag.MessageStatus = TempData["MessageStatus"];
            ViewBag.Message = TempData["Message"];
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            string userName = User.Identity.Name;           
            groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id;
            if (userName == "administrator")
            {
                ViewBag.listSite = sitesService.sitesResponsitory.GetAll().ToList();
                ViewBag.listGroups = groupService.groupResponsitory.GetAll().ToList();
                ViewBag.listArea = areasService.areaResponsitory.GetAll().ToList();
            }
            else
            {
                ViewBag.listGroups = groupService.GetGroups(groupId).ToList();
                ViewBag.listArea = areasService.GetAreasByGroupId(groupId).ToList();
                ViewBag.listSite = sitesService.GetBygroupId(groupId).ToList();
            }
            DateTime date = DateTime.Today;
            if (Date != "")
            {
                try
                {
                    date = Convert.ToDateTime(Date);
                }
                catch { }
            }
            if(!deviceId.HasValue)
            {
                deviceId = sitesService.GetBygroupId(groupId).FirstOrDefault().DeviceId;
            }    
            ReportType rp0 = reportTypeService.GetByCode("RP0").FirstOrDefault();
            ReportType rp1 = reportTypeService.GetByCode("RP1").FirstOrDefault();
            ReportType rp2 = reportTypeService.GetByCode("RP2").FirstOrDefault();
            ReportType rp3 = reportTypeService.GetByCode("RP3").FirstOrDefault();
            ReportType rp4 = reportTypeService.GetByCode("RP4").FirstOrDefault();
            ReportType rp5 = reportTypeService.GetByCode("RP5").FirstOrDefault();
            ReportType rp6 = reportTypeService.GetByCode("RP6").FirstOrDefault();
            ReportType rp7 = reportTypeService.GetByCode("RP6").FirstOrDefault();
            ReportType rp8 = reportTypeService.GetByCode("RP8").FirstOrDefault();
            Report_NhietDo_DoAm_ApSuat_DongChay_DailyModel nhietDo = reportDailyNhietDoService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new Report_NhietDo_DoAm_ApSuat_DongChay_DailyModel
            {
                Distance1 = s.Distance1,
                Distance2 = s.Distance2,
                Distance3 = s.Distance3,
                Distance4 = s.Distance4,
                Distance5 = s.Distance5,
                Distance6 = s.Distance6,
                Distance7 = s.Distance7,
                Distance8 = s.Distance8,
                Distance9 = s.Distance9,
                Distance10 = s.Distance10,
                Distance11 = s.Distance11,
                Distance12 = s.Distance12,
                Distance13 = s.Distance13,
                Distance14 = s.Distance14,
                Distance15 = s.Distance15,
                Distance16 = s.Distance16,
                Distance17 = s.Distance17,
                Distance18 = s.Distance18,
                Distance19 = s.Distance19,
                Distance20 = s.Distance20,
                Distance21 = s.Distance21,
                Distance22 = s.Distance22,
                Distance23 = s.Distance23,
                Distance24 = s.Distance24,
                MinValue = s.MinValue,
                MaxValue = s.MaxValue,
                TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                Measure = rp0.Measure,
                ReportTypeCode = s.ReportTypeCode.Trim(),
                ReportTypeName = rp0.Description,
                Title = rp0.Title
            }).FirstOrDefault();
            Report_NhietDo_DoAm_ApSuat_DongChay_DailyModel doAm = reportDailyDoAmService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new Report_NhietDo_DoAm_ApSuat_DongChay_DailyModel
            {
                Distance1 = s.Distance1,
                Distance2 = s.Distance2,
                Distance3 = s.Distance3,
                Distance4 = s.Distance4,
                Distance5 = s.Distance5,
                Distance6 = s.Distance6,
                Distance7 = s.Distance7,
                Distance8 = s.Distance8,
                Distance9 = s.Distance9,
                Distance10 = s.Distance10,
                Distance11 = s.Distance11,
                Distance12 = s.Distance12,
                Distance13 = s.Distance13,
                Distance14 = s.Distance14,
                Distance15 = s.Distance15,
                Distance16 = s.Distance16,
                Distance17 = s.Distance17,
                Distance18 = s.Distance18,
                Distance19 = s.Distance19,
                Distance20 = s.Distance20,
                Distance21 = s.Distance21,
                Distance22 = s.Distance22,
                Distance23 = s.Distance23,
                Distance24 = s.Distance24,
                MinValue = s.MinValue,
                MaxValue = s.MaxValue,
                TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                Measure = rp1.Measure,
                ReportTypeCode = s.ReportTypeCode.Trim(),
                ReportTypeName = rp1.Description,
                Title = rp1.Title
            }).FirstOrDefault();
            Report_NhietDo_DoAm_ApSuat_DongChay_DailyModel apSuat = reportDailyApSuatService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new Report_NhietDo_DoAm_ApSuat_DongChay_DailyModel
            {
                Distance1 = s.Distance1,
                Distance2 = s.Distance2,
                Distance3 = s.Distance3,
                Distance4 = s.Distance4,
                Distance5 = s.Distance5,
                Distance6 = s.Distance6,
                Distance7 = s.Distance7,
                Distance8 = s.Distance8,
                Distance9 = s.Distance9,
                Distance10 = s.Distance10,
                Distance11 = s.Distance11,
                Distance12 = s.Distance12,
                Distance13 = s.Distance13,
                Distance14 = s.Distance14,
                Distance15 = s.Distance15,
                Distance16 = s.Distance16,
                Distance17 = s.Distance17,
                Distance18 = s.Distance18,
                Distance19 = s.Distance19,
                Distance20 = s.Distance20,
                Distance21 = s.Distance21,
                Distance22 = s.Distance22,
                Distance23 = s.Distance23,
                Distance24 = s.Distance24,
                MinValue = s.MinValue,
                MaxValue = s.MaxValue,
                Measure = rp2.Measure,
                TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                Title = rp2.Title,
                ReportTypeCode = s.ReportTypeCode.Trim(),
                ReportTypeName = rp2.Description,
            }).FirstOrDefault();
            Report_NhietDo_DoAm_ApSuat_DongChay_DailyModel tocDoDongChay = reportDailyTocDoDongChayService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new Report_NhietDo_DoAm_ApSuat_DongChay_DailyModel
            {
                Distance1 = s.Distance1,
                Distance2 = s.Distance2,
                Distance3 = s.Distance3,
                Distance4 = s.Distance4,
                Distance5 = s.Distance5,
                Distance6 = s.Distance6,
                Distance7 = s.Distance7,
                Distance8 = s.Distance8,
                Distance9 = s.Distance9,
                Distance10 = s.Distance10,
                Distance11 = s.Distance11,
                Distance12 = s.Distance12,
                Distance13 = s.Distance13,
                Distance14 = s.Distance14,
                Distance15 = s.Distance15,
                Distance16 = s.Distance16,
                Distance17 = s.Distance17,
                Distance18 = s.Distance18,
                Distance19 = s.Distance19,
                Distance20 = s.Distance20,
                Distance21 = s.Distance21,
                Distance22 = s.Distance22,
                Distance23 = s.Distance23,
                Distance24 = s.Distance24,
                MinValue = s.MinValue,
                MaxValue = s.MaxValue,
                TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                Measure = rp1.Measure,
                ReportTypeCode = s.ReportTypeCode.Trim(),
                ReportTypeName = rp1.Description,
                Title = rp1.Title
            }).FirstOrDefault();
            Report_NhietDo_DoAm_ApSuat_DongChay_DailyModel luuLuongDongChay = reportDailyLuuLuongDongChayService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new Report_NhietDo_DoAm_ApSuat_DongChay_DailyModel
            {
                Distance1 = s.Distance1,
                Distance2 = s.Distance2,
                Distance3 = s.Distance3,
                Distance4 = s.Distance4,
                Distance5 = s.Distance5,
                Distance6 = s.Distance6,
                Distance7 = s.Distance7,
                Distance8 = s.Distance8,
                Distance9 = s.Distance9,
                Distance10 = s.Distance10,
                Distance11 = s.Distance11,
                Distance12 = s.Distance12,
                Distance13 = s.Distance13,
                Distance14 = s.Distance14,
                Distance15 = s.Distance15,
                Distance16 = s.Distance16,
                Distance17 = s.Distance17,
                Distance18 = s.Distance18,
                Distance19 = s.Distance19,
                Distance20 = s.Distance20,
                Distance21 = s.Distance21,
                Distance22 = s.Distance22,
                Distance23 = s.Distance23,
                Distance24 = s.Distance24,
                MinValue = s.MinValue,
                MaxValue = s.MaxValue,
                Measure = rp2.Measure,
                TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                Title = rp2.Title,
                ReportTypeCode = s.ReportTypeCode.Trim(),
                ReportTypeName = rp2.Description,
            }).FirstOrDefault();
            double minValueApSuat = GetMinValue(apSuat);
            ViewBag.minValueApSuat = minValueApSuat;
            ViewBag.minValueNhietDo = GetMinValue(nhietDo);
            ViewBag.minValueDoAm = GetMinValue(doAm);
            ViewBag.minTocDoDongChay = GetMinValue(tocDoDongChay);
            ViewBag.minLuuLuongDongChay = GetMinValue(luuLuongDongChay);
            Report_MucNuoc_DailyModel mucNuoc = reportDailyMucNuocService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new Report_MucNuoc_DailyModel
            {
                Distance1 = s.Distance1,
                Distance2 = s.Distance2,
                Distance3 = s.Distance3,
                Distance4 = s.Distance4,
                Distance5 = s.Distance5,
                Distance6 = s.Distance6,
                Distance7 = s.Distance7,
                Distance8 = s.Distance8,
                Distance9 = s.Distance9,
                Distance10 = s.Distance10,
                Distance11 = s.Distance11,
                Distance12 = s.Distance12,
                Distance13 = s.Distance13,
                Distance14 = s.Distance14,
                Distance15 = s.Distance15,
                Distance16 = s.Distance16,
                Distance17 = s.Distance17,
                Distance18 = s.Distance18,
                Distance19 = s.Distance19,
                Distance20 = s.Distance20,
                Distance21 = s.Distance21,
                Distance22 = s.Distance22,
                Distance23 = s.Distance23,
                Distance24 = s.Distance24,
                TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                MaxValue = s.MaxValue,
                MinValue=s.MinValue,
                Measure = rp5.Measure,
                ReportTypeCode = s.ReportTypeCode.Trim(),
                ReportTypeName = rp5.Description,
                Title = rp5.Title
            }).FirstOrDefault();
            Report_LuongMuc_DailyModel luongMua = reportDailyLuongMuaService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new Report_LuongMuc_DailyModel
            {
                Distance1 = s.Distance1,
                Distance2 = s.Distance2,
                Distance3 = s.Distance3,
                Distance4 = s.Distance4,
                Distance5 = s.Distance5,
                Distance6 = s.Distance6,
                Distance7 = s.Distance7,
                Distance8 = s.Distance8,
                Distance9 = s.Distance9,
                Distance10 = s.Distance10,
                Distance11 = s.Distance11,
                Distance12 = s.Distance12,
                Distance13 = s.Distance13,
                Distance14 = s.Distance14,
                Distance15 = s.Distance15,
                Distance16 = s.Distance16,
                Distance17 = s.Distance17,
                Distance18 = s.Distance18,
                Distance19 = s.Distance19,
                Distance20 = s.Distance20,
                Distance21 = s.Distance21,
                Distance22 = s.Distance22,
                Distance23 = s.Distance23,
                Distance24 = s.Distance24,
                TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                Measure = rp3.Measure,
                TongLuongMua = CheckValueReport(s.TongLuongMua, s.ReportTypeCode),
                ReportTypeCode = s.ReportTypeCode.Trim(),
                ReportTypeName = rp3.Description,
                Title = rp3.Title
            }).FirstOrDefault();
            Report_TocDoGio_DailyModel tocDoGio = reportDailyTocDoGioService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new Report_TocDoGio_DailyModel
            {
                Distance1 = s.Distance1,
                Distance2 = s.Distance2,
                Distance3 = s.Distance3,
                Distance4 = s.Distance4,
                Distance5 = s.Distance5,
                Distance6 = s.Distance6,
                Distance7 = s.Distance7,
                Distance8 = s.Distance8,
                Distance9 = s.Distance9,
                Distance10 = s.Distance10,
                Distance11 = s.Distance11,
                Distance12 = s.Distance12,
                Distance13 = s.Distance13,
                Distance14 = s.Distance14,
                Distance15 = s.Distance15,
                Distance16 = s.Distance16,
                Distance17 = s.Distance17,
                Distance18 = s.Distance18,
                Distance19 = s.Distance19,
                Distance20 = s.Distance20,
                Distance21 = s.Distance21,
                Distance22 = s.Distance22,
                Distance23 = s.Distance23,
                Distance24 = s.Distance24,
                TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                Measure = rp6.Measure,
                HuongGioCuaTocDoLonNhat = s.HuongGioCuaTocDoLonNhat,
                HuongGioCuarTocDoNhoNhat = s.HuongGioCuarTocDoNhoNhat,
                TocDoGioLonNhat = s.TocDoGioLonNhat.Value,
                TocDoGioNhoNhat = s.TocDoGioNhoNhat,
                ReportTypeCode = s.ReportTypeCode.Trim(),
                ReportTypeName = rp6.Description,
                Title = rp6.Title
            }).FirstOrDefault();
            Report_HuongGio_DailyModel huongGio = reportDailyHuongGioService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new Report_HuongGio_DailyModel
            {
                Distance1 = s.Distance1,
                Distance2 = s.Distance2,
                Distance3 = s.Distance3,
                Distance4 = s.Distance4,
                Distance5 = s.Distance5,
                Distance6 = s.Distance6,
                Distance7 = s.Distance7,
                Distance8 = s.Distance8,
                Distance9 = s.Distance9,
                Distance10 = s.Distance10,
                Distance11 = s.Distance11,
                Distance12 = s.Distance12,
                Distance13 = s.Distance13,
                Distance14 = s.Distance14,
                Distance15 = s.Distance15,
                Distance16 = s.Distance16,
                Distance17 = s.Distance17,
                Distance18 = s.Distance18,
                Distance19 = s.Distance19,
                Distance20 = s.Distance20,
                Distance21 = s.Distance21,
                Distance22 = s.Distance22,
                Distance23 = s.Distance23,
                Distance24 = s.Distance24,
                TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                Measure = rp4.Measure,
                HuongGioDacTrungNhieuNhat = GetTenHuongGio(s.HuongGioDacTrungNhieuNhat),
                HuongGioDacTrungNhieuThuHai = GetTenHuongGio(s.HuongGioDacTrungNhieuThuHai),
                ReportTypeCode = s.ReportTypeCode.Trim(),
                ReportTypeName = rp4.Description,
                Title = rp4.Title
            }).FirstOrDefault();
            ReportDailyViewModel viewModel = new ReportDailyViewModel
            {
                Date = date,
                NhietDo = nhietDo,
                DoAm = doAm,
                ApSuat = apSuat,
                MucNuoc = mucNuoc,
                LuongMua = luongMua,
                TocDoDongChay = tocDoDongChay,
                LuuLuongDongChay = luuLuongDongChay,
                DeviceId = deviceId,
                TocDoGio = tocDoGio,
                HuongGio = huongGio,
                AreaId = areaId,
                GroupId = groupId
            };
            return View(viewModel);
        }
        /// <summary>
        /// Báo cáo theo tháng
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="Date"></param>
        /// <param name="deviceId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<ActionResult> MonthLy(int? areaId = null, string Date = "", int? deviceId = null, string type = "RP0", int? groupId = null)
        {
            ViewBag.Title = "";
            ViewBag.MessageStatus = TempData["MessageStatus"];
            ViewBag.Message = TempData["Message"];
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            string userName = User.Identity.Name;
            groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id;
            ViewBag.listTypeReport = reportTypeService.reportTypeRepository.GetAll().ToList();
            if (!deviceId.HasValue)
            {
                deviceId = sitesService.GetByAreaId(areaId).FirstOrDefault().DeviceId;
            }
            if (userName == "administrator")
            {
                ViewBag.listSite = sitesService.sitesResponsitory.GetAll().ToList();
                ViewBag.listArea = areasService.areaResponsitory.GetAll().ToList();
                ViewBag.listGroups = groupService.groupResponsitory.GetAll().ToList();
            }
            else
            {
                ViewBag.listArea = areasService.GetAreasByGroupId(groupId).ToList();
                ViewBag.listSite = sitesService.GetBygroupId(groupId).ToList();
                ViewBag.listGroups = groupService.GetGroups(groupId).ToList();
            }
            ReportMonthlyModel model = new ReportMonthlyModel();
            model = await cacheBO.GetReportByMonth(deviceId.Value, type, Date);
            DateTime date = DateTime.Today;
            if (Date != "")
            {
                try
                {
                    date = Convert.ToDateTime(Date);
                }
                catch { }
            }
            ReportMonthlyViewModel viewModel = new ReportMonthlyViewModel
            {
                Date = date,
                DeviceId = deviceId,
                data = model,
                AreaId = areaId,

            };
            return View(viewModel);
        }

        /// <summary>
        /// Hàm trả về dữ liệu json
        /// </summary>
        /// <param name="day"></param>
        /// <param name="deviceId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetReportMonthByType(string day = "", int? deviceId = null, string type = "")
        {
            double total1 = 0, total2 = 0, total3 = 0;
            int count1 = 0, count2 = 0, count3 = 0;
            DateTime date = DateTime.Today;
            if (day != "")
            {
                try
                {
                    date = Convert.ToDateTime(day);
                }
                catch { }
            }
            DateTime distance1 = new DateTime(date.Year, date.Month, 1);
            DateTime distance2 = new DateTime(date.Year, date.Month, 10, 23, 59, 59);
            DateTime distance3 = new DateTime(date.Year, date.Month, 11);
            DateTime distance4 = new DateTime(date.Year, date.Month, 20, 23, 59, 59);
            DateTime distance5 = new DateTime(date.Year, date.Month, 21);
            DateTime distance6 = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);
            int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
            int? groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id;
            if (!deviceId.HasValue)
            {
                deviceId = sitesService.GetBygroupId(groupId).FirstOrDefault().DeviceId;
            }
            var result = await Task.WhenAll(dataObservationMongoService.GetDataByDeviceId(distance1, distance2, deviceId.Value)
                , dataObservationMongoService.GetDataByDeviceId(distance3, distance4, deviceId.Value)
                , dataObservationMongoService.GetDataByDeviceId(distance5, distance6, deviceId.Value));
            foreach (var item in result[0].ToList())
            {
                total1 = Total(item, type.Trim(), total1);
                count1++;
            }
            double tb1 = total1 / count1;
            foreach (var item in result[1].ToList())
            {
                total2 = Total(item, type.Trim(), total2);
                count2++;
            }
            double tb2 = total2 / count2;
            foreach (var item in result[2].ToList())
            {
                total3 = Total(item, type.Trim(), total3);
                count3++;
            }
            double tb3 = total3 / count3;
            ReportType reportType = reportTypeService.GetByCode(type).FirstOrDefault();
            Site site = sitesService.GetByDeviceId(deviceId).FirstOrDefault();
            ReportMonthlyModel model = new ReportMonthlyModel();
            model.Distance1 = tb1;
            model.Distance2 = tb2;
            model.Distance3 = tb3;
            model.Title = reportType == null ? "" : reportType.Title;
            model.Measure = reportType == null ? "" : reportType.Measure;
            model.Name = reportType == null ? "" : reportType.Description + " " + (site == null ? "" : site.Name);
            model.dayInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            return Json(new { model }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Hàm tính tổng số
        /// </summary>
        /// <param name="data"></param>
        /// <param name="code"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private double Total(Models.Entity.Data data, string code, double input)
        {
            if (code == "RP0")
            {
                input += data.BTI;
            }
            else if (code == "RP1")
            {
                input += data.BHU;
            }
            else if (code == "RP2")
            {
                input += data.BAV;
            }
            else if (code == "RP3")
            {
                input += data.BAC;
            }
            else if (code == "RP4")
            {
                input += data.BAP;
            }
            else if (code == "RP5")
            {
                input += data.BAF;
            }
            else if (code == "RP6")
            {
                input += data.BWS;
            }
            return input;
        }
        /// <summary>
        /// Hàm lấy danh sách trạm theo id khu vực
        /// </summary>
        /// <param name="idArea"></param>
        /// <returns></returns>
        public JsonResult GetSite(int idArea)
        {
            List<Site> data = sitesService.GetByAreaId(idArea).ToList();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Hàm lấy danh sách trạm theo id khu vực
        /// </summary>
        /// <param name="idArea"></param>
        /// <returns></returns>
        public JsonResult LoadArea(int groupId)
        {
            List<Area> data = areasService.GetAreasByGroupId(groupId).ToList();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        private static double CheckValueReport(double? input, string code)
        {
            double result = 0;
            if (code.Trim() == "RP0" || code.Trim() == "RP1" || code.Trim() == "RP2" || code.Trim() == "RP6")
            {

                result = input.Value / 10 + input.Value % 10;
                return result;
            }
            else if (code.Trim() == "RP3" || code.Trim() == "RP4")
            {

                return input.Value;
            }
            else if (code.Trim() == "RP5")
            {

                result = input.Value / 1000;

                return result;
            }
            return result;

        }
        private static string GetTenHuongGio(double? huongGio)
        {
            string result = "Không xác định";
            if (huongGio == 0)
            {
                result = "Hướng Bắc";
            }
            else if (huongGio == 1)
            {

                result = "Hướng Đông Bắc";
            }
            else if (huongGio == 2)
            {

                result = "Hướng Đông";
            }
            else if (huongGio == 3)
            {

                result = "Hướng Đông Nam";
            }
            else if (huongGio == 4)
            {

                result = "Hướng Nam";
            }
            else if (huongGio == 5)
            {

                result = "Hướng Tây Nam";
            }
            else if (huongGio == 6)
            {

                result = "Hướng Tây";
            }
            else if (huongGio == 7)
            {

                result = "Hướng Tây Bắc";
            }
            return result;

        }
        public void ExportExel(string Date = "", int? deviceId = null)
        {
            try
            {               
                DateTime date = DateTime.Today;
                if (Date != "")
                {
                    try
                    {
                        date = Convert.ToDateTime(Date);
                    }
                    catch { }
                }
                string[] cellColump = { "A", "B" };
                int rowStartAllTable = 7;
                ExcelPackage pck = new ExcelPackage();
                List<ReportExcelModel> lst = new List<ReportExcelModel>();
                int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
                int? groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id;
                if (!deviceId.HasValue)
                {
                    deviceId = sitesService.GetBygroupId(groupId).FirstOrDefault().DeviceId;
                }

                #region Nhiet độ
                ReportExcelModel nhietDo = reportDailyNhietDoService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new ReportExcelModel
                {
                    Distance1 = s.Distance1,
                    Distance2 = s.Distance2,
                    Distance3 = s.Distance3,
                    Distance4 = s.Distance4,
                    Distance5 = s.Distance5,
                    Distance6 = s.Distance6,
                    Distance7 = s.Distance7,
                    Distance8 = s.Distance8,
                    Distance9 = s.Distance9,
                    Distance10 = s.Distance10,
                    Distance11 = s.Distance11,
                    Distance12 = s.Distance12,
                    Distance13 = s.Distance13,
                    Distance14 = s.Distance14,
                    Distance15 = s.Distance15,
                    Distance16 = s.Distance16,
                    Distance17 = s.Distance17,
                    Distance18 = s.Distance18,
                    Distance19 = s.Distance19,
                    Distance20 = s.Distance20,
                    Distance21 = s.Distance21,
                    Distance22 = s.Distance22,
                    Distance23 = s.Distance23,
                    Distance24 = s.Distance24,
                    MinValue = s.MinValue,
                    MaxValue = s.MaxValue,
                    TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                    TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                }).FirstOrDefault();
                if (nhietDo != null)
                {
                    ExcelWorksheet wsNhietDo = pck.Workbook.Worksheets.Add("Nhiệt độ");
                    int time1 = 0;
                    for (int i = 8; i < 32; i++)
                    {
                        wsNhietDo.Cells[string.Format("A{0}", i)].Value = time1 + "h - " + (time1+1)+ "h";
                        time1++;
                    }
                    SetBorderExportExcel(wsNhietDo, cellColump, 24, rowStartAllTable);
                    wsNhietDo.Cells["A:AZ"].AutoFitColumns();
                    wsNhietDo.Cells["A:A"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    wsNhietDo.Cells["B2:F2"].Merge = true;
                    wsNhietDo.Cells["B3:F3"].Merge = true;
                    wsNhietDo.Cells["B2"].Value = "THỐNG KÊ NHIỆT ĐỘ";
                    wsNhietDo.Cells["B3"].Value = "Trạm " + sitesService.GetByDeviceId(deviceId).FirstOrDefault().Name.ToString() + "(Ngày " + date.ToString() + ")";
                    wsNhietDo.Cells["A4"].Value = "Nhiệt độ lớn nhất";
                    wsNhietDo.Cells["B4"].Value = nhietDo.MaxValue+ " °C";
                    wsNhietDo.Cells["C4"].Value = "Thời gian: " + nhietDo.TimeMaxValue.ToString();
                    wsNhietDo.Cells["A5"].Value = "Nhiệt độ nhỏ nhất";
                    wsNhietDo.Cells["B5"].Value = nhietDo.MinValue+ " °C";
                    wsNhietDo.Cells["C5"].Value = "Thời gian: " + nhietDo.TimeMinValue.ToString();
                    wsNhietDo.Cells["A7"].Value = "Thời gian";
                    wsNhietDo.Cells["B7"].Value = "Giá trị (°C)";
                    wsNhietDo.Cells[string.Format("B{0}", 8)].Value = nhietDo.Distance1;
                    wsNhietDo.Cells[string.Format("B{0}", 9)].Value = nhietDo.Distance2;
                    wsNhietDo.Cells[string.Format("B{0}", 10)].Value = nhietDo.Distance3;
                    wsNhietDo.Cells[string.Format("B{0}", 11)].Value = nhietDo.Distance4;
                    wsNhietDo.Cells[string.Format("B{0}", 12)].Value = nhietDo.Distance5;
                    wsNhietDo.Cells[string.Format("B{0}", 13)].Value = nhietDo.Distance6;
                    wsNhietDo.Cells[string.Format("B{0}", 14)].Value = nhietDo.Distance7;
                    wsNhietDo.Cells[string.Format("B{0}", 15)].Value = nhietDo.Distance8;
                    wsNhietDo.Cells[string.Format("B{0}", 16)].Value = nhietDo.Distance9;
                    wsNhietDo.Cells[string.Format("B{0}", 17)].Value = nhietDo.Distance10;
                    wsNhietDo.Cells[string.Format("B{0}", 18)].Value = nhietDo.Distance11;
                    wsNhietDo.Cells[string.Format("B{0}", 19)].Value = nhietDo.Distance12;
                    wsNhietDo.Cells[string.Format("B{0}", 20)].Value = nhietDo.Distance13;
                    wsNhietDo.Cells[string.Format("B{0}", 21)].Value = nhietDo.Distance14;
                    wsNhietDo.Cells[string.Format("B{0}", 22)].Value = nhietDo.Distance15;
                    wsNhietDo.Cells[string.Format("B{0}", 23)].Value = nhietDo.Distance16;
                    wsNhietDo.Cells[string.Format("B{0}", 24)].Value = nhietDo.Distance17;
                    wsNhietDo.Cells[string.Format("B{0}", 25)].Value = nhietDo.Distance18;
                    wsNhietDo.Cells[string.Format("B{0}", 26)].Value = nhietDo.Distance19;
                    wsNhietDo.Cells[string.Format("B{0}", 27)].Value = nhietDo.Distance20;
                    wsNhietDo.Cells[string.Format("B{0}", 28)].Value = nhietDo.Distance21;
                    wsNhietDo.Cells[string.Format("B{0}", 29)].Value = nhietDo.Distance22;
                    wsNhietDo.Cells[string.Format("B{0}", 30)].Value = nhietDo.Distance23;
                    wsNhietDo.Cells[string.Format("B{0}", 31)].Value = nhietDo.Distance24;
                }
                #endregion

                #region dộ ẩm
                ReportExcelModel doAm = reportDailyDoAmService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new ReportExcelModel
                {
                    Distance1 = s.Distance1,
                    Distance2 = s.Distance2,
                    Distance3 = s.Distance3,
                    Distance4 = s.Distance4,
                    Distance5 = s.Distance5,
                    Distance6 = s.Distance6,
                    Distance7 = s.Distance7,
                    Distance8 = s.Distance8,
                    Distance9 = s.Distance9,
                    Distance10 = s.Distance10,
                    Distance11 = s.Distance11,
                    Distance12 = s.Distance12,
                    Distance13 = s.Distance13,
                    Distance14 = s.Distance14,
                    Distance15 = s.Distance15,
                    Distance16 = s.Distance16,
                    Distance17 = s.Distance17,
                    Distance18 = s.Distance18,
                    Distance19 = s.Distance19,
                    Distance20 = s.Distance20,
                    Distance21 = s.Distance21,
                    Distance22 = s.Distance22,
                    Distance23 = s.Distance23,
                    Distance24 = s.Distance24,
                    MinValue = s.MinValue,
                    MaxValue = s.MaxValue,
                    TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                    TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                }).FirstOrDefault();
                if (doAm != null)
                {
                    ExcelWorksheet wsDoAm = pck.Workbook.Worksheets.Add("Độ ẩm");
                    wsDoAm.Cells["B2:F2"].Merge = true;
                    wsDoAm.Cells["B3:F3"].Merge = true;
                    wsDoAm.Cells["B2"].Value = "THỐNG KÊ ĐỘ ẨM";
                    wsDoAm.Cells["B3"].Value = "Trạm " + sitesService.GetByDeviceId(deviceId).FirstOrDefault().Name.ToString() + "(Ngày " + date.ToString() + ")";
                    wsDoAm.Cells["A4"].Value = "Độ ẩm lớn nhất";
                    wsDoAm.Cells["B4"].Value = doAm.MaxValue+ " %";
                    wsDoAm.Cells["C4"].Value = "Thời gian: " + doAm.TimeMaxValue.ToString();
                    wsDoAm.Cells["A5"].Value = "Độ ẩm nhỏ nhất";
                    wsDoAm.Cells["B5"].Value = doAm.MinValue+ " %";
                    wsDoAm.Cells["C5"].Value = "Thời gian: " + doAm.TimeMinValue.ToString();
                    wsDoAm.Cells["A7"].Value = "Thời gian";
                    wsDoAm.Cells["B7"].Value = "Giá trị (%)";
                    wsDoAm.Cells[string.Format("B{0}", 8)].Value = doAm.Distance1;
                    wsDoAm.Cells[string.Format("B{0}", 9)].Value = doAm.Distance2;
                    wsDoAm.Cells[string.Format("B{0}", 10)].Value = doAm.Distance3;
                    wsDoAm.Cells[string.Format("B{0}", 11)].Value = doAm.Distance4;
                    wsDoAm.Cells[string.Format("B{0}", 12)].Value = doAm.Distance5;
                    wsDoAm.Cells[string.Format("B{0}", 13)].Value = doAm.Distance6;
                    wsDoAm.Cells[string.Format("B{0}", 14)].Value = doAm.Distance7;
                    wsDoAm.Cells[string.Format("B{0}", 15)].Value = doAm.Distance8;
                    wsDoAm.Cells[string.Format("B{0}", 16)].Value = doAm.Distance9;
                    wsDoAm.Cells[string.Format("B{0}", 17)].Value = doAm.Distance10;
                    wsDoAm.Cells[string.Format("B{0}", 18)].Value = doAm.Distance11;
                    wsDoAm.Cells[string.Format("B{0}", 19)].Value = doAm.Distance12;
                    wsDoAm.Cells[string.Format("B{0}", 20)].Value = doAm.Distance13;
                    wsDoAm.Cells[string.Format("B{0}", 21)].Value = doAm.Distance14;
                    wsDoAm.Cells[string.Format("B{0}", 22)].Value = doAm.Distance15;
                    wsDoAm.Cells[string.Format("B{0}", 23)].Value = doAm.Distance16;
                    wsDoAm.Cells[string.Format("B{0}", 24)].Value = doAm.Distance17;
                    wsDoAm.Cells[string.Format("B{0}", 25)].Value = doAm.Distance18;
                    wsDoAm.Cells[string.Format("B{0}", 26)].Value = doAm.Distance19;
                    wsDoAm.Cells[string.Format("B{0}", 27)].Value = doAm.Distance20;
                    wsDoAm.Cells[string.Format("B{0}", 28)].Value = doAm.Distance21;
                    wsDoAm.Cells[string.Format("B{0}", 29)].Value = doAm.Distance22;
                    wsDoAm.Cells[string.Format("B{0}", 30)].Value = doAm.Distance23;
                    wsDoAm.Cells[string.Format("B{0}", 31)].Value = doAm.Distance24;
                    int time2 = 0;
                    for (int i = 8; i < 32; i++)
                    {
                        wsDoAm.Cells[string.Format("A{0}", i)].Value = time2 + "h - " + (time2+1)+ "h";
                        time2++;
                    }
                    SetBorderExportExcel(wsDoAm, cellColump, 24, rowStartAllTable);
                    wsDoAm.Cells["A:AZ"].AutoFitColumns();
                    wsDoAm.Cells["A:A"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }
                #endregion

                #region Áp suất
                ReportExcelModel apSuat = reportDailyApSuatService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new ReportExcelModel
                {
                    Distance1 = s.Distance1,
                    Distance2 = s.Distance2,
                    Distance3 = s.Distance3,
                    Distance4 = s.Distance4,
                    Distance5 = s.Distance5,
                    Distance6 = s.Distance6,
                    Distance7 = s.Distance7,
                    Distance8 = s.Distance8,
                    Distance9 = s.Distance9,
                    Distance10 = s.Distance10,
                    Distance11 = s.Distance11,
                    Distance12 = s.Distance12,
                    Distance13 = s.Distance13,
                    Distance14 = s.Distance14,
                    Distance15 = s.Distance15,
                    Distance16 = s.Distance16,
                    Distance17 = s.Distance17,
                    Distance18 = s.Distance18,
                    Distance19 = s.Distance19,
                    Distance20 = s.Distance20,
                    Distance21 = s.Distance21,
                    Distance22 = s.Distance22,
                    Distance23 = s.Distance23,
                    Distance24 = s.Distance24,
                    MinValue = s.MinValue,
                    MaxValue = s.MaxValue,
                    TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                    TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                }).FirstOrDefault();
                if (apSuat != null)
                {
                    ExcelWorksheet wsApSuat = pck.Workbook.Worksheets.Add("Áp suất");
                    wsApSuat.Cells["B2:F2"].Merge = true;
                    wsApSuat.Cells["B3:F3"].Merge = true;
                    wsApSuat.Cells["B2"].Value = "THỐNG KÊ ÁP SUẤT";
                    wsApSuat.Cells["B3"].Value = "Trạm " + sitesService.GetByDeviceId(deviceId).FirstOrDefault().Name.ToString() + "(Ngày " + date.ToString() + ")";
                    wsApSuat.Cells["A4"].Value = "Áp suất lớn nhất";
                    wsApSuat.Cells["B4"].Value = apSuat.MaxValue+ " hPA";
                    wsApSuat.Cells["C4"].Value = "Thời gian: " + apSuat.TimeMaxValue.ToString();
                    wsApSuat.Cells["A5"].Value = "Áp suất nhỏ nhất";
                    wsApSuat.Cells["B5"].Value = apSuat.MinValue + " hPA";
                    wsApSuat.Cells["C5"].Value = "Thời gian: " + apSuat.TimeMinValue.ToString();
                    wsApSuat.Cells["A7"].Value = "Thời gian";
                    wsApSuat.Cells["B7"].Value = "Giá trị (hPA)";
                    wsApSuat.Cells[string.Format("B{0}", 8)].Value = apSuat.Distance1;
                    wsApSuat.Cells[string.Format("B{0}", 9)].Value = apSuat.Distance2;
                    wsApSuat.Cells[string.Format("B{0}", 10)].Value = apSuat.Distance3;
                    wsApSuat.Cells[string.Format("B{0}", 11)].Value = apSuat.Distance4;
                    wsApSuat.Cells[string.Format("B{0}", 12)].Value = apSuat.Distance5;
                    wsApSuat.Cells[string.Format("B{0}", 13)].Value = apSuat.Distance6;
                    wsApSuat.Cells[string.Format("B{0}", 14)].Value = apSuat.Distance7;
                    wsApSuat.Cells[string.Format("B{0}", 15)].Value = apSuat.Distance8;
                    wsApSuat.Cells[string.Format("B{0}", 16)].Value = apSuat.Distance9;
                    wsApSuat.Cells[string.Format("B{0}", 17)].Value = apSuat.Distance10;
                    wsApSuat.Cells[string.Format("B{0}", 18)].Value = apSuat.Distance11;
                    wsApSuat.Cells[string.Format("B{0}", 19)].Value = apSuat.Distance12;
                    wsApSuat.Cells[string.Format("B{0}", 20)].Value = apSuat.Distance13;
                    wsApSuat.Cells[string.Format("B{0}", 21)].Value = apSuat.Distance14;
                    wsApSuat.Cells[string.Format("B{0}", 22)].Value = apSuat.Distance15;
                    wsApSuat.Cells[string.Format("B{0}", 23)].Value = apSuat.Distance16;
                    wsApSuat.Cells[string.Format("B{0}", 24)].Value = apSuat.Distance17;
                    wsApSuat.Cells[string.Format("B{0}", 25)].Value = apSuat.Distance18;
                    wsApSuat.Cells[string.Format("B{0}", 26)].Value = apSuat.Distance19;
                    wsApSuat.Cells[string.Format("B{0}", 27)].Value = apSuat.Distance20;
                    wsApSuat.Cells[string.Format("B{0}", 28)].Value = apSuat.Distance21;
                    wsApSuat.Cells[string.Format("B{0}", 29)].Value = apSuat.Distance22;
                    wsApSuat.Cells[string.Format("B{0}", 30)].Value = apSuat.Distance23;
                    wsApSuat.Cells[string.Format("B{0}", 31)].Value = apSuat.Distance24;
                    int time3 = 0;
                    for (int i = 8; i < 32; i++)
                    {
                        wsApSuat.Cells[string.Format("A{0}", i)].Value = time3 + "h - " + (time3 +1) +"h";
                        time3++;
                    }
                    SetBorderExportExcel(wsApSuat, cellColump, 24, rowStartAllTable);
                    wsApSuat.Cells["A:AZ"].AutoFitColumns();
                    wsApSuat.Cells["A:A"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }
                #endregion

                #region Mực nước
                ReportExcelModel mucNuoc = reportDailyMucNuocService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new ReportExcelModel
                {
                    Distance1 = s.Distance1,
                    Distance2 = s.Distance2,
                    Distance3 = s.Distance3,
                    Distance4 = s.Distance4,
                    Distance5 = s.Distance5,
                    Distance6 = s.Distance6,
                    Distance7 = s.Distance7,
                    Distance8 = s.Distance8,
                    Distance9 = s.Distance9,
                    Distance10 = s.Distance10,
                    Distance11 = s.Distance11,
                    Distance12 = s.Distance12,
                    Distance13 = s.Distance13,
                    Distance14 = s.Distance14,
                    Distance15 = s.Distance15,
                    Distance16 = s.Distance16,
                    Distance17 = s.Distance17,
                    Distance18 = s.Distance18,
                    Distance19 = s.Distance19,
                    Distance20 = s.Distance20,
                    Distance21 = s.Distance21,
                    Distance22 = s.Distance22,
                    Distance23 = s.Distance23,
                    Distance24 = s.Distance24,            
                    MaxValue = s.MaxValue,
                    MinValue = s.MinValue,
                    TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                    TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                }).FirstOrDefault();
                if (mucNuoc != null)
                {
                    ExcelWorksheet wsMucNuoc = pck.Workbook.Worksheets.Add("Mực nước");
                    wsMucNuoc.Cells["B2:F2"].Merge = true;
                    wsMucNuoc.Cells["B3:F3"].Merge = true;
                    wsMucNuoc.Cells["B2"].Value = "THỐNG KÊ MỰC NƯỚC";
                    wsMucNuoc.Cells["B3"].Value = "Trạm " + sitesService.GetByDeviceId(deviceId).FirstOrDefault().Name.ToString() + "(Ngày " + date.ToString() + ")";
                    wsMucNuoc.Cells["A4"].Value = "Áp suất lớn nhất";
                    wsMucNuoc.Cells["B4"].Value = mucNuoc.MaxValue + " m";
                    wsMucNuoc.Cells["C4"].Value = "Thời gian: " + mucNuoc.TimeMaxValue.ToString();
                    wsMucNuoc.Cells["A5"].Value = "Áp suất nhỏ nhất";
                    wsMucNuoc.Cells["B5"].Value = mucNuoc.MinValue + " m";
                    wsMucNuoc.Cells["C5"].Value = "Thời gian: " + mucNuoc.TimeMinValue.ToString();
                    wsMucNuoc.Cells["A5"].Value = "Thời gian";
                    wsMucNuoc.Cells["B5"].Value = "Giá trị (m)";
                    wsMucNuoc.Cells[string.Format("B{0}", 6)].Value = mucNuoc.Distance1;
                    wsMucNuoc.Cells[string.Format("B{0}", 7)].Value = mucNuoc.Distance2;
                    wsMucNuoc.Cells[string.Format("B{0}", 8)].Value = mucNuoc.Distance3;
                    wsMucNuoc.Cells[string.Format("B{0}", 9)].Value = mucNuoc.Distance4;
                    wsMucNuoc.Cells[string.Format("B{0}", 10)].Value = mucNuoc.Distance5;
                    wsMucNuoc.Cells[string.Format("B{0}", 11)].Value = mucNuoc.Distance6;
                    wsMucNuoc.Cells[string.Format("B{0}", 12)].Value = mucNuoc.Distance7;
                    wsMucNuoc.Cells[string.Format("B{0}", 13)].Value = mucNuoc.Distance8;
                    wsMucNuoc.Cells[string.Format("B{0}", 14)].Value = mucNuoc.Distance9;
                    wsMucNuoc.Cells[string.Format("B{0}", 15)].Value = mucNuoc.Distance10;
                    wsMucNuoc.Cells[string.Format("B{0}", 16)].Value = mucNuoc.Distance11;
                    wsMucNuoc.Cells[string.Format("B{0}", 17)].Value = mucNuoc.Distance12;
                    wsMucNuoc.Cells[string.Format("B{0}", 18)].Value = mucNuoc.Distance13;
                    wsMucNuoc.Cells[string.Format("B{0}", 19)].Value = mucNuoc.Distance14;
                    wsMucNuoc.Cells[string.Format("B{0}", 20)].Value = mucNuoc.Distance15;
                    wsMucNuoc.Cells[string.Format("B{0}", 21)].Value = mucNuoc.Distance16;
                    wsMucNuoc.Cells[string.Format("B{0}", 22)].Value = mucNuoc.Distance17;
                    wsMucNuoc.Cells[string.Format("B{0}", 23)].Value = mucNuoc.Distance18;
                    wsMucNuoc.Cells[string.Format("B{0}", 24)].Value = mucNuoc.Distance19;
                    wsMucNuoc.Cells[string.Format("B{0}", 25)].Value = mucNuoc.Distance20;
                    wsMucNuoc.Cells[string.Format("B{0}", 26)].Value = mucNuoc.Distance21;
                    wsMucNuoc.Cells[string.Format("B{0}", 27)].Value = mucNuoc.Distance22;
                    wsMucNuoc.Cells[string.Format("B{0}", 28)].Value = mucNuoc.Distance23;
                    wsMucNuoc.Cells[string.Format("B{0}", 29)].Value = mucNuoc.Distance24;
                    int time4 = 0;
                    for (int i = 6; i < 30; i++)
                    {
                        wsMucNuoc.Cells[string.Format("A{0}", i)].Value = time4 + "h - " + (time4+1) + "h";
                        time4++;
                    }
                    SetBorderExportExcel(wsMucNuoc, cellColump, 24, 5);
                    wsMucNuoc.Cells["A:AZ"].AutoFitColumns();
                    wsMucNuoc.Cells["A:A"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }
                #endregion

                #region Lượng mưa
                ReportExcelModel luongMua = reportDailyLuongMuaService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new ReportExcelModel
                {
                    Distance1 = s.Distance1,
                    Distance2 = s.Distance2,
                    Distance3 = s.Distance3,
                    Distance4 = s.Distance4,
                    Distance5 = s.Distance5,
                    Distance6 = s.Distance6,
                    Distance7 = s.Distance7,
                    Distance8 = s.Distance8,
                    Distance9 = s.Distance9,
                    Distance10 = s.Distance10,
                    Distance11 = s.Distance11,
                    Distance12 = s.Distance12,
                    Distance13 = s.Distance13,
                    Distance14 = s.Distance14,
                    Distance15 = s.Distance15,
                    Distance16 = s.Distance16,
                    Distance17 = s.Distance17,
                    Distance18 = s.Distance18,
                    Distance19 = s.Distance19,
                    Distance20 = s.Distance20,
                    Distance21 = s.Distance21,
                    Distance22 = s.Distance22,
                    Distance23 = s.Distance23,
                    MaxValue=s.TongLuongMua,
                    Distance24 = s.Distance24,
                    TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                    TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                }).FirstOrDefault();
                if (luongMua != null)
                {
                    ExcelWorksheet wsLuongMua = pck.Workbook.Worksheets.Add("Lượng mưa");

                    wsLuongMua.Cells["B2:F2"].Merge = true;
                    wsLuongMua.Cells["B3:F3"].Merge = true;
                    wsLuongMua.Cells["B2"].Value = "THỐNG KÊ LƯỢNG MƯA";
                    wsLuongMua.Cells["B3"].Value = "Trạm " + sitesService.GetByDeviceId(deviceId).FirstOrDefault().Name.ToString() + "(Ngày " + date.ToString() + ")";
                    wsLuongMua.Cells["A4"].Value = "Tổng lượng mưa";
                    wsLuongMua.Cells["B4"].Value = luongMua.MaxValue+ " mm";
                    wsLuongMua.Cells["C4"].Value = "Thời gian: " + luongMua.TimeMaxValue.ToString();
                   
                    wsLuongMua.Cells["A7"].Value = "Thời gian";
                    wsLuongMua.Cells["B7"].Value = "Giá trị (mm)";
                    wsLuongMua.Cells[string.Format("B{0}", 8)].Value = luongMua.Distance1;
                    wsLuongMua.Cells[string.Format("B{0}", 9)].Value = luongMua.Distance2;
                    wsLuongMua.Cells[string.Format("B{0}", 10)].Value = luongMua.Distance3;
                    wsLuongMua.Cells[string.Format("B{0}", 11)].Value = luongMua.Distance4;
                    wsLuongMua.Cells[string.Format("B{0}", 12)].Value = luongMua.Distance5;
                    wsLuongMua.Cells[string.Format("B{0}", 13)].Value = luongMua.Distance6;
                    wsLuongMua.Cells[string.Format("B{0}", 14)].Value = luongMua.Distance7;
                    wsLuongMua.Cells[string.Format("B{0}", 15)].Value = luongMua.Distance8;
                    wsLuongMua.Cells[string.Format("B{0}", 16)].Value = luongMua.Distance9;
                    wsLuongMua.Cells[string.Format("B{0}", 17)].Value = luongMua.Distance10;
                    wsLuongMua.Cells[string.Format("B{0}", 18)].Value = luongMua.Distance11;
                    wsLuongMua.Cells[string.Format("B{0}", 19)].Value = luongMua.Distance12;
                    wsLuongMua.Cells[string.Format("B{0}", 20)].Value = luongMua.Distance13;
                    wsLuongMua.Cells[string.Format("B{0}", 21)].Value = luongMua.Distance14;
                    wsLuongMua.Cells[string.Format("B{0}", 22)].Value = luongMua.Distance15;
                    wsLuongMua.Cells[string.Format("B{0}", 23)].Value = luongMua.Distance16;
                    wsLuongMua.Cells[string.Format("B{0}", 24)].Value = luongMua.Distance17;
                    wsLuongMua.Cells[string.Format("B{0}", 25)].Value = luongMua.Distance18;
                    wsLuongMua.Cells[string.Format("B{0}", 26)].Value = luongMua.Distance19;
                    wsLuongMua.Cells[string.Format("B{0}", 27)].Value = luongMua.Distance20;
                    wsLuongMua.Cells[string.Format("B{0}", 28)].Value = luongMua.Distance21;
                    wsLuongMua.Cells[string.Format("B{0}", 29)].Value = luongMua.Distance22;
                    wsLuongMua.Cells[string.Format("B{0}", 30)].Value = luongMua.Distance23;
                    wsLuongMua.Cells[string.Format("B{0}", 31)].Value = luongMua.Distance24;
                    int time5 = 0;
                    for (int i = 8; i < 32; i++)
                    {
                        wsLuongMua.Cells[string.Format("A{0}", i)].Value = time5 + "h - " + (time5+1) + "h";
                        time5++;
                    }

                    SetBorderExportExcel(wsLuongMua, cellColump, 24, rowStartAllTable);
                    wsLuongMua.Cells["A:AZ"].AutoFitColumns();
                    wsLuongMua.Cells["A:A"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }
                #endregion

                #region tốc độ gió
                ReportExcelModel tocDoGio = reportDailyTocDoGioService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new ReportExcelModel
                {
                    Distance1 = s.Distance1,
                    Distance2 = s.Distance2,
                    Distance3 = s.Distance3,
                    Distance4 = s.Distance4,
                    Distance5 = s.Distance5,
                    Distance6 = s.Distance6,
                    Distance7 = s.Distance7,
                    Distance8 = s.Distance8,
                    Distance9 = s.Distance9,
                    Distance10 = s.Distance10,
                    Distance11 = s.Distance11,
                    Distance12 = s.Distance12,
                    Distance13 = s.Distance13,
                    Distance14 = s.Distance14,
                    Distance15 = s.Distance15,
                    Distance16 = s.Distance16,
                    Distance17 = s.Distance17,
                    Distance18 = s.Distance18,
                    Distance19 = s.Distance19,
                    Distance20 = s.Distance20,
                    Distance21 = s.Distance21,
                    Distance22 = s.Distance22,
                    Distance23 = s.Distance23,
                    Distance24 = s.Distance24,
                    MaxValue=s.TocDoGioLonNhat,
                    MinValue = s.TocDoGioNhoNhat,
                    TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                    TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                }).FirstOrDefault();
                if (tocDoGio != null)
                {
                    ExcelWorksheet wsTocDoGio = pck.Workbook.Worksheets.Add("Tốc độ gió");
                    wsTocDoGio.Cells["B2:F2"].Merge = true;
                    wsTocDoGio.Cells["B3:F3"].Merge = true;
                    wsTocDoGio.Cells["B2"].Value = "THỐNG KÊ TỐC ĐỘ GIÓ";
                    wsTocDoGio.Cells["B3"].Value = "Trạm " + sitesService.GetByDeviceId(deviceId).FirstOrDefault().Name.ToString() + "(Ngày " + date.ToString() + ")";
                    wsTocDoGio.Cells["A4"].Value = "Tốc độ gió lớn nhất trong 2s";
                    wsTocDoGio.Cells["B4"].Value = tocDoGio.MaxValue+ " m/s";
                    wsTocDoGio.Cells["C4"].Value = "Thời gian: " + tocDoGio.TimeMaxValue.ToString();
                    wsTocDoGio.Cells["A5"].Value = "Tốc độ gió lớn nhất trong 2m";
                    wsTocDoGio.Cells["B5"].Value = tocDoGio.MinValue + " m/s";
                    wsTocDoGio.Cells["C5"].Value = "Thời gian: " + tocDoGio.TimeMinValue.ToString();
                    wsTocDoGio.Cells["A7"].Value = "Thời gian";
                    wsTocDoGio.Cells["B7"].Value = "Giá trị (m/s)";
                    wsTocDoGio.Cells[string.Format("B{0}", 8)].Value = tocDoGio.Distance1;
                    wsTocDoGio.Cells[string.Format("B{0}", 9)].Value = tocDoGio.Distance2;
                    wsTocDoGio.Cells[string.Format("B{0}", 10)].Value = tocDoGio.Distance3;
                    wsTocDoGio.Cells[string.Format("B{0}", 11)].Value = tocDoGio.Distance4;
                    wsTocDoGio.Cells[string.Format("B{0}", 12)].Value = tocDoGio.Distance5;
                    wsTocDoGio.Cells[string.Format("B{0}", 13)].Value = tocDoGio.Distance6;
                    wsTocDoGio.Cells[string.Format("B{0}", 14)].Value = tocDoGio.Distance7;
                    wsTocDoGio.Cells[string.Format("B{0}", 15)].Value = tocDoGio.Distance8;
                    wsTocDoGio.Cells[string.Format("B{0}", 16)].Value = tocDoGio.Distance9;
                    wsTocDoGio.Cells[string.Format("B{0}", 17)].Value = tocDoGio.Distance10;
                    wsTocDoGio.Cells[string.Format("B{0}", 18)].Value = tocDoGio.Distance11;
                    wsTocDoGio.Cells[string.Format("B{0}", 19)].Value = tocDoGio.Distance12;
                    wsTocDoGio.Cells[string.Format("B{0}", 20)].Value = tocDoGio.Distance13;
                    wsTocDoGio.Cells[string.Format("B{0}", 21)].Value = tocDoGio.Distance14;
                    wsTocDoGio.Cells[string.Format("B{0}", 22)].Value = tocDoGio.Distance15;
                    wsTocDoGio.Cells[string.Format("B{0}", 23)].Value = tocDoGio.Distance16;
                    wsTocDoGio.Cells[string.Format("B{0}", 24)].Value = tocDoGio.Distance17;
                    wsTocDoGio.Cells[string.Format("B{0}", 25)].Value = tocDoGio.Distance18;
                    wsTocDoGio.Cells[string.Format("B{0}", 26)].Value = tocDoGio.Distance19;
                    wsTocDoGio.Cells[string.Format("B{0}", 27)].Value = tocDoGio.Distance20;
                    wsTocDoGio.Cells[string.Format("B{0}", 28)].Value = tocDoGio.Distance21;
                    wsTocDoGio.Cells[string.Format("B{0}", 29)].Value = tocDoGio.Distance22;
                    wsTocDoGio.Cells[string.Format("B{0}", 30)].Value = tocDoGio.Distance23;
                    wsTocDoGio.Cells[string.Format("B{0}", 31)].Value = tocDoGio.Distance24;
                    int time6 = 0;
                    for (int i = 8; i < 32; i++)
                    {
                        wsTocDoGio.Cells[string.Format("A{0}", i)].Value = time6 + "h - " + (time6+1)+ "h";
                        time6++;
                    }
                    SetBorderExportExcel(wsTocDoGio, cellColump, 24, rowStartAllTable);
                    wsTocDoGio.Cells["A:AZ"].AutoFitColumns();
                    wsTocDoGio.Cells["A:A"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }
                #endregion

                #region Hướng gió
                ReportExcelModel huongGio = reportDailyHuongGioService.GetByDeviceIdAndDate(date, deviceId).AsEnumerable().Select(s => new ReportExcelModel
                {
                    Distance1 = s.Distance1,
                    Distance2 = s.Distance2,
                    Distance3 = s.Distance3,
                    Distance4 = s.Distance4,
                    Distance5 = s.Distance5,
                    Distance6 = s.Distance6,
                    Distance7 = s.Distance7,
                    Distance8 = s.Distance8,
                    Distance9 = s.Distance9,
                    Distance10 = s.Distance10,
                    Distance11 = s.Distance11,
                    Distance12 = s.Distance12,
                    Distance13 = s.Distance13,
                    Distance14 = s.Distance14,
                    Distance15 = s.Distance15,
                    Distance16 = s.Distance16,
                    Distance17 = s.Distance17,
                    Distance18 = s.Distance18,
                    Distance19 = s.Distance19,
                    Distance20 = s.Distance20,
                    Distance21 = s.Distance21,
                    Distance22 = s.Distance22,
                    Distance23 = s.Distance23,
                    Distance24 = s.Distance24,
                    MaxValue = s.HuongGioDacTrungNhieuNhat,
                    MinValue = s.HuongGioDacTrungNhieuThuHai,
                    TimeMaxValue = s.TimeMaxValue == null ? "" : s.TimeMaxValue.Value.ToString("HH:mm"),
                    TimeMinValue = s.TimeMinValue == null ? "" : s.TimeMinValue.Value.ToString("HH:mm"),
                }).FirstOrDefault();
                if (huongGio != null)
                {
                    ExcelWorksheet wsHuongGio = pck.Workbook.Worksheets.Add("Hướng gió");
                    wsHuongGio.Cells["B2:F2"].Merge = true;
                    wsHuongGio.Cells["B3:F3"].Merge = true;
                    wsHuongGio.Cells["B2"].Value = "THỐNG KÊ HƯỚNG GIÓ";
                    wsHuongGio.Cells["B3"].Value = "Trạm " + sitesService.GetByDeviceId(deviceId).FirstOrDefault().Name.ToString() + "(Ngày " + date.ToString()+")";
                    wsHuongGio.Cells["A4"].Value = "Hướng gió đặc trưng trong ngày";
                    wsHuongGio.Cells["B4"].Value = GetTenHuongGio(huongGio.MaxValue);
                    wsHuongGio.Cells["C4"].Value = "Thời gian: " + huongGio.TimeMaxValue.ToString();
                    wsHuongGio.Cells["A5"].Value = "Hướng gió đặc trưng nhiều thứ 2 trong ngày";
                    wsHuongGio.Cells["B5"].Value = GetTenHuongGio(huongGio.MinValue);
                    wsHuongGio.Cells["C5"].Value = "Thời gian: " + huongGio.TimeMinValue.ToString();
                    wsHuongGio.Cells["A7"].Value = "Thời gian";
                    wsHuongGio.Cells["B7"].Value = "Giá trị (s)";
                    wsHuongGio.Cells[string.Format("B{0}", 8)].Value = huongGio.Distance1;
                    wsHuongGio.Cells[string.Format("B{0}", 9)].Value = huongGio.Distance2;
                    wsHuongGio.Cells[string.Format("B{0}", 10)].Value = huongGio.Distance3;
                    wsHuongGio.Cells[string.Format("B{0}", 11)].Value = huongGio.Distance4;
                    wsHuongGio.Cells[string.Format("B{0}", 12)].Value = huongGio.Distance5;
                    wsHuongGio.Cells[string.Format("B{0}", 13)].Value = huongGio.Distance6;
                    wsHuongGio.Cells[string.Format("B{0}", 14)].Value = huongGio.Distance7;
                    wsHuongGio.Cells[string.Format("B{0}", 15)].Value = huongGio.Distance8;
                    //wsHuongGio.Cells[string.Format("B{0}", 14)].Value = huongGio.Distance9;
                    //wsHuongGio.Cells[string.Format("B{0}", 15)].Value = huongGio.Distance10;
                    //wsHuongGio.Cells[string.Format("B{0}", 16)].Value = huongGio.Distance11;
                    //wsHuongGio.Cells[string.Format("B{0}", 17)].Value = huongGio.Distance12;
                    //wsHuongGio.Cells[string.Format("B{0}", 18)].Value = huongGio.Distance13;
                    //wsHuongGio.Cells[string.Format("B{0}", 19)].Value = huongGio.Distance14;
                    //wsHuongGio.Cells[string.Format("B{0}", 20)].Value = huongGio.Distance15;
                    //wsHuongGio.Cells[string.Format("B{0}", 21)].Value = huongGio.Distance16;
                    //wsHuongGio.Cells[string.Format("B{0}", 22)].Value = huongGio.Distance17;
                    //wsHuongGio.Cells[string.Format("B{0}", 23)].Value = huongGio.Distance18;
                    //wsHuongGio.Cells[string.Format("B{0}", 24)].Value = huongGio.Distance19;
                    //wsHuongGio.Cells[string.Format("B{0}", 25)].Value = huongGio.Distance20;
                    //wsHuongGio.Cells[string.Format("B{0}", 26)].Value = huongGio.Distance21;
                    //wsHuongGio.Cells[string.Format("B{0}", 27)].Value = huongGio.Distance22;
                    //wsHuongGio.Cells[string.Format("B{0}", 28)].Value = huongGio.Distance23;
                    //wsHuongGio.Cells[string.Format("B{0}", 29)].Value = huongGio.Distance24;

                    wsHuongGio.Cells[string.Format("A{0}", 8)].Value = "Hướng bắc";
                    wsHuongGio.Cells[string.Format("A{0}", 9)].Value = "Hướng đông bắc";
                    wsHuongGio.Cells[string.Format("A{0}", 10)].Value = "Hướng đông";
                    wsHuongGio.Cells[string.Format("A{0}", 11)].Value = "Hướng đông nam";
                    wsHuongGio.Cells[string.Format("A{0}", 12)].Value = "Hướng nam";
                    wsHuongGio.Cells[string.Format("A{0}", 13)].Value = "Hướng tây nam";
                    wsHuongGio.Cells[string.Format("A{0}", 14)].Value = "Hướng tây";
                    wsHuongGio.Cells[string.Format("A{0}", 15)].Value = "Hướng tây bắc";

                    SetBorderExportExcel(wsHuongGio, cellColump, 8, rowStartAllTable);
                    wsHuongGio.Cells["A:AZ"].AutoFitColumns();
                    wsHuongGio.Cells["A:A"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }
                #endregion
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + "THONG_KE_NGAY_" + date.ToString("dd/MM/yyyy") + ".xls");
                    pck.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            catch { }
        }
        private void SetBorderExportExcel(ExcelWorksheet ws, string[] arr, int count, int rowsStart)
        {
            if (arr != null && arr.Count() > 0)
            {
                for (int i = 0; i <= count; i++)
                {
                    foreach (var item in arr)
                    {
                        ws.Cells[string.Format(item + "{0}", rowsStart + i)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[string.Format(item + "{0}", rowsStart + i)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[string.Format(item + "{0}", rowsStart + i)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[string.Format(item + "{0}", rowsStart + i)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                }
            }
        }
        private double GetMinValue(Report_NhietDo_DoAm_ApSuat_DongChay_DailyModel model)
        {
            double result = 0;           
            if(model != null)
            {
                result = model.Distance1.Value;
                if (model.Distance2.Value< result)
                {
                    result = model.Distance2.Value;
                }
                if (model.Distance3.Value < result)
                {
                    result = model.Distance3.Value;
                }
                if (model.Distance4.Value < result)
                {
                    result = model.Distance4.Value;
                }
                if (model.Distance5.Value < result)
                {
                    result = model.Distance5.Value;
                }
                if (model.Distance6.Value < result)
                {
                    result = model.Distance6.Value;
                }
                if (model.Distance7.Value < result)
                {
                    result = model.Distance7.Value;
                }
                if (model.Distance8.Value < result)
                {
                    result = model.Distance8.Value;
                }
                if (model.Distance9.Value < result)
                {
                    result = model.Distance9.Value;
                }
                if (model.Distance10.Value < result)
                {
                    result = model.Distance11.Value;
                }
                if (model.Distance12.Value < result)
                {
                    result = model.Distance12.Value;
                }
                if (model.Distance13.Value < result)
                {
                    result = model.Distance13.Value;
                }
                if (model.Distance14.Value < result)
                {
                    result = model.Distance14.Value;
                }
                if (model.Distance15.Value < result)
                {
                    result = model.Distance15.Value;
                }
                if (model.Distance16.Value < result)
                {
                    result = model.Distance16.Value;
                }
                if (model.Distance17.Value < result)
                {
                    result = model.Distance17.Value;
                }
                if (model.Distance18.Value < result)
                {
                    result = model.Distance18.Value;
                }
                if (model.Distance19.Value < result)
                {
                    result = model.Distance19.Value;
                }
                if (model.Distance20.Value < result)
                {
                    result = model.Distance20.Value;
                }
                if (model.Distance21.Value < result)
                {
                    result = model.Distance21.Value;
                }
                if (model.Distance22.Value < result)
                {
                    result = model.Distance22.Value;
                }
                if (model.Distance23.Value < result)
                {
                    result = model.Distance23.Value;
                }                
            }
            return result;
        }        
    }
}
