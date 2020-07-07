using ES_CapDien.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_CapDien.AppCode
{
    public class UsersProfileService
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly BaseRepository<UserProfile> userProfileRepository;
        public readonly BaseRepository<webpages_Membership> webpages_MembershipResponsitory;
        public UsersProfileService()
        {
            unitOfWork = new UnitOfWork();
            userProfileRepository = new BaseRepository<UserProfile>(unitOfWork);
            webpages_MembershipResponsitory = new BaseRepository<webpages_Membership>(unitOfWork);
        }
        public IQueryable<UserProfile> GetAll(int skip, int take, out int totalRow, string title = "", int? groupid= null)
        {
            IQueryable<UserProfile> query = userProfileRepository.GetAll();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(q => q.UserName.ToLower().Contains(title.ToLower()));
            }           
            if(groupid.HasValue)
            {
                query = query.Where(i => i.Group_Id == groupid);
            }

            query = query.OrderByDescending(q => q.UserId);

            totalRow = query.Count();
            return query.Skip(skip).Take(take);
        }
    }
}