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
    public class RegisterSMSService
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly BaseRepository<RegisterSMS>registerResponsitory;

        /// <summary>
        /// Khởi tạo dữ liệu
        /// </summary>
        public RegisterSMSService()
        {
            unitOfWork = new UnitOfWork();
            registerResponsitory = new BaseRepository<RegisterSMS>(unitOfWork);
        }

        /// <summary>
        /// Lấy dữ liệu hiển thị ra băngr và phân trang
        /// </summary>
        /// <param name="skip">số bản ghi bỏ qua</param>
        /// <param name="take">số bản ghi lấy</param>
        /// <param name="totalRow">tổng số hàng</param>
        /// <param name="title">title trang web</param>
        /// <returns></returns>
        public IQueryable<RegisterSMS> GetAll(int skip, int take, out int totalRow, string title = "", int? groupId = null, int? deviceid = null)
        {
            IQueryable<RegisterSMS> query = registerResponsitory.GetAll(); //Query lấy điều kiện dữ liệu
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(q => q.DanhSachSDT.ToLower().Contains(title.ToLower()));
            }
            if(groupId.HasValue)
            {
                query = query.Where(q => q.GroupId==groupId);
            }
            if (deviceid.HasValue)
            {
                query = query.Where(q => q.DeviceId == deviceid);
            }
            query = query.OrderBy(q => q.Id); // Sắp xếp dữ liệu lấy ra theo thứ tự tăng dần
            totalRow = query.Count();
            return query.Skip(skip).Take(take);
        }        
    }
}