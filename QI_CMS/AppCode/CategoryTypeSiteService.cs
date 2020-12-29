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
    public class CategoryTypeSiteService
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly BaseRepository<CategoryTypeSite> categoryTypeSiteRepository;

        /// <summary>
        /// Khởi tạo dữ liệu
        /// </summary>
        public CategoryTypeSiteService()
        {
            unitOfWork = new UnitOfWork();
            categoryTypeSiteRepository = new BaseRepository<CategoryTypeSite>(unitOfWork);
        }              
    }
}