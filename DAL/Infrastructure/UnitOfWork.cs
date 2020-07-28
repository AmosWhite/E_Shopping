using DAL.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CmsShoppingCartEntities _dbContext;

        public UnitOfWork()
        {
            _dbContext = new CmsShoppingCartEntities();
        }
        public DbContext Db
        {
            get { return _dbContext; }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
