using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ES_CapDien.AppCode;

namespace ES_CapDien.Repository
{
    /// <summary>
    /// Base class for all SQL based service classes
    /// </summary>
    /// <typeparam name="T">The domain object type</typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        internal DbSet<T> DbSet;

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            DbSet = UnitOfWork.Db.Set<T>();
        }

        /// <summary>
        /// Returns the object with the primary key specifies or throws
        /// </summary>
        /// <typeparam name="TU">The type to map the result to</typeparam>
        /// <param name="primaryKey">The primary key</param>
        /// <returns>The result mapped to the specified type</returns>
        public T Single(object primaryKey)
        {
            return DbSet.Find(primaryKey);
        }

        /// <summary>
        /// Returns the object with the primary key specifies or the default for the type
        /// </summary>
        /// <typeparam name="TU">The type to map the result to</typeparam>
        /// <param name="primaryKey">The primary key</param>
        /// <returns>The result mapped to the specified type</returns>
        public T SingleOrDefault(object primaryKey)
        {
            return DbSet.Find(primaryKey);
        }

        //public virtual IEnumerable<T> GetAllWithDeleted()
        //{
        //    var dbresult = _unitOfWork.Db.Fetch<T>("");

        //    return dbresult;
        //}

        public bool Exists(object primaryKey)
        {
            return DbSet.Find(primaryKey) != null;
        }

        public virtual int InsertReturnId(T entity)
        {
            dynamic obj = DbSet.Add(entity);
            UnitOfWork.Db.SaveChanges();
            return obj.Id;
        }

        public virtual bool Insert(T entity)
        {
            DbSet.Add(entity);


            return UnitOfWork.Db.SaveChanges() > 0;

            //Loging
            //StackTrace stackTrace = new StackTrace();
            //_unitOfWork.UserActionLogInfo.Actor =  stackTrace.GetFrame(0).GetMethod().Name;//todo: viết sau nhé
        }

        public virtual bool Update(T entity)
        {
            DbSet.Attach(entity);
            UnitOfWork.Db.Entry(entity).State = EntityState.Modified;
            return UnitOfWork.Db.SaveChanges() > 0;
        }

        public int DeleteReturnId(T entity)
        {
            if (UnitOfWork.Db.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dynamic obj = DbSet.Remove(entity);
            UnitOfWork.Db.SaveChanges();
            return obj.Id;
        }

        public bool Delete(T entity)
        {
            if (UnitOfWork.Db.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
            return UnitOfWork.Db.SaveChanges() > 0;
        }

        public IUnitOfWork UnitOfWork { get; }       

        internal DbContext Database => UnitOfWork.Db;
        public Dictionary<string, string> GetAuditNames(dynamic dynamicObject)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }       
    }
}
