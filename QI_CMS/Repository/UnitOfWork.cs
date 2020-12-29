using System.Data.Entity;
using System.Transactions;

namespace ES_CapDien.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private TransactionScope _transaction;
        private readonly ObservationEntities _db;//todo: change DBcontext => your context

        public UnitOfWork()
        {
            _db = new ObservationEntities();
        }

        public void Dispose()
        {

        }

        public void StartTransaction()
        {
            _transaction = new TransactionScope();
        }

        public void Commit()
        {
            _db.SaveChanges();
            _transaction.Complete();
        }

        public DbContext Db => _db;

        public UserActionLogInfo UserActionLogInfo { get; set; }
    }
}
