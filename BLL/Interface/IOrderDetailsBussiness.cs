using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IOrderDetailBussiness
    {
        void SaveOrderDetails(OrderDetailDomainModel model);
        List<OrderDetailDomainModel> GetOrderDetailsById(int id);
    }
}
