using ES_CapDien.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_CapDien.AppCode
{
    public class MembershipService
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly BaseRepository<UserProfile> userProfileRepository;
        public readonly BaseRepository<webpages_Membership> webpages_MembershipResponsitory;
        public MembershipService()
        {
            unitOfWork = new UnitOfWork();
            userProfileRepository = new BaseRepository<UserProfile>(unitOfWork);
            webpages_MembershipResponsitory = new BaseRepository<webpages_Membership>(unitOfWork);
        }
        public IQueryable<webpages_Membership> GetAll()
        {
            IQueryable<webpages_Membership> query = webpages_MembershipResponsitory.GetAll();           
            query = query.OrderByDescending(q => q.UserId);
            return query;
        }
    }
}