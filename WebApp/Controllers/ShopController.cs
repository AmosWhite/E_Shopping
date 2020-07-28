using BLL.Interface;
using DL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApp.Areas.Admin.Models.VM;
using WebApp.Areas.Admin.Models.VM.Shop;
using WebApp.Custom_Security;

namespace WebApp.Controllers
{
    
    public class ShopController : Controller
    {
        ICategoryBussiness categoryBussiness;
        IProductBussiness productBussiness;

        // Constructor  PagesContriller
        public ShopController(ICategoryBussiness _categoryBussiness, IProductBussiness _ProductBussiness)
        {
            this.categoryBussiness = _categoryBussiness;
            this.productBussiness = _ProductBussiness;
        }

        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }

        public ActionResult CategoryMenuPartial()
        {
            // Declare list of CategoryVM
            List<CategoryVM> categoryVMList;

            // Init the list
            categoryVMList = categoryBussiness.GetAllCategories().Select(x => new CategoryVM(x)).ToList();

            // Return partial with list
            return PartialView(categoryVMList);
        }


        // GET: /shop/category/name
        public ActionResult Category(string name)
        {
            // Declare a list of ProductVM
            List<ProductVM> productVMList;


            // Get category id
            int catId = categoryBussiness.GetCategoryByName(name).Id;

            // Init the list
            productVMList = productBussiness.GetAllProducts().Where(x => x.CategoryId == catId)
                            .Select(x => new ProductVM(x)).ToList();

            // Get category name
            var productCat = productBussiness.GetProductById(catId);
            ViewBag.CategoryName = productCat.CategoryName;


            // Return view with list
            return View(productVMList);
        }

        // GET: /shop/product-details/name
        public ActionResult ProductDetails(string name)
        {

            // Check if product exists
            if (!productBussiness.IsProductSlugExist(name))
            {
                return RedirectToAction("Index", "Shop");
            }

            ProductDomainModel productDM = productBussiness.GetProductBySlug(name);

            // Init productVM
            ProductVM productVM = new ProductVM(productDM);

            // Get id
            int id = productVM.Id;

            // Get gallery images
            productVM.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                                .Select(fn => Path.GetFileName(fn));

            // Return view with model
            return View("ProductDetails", productVM);
        }
    }
}