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
    public class ReportDailyApSuatService
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly BaseRepository<ReportDailyApSuat> reportDailyApSuatRepository;

        /// <summary>
        /// Khởi tạo dữ liệu
        /// </summary>
        public ReportDailyApSuatService()
        {
            unitOfWork = new UnitOfWork();
            reportDailyApSuatRepository = new BaseRepository<ReportDailyApSuat>(unitOfWork);
        }      
        public IQueryable<ReportDailyApSuat> GetByDeviceIdAndDate(DateTime?date, int ? deviceId)
        {
            IQueryable<ReportDailyApSuat> query = reportDailyApSuatRepository.GetAll(); //Query lấy điều kiện dữ liệu

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