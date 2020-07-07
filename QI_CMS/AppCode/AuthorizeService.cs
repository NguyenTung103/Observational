using ES_CapDien.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_CapDien.AppCode
{
    public class AuthorizeService
    {
        private readonly IUnitOfWork unitOfWork;
        public readonly BaseRepository<webpages_UsersInRoles> userInRolesResponsitory;
        public readonly BaseRepository<webpages_Roles> roleResponsitory;
        public AuthorizeService()
        {
            unitOfWork = new UnitOfWork();
            userInRolesResponsitory = new BaseRepository<webpages_UsersInRoles>(unitOfWork);
            roleResponsitory = new BaseRepository<webpages_Roles>(unitOfWork);
        }
        public IQueryable<webpages_Roles> GetAll()
        {
            IQueryable<webpages_Roles> query = roleResponsitory.GetAll();
                       
            query = query.OrderByDescending(q => q.RoleId);

            return query;
        }
    }
}