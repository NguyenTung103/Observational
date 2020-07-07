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
    public class ReportDailyHuongGioService
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly BaseRepository<ReportDailyHuongGio> baseRepository;

        /// <summary>
        /// Khởi tạo dữ liệu
        /// </summary>
        public ReportDailyHuongGioService()
        {
            unitOfWork = new UnitOfWork();
            baseRepository = new BaseRepository<ReportDailyHuongGio>(unitOfWork);
        }
        public IQueryable<ReportDailyHuongGio> GetByDeviceIdAndDate(DateTime? date, int? deviceId)
        {
            IQueryable<ReportDailyHuongGio> query = baseRepository.GetAll(); //Query lấy điều kiện dữ liệu

            if (date.HasValue)
            {
                query = query.Where(q => q.DateReport == date);
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