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
    public class CommandService
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly BaseRepository<Command> commandResponsitory;

        /// <summary>
        /// Khởi tạo dữ liệu
        /// </summary>
        public CommandService()
        {
            unitOfWork = new UnitOfWork();
            commandResponsitory = new BaseRepository<Command>(unitOfWork);
        }

        /// <summary>
        /// Lấy dữ liệu hiển thị ra băngr và phân trang
        /// </summary>
        /// <param name="skip">số bản ghi bỏ qua</param>
        /// <param name="take">số bản ghi lấy</param>
        /// <param name="totalRow">tổng số hàng</param>
        /// <param name="title">title trang web</param>
        /// <returns></returns>
        public IQueryable<Command> GetAll(int skip, int take, out int totalRow, int? deviceId=null, int? groupId=null)
        {
            IQueryable<Command> query = commandResponsitory.GetAll(); //Query lấy điều kiện dữ liệu            
            if (deviceId.HasValue)
            {
                query = query.Where(q => q.Device_Id == groupId);
            }
            if (groupId.HasValue)
            {
                query = query.Where(q => q.GroupId == groupId);
            }
            query = query.OrderBy(q => q.Id); // Sắp xếp dữ liệu lấy ra theo thứ tự tăng dần
            totalRow = query.Count();
            return query.Skip(skip).Take(take);
        }       
    }
}