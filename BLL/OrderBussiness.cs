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
    public class OrderBussiness : IOrderBussiness
    {
        private readonly OrderRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;
        public OrderBussiness(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
            this.orderRepository = new OrderRepository(unitOfWork);
        }

        public OrderDomainModel SaveOrder(OrderDomainModel model)
        {
            Order orderDTO = new Order();
            orderDTO.UserId = model.UserId;
            orderDTO.CreatedAt = model.CreatedAt;

            orderDTO = orderRepository.Insert(orderDTO);

            OrderDomainModel orderDM = new OrderDomainModel();
            orderDTO.Id = orderDM.Id;
            orderDTO.CreatedAt = orderDM.CreatedAt;

            return orderDM;
        }

        public List<OrderDomainModel> GetOrders(int userId)
        {
            List<OrderDomainModel> ordersDM = orderRepository.GetAll(x => x.UserId == userId)
                .Select(x => new OrderDomainModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    CreatedAt = x.CreatedAt
                }).ToList();
            return ordersDM;
        }

        public List<OrderDomainModel> GetOrders()
        {
            List<OrderDomainModel> ordersDM = orderRepository.GetAll()
                .Select(x => new OrderDomainModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    CreatedAt = x.CreatedAt
                }).ToList();

            return ordersDM;
        }
    }
}
