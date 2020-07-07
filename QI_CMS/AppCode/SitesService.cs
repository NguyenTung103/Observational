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
    public class SitesService
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly BaseRepository<Site> sitesResponsitory;

        /// <summary>
        /// Khởi tạo dữ liệu
        /// </summary>
        public SitesService()
        {
            unitOfWork = new UnitOfWork();
            sitesResponsitory = new BaseRepository<Site>(unitOfWork);
        }

        /// <summary>
        /// Lấy dữ liệu hiển thị ra băngr và phân trang
        /// </summary>
        /// <param name="skip">số bản ghi bỏ qua</param>
        /// <param name="take">số bản ghi lấy</param>
        /// <param name="totalRow">tổng số hàng</param>
        /// <param name="title">title trang web</param>
        /// <returns></returns>
        public IQueryable<Site> GetAll(int skip, int take, out int totalRow, string title = "", int? areaId=null, int? groupId=null)
        {
            IQueryable<Site> query = sitesResponsitory.GetAll(); //Query lấy điều kiện dữ liệu

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(q => q.Name.ToLower().Contains(title.ToLower()));
            }
            if (areaId.HasValue)
            {
                query = query.Where(q => q.Area_Id==areaId);
            }
            if (groupId.HasValue)
            {
                query = query.Where(q => q.Group_Id == groupId);
            }
            query = query.OrderBy(q => q.Id); // Sắp xếp dữ liệu lấy ra theo thứ tự tăng dần
            totalRow = query.Count();
            return query.Skip(skip).Take(take);
        }
        /// <summary>
        /// Lấy dữ liệu hiển thị ra băngr và phân trang
        /// </summary>
        /// <param name="areaId">số bản ghi bỏ qua</param>      
        /// <returns></returns>
        public IQueryable<Site> GetByAreaId( int? areaId = null)
        {
            IQueryable<Site> query = sitesResponsitory.GetAll().Where(i => i.DeviceId != null); //Query lấy điều kiện dữ liệu
            if (areaId.HasValue)
            {
                query = query.Where(q => q.Area_Id == areaId);
            }          
            query = query.OrderBy(q => q.Id); // Sắp xếp dữ liệu lấy ra theo thứ tự tăng dần           
            return query;
        }
        /// <summary>
        /// Lấy dữ liệu theo groupId
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IQueryable<Site> GetBygroupId(int? groupId = null)
        {
            IQueryable<Site> query = sitesResponsitory.GetAll().Where(i=>i.DeviceId != null); //Query lấy điều kiện dữ liệu
            if (groupId.HasValue)
            {
                query = query.Where(q => q.Group_Id == groupId);
            }
            query = query.OrderBy(q => q.Id); // Sắp xếp dữ liệu lấy ra theo thứ tự tăng dần           
            return query;
        }
        /// <summary>
        /// Lấy dữ liệu theo DeviceId
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IQueryable<Site> GetByDeviceId(int? deviceId = null)
        {
            IQueryable<Site> query = sitesResponsitory.GetAll(); //Query lấy điều kiện dữ liệu
            if (deviceId.HasValue)
            {
                query = query.Where(q => q.DeviceId == deviceId);
            }               
            return query;
        }
    }
}