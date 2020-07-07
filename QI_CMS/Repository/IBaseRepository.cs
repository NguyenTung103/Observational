using System.Collections.Generic;
using System.Linq;

#pragma warning disable 1591

namespace ES_CapDien.Repository
{
    public interface IBaseRepository<T>
    {
        /// <summary>
        /// Retrieve a single item using it's primary key, exception if not found
        /// </summary>
        /// <param name="primaryKey">The primary key of the record</param>
        /// <returns>T</returns>
        T Single(object primaryKey);
        /// <summary>
        /// Retrieve created by and modified by id's FullName
        /// </summary>
        /// <param name="dynamicObject">The primary key of the record</param>
        /// <returns>T</returns>
        Dictionary<string, string> GetAuditNames(dynamic dynamicObject);
        /// <summary>
        /// Retrieve a single item by it's primary key or return null if not found
        /// </summary>
        /// <param name="primaryKey">Prmary key to find</param>
        /// <returns>T</returns>
        T SingleOrDefault(object primaryKey);

        /// <summary>
        /// Returns all the rows for type T
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Does this item exist by it's primary key
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        bool Exists(object primaryKey);

        /// <summary>
        /// Inserts the data into the table And Return Id. Item Must have Id Field!!!
        /// </summary>
        /// <param name="entity">The entity to insert</param>
        /// <param name="userId">The user performing the insert</param>
        /// <returns></returns>
        int InsertReturnId(T entity);

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="entity"></param>
        bool Insert(T entity);

        /// <summary>
        /// Updates this entity in the database using it's primary key
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="userId">The user performing the update</param>
        bool Update(T entity);

        /// <summary>
        /// Deletes this entry fro the database
        /// ** WARNING - Most items should be marked inactive and Updated, not deleted
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <param name="userId">The user Id who deleted the entity</param>
        /// <returns></returns>
        bool Delete(T entity);

        /// <summary>
        /// Deletes this entry fro the database
        /// ** WARNING - Most items should be marked inactive and Updated, not deleted
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <param name="userId">The user Id who deleted the entity</param>
        /// <returns></returns>
        int DeleteReturnId(T entity);

        IUnitOfWork UnitOfWork { get; }
    }
}