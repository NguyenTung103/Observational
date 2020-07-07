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
        string strSession = "GetReportByMonth_" + deviceId+ type+ Date;
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
            var result = await Task.WhenAll(dataObservationMongoService.GetDatayDeviceId(distance1, distance2, deviceId)
                , dataObservationMongoService.GetDatayDeviceId(distance3, distance4, deviceId)
                , dataObservationMongoService.GetDatayDeviceId(distance5, distance6, deviceId));
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
    #endregion
}
