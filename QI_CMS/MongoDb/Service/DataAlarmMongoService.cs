using ES_CapDien.AppCode;
using ES_CapDien.MongoDb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_CapDien.MongoDb.Service
{
    public class DataAlarmMongoService
    {
        DataAlarmData data = new DataAlarmData();
        public List<ES_CapDien.MongoDb.Entity.DataAlarm> GetDataByDay(out int totalRow, int limit)
        {
            List<ES_CapDien.MongoDb.Entity.DataAlarm> list = new List<ES_CapDien.MongoDb.Entity.DataAlarm>();
            DateTime from = DateTime.Today;
            DateTime to = DateTime.Today.AddDays(1);
            list = data.FindAll(i => i.DateCreate < to && i.DateCreate > from).OrderByDescending(i => i.DateCreate).ToList();
            totalRow = list.Count();            
            return list;
        }
        public ES_CapDien.MongoDb.Entity.DataAlarm FindByKey(string alarmId)
        {
            ES_CapDien.MongoDb.Entity.DataAlarm entity = new ES_CapDien.MongoDb.Entity.DataAlarm();
            entity = data.FindByKey(alarmId);            
            return entity;
        }
        public List<ES_CapDien.MongoDb.Entity.DataAlarm> GetDataPaging(DateTime fromDate,DateTime toDate, int skip, int limit, int deviceId, out int total)
        {
            List<ES_CapDien.MongoDb.Entity.DataAlarm> list = new List<ES_CapDien.MongoDb.Entity.DataAlarm>();           
            list = data.FindPagingOption(i =>  i.Device_Id==deviceId, limit, skip,out long totalRow).OrderByDescending(i => i.DateCreate).ToList();
            total = Convert.ToInt32(totalRow);
            return list;
        }
        public List<ES_CapDien.MongoDb.Entity.DataAlarm> GetDataOption(int limit, int deviceid)
        {
            List<ES_CapDien.MongoDb.Entity.DataAlarm> list = new List<ES_CapDien.MongoDb.Entity.DataAlarm>();         
            list = data.FindPagingOption(i => i.Device_Id==deviceid, 300,0,out long total).OrderByDescending(i => i.DateCreate).ToList();           
            return list;
        }
    }
}