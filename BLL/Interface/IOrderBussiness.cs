using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IOrderBussiness
    {
        /// <summary>
        /// ...Saves order to Db and Return save details....
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        OrderDomainModel SaveOrder(OrderDomainModel model);
        List<OrderDomainModel> GetOrders(int userId);
        List<OrderDomainModel> GetOrders();
    }
}
