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
    public class ReportTypeService
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly BaseRepository<ReportType> reportTypeRepository;

        /// <summary>
        /// Khởi tạo dữ liệu
        /// </summary>
        public ReportTypeService()
        {
            unitOfWork = new UnitOfWork();
            reportTypeRepository = new BaseRepository<ReportType>(unitOfWork);
        }
        public IQueryable<ReportType> GetByCode(string code = "")
        {
            IQueryable<ReportType> query = reportTypeRepository.GetAll(); //Query lấy điều kiện dữ liệu
           
            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(q => q.Code == code);
            }           
            return query;
        }
    }
}