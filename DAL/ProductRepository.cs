using DAL.Infrastructure;
using DAL.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Check if a row name field is unique from others 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsRowNameUniqueRepo(int id, string name)
        {
            if (dbSet.Where(x => x.Id != id).Any(x => x.Name == name))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
