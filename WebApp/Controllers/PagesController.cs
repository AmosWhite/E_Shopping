using BLL.Interface;
using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.Models.VM.Pages;

namespace WebApp.Controllers
{
    public class PagesController : Controller
    {
        IPageBussiness pageBussiness;
        ISidebarBussiness sidebarBussiness;

        // Constructor  PagesContriller
        public PagesController(IPageBussiness _pageBussiness, ISidebarBussiness _sidebarBussiness)
        {
            this.pageBussiness = _pageBussiness;
            this.sidebarBussiness = _sidebarBussiness;
        }
        // GET: Index/{page}
        public ActionResult Index(string page = "")
        {
            // Get/set page slug
            if (page == "")
                page = "home";

            // Check if page exists
            if (!pageBussiness.PageExist(page))
            {
                return RedirectToAction("Index", new { page = "" });
            }

            // Get page DTO
            PageVM pageVM = new PageVM(pageBussiness.GetPageBySlug(page));

            // Set page title
            ViewBag.PageTitle = pageVM.Title;

            // Check for sidebar
            if (pageVM.HasSidebar == true)
            {
                ViewBag.Sidebar = "Yes";
            }
            else
            {
                ViewBag.Sidebar = "No";
            }

            // Return view with model
            return View(pageVM);
        }

        public ActionResult PagesMenuPartial()
        {
            // Declare a list of PageVM
            List<PageVM> pageVMList;

            // Get all pages except home

            pageVMList = pageBussiness.GetAllPages().Where(x => x.Slug != "home").Select(x => new PageVM(x)).ToList();

            // Return partial view with list
            return PartialView(pageVMList);
        }

        public ActionResult SidebarPartial()
        {
            // Init model
            SidebarDomainModel sidebarDM = sidebarBussiness.GetSidebarById(1);

            SidebarVM sidebarVM = new SidebarVM { Id = sidebarDM.Id, Body = sidebarDM.Body };

            // Return partial view with model
            return PartialView(sidebarVM);
        }
    }
}