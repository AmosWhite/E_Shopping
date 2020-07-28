using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
   public class OrderDomainModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
