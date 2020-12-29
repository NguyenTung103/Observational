using ES_CapDien.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_CapDien.AppCode
{
    /// <summary>
    /// Lớp xử lý các thao tác lấy dữ liệu từ entity framework
    /// </summary>
    public class ReportDailyLuuLuongDongChayService
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly BaseRepository<ReportDailyLuuLuongDongChay> reportDailyLuuLuongRepository;

        /// <summary>
        /// Khởi tạo dữ liệu
        /// </summary>
        public ReportDailyLuuLuongDongChayService()
        {
            unitOfWork = new UnitOfWork();
            reportDailyLuuLuongRepository = new BaseRepository<ReportDailyLuuLuongDongChay>(unitOfWork);
        }      
        public IQueryable<ReportDailyLuuLuongDongChay> GetByDeviceIdAndDate(DateTime?date, int ? deviceId)
        {
            IQueryable<ReportDailyLuuLuongDongChay> query = reportDailyLuuLuongRepository.GetAll(); //Query lấy điều kiện dữ liệu

            if (date.HasValue)
            {
                query = query.Where(q => q.DateReport==date);
            }
            if (deviceId.HasValue)
            {
                query = query.Where(q => q.DeviceId == deviceId);
            }
            query = query.OrderByDescending(i => i.Id);
            return query;
        }
    }
}