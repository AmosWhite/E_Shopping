using DAL.Infrastructure;
using DAL.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public class SidebarRepository : BaseRepository<Sidebar>
    {
        public SidebarRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
