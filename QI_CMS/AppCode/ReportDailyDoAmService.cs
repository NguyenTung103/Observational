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
    public class ReportDailyDoAmService
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly BaseRepository<ReportDailyDoAm> baseRepository;

        /// <summary>
        /// Khởi tạo dữ liệu
        /// </summary>
        public ReportDailyDoAmService()
        {
            unitOfWork = new UnitOfWork();
            baseRepository = new BaseRepository<ReportDailyDoAm>(unitOfWork);
        }
        public IQueryable<ReportDailyDoAm> GetByDeviceIdAndDate(DateTime? date, int? deviceId)
        {
            IQueryable<ReportDailyDoAm> query = baseRepository.GetAll(); //Query lấy điều kiện dữ liệu

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