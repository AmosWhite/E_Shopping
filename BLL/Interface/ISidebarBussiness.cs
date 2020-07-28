using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface ISidebarBussiness
    {
        SidebarDomainModel GetSidebarById(int id);

        bool EditSidebar(SidebarDomainModel model);
    }
}
