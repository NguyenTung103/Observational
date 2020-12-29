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
    public List<DataObservationModel> GetData(DateTime from, DateTime to, int groupId, int? buoc = null, int? siteID = null)
    {
        var QICache = new Cache_Provider();
        string strSession = "GetData_" + from + to + groupId + buoc + siteID;
        List<DataObservationModel> list = new List<DataObservationModel>();
        List<DataObservationModel> result = new List<DataObservationModel>();
        if (!QICache.IsSet(strSession))
        {
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
    #endregion
}
