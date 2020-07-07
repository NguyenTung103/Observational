using ES_CapDien.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_CapDien.AppCode
{
    public class ObservationService
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly BaseRepository<Observation> observationResponsitory;

        /// <summary>
        /// Khởi tạo dữ liệu
        /// </summary>
        public ObservationService()
        {
            unitOfWork = new UnitOfWork();
            observationResponsitory = new BaseRepository<Observation>(unitOfWork);
        }

        /// <summary>
        /// Lấy dữ liệu hiển thị ra băngr và phân trang
        /// </summary>
        /// <param name="skip">số bản ghi bỏ qua</param>
        /// <param name="take">số bản ghi lấy</param>
        /// <param name="totalRow">tổng số hàng</param>
        /// <param name="title">title trang web</param>
        /// <returns></returns>
        public IQueryable<Observation> GetAll(int skip, int take, out int totalRow, string title = "")
        {
            IQueryable<Observation> query = observationResponsitory.GetAll(); //Query lấy điều kiện dữ liệu

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(q => q.Name.ToLower().Contains(title.ToLower()));
            }           
            query = query.OrderBy(q => q.Id); // Sắp xếp dữ liệu lấy ra theo thứ tự tăng dần
            totalRow = query.Count();
            return query.Skip(skip).Take(take);
        }       
    }
}