using DAL.Infrastructure;
using DAL.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PageRepository : BaseRepository<Page>
    {
        public PageRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            
        }
        public bool IsRowSlugTitleUniqueRepo(int id, string slug, string title)
        {
            if (dbSet.Where(x => x.Id != id).Any(x => x.Slug == slug) || 
                dbSet.Where(x => x.Id != id).Any(x => x.Title == title))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
