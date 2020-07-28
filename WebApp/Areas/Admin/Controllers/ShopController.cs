using BLL.Interface;
using DL;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApp.Areas.Admin.Models.VM;
using WebApp.Areas.Admin.Models.VM.Shop;
using WebApp.Models.ViewModels.Shop;

namespace WebApp.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        IOrderBussiness orderBussiness;
        ICategoryBussiness categoryBussiness;
        IProductBussiness productBussiness;
        IOrderDetailBussiness orderDetailBussiness;
        IUserBussiness userBussiness;

        public ShopController
            (ICategoryBussiness _categoryBussiness,
            IProductBussiness _productBussiness,
            IOrderBussiness _orderBussiness,
            IOrderDetailBussiness _orderDetailBussiness,
            IUserBussiness _userBussiness
            )
        {
            orderBussiness = _orderBussiness;
            categoryBussiness = _categoryBussiness;
            productBussiness = _productBussiness;
            orderDetailBussiness = _orderDetailBussiness;
            userBussiness = _userBussiness;
        }

        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            //get and init categoryDM  list
            List<CategoryVM> listCategoryVM = categoryBussiness
                .GetAllCategories()
                .Select(x => new CategoryVM(x)).ToList();

            //Return view with list 
            return View(listCategoryVM);
        }

        // Post: Admin/Shop/Categories
        [HttpPost]
        public string AddNewCategory(string catName)
        {
            //check if category name already exist, otherwise create new category
            if (!categoryBussiness.CategoryExist(catName))
            {
                CategoryDomainModel categoryDM = new CategoryDomainModel()
                {
                    Name = catName,
                    Slug = catName.Replace(" ", "-").ToLower(),
                    Sorting = 100
                };


                string id = categoryBussiness.AddCategory(categoryDM);
                return id;
            }
            else
                return "titletaken";

        }

        // Post: Admin/Shop/Categories
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            // set sorting and save each page
            categoryBussiness.SortCategory(id);

        }

        //GET: Admin/Pages/DeleteCategory/id
        public ActionResult DeleteCategory(int id)
        {
            // Remove the Category
            try
            {
                categoryBussiness.RemoveCategory(id);
                return RedirectToAction("Categories");
            }
            catch
            {
                return Content("Some thing went wrong");
            }
            //save

        }

        // Post: Admin/Shop/RenameCategory
        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {

            //Check if Name Exist
            if (categoryBussiness.CategoryExist(newCatName))
            {
                return "titletaken";
            }
            else
            {
                //Save Category
                categoryBussiness.EditCategory(newCatName, id);
                return "ok";
            }


        }

        // GET: Admin/Shop/AddProduct
        [HttpGet]
        public ActionResult AddProduct()
        {
            // Init ProductVM
            ProductVM productVM = new ProductVM();

            //Add List Of Category to productVM
            productVM.Categories = new SelectList(categoryBussiness.GetAllCategories(), "Id", "Name");

            return View(productVM);
        }

        // POST: Admin/Shop/AddProduct
        [HttpPost]
        public ActionResult AddProduct(ProductVM model, HttpPostedFileBase file)
        {
            int id;
            //Add List Of Category to productVM
            model.Categories = new SelectList(categoryBussiness.GetAllCategories(), "Id", "Name");

            //Check model State
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Check if product name already exist
            if (productBussiness.IsProductNameExist(model.Name))
            {
                ModelState.AddModelError("", "That product name ia taken");
                return View(model);
            }

            ProductVM productVM = new ProductVM();

            //Transforming to DM
            ProductDomainModel productDM = productVM.NewPrductVmToProductDM(model);

            //save product to Db and Get inserted id
            id = productBussiness.SaveNewProduct(productDM);

            // set TempData msg
            @TempData["SuccessMessage"] = "You have added a product";

            #region Upload image

            //Create neccessary directories
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");


            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);

            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);

            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);

            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);

            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);

            //Check if a file was uploaded
            if (file != null && file.ContentLength > 0)
            {
                // Get file extension
                string ext = file.ContentType.ToLower();

                // Verify extension
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                    return View(model);
                }


                // Init image name
                string imageName = file.FileName;

                // Save image name to Db
                productBussiness.UpDateImageName(id, imageName);

                // Set original and thumb image paths
                var path = string.Format("{0}\\{1}", pathString2, imageName);
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                // Save original
                file.SaveAs(path);

                // Create and save thumb
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }

            #endregion

            return RedirectToAction("AddProduct");
        }

        // GET: Admin/Shop/Products
        public ActionResult Products(int? page, int? catId)
        {
            // Declare a list of ProductVM
            List<ProductVM> listOfProductVM;

            // Set page number
            var pageNumber = page ?? 1;

            // Init the list
            listOfProductVM = productBussiness.GetAllProducts().ToArray()
                              .Where(x => catId == null || catId == 0 || x.CategoryId == catId)
                              .Select(x => new ProductVM(x))
                              .ToList();

            // Populate categories select list
            ViewBag.Categories = new SelectList(categoryBussiness.GetAllCategories(), "Id", "Name");

            // Set selected category
            ViewBag.SelectedCat = catId.ToString();


            // Set pagination
            var onePageOfProducts = listOfProductVM.ToPagedList(pageNumber, 3);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            // Return view with list
            return View(listOfProductVM);
        }

        // GET: Admin/Shop/EditProduct/id
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            //Declare ProductVM and productDM
            ProductVM productVM;
            ProductDomainModel productDM;

            // Get the product
            productDM = productBussiness.GetProductById(id);

            // Make sure product exists
            if (productDM == null)
            {
                return Content("That product does not exist.");
            }
            else
            {
                // init productVM
                productVM = new ProductVM(productDM);
            }


            // Make a select list
            productVM.Categories = new SelectList(categoryBussiness.GetAllCategories(), "Id", "Name");

            // Get all gallery images
            productVM.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                            .Select(fn => Path.GetFileName(fn));

            // Return view with model
            return View(productVM);
        }

        // POST: Admin/Shop/EditProduct/id
        [HttpPost]
        public ActionResult EditProduct(ProductVM model, HttpPostedFileBase file)

        {
            // Declear product Id
            int id = model.Id;

            // Populate categories select list and gallery images
            model.Categories = new SelectList(categoryBussiness.GetAllCategories(), "Id", "Name");

            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                                .Select(fn => Path.GetFileName(fn));

            // Check productVM state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Make sure product name is unique
            if (!productBussiness.IsRowNameUnique(model.Id, model.Name))
            {
                ModelState.AddModelError("", "That product name is taken!");
                return View(model);
            }

            //Transforming to DM
            ProductVM productVM = new ProductVM();
            ProductDomainModel productDM = productVM.NewPrductVmToProductDM(model);

            // Update product
            productBussiness.UpdateProduct(productDM);

            // Set TempData message
            TempData["SM"] = "You have edited the product!";

            #region Image Upload

            // Check for file upload
            if (file != null && file.ContentLength > 0)
            {

                // Get extension
                string ext = file.ContentType.ToLower();

                // Verify extension
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {

                    ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                    return View(model);
                }

                // Set uplpad directory paths
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                var pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");

                // Delete files from directories

                DirectoryInfo di1 = new DirectoryInfo(pathString1);
                DirectoryInfo di2 = new DirectoryInfo(pathString2);

                foreach (FileInfo file2 in di1.GetFiles())
                    file2.Delete();

                foreach (FileInfo file3 in di2.GetFiles())
                    file3.Delete();

                // Init image name
                string imageName = file.FileName;

                // Save image name to Db
                productBussiness.UpDateImageName(id, imageName);

                // Save original and thumb images

                var path = string.Format("{0}\\{1}", pathString1, imageName);
                var path2 = string.Format("{0}\\{1}", pathString2, imageName);

                file.SaveAs(path);

                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }

            #endregion

            // Redirect
            return RedirectToAction("EditProduct");
        }

        //GET: Admin/Shop/DeleteProduct
        public ActionResult DeleteProduct(int id)
        {
            //Delete products from Db
            productBussiness.DeleteProduct(id);

            //Delect product folder
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            string pathString = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());

            if (Directory.Exists(pathString))
                Directory.Delete(pathString, true);

            //Redirect 
            return RedirectToAction("Products");

        }

        // POST: Admin/Shop/SaveGalleryImages
        [HttpPost]
        public void SaveGalleryImages(int id)
        {
            // Loop through files
            foreach (string fileName in Request.Files)
            {
                // Init the file
                HttpPostedFileBase file = Request.Files[fileName];

                // Check it's not null
                if (file != null && file.ContentLength > 0)
                {
                    // Set directory paths
                    var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                    string pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
                    string pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

                    // Set image paths
                    var path = string.Format("{0}\\{1}", pathString1, file.FileName);
                    var path2 = string.Format("{0}\\{1}", pathString2, file.FileName);

                    // Save original and thumb

                    file.SaveAs(path);
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2);
                }

            }

        }

        // POST: Admin/Shop/DeleteImage
        [HttpPost]
        public void DeleteImage(int id, string imageName)
        {
            string fullPath1 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/" + imageName);
            string fullPath2 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/Thumbs/" + imageName);

            if (System.IO.File.Exists(fullPath1))
                System.IO.File.Delete(fullPath1);

            if (System.IO.File.Exists(fullPath2))
                System.IO.File.Delete(fullPath2);
        }

        // GET: Admin/Shop/Orders
        public ActionResult Orders()
        {
            // Init list of OrdersForAdminVM
            List<OrdersForAdminVM> ordersForAdmin = new List<OrdersForAdminVM>();

            // Init list of OrderVM
            List<OrderVM> orders = orderBussiness.GetOrders().Select(x => new OrderVM(x)).ToList();

            // Loop through list of OrderVM
            foreach (var order in orders)
            {
                // Init product dict
                Dictionary<string, int> productsAndQty = new Dictionary<string, int>();

                // Declare total
                decimal total = 0m;

                // Init list of OrderDetailsDTO
                List<OrderDetailDomainModel> orderDetailsList = orderDetailBussiness.GetOrderDetailsById(order.OrderId).ToList();

                // Get username
                UserDomainModel user = userBussiness.GetUserById(order.UserId);
                string email = user.Email;

                // Loop through list of OrderDetailsDTO
                foreach (var orderDetails in orderDetailsList)
                {
                    // Get product
                    ProductDomainModel productDM = productBussiness.GetProductById(orderDetails.ProductId);

                    // Get product price
                    int price = productDM.Price;

                    // Get product name
                    string productName = productDM.Name;

                    // Add to products dict
                    productsAndQty.Add(productName, orderDetails.Quantity);

                    // Get total
                    total += orderDetails.Quantity * price;
                }

                // Add to ordersForAdminVM list
                ordersForAdmin.Add(new OrdersForAdminVM()
                {
                    OrderNumber = order.OrderId,
                    Username = email,
                    Total = total,
                    ProductsAndQty = productsAndQty,
                    CreatedAt = order.CreatedAt
                });
            }
            // Return view with OrdersForAdminVM list
            return View(ordersForAdmin);
        }
    }
}




