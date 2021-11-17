using Administrator;
using ES_CapDien.AppCode;
using ES_CapDien.AppCode.Interface;
using ES_CapDien.Helpers;
using ES_CapDien.Models;
using ES_CapDien.MongoDb.Service;
using HelperLibrary;
using Ionic.Zip;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ES_CapDien.Controllers
{
    public class DataController : BaseController
    {
        //
        // GET: /Data/
        private readonly DataObservationMongoService dataObservationMongoService;
        private readonly AreasService areasService;
        private readonly SitesService sitesService;
        private readonly GroupService groupService;
        private readonly UserProfileService userProfileService;
        Cache_BO cacheBO = new Cache_BO();
        public DataController()
        {
            dataObservationMongoService = new DataObservationMongoService();
            areasService = new AreasService();
            sitesService = new SitesService();
            groupService = new GroupService();
            userProfileService = new UserProfileService();
        }
        public async Task<ActionResult> Management(int page = 1, int pageSize = 50, string title = "", int? areaId = null, string fromDate = "", string toDate = "", int? siteID = null, int? buoc = null)
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
            int groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id.Value;
            ViewBag.lstTram = sitesService.GetBygroupId(groupId).ToList();
            string userName = User.Identity.Name;
            int skip = (page - 1) * pageSize;
            DateTime from = DateTime.Today;
            DateTime to = from.AddDays(1);
            if (fromDate != "")
            {
                try
                {
                    from = Convert.ToDateTime(fromDate);
                    to = Convert.ToDateTime(toDate);
                }
                catch { }
            }
            List<DataObservationModel> list = new List<DataObservationModel>();
            int totalRows = 0;
            list = cacheBO.GetDataTheoBuoc(from, to, groupId, buoc, siteID);
            totalRows = await GetCountData(from, to, groupId, buoc, siteID);
            int typeSite = 0;
            if (siteID.HasValue)
            {
                typeSite = sitesService.sitesResponsitory.Single(siteID).TypeSiteId;
            }
            ViewBag.TypeSite = typeSite;
            #region Hiển thị dữ liệu và phân trang
            DataObservationViewModel viewModel = new DataObservationViewModel
            {
                DataO = new StaticPagedList<DataObservationModel>(list.Skip(skip).Take(pageSize), page, pageSize, totalRows),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = totalRows
                },
                From = from,
                To = to,
            };
            #endregion
            return View(viewModel);
        }
        public ActionResult ManagementByHours(int page = 1, int pageSize = 50, string title = "", int? areaId = null, string fromDate = "", string toDate = "", int? siteID = null)
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
            int groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id.Value;
            List<Site> lstSite = sitesService.GetBygroupId(groupId).ToList();
            ViewBag.lstTram = lstSite;
            string userName = User.Identity.Name;
            int skip = (page - 1) * pageSize;
            DateTime from = DateTime.Today;
            DateTime to = from.AddDays(1);
            int typeSite = 0;
            if (siteID.HasValue)
            {
                typeSite = sitesService.sitesResponsitory.Single(siteID).TypeSiteId;
            }
            else
            {
                siteID = lstSite.FirstOrDefault().Id;
            }
            if (fromDate != "")
            {
                try
                {
                    from = Convert.ToDateTime(fromDate);
                    to = Convert.ToDateTime(toDate);
                }
                catch { }
            }
            List<DataObservationModel> list = new List<DataObservationModel>();
            int totalRows = 0;
            list = cacheBO.GetDataTheoGio(from, to, groupId, siteID);
            ViewBag.TypeSite = typeSite;
            #region Hiển thị dữ liệu và phân trang
            DataObservationViewModel viewModel = new DataObservationViewModel
            {
                DataO = new StaticPagedList<DataObservationModel>(list.Skip(skip).Take(pageSize), page, pageSize, list.Count()),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = totalRows
                },
                From = from,
                To = to,
            };
            #endregion
            return View(viewModel);
        }
        public async Task<List<DataObservationModel>> GetDataAsync(DateTime from, DateTime to, int groupId, int? buoc = null, int? siteID = null)
        {
            List<DataObservationModel> list = new List<DataObservationModel>();
            List<DataObservationModel> result = new List<DataObservationModel>();
            if (siteID.HasValue)
            {
                int deviceId = sitesService.sitesResponsitory.Single(siteID).DeviceId.Value;

                list = (await dataObservationMongoService.GetDataByDeviceId(from, to, deviceId)).OrderByDescending(i => i.DateCreate).Select(x => new DataObservationModel
                {
                    BTI = x.BTI,
                    BTO = x.BTO,
                    BHU = x.BHU,
                    BWS = x.BWS,
                    BAP = x.BAP,
                    BAV = x.BAV,
                    BAC = x.BAC,
                    BAF = x.BAF,
                    NameSite = sitesService.sitesResponsitory.GetAll().Where(i => i.DeviceId == x.Device_Id).FirstOrDefault().Name,
                    DateCreate = x.DateCreate
                }).ToList();
                if (buoc.HasValue)
                {
                    int countFor = list.Count() / buoc.Value;
                    for (int i = 0; i <= countFor - 1; i++)
                    {
                        DataObservationModel item = list.Skip(i * buoc.Value).FirstOrDefault();
                        if (item != null)
                            result.Add(item);
                    }
                }
                else
                {
                    result.AddRange(list);
                }

            }
            else
            {
                List<Site> lstSite = sitesService.GetBygroupId(groupId).ToList();
                foreach (var site in lstSite)
                {
                    list.AddRange((await dataObservationMongoService.GetDataByDeviceId(from, to, site.DeviceId.Value)).OrderByDescending(i => i.DateCreate).Select(x => new DataObservationModel
                    {
                        BTI = x.BTI,
                        BTO = x.BTO,
                        BHU = x.BHU,
                        BWS = x.BWS,
                        BAP = x.BAP,
                        BAV = x.BAV,
                        BAC = x.BAC,
                        BAF = x.BAF,
                        NameSite = site.Name,
                        DateCreate = x.DateCreate
                    }).ToList());
                }
                if (buoc.HasValue)
                {
                    int countFor = list.Count() / buoc.Value;
                    for (int i = 0; i <= countFor - 1; i++)
                    {
                        DataObservationModel item = list.Skip(i * buoc.Value).FirstOrDefault();
                        if (item != null)
                            result.Add(item);
                    }
                }
                else
                {
                    result.AddRange(list);
                }
            }
            return result.OrderByDescending(o => o.DateCreate).ToList();
        }
        public List<DataObservationModel> GetData(DateTime from, DateTime to, int groupId, int? buoc = null, int? siteID = null)
        {
            List<DataObservationModel> list = new List<DataObservationModel>();
            List<DataObservationModel> result = new List<DataObservationModel>();
            if (siteID.HasValue)
            {
                int deviceId = sitesService.sitesResponsitory.Single(siteID).DeviceId.Value;

                list = (dataObservationMongoService.GetDataByDeviceIdAndDate(from, to, deviceId)).OrderByDescending(i => i.DateCreate).Select(x => new DataObservationModel
                {
                    BTI = x.BTI,
                    BTO = x.BTO,
                    BHU = x.BHU,
                    BWS = x.BWS,
                    BAP = x.BAP,
                    BAV = x.BAV,
                    BAC = x.BAC,
                    BAF = x.BAF,
                    NameSite = sitesService.sitesResponsitory.GetAll().Where(i => i.DeviceId == x.Device_Id).FirstOrDefault().Name,
                    DateCreate = x.DateCreate
                }).ToList();
                if (buoc.HasValue)
                {
                    int countFor = list.Count() / buoc.Value;
                    for (int i = 0; i <= countFor - 1; i++)
                    {
                        DataObservationModel item = list.Skip(i * buoc.Value).FirstOrDefault();
                        if (item != null)
                            result.Add(item);
                    }
                }
                else
                {
                    result.AddRange(list);
                }

            }
            else
            {
                List<Site> lstSite = sitesService.GetBygroupId(groupId).ToList();
                foreach (var site in lstSite)
                {
                    list.AddRange((dataObservationMongoService.GetDataByDeviceIdAndDate(from, to, site.DeviceId.Value)).OrderByDescending(i => i.DateCreate).Select(x => new DataObservationModel
                    {
                        BTI = x.BTI,
                        BTO = x.BTO,
                        BHU = x.BHU,
                        BWS = x.BWS,
                        BAP = x.BAP,
                        BAV = x.BAV,
                        BAC = x.BAC,
                        BAF = x.BAF,
                        NameSite = site.Name,
                        DateCreate = x.DateCreate
                    }).ToList());
                }
                if (buoc.HasValue)
                {
                    int countFor = list.Count() / buoc.Value;
                    for (int i = 0; i <= countFor - 1; i++)
                    {
                        DataObservationModel item = list.Skip(i * buoc.Value).FirstOrDefault();
                        if (item != null)
                            result.Add(item);
                    }
                }
                else
                {
                    result.AddRange(list);
                }
            }
            return result.OrderByDescending(o => o.DateCreate).ToList();
        }
        public async Task<int> GetCountData(DateTime from, DateTime to, int groupId, int? buoc = null, int? siteID = null)
        {
            List<DataObservationModel> list = new List<DataObservationModel>();
            List<DataObservationModel> result = new List<DataObservationModel>();
            if (siteID.HasValue)
            {
                int deviceId = sitesService.sitesResponsitory.Single(siteID).DeviceId.Value;

                list = (await dataObservationMongoService.GetDataByDeviceId(from, to, deviceId)).OrderByDescending(i => i.DateCreate).Select(x => new DataObservationModel
                {
                    BTI = x.BTI,
                    BTO = x.BTO,
                    BHU = x.BHU,
                    BWS = x.BWS,
                    BAP = x.BAP,
                    BAV = x.BAV,
                    BAC = x.BAC,
                    BAF = x.BAF,
                    NameSite = sitesService.sitesResponsitory.GetAll().Where(i => i.DeviceId == x.Device_Id).FirstOrDefault().Name,
                    DateCreate = x.DateCreate
                }).ToList();
                if (buoc.HasValue)
                {
                    int countFor = list.Count() / buoc.Value;
                    for (int i = 0; i <= countFor - 1; i++)
                    {
                        DataObservationModel item = list.Skip(i * buoc.Value).FirstOrDefault();
                        if (item != null)
                            result.Add(item);
                    }
                }
                else
                {
                    result.AddRange(list);
                }
            }
            else
            {
                List<Site> lstSite = sitesService.GetBygroupId(groupId).ToList();
                foreach (var site in lstSite)
                {
                    list.AddRange((await dataObservationMongoService.GetDataByDeviceId(from, to, site.DeviceId.Value)).OrderByDescending(i => i.DateCreate).Select(x => new DataObservationModel
                    {
                        BTI = x.BTI,
                        BTO = x.BTO,
                        BHU = x.BHU,
                        BWS = x.BWS,
                        BAP = x.BAP,
                        BAV = x.BAV,
                        BAC = x.BAC,
                        BAF = x.BAF,
                        NameSite = site.Name,
                        DateCreate = x.DateCreate
                    }).ToList());
                }
                if (buoc.HasValue)
                {
                    int countFor = list.Count() / buoc.Value;
                    for (int i = 0; i <= countFor - 1; i++)
                    {
                        DataObservationModel item = list.Skip(i * buoc.Value).FirstOrDefault();
                        if (item != null)
                            result.Add(item);
                    }
                }
                else
                {
                    result.AddRange(list);
                }
            }
            return result.Count();
        }
        public void ExportExelTheoBuoc(string title = "", int? areaId = null, string fromDate = "", string toDate = "", int? siteID = null, int? buoc = null)
        {
            try
            {
                DateTime from = DateTime.Today;
                DateTime to = from.AddDays(1);
                if (fromDate != "")
                {
                    try
                    {
                        from = Convert.ToDateTime(fromDate);
                        to = Convert.ToDateTime(toDate);
                    }
                    catch { }
                }
                int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
                int groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id.Value;
                List<DataObservationModel> list = new List<DataObservationModel>();
                list = cacheBO.GetDataTheoBuoc(from, to, groupId, buoc, siteID);

                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
                ws.Cells["A2:F2"].Merge = true;
                ws.Cells["A3:F3"].Merge = true;
                ws.Cells["A2"].Value = "THỐNG KÊ QUAN TRẮC";
                ws.Cells["A3"].Value = "Từ ngày " + from.ToString() + " đến ngày " + to.ToString();
                ws.Cells["A5"].Value = "STT";
                ws.Cells["B5"].Value = "Trạm";
                ws.Cells["C5"].Value = "Thời gian";
                int typeSite = 0;
                int rowsStart = 6;
                int sTT = 1;
                if (siteID.HasValue)
                {
                    typeSite = sitesService.sitesResponsitory.Single(siteID).TypeSiteId;
                }
                if (typeSite == 1)
                {
                    ws.Cells["D5"].Value = "Nhiệt độ môi trường (oC)";
                    ws.Cells["E5"].Value = "Độ ẩm môi trường (%)";
                    ws.Cells["F5"].Value = "Tốc độ gió";
                    ws.Cells["G5"].Value = "Hướng gió";
                    ws.Cells["H5"].Value = "Áp suất KQ (hPA)";
                    ws.Cells["I5"].Value = "Lượng mưa (mm)";
                    ws.Cells["J5"].Value = "Bức xạ mặt trời (W/m2)";
                    //ws.Row(5).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //ws.Row(5).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("pink")));
                    foreach (var item in list)
                    {
                        ws.Cells[string.Format("A{0}", rowsStart)].Value = sTT;
                        ws.Cells[string.Format("B{0}", rowsStart)].Value = item.NameSite;
                        ws.Cells[string.Format("C{0}", rowsStart)].Value = item.DateCreate.Value.ToString("dd/MM/yyyy HH:mm");
                        ws.Cells[string.Format("D{0}", rowsStart)].Value = item.BTI;
                        ws.Cells[string.Format("E{0}", rowsStart)].Value = item.BHU;
                        ws.Cells[string.Format("F{0}", rowsStart)].Value = item.BWS;
                        ws.Cells[string.Format("G{0}", rowsStart)].Value = item.BAP;
                        ws.Cells[string.Format("H{0}", rowsStart)].Value = item.BAV;
                        ws.Cells[string.Format("I{0}", rowsStart)].Value = item.BAC;
                        ws.Cells[string.Format("J{0}", rowsStart)].Value = item.BPR;

                        rowsStart++;
                        sTT++;
                    }
                    string[] cellColump = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
                    int rowStartAllTable = 5;
                    SetBorderExportExcel(ws, cellColump, list.Count, rowStartAllTable);
                }
                else if (typeSite == 2)
                {
                    ws.Cells["D5"].Value = "Mực nước (m)";
                    //ws.Row(5).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //ws.Row(5).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("pink")));
                    foreach (var item in list)
                    {
                        ws.Cells[string.Format("A{0}", rowsStart)].Value = sTT;
                        ws.Cells[string.Format("B{0}", rowsStart)].Value = item.NameSite;
                        ws.Cells[string.Format("C{0}", rowsStart)].Value = item.DateCreate.Value.ToString("dd/MM/yyyy HH:mm");
                        ws.Cells[string.Format("D{0}", rowsStart)].Value = item.BAF;

                        rowsStart++;
                        sTT++;
                    }
                    string[] cellColump = { "A", "B", "C", "D" };
                    int rowStartAllTable = 5;
                    SetBorderExportExcel(ws, cellColump, list.Count, rowStartAllTable);
                }
                else
                {
                    ws.Cells["D5"].Value = "Nhiệt độ môi trường (oC)";
                    ws.Cells["E5"].Value = "Độ ẩm môi trường (%)";
                    ws.Cells["F5"].Value = "Tốc độ gió";
                    ws.Cells["G5"].Value = "Hướng gió";
                    ws.Cells["H5"].Value = "Áp suất KQ (hPA)";
                    ws.Cells["I5"].Value = "Lượng mưa (mm)";
                    ws.Cells["J5"].Value = "Mực nước (m)";
                    ws.Cells["K5"].Value = "Bức xạ mặt trời (W/m2)";
                    //ws.Row(5).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //ws.Row(5).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("pink")));
                    foreach (var item in list)
                    {
                        ws.Cells[string.Format("A{0}", rowsStart)].Value = sTT;
                        ws.Cells[string.Format("B{0}", rowsStart)].Value = item.NameSite;
                        ws.Cells[string.Format("C{0}", rowsStart)].Value = item.DateCreate.Value.ToString("dd/MM/yyyy HH:mm");
                        ws.Cells[string.Format("D{0}", rowsStart)].Value = item.BTI;
                        ws.Cells[string.Format("E{0}", rowsStart)].Value = item.BHU;
                        ws.Cells[string.Format("F{0}", rowsStart)].Value = item.BWS;
                        ws.Cells[string.Format("G{0}", rowsStart)].Value = item.BAP;
                        ws.Cells[string.Format("H{0}", rowsStart)].Value = item.BAV;
                        ws.Cells[string.Format("I{0}", rowsStart)].Value = item.BAC;
                        ws.Cells[string.Format("J{0}", rowsStart)].Value = item.BAF;
                        ws.Cells[string.Format("K{0}", rowsStart)].Value = item.BPR;

                        rowsStart++;
                        sTT++;
                    }
                    string[] cellColump = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K" };
                    int rowStartAllTable = 5;
                    SetBorderExportExcel(ws, cellColump, list.Count, rowStartAllTable);
                }



                ws.Cells["A:AZ"].AutoFitColumns();
                ws.Cells["A:A"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + "Thống kê Từ ngày " + from.ToString() + " đến ngày " + to.ToString() + ".xlsx");
                    pck.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            catch { }
        }
        public void ExportExelTheoGio(string title = "", int? areaId = null, string fromDate = "", string toDate = "", int? siteID = null)
        {
            try
            {
                DateTime from = DateTime.Today;
                DateTime to = from.AddDays(1);
                if (fromDate != "")
                {
                    try
                    {
                        from = Convert.ToDateTime(fromDate);
                        to = Convert.ToDateTime(toDate);
                    }
                    catch { }
                }
                int CurrentUserId = WebMatrix.WebData.WebSecurity.CurrentUserId;
                int groupId = userProfileService.userProfileResponsitory.Single(CurrentUserId).Group_Id.Value;
                List<DataObservationModel> list = new List<DataObservationModel>();
                Site tram = new Site();
                if (!siteID.HasValue)
                {
                    List<Site> lstSite = sitesService.GetBygroupId(groupId).ToList();
                    tram = lstSite.FirstOrDefault();
                    siteID = tram.Id;
                }
                list = cacheBO.GetDataTheoGio(from, to, groupId, siteID);

                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
                ws.Cells["A2:F2"].Merge = true;
                ws.Cells["A3:F3"].Merge = true;
                ws.Cells["A2"].Value = "THỐNG KÊ QUAN TRẮC";
                ws.Cells["A3"].Value = "Từ ngày " + from.ToString("dd/MM/yyyy") + " đến ngày " + to.ToString("dd/MM/yyyy");
                ws.Cells["A5"].Value = "STT";
                ws.Cells["B5"].Value = "Trạm";
                ws.Cells["C5"].Value = "Ngày";
                ws.Cells["D5"].Value = "Thời gian";
                int typeSite = 0;
                int rowsStart = 6;
                int sTT = 1;
                if (siteID.HasValue)
                {
                    typeSite = sitesService.sitesResponsitory.Single(siteID).TypeSiteId;
                }
                if (typeSite == 1)
                {
                    ws.Cells["E5"].Value = "Nhiệt độ môi trường (oC)";
                    ws.Cells["F5"].Value = "Độ ẩm môi trường (%)";
                    ws.Cells["G5"].Value = "Tốc độ gió";
                    ws.Cells["H5"].Value = "Hướng gió";
                    ws.Cells["I5"].Value = "Áp suất KQ (hPA)";
                    ws.Cells["J5"].Value = "Lượng mưa (mm)";
                    ws.Cells["K5"].Value = "Bức xạ mặt trời (W/m2)";
                    //ws.Row(5).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //ws.Row(5).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("pink")));
                    foreach (var item in list)
                    {
                        ws.Cells[string.Format("A{0}", rowsStart)].Value = sTT;
                        ws.Cells[string.Format("B{0}", rowsStart)].Value = item.NameSite;
                        ws.Cells[string.Format("C{0}", rowsStart)].Value = item.DateCreate.Value.ToString("dd/MM/yyyy");
                        ws.Cells[string.Format("D{0}", rowsStart)].Value = item.DateCreate.Value.ToString("HH:mm");
                        ws.Cells[string.Format("E{0}", rowsStart)].Value = item.BTI;
                        ws.Cells[string.Format("F{0}", rowsStart)].Value = item.BHU;
                        ws.Cells[string.Format("G{0}", rowsStart)].Value = item.BWS;
                        ws.Cells[string.Format("H{0}", rowsStart)].Value = item.BAP;
                        ws.Cells[string.Format("I{0}", rowsStart)].Value = item.BAV;
                        ws.Cells[string.Format("J{0}", rowsStart)].Value = item.BAC;
                        ws.Cells[string.Format("K{0}", rowsStart)].Value = item.BPR;

                        rowsStart++;
                        sTT++;
                    }
                    string[] cellColump = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K" };
                    int rowStartAllTable = 5;
                    SetBorderExportExcel(ws, cellColump, list.Count, rowStartAllTable);
                }
                else if (typeSite == 2)
                {
                    ws.Cells["E5"].Value = "Mực nước (m)";
                    //ws.Row(5).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //ws.Row(5).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("pink")));
                    foreach (var item in list)
                    {
                        ws.Cells[string.Format("A{0}", rowsStart)].Value = sTT;
                        ws.Cells[string.Format("B{0}", rowsStart)].Value = item.NameSite;
                        ws.Cells[string.Format("C{0}", rowsStart)].Value = item.DateCreate.Value.ToString("dd/MM/yyyy");
                        ws.Cells[string.Format("D{0}", rowsStart)].Value = item.DateCreate.Value.ToString("HH:mm") + " - " + item.DateCreate.Value.AddHours(-1).ToString("HH:mm");
                        ws.Cells[string.Format("E{0}", rowsStart)].Value = item.BAF;

                        rowsStart++;
                        sTT++;
                    }
                    string[] cellColump = { "A", "B", "C", "D", "E" };
                    int rowStartAllTable = 5;
                    SetBorderExportExcel(ws, cellColump, list.Count, rowStartAllTable);
                }
                else
                {
                    ws.Cells["E5"].Value = "Nhiệt độ môi trường (oC)";
                    ws.Cells["F5"].Value = "Độ ẩm môi trường (%)";
                    ws.Cells["G5"].Value = "Tốc độ gió";
                    ws.Cells["H5"].Value = "Hướng gió";
                    ws.Cells["I5"].Value = "Áp suất KQ (hPA)";
                    ws.Cells["J5"].Value = "Lượng mưa (mm)";
                    ws.Cells["K5"].Value = "Mực nước (m)";
                    //ws.Row(5).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //ws.Row(5).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("pink")));
                    foreach (var item in list)
                    {
                        ws.Cells[string.Format("A{0}", rowsStart)].Value = sTT;
                        ws.Cells[string.Format("B{0}", rowsStart)].Value = item.NameSite;
                        ws.Cells[string.Format("C{0}", rowsStart)].Value = item.DateCreate.Value.ToString("dd/MM/yyyy");
                        ws.Cells[string.Format("D{0}", rowsStart)].Value = item.DateCreate.Value.ToString("HH:mm") + " - " + item.DateCreate.Value.AddHours(-1).ToString("HH:mm");
                        ws.Cells[string.Format("E{0}", rowsStart)].Value = item.BTI;
                        ws.Cells[string.Format("F{0}", rowsStart)].Value = item.BHU;
                        ws.Cells[string.Format("G{0}", rowsStart)].Value = item.BWS;
                        ws.Cells[string.Format("H{0}", rowsStart)].Value = item.BAP;
                        ws.Cells[string.Format("I{0}", rowsStart)].Value = item.BAV;
                        ws.Cells[string.Format("J{0}", rowsStart)].Value = item.BAC;
                        ws.Cells[string.Format("K{0}", rowsStart)].Value = item.BAF;

                        rowsStart++;
                        sTT++;
                    }
                    string[] cellColump = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K" };
                    int rowStartAllTable = 5;
                    SetBorderExportExcel(ws, cellColump, list.Count, rowStartAllTable);
                }



                ws.Cells["A:AZ"].AutoFitColumns();
                ws.Cells["A:A"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + "Thống kê theo giờ từ ngày " + from.ToString("dd/MM/yyyy") + " đến ngày " + to.ToString("dd/MM/yyyy") + "_" + tram.Name + ".xlsx");
                    pck.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            catch { }
        }
        public ActionResult DotnetZip(string ZipName, string ExcelName, List<ExcelPackage> lstpackage)
        {
            using (var stream = new MemoryStream())
            {
                using (ZipFile zip = new ZipFile())
                {
                    foreach (var package in lstpackage)
                    {
                        var sheet = package.Workbook.Worksheets.Add("Sheet1");
                        zip.AddEntry(ExcelName, package.GetAsByteArray());
                        zip.Save(stream);
                    }
                    return File(
                            stream.ToArray(),
                            System.Net.Mime.MediaTypeNames.Application.Zip,
                            ZipName
                        );
                }
            }
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
    }
}
