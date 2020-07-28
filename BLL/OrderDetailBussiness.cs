using BLL.Interface;
using DAL;
using DAL.Infrastructure.Contract;
using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class OrderDetailBussiness : IOrderDetailBussiness
    {
       private readonly  OrderDetailRepository orderDetailsRepository;
       private readonly IUnitOfWork unitOfWork;
        public OrderDetailBussiness(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
            this.orderDetailsRepository = new OrderDetailRepository(unitOfWork);
        }

        public void SaveOrderDetails(OrderDetailDomainModel model)
        {
            OrderDetail orderDetailsDTO = new OrderDetail();
            orderDetailsDTO.orderId = model.OrderId;
            orderDetailsDTO.ProductId = model.ProductId;
            orderDetailsDTO.UserId = model.UserId;
            orderDetailsDTO.Quantity = model.Quantity;

            orderDetailsRepository.Insert(orderDetailsDTO);
        }

        public List<OrderDetailDomainModel> GetOrderDetailsById(int id)
        {
            List<OrderDetailDomainModel> orderDetailsDM = orderDetailsRepository.GetAll(x => x.orderId == id)
                .Select(x => new OrderDetailDomainModel
                {
                    OrderId = x.orderId,
                    UserId = x.UserId,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList();

            return orderDetailsDM;
        }
    }
}
