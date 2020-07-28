using DL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Models.VM.Shop
{
    public class ProductVM
    {
        public ProductVM()
        {

        }
        public ProductVM(ProductDomainModel productDM)
        {
            Id = productDM.Id;
            Name = productDM.Name;
            Description = productDM.Description;
            Price = productDM.Price;
            CategoryName = productDM.CategoryName;
            Slug = productDM.Slug;
            ImageName = productDM.ImageName;
            CategoryId = productDM.CategoryId;

        }

        public ProductDomainModel NewPrductVmToProductDM(ProductVM productVM)
        {
            ProductDomainModel productDM = new ProductDomainModel
            {
                Id = productVM.Id,
                Name = productVM.Name,
                Description = productVM.Description,
                Price = productVM.Price,
                CategoryName = productVM.CategoryName,
                CategoryId = productVM.CategoryId,
                Slug = productVM.Name.Replace(" ", "-"),
                ImageName = productVM.ImageName
            };

            return productDM;
        }


        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string ImageName { get; set; }
        public string Slug { get; set; }
        public string CategoryName { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
    }
}