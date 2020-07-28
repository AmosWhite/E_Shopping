using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class OrderDetailDomainModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        //public virtual OrderDTO Orders { get; set; }
        //public virtual UserDTO Users { get; set; }
        //public virtual ProductDTO Products { get; set; }
    }
}
