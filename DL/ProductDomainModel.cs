using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
   public class ProductDomainModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public string ImageName { get; set; }
        public string Slug { get; set; }
        public string CategoryName { get; set; }
    }
}
