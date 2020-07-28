using System;
using DL;

namespace WebApp.Models.ViewModels.Shop
{
    public class OrderVM
    {
        public OrderVM()
        {
        }

        public OrderVM(OrderDomainModel model)
        {
            OrderId = model.Id;
            UserId = model.UserId;
            CreatedAt = model.CreatedAt;
        }

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}