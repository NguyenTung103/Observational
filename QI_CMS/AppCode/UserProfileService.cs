using ES_CapDien.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_CapDien.AppCode
{   
    public class UserProfileService
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly BaseRepository<UserProfile> userProfileResponsitory;

        /// <summary>
        /// Khởi tạo dữ liệu
        /// </summary>
        public UserProfileService()
        {
            unitOfWork = new UnitOfWork();
            userProfileResponsitory = new BaseRepository<UserProfile>(unitOfWork);
        }
       
    }
}