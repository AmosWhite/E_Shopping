using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Models.VM.Pages
{
    public class SidebarVM
    {
        public SidebarVM()
        {
                
        }
        public SidebarVM(SidebarDomainModel row)
        {
            Id = row.Id;
            Body = row.Body;
        }
        public int Id { get; set; }
        [AllowHtml]
        public string Body { get; set; }
    }
}