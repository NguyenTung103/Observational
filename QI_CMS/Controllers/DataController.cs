using Administrator;
using ES_CapDien.AppCode;
using ES_CapDien.AppCode.Interface;
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
        public DataController()
        {
            dataObservationMongoService = new DataObservationMongoService();
            areasService = new AreasService();
            sitesService = new SitesService();
            groupService = new GroupService();
            userProfileService = new UserProfileService();
        }
        public ActionResult Management(int page = 1, int pageSize = 50, string title = "", int? areaId = null, string fromDate = "",string toDate="", int? siteID = null)
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
            if (siteID.HasValue)
            {
                int deviceId = sitesService.sitesResponsitory.Single(siteID).DeviceId.Value;
                list = dataObservationMongoService.GetDataPagingByDeviceId(from, to, deviceId, skip, pageSize, out int total).OrderByDescending(i => i.DateCreate).Select(x => new DataObservationModel
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
                totalRows = total;
            }
            else
            {
                list = dataObservationMongoService.GetDataPaging(from, to, skip, pageSize, out int total).OrderByDescending(i => i.DateCreate).Select(x => new DataObservationModel
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
                totalRows= total;
            }
           


            #region Hiển thị dữ liệu và phân trang
            DataObservationViewModel viewModel = new DataObservationViewModel
            {
                DataO = new StaticPagedList<DataObservationModel>(list, page, pageSize, totalRows),
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
        public void ExportExel(string title = "", int? areaId = null, string fromDate = "", string toDate = "", int? siteID = null)
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
                List<DataObservationModel> list = new List<DataObservationModel>();
                int totalRows = 0;
                if (siteID.HasValue)
                {
                    int deviceId = sitesService.sitesResponsitory.Single(siteID).DeviceId.Value;
                    list = dataObservationMongoService.GetDataPagingByDeviceId(from, to, deviceId, 0, 1000, out int total).OrderByDescending(i => i.DateCreate).Select(x => new DataObservationModel
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
                    totalRows = total;
                }
                else
                {
                    list = dataObservationMongoService.GetDataPaging(from, to, 0, 1000, out int total).OrderByDescending(i => i.DateCreate).Select(x => new DataObservationModel
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
                    totalRows = total;
                }

                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
                ws.Cells["A2:F2"].Merge = true;
                ws.Cells["A3:F3"].Merge = true;
                ws.Cells["A2"].Value = "THỐNG KÊ QUAN TRẮC";
                ws.Cells["A3"].Value = "Từ ngày " + from.ToString() + " đến ngày " + to.ToString();
                ws.Cells["A5"].Value = "STT";
                ws.Cells["B5"].Value = "Trạm";
                ws.Cells["C5"].Value = "Thời gian";
                ws.Cells["D5"].Value = "Nhiệt độ môi trường (oC)";
                ws.Cells["E5"].Value = "Nhiệt độ trong (oC)";
                ws.Cells["F5"].Value = "Độ ẩm môi trường (%)";
                ws.Cells["G5"].Value = "Tốc độ gió";
                ws.Cells["H5"].Value = "Hướng gió";
                ws.Cells["I5"].Value = "Áp suất KQ (hPA)";
                ws.Cells["J5"].Value = "Lượng mưa (mm)";
                ws.Cells["K5"].Value = "Mực nước (m)";
                int rowsStart = 6;
                int sTT = 1;
                //ws.Row(5).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //ws.Row(5).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("pink")));
                foreach (var item in list)
                {
                    ws.Cells[string.Format("A{0}", rowsStart)].Value = sTT;
                    ws.Cells[string.Format("B{0}", rowsStart)].Value = item.NameSite;
                    ws.Cells[string.Format("C{0}", rowsStart)].Value = item.DateCreate.ToString();
                    ws.Cells[string.Format("D{0}", rowsStart)].Value = item.BTI;
                    ws.Cells[string.Format("E{0}", rowsStart)].Value = item.BTO;
                    ws.Cells[string.Format("F{0}", rowsStart)].Value = item.BHU;
                    ws.Cells[string.Format("G{0}", rowsStart)].Value = item.BWS;
                    ws.Cells[string.Format("H{0}", rowsStart)].Value = item.BAP;
                    ws.Cells[string.Format("I{0}", rowsStart)].Value = item.BAV;
                    ws.Cells[string.Format("J{0}", rowsStart)].Value = item.BAC;
                    ws.Cells[string.Format("K{0}", rowsStart)].Value = item.BAF;
                    
                    rowsStart++;
                    sTT++;
                }
                string[] cellColump = { "A", "B", "C", "D", "E", "F", "G", "H","I","J","K" };
                int rowStartAllTable = 5;
                SetBorderExportExcel(ws, cellColump, list.Count, rowStartAllTable);
                ws.Cells["A:AZ"].AutoFitColumns();
                ws.Cells["A:A"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;               
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + "Thống kê Từ ngày " + from.ToString() + " đến ngày " + to.ToString()+".xlsx");
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

    }
}
