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
    public class AreasService
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly BaseRepository<Area> areaResponsitory;

        /// <summary>
        /// Khởi tạo dữ liệu
        /// </summary>
        public AreasService()
        {
            unitOfWork = new UnitOfWork();
            areaResponsitory = new BaseRepository<Area>(unitOfWork);
        }

        /// <summary>
        /// Lấy dữ liệu hiển thị ra băngr và phân trang
        /// </summary>
        /// <param name="skip">số bản ghi bỏ qua</param>
        /// <param name="take">số bản ghi lấy</param>
        /// <param name="totalRow">tổng số hàng</param>
        /// <param name="title">title trang web</param>
        /// <returns></returns>
        public IQueryable<Area> GetAll(int skip, int take, out int totalRow, string title = "", int? groupId= null)
        {
            IQueryable<Area> query = areaResponsitory.GetAll(); //Query lấy điều kiện dữ liệu

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(q => q.Name.ToLower().Contains(title.ToLower()));
            }
            if (groupId.HasValue)
            {
                query = query.Where(q => q.Group_Id==groupId);
            }
            query = query.OrderBy(q => q.Id); // Sắp xếp dữ liệu lấy ra theo thứ tự tăng dần
            totalRow = query.Count();
            return query.Skip(skip).Take(take);
        }
        public IQueryable<Area> GetAreasByGroupId(int? groupId = null)
        {
            IQueryable<Area> query = areaResponsitory.GetAll(); //Query lấy điều kiện dữ liệu
           
            if (groupId.HasValue)
            {
                query = query.Where(q => q.Group_Id == groupId);
            }           
            return query;
        }
    }
}