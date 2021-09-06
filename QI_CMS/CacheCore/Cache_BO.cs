using ES_CapDien;
using ES_CapDien.AppCode;
using ES_CapDien.Helpers;
using ES_CapDien.Models;
using ES_CapDien.MongoDb.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

public class Cache_BO
{
    private readonly DataObservationMongoService dataObservationMongoService;
    //private readonly ReportDailyService reportDailyService;
    private readonly ReportTypeService reportTypeService;
    private readonly SitesService sitesService;
    public Cache_BO()
    {
        //reportDailyService = new ReportDailyService();
        reportTypeService = new ReportTypeService();
        dataObservationMongoService = new DataObservationMongoService();
        sitesService = new SitesService();
    }

    #region 
    //lấy tên đơn vị theo user ID

    public async Task<ReportMonthlyModel> GetReportByMonth(int deviceId, string type, string Date = "")
    {
        var QICache = new Cache_Provider();
        string strSession = "GetReportByMonth_" + deviceId + type + Date;
        ReportMonthlyModel model = new ReportMonthlyModel();
        if (!QICache.IsSet(strSession))
        {
            double total1 = 0, total2 = 0, total3 = 0;
            int count1 = 0, count2 = 0, count3 = 0;
            DateTime date = DateTime.Today;
            if (Date != "")
            {
                try
                {
                    date = Convert.ToDateTime(Date);
                }
                catch { }
            }
            DateTime distance1 = new DateTime(date.Year, date.Month, 1);
            DateTime distance2 = new DateTime(date.Year, date.Month, 10, 23, 59, 59);
            DateTime distance3 = new DateTime(date.Year, date.Month, 11);
            DateTime distance4 = new DateTime(date.Year, date.Month, 20, 23, 59, 59);
            DateTime distance5 = new DateTime(date.Year, date.Month, 21);
            DateTime distance6 = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);
            var result = await Task.WhenAll(dataObservationMongoService.GetDataByDeviceId(distance1, distance2, deviceId)
                , dataObservationMongoService.GetDataByDeviceId(distance3, distance4, deviceId)
                , dataObservationMongoService.GetDataByDeviceId(distance5, distance6, deviceId));
            foreach (var item in result[0].ToList())
            {
                total1 = CMSHelper.Total(item, type.Trim(), total1);
                count1++;
            }
            double tb1 = total1 / count1;
            foreach (var item in result[1].ToList())
            {
                total2 = CMSHelper.Total(item, type.Trim(), total2);
                count2++;
            }
            double tb2 = total2 / count2;
            foreach (var item in result[2].ToList())
            {
                total3 = CMSHelper.Total(item, type.Trim(), total3);
                count3++;
            }
            double tb3 = total3 / count3;
            ReportType reportType = reportTypeService.GetByCode(type).FirstOrDefault();
            Site site = sitesService.GetByDeviceId(deviceId).FirstOrDefault();
            model.Distance1 = tb1;
            model.Distance2 = tb2;
            model.Distance3 = tb3;
            model.Title = reportType == null ? "" : reportType.Title;
            model.Measure = reportType == null ? "" : reportType.Measure;
            model.Name = reportType == null ? "" : reportType.Description + " " + (site == null ? "" : site.Name);
            model.dayInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            QICache.Set(strSession, model, int.Parse(ConfigurationManager.AppSettings["timeout_cacheserver"]));
        }
        else
        {
            try
            {
                model = QICache.Get(strSession) as ReportMonthlyModel;
            }
            catch
            {
                QICache.Invalidate(strSession);
            }
        }
        return model;
    }
    public List<DataObservationModel> GetDataTheoBuoc(DateTime from, DateTime to, int groupId, int? buoc = null, int? siteID = null)
    {
        var QICache = new Cache_Provider();
        string strSession = "GetDataTheoBuoc_" + from + to + groupId + buoc + siteID;
        List<DataObservationModel> list = new List<DataObservationModel>();
        List<DataObservationModel> result = new List<DataObservationModel>();
        if (!QICache.IsSet(strSession))
        {
            if (siteID.HasValue)
            {
                var lstSite = sitesService.sitesResponsitory.GetAll().ToList();
                int deviceId = sitesService.sitesResponsitory.Single(siteID).DeviceId.Value;
                list = (dataObservationMongoService.GetDataByDeviceIdAndDate(from, to, deviceId)).Where(i => i.DateCreate != null).OrderByDescending(i => i.DateCreate).Select(x => new DataObservationModel
                {
                    BTI = x.BTI,
                    BTO = x.BTO,
                    BHU = x.BHU,
                    BWS = x.BWS,
                    BAP = x.BAP,
                    BAV = x.BAV,
                    BAC = x.BAC,
                    BAF = x.BAF,
                    BPR = x.BPR,
                    NameSite = lstSite.FirstOrDefault(i => i.DeviceId == x.Device_Id).Name,
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
                    list.AddRange((dataObservationMongoService.GetDataByDeviceIdAndDate(from, to, site.DeviceId.Value)).Where(i => i.DateCreate != null).OrderByDescending(i => i.DateCreate).Select(x => new DataObservationModel
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
            QICache.Set(strSession, result, int.Parse(ConfigurationManager.AppSettings["timeout_cacheserver"]));
        }
        else
        {
            try
            {
                result = QICache.Get(strSession) as List<DataObservationModel>;
            }
            catch
            {
                QICache.Invalidate(strSession);
            }
        }
        return result.OrderByDescending(o => o.DateCreate).ToList();
    }
    public List<DataObservationModel> GetDataTheoGio(DateTime from, DateTime to, int groupId, int? siteID = null)
    {
        var QICache = new Cache_Provider();
        string strSession = "GetDataTheoGio_" + from + to + groupId + siteID;
        List<DataObservationModel> list = new List<DataObservationModel>();
        List<DataObservationModel> result = new List<DataObservationModel>();
        if (!QICache.IsSet(strSession))
        {
            if (siteID.HasValue)
            {
                var lstSite = sitesService.sitesResponsitory.GetAll().ToList();
                int deviceId = sitesService.sitesResponsitory.Single(siteID).DeviceId.Value;
                list = (dataObservationMongoService.GetDataByDeviceIdAndDate(from, to, deviceId)).Where(i => i.DateCreate != null).OrderByDescending(i => i.DateCreate).Select(x => new DataObservationModel
                {
                    BTI = x.BTI,
                    BTO = x.BTO,
                    BHU = x.BHU,
                    BWS = x.BWS,
                    BAP = x.BAP,
                    BAV = x.BAV,
                    BAC = x.BAC,
                    BAF = x.BAF,
                    BPR = x.BPR,
                    NameSite = lstSite.FirstOrDefault(i => i.DeviceId == x.Device_Id).Name,
                    DateCreate = x.DateCreate
                }).ToList();
                if (list != null)
                    result.AddRange(GetDataTrungBinhTheoGio(list, from, to));
            }
            else
            {
                List<Site> lstSite = sitesService.GetBygroupId(groupId).ToList();
                foreach (var site in lstSite)
                {
                    var lstDataBySite = (dataObservationMongoService.GetDataByDeviceIdAndDate(from, to, site.DeviceId.Value)).Where(i => i.DateCreate != null).OrderByDescending(i => i.DateCreate).Select(x => new DataObservationModel
                    {
                        BTI = x.BTI,
                        BTO = x.BTO,
                        BHU = x.BHU,
                        BWS = x.BWS,
                        BAP = x.BAP,
                        BAV = x.BAV,
                        BAC = x.BAC,
                        BAF = x.BAF,
                        BPR = x.BPR,
                        NameSite = site.Name,
                        DateCreate = x.DateCreate
                    }).ToList();
                    if (lstDataBySite != null)
                        result.AddRange(GetDataTrungBinhTheoGio(lstDataBySite, from,to));
                }                
            }
            result = result.OrderByDescending(i => i.Date).ThenByDescending(i=>i.Hours).ToList();
            QICache.Set(strSession, result, int.Parse(ConfigurationManager.AppSettings["timeout_cacheserver"]));
        }
        else
        {
            try
            {
                result = QICache.Get(strSession) as List<DataObservationModel>;
            }
            catch
            {
                QICache.Invalidate(strSession);
            }
        }       
        return result.ToList();
    }
    public List<DataObservationModel> GetDataTrungBinhTheoGio(List<DataObservationModel> list, DateTime fromDay, DateTime toDay)
    {
        List<DataObservationModel> result = new List<DataObservationModel>();
        double numberDay = (toDay - fromDay).TotalDays;        
        for(int j=0; j<numberDay; j++)
        {
            DateTime startDay = fromDay.AddDays(j);
            DateTime endDay = fromDay.AddDays(j+1);
            var listData = list.Where(k => k.DateCreate >= startDay && k.DateCreate <= endDay).ToList();
            string nameSite = list.FirstOrDefault().NameSite;
            if (listData.FirstOrDefault() != null)
            {
                DateTime endTimeList = listData.FirstOrDefault().DateCreate.Value;
                for (int i = 1; i <= 24; i++)
                {
                    DateTime start = startDay.AddHours(i - 1);
                    if (start < endTimeList)
                    {
                        DataObservationModel trungBinhTheoGio = new DataObservationModel();
                        DateTime end = startDay.AddHours(i);
                        var lst = list.Where(x => x.DateCreate >= start && x.DateCreate <= end);
                        if (lst != null && lst.Count() > 0)
                        {
                            lst.ToList().ForEach(delegate (DataObservationModel item)
                            {
                                trungBinhTheoGio.BTI += item.BTI;
                                trungBinhTheoGio.BTO += item.BTO;
                                trungBinhTheoGio.BHU += item.BHU;
                                trungBinhTheoGio.BWS += item.BWS;
                                trungBinhTheoGio.BAP += item.BAP;
                                trungBinhTheoGio.BAV += item.BAV;
                                trungBinhTheoGio.BAC += item.BAC;
                                trungBinhTheoGio.BAF += item.BAF;
                                trungBinhTheoGio.BPR += item.BPR;
                                trungBinhTheoGio.NameSite = nameSite;
                            });
                            int totalCount = lst.Count();
                            trungBinhTheoGio.BTI = Math.Round((trungBinhTheoGio.BTI / totalCount), 1);
                            trungBinhTheoGio.BTO = Math.Round((trungBinhTheoGio.BTO / totalCount), 1);
                            trungBinhTheoGio.BHU = Math.Round((trungBinhTheoGio.BHU / totalCount), 1);
                            trungBinhTheoGio.BWS = Math.Round((trungBinhTheoGio.BWS / totalCount), 1);
                            trungBinhTheoGio.BAP = Math.Round((trungBinhTheoGio.BAP / totalCount), 1);
                            trungBinhTheoGio.BAV = Math.Round((trungBinhTheoGio.BAV / totalCount), 1);
                            trungBinhTheoGio.BAC = Math.Round((trungBinhTheoGio.BAC / totalCount), 1);
                            trungBinhTheoGio.BAF = Math.Round((trungBinhTheoGio.BAF / totalCount), 1);
                            trungBinhTheoGio.BPR = Math.Round((trungBinhTheoGio.BPR / totalCount), 1);
                            trungBinhTheoGio.NameSite = nameSite;
                            trungBinhTheoGio.Date = start.Date;
                            trungBinhTheoGio.Hours = start.Hour;
                            trungBinhTheoGio.DateCreate = start;
                            result.Add(trungBinhTheoGio);
                        }
                        else
                        {
                            trungBinhTheoGio.NameSite = nameSite;
                            trungBinhTheoGio.DateCreate = start;
                            trungBinhTheoGio.Date = start.Date;
                            trungBinhTheoGio.Hours = start.Hour;
                            result.Add(trungBinhTheoGio);
                        }

                    }
                }
            }
        }    
              
        return result;
    }
    public DataObservationModel GetDataReport(DateTime date, double addSecond, int i, List<DataObservationModel> list)
    {
        DataObservationModel duLieuThemVaoListKetQua = new DataObservationModel();

        try
        {
            DateTime dateStart = new DateTime();
            DateTime dateEnd = new DateTime();
            dateEnd = date.AddSeconds(addSecond);
            dateStart = date;
            duLieuThemVaoListKetQua = list.FirstOrDefault(k => k.DateCreate >= dateEnd && k.DateCreate <= dateStart);
            if (duLieuThemVaoListKetQua == null)
            {
                duLieuThemVaoListKetQua = GetDataReport(dateEnd, addSecond, i++, list);
            }
            else
            {
                duLieuThemVaoListKetQua.DateCreate = dateStart;
            }
        }
        catch (Exception ex)
        {

        }
        return duLieuThemVaoListKetQua;
    }
    public double GetTotalSeconds(DateTime date)
    {
        double result = 0;
        double soGiayTrongThang = 0;
        for (int i = date.Month - 1; i > 0; i--)
        {
            int day = CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(date.Year, date.Month);
            soGiayTrongThang += (day * 86400);
        }

        double soGiayTrongNgay = date.Day * 86400 + date.Hour * 3600 + date.Minute * 60 + date.Second;
        result += soGiayTrongThang + soGiayTrongNgay;
        return result;
    }
    #endregion
}
