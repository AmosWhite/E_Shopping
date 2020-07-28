using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.Models.VM
{
    public class CategoryVM
    {
        public CategoryVM()
        {

        }
        public CategoryVM(CategoryDomainModel row)
        {
            Id = row.Id;
            Name = row.Name;
            Slug = row.Slug;
            Sorting = row.Sorting;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public Nullable<int> Sorting { get; set; }
    }
}