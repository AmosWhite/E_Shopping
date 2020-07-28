using BLL.Interface;
using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.Models.VM.Pages;
using WebApp.Custom_Security;

namespace WebApp.Areas.Admin.Controllers
{
    
    [CustomAuthorization(Role = "Admin")]
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

        // GET: Admin/Pages
        public ActionResult Index()
        {
            // Init The list
            List<PageVM> pagesList = pageBussiness.GetAllPages().Select(x => new PageVM(x)).ToList();
            return View(pagesList);
        }

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // Post: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Declare slug
            string slug;

            // Check for and set slug if need be
            if (string.IsNullOrWhiteSpace(model.Slug))
            {
                slug = model.Title.Replace(" ", "-").ToLower();
            }
            else
            {
                slug = model.Slug.Replace(" ", "-").ToLower();
            }

            // make sure title and slug are unique
            if (pageBussiness.SlugTitleExist(model.Slug, model.Title))
            {
                ModelState.AddModelError("", "The title or slug already Exit");
                return View(model);
            };

            
            //Set PageDomainModel
            PageDomainModel page = new PageDomainModel();
            page.Slug = slug;
            page.Title = model.Title;
            page.Body = model.Body;
            page.HasSidebar = model.HasSidebar;
            page.Sorting = 100;

            //save Page
            if (pageBussiness.InsertPage(page))
            {

                //set TempData message
                TempData["SuccessMessage"] = "You have added a new page";

                // redirect
                return RedirectToAction("AddPage");
            }
            else
            {
                ModelState.AddModelError("", "Something Went Wrong!");
                return View(model);
            }


        }

        // get: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            // retriving page
            PageDomainModel model = pageBussiness.GetPageById(id);

            //Confirm page exist
            if (model == null)
            {
                return Content("The page does not exist");
            }
            //Init pageVM
            PageVM pageVM = new PageVM(model);

            //Return view with pageVM
            return View(pageVM);
        }

        // Post: Admin/Pages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            // model State
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Declare slug
            string slug = "home";

            //Check for slug and set it need be
            if (model.Slug != "home")
            {
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
            }

            //Make sure the specify row title and slug are unique from others
            if (pageBussiness.IsRowSlugTitleUnique(model.Id, model.Slug, model.Title))
            {
                ModelState.AddModelError("", "The title or slug already Exit");
                return View(model);
            }

            else
            {
                //Set PageDomainModel
                PageDomainModel page = new PageDomainModel();

                page.Id = model.Id;
                page.Slug = slug;
                page.Title = model.Title;
                page.Body = model.Body;
                page.HasSidebar = model.HasSidebar;


                //save Page
                if (pageBussiness.EditPage(page))
                {

                    //set TempData message
                    TempData["SuccessMessage"] = "You have Edit page";

                    // redirect
                    return RedirectToAction("EditPage");
                }
                else
                {
                    ModelState.AddModelError("", "Something Went Wrong!");
                    return View(model);
                }
            }

        }

        // GET: Admin/Pages/PageDetails/id

        public ActionResult PageDetails(int id)
        {
            // retriving page
            PageDomainModel model = pageBussiness.GetPageById(id);

            //Confirm if page exist
            if (model == null)
            {
                return Content("The page does not exist");
            }

            //Init pageVM
            PageVM pageVM;
            pageVM = new PageVM(model);

            //Return view with pageVM
            return View(pageVM);
        }

        //GET: Admin/Pages/DeletePage/id
        public ActionResult DeletePage(int id)
        {
            // Remove the page
            try
            {
                pageBussiness.RemovePage(id);
                return RedirectToAction("index");
            }
            catch
            {
                return Content("Some thing went wrong");
            }
            //save

        }

        // POST: Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            // set sorting and save each page
            pageBussiness.SortPages(id);

        }

        // GET: Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar(int id = 1)
        {
            //Retrive SidebarDM
            SidebarDomainModel sidebarDM = sidebarBussiness.GetSidebarById(id);

            //inti SidebarVM
            SidebarVM sidebarVM = new SidebarVM(sidebarDM);

            //Return view with model
            return View(sidebarVM);
        }

        // POST: Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            //Retrive SidebarDM
            SidebarDomainModel sidebarDM = sidebarBussiness.GetSidebarById(model.Id);

            //change sidebarDM the Body
            sidebarDM.Body = model.Body;

            // Save
            if (sidebarBussiness.EditSidebar(sidebarDM))
            {
                //set TempData message
                TempData["SuccessMessage"] = "You have Edit page";

                // redirect
                return RedirectToAction("EditSidebar");
            }
            else
            {
                ModelState.AddModelError("", "Something Went Wrong !");
                return View(model);
            }

        }

    }
}