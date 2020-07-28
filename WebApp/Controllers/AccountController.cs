using BLL.Interface;
using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApp.Custom_Security;
using WebApp.Models.ViewModels.Account;
using WebApp.Models.ViewModels.Shop;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        IUserBussiness userBussiness;
        IOrderBussiness orderBussiness;
        IOrderDetailBussiness orderDetailBussiness;
        IProductBussiness productBussiness;
         
        public AccountController
            (
            IUserBussiness _userBussiness,
            IOrderBussiness _orderBussiness,
            IOrderDetailBussiness _orderDetailBussiness,
            IProductBussiness _productBussiness
            )
        {
            this.userBussiness = _userBussiness;
            this.orderBussiness = _orderBussiness;
            this.productBussiness = _productBussiness;
            this.orderDetailBussiness = _orderDetailBussiness;
            
        }
        // GET: Account
        public ActionResult Index()
        {
            return Redirect("~/account/login");
        }

        // GET: /account/login
        [HttpGet]
        public ActionResult Login()
        {
            // Confirm user is not logged in

            string username = User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
                return RedirectToAction("user-profile");

            // Return view
            return View();
        }

        // POST: /account/login
        [HttpPost]
        public ActionResult Login(LoginUserVM model)
        {

            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UserDomainModel userDM = new UserDomainModel();
            userDM.Email = model.Email;
            userDM.Password = model.Password;


            // Check and retrieve user details
            UserDomainModel userModel = userBussiness.GetUser(userDM);

            if (userDM == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }
            else
            {
                string userData = $"{userModel.Id.ToString()}|{userModel.UserRoleName}|{userModel.Email}";
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
                    (1,
                    userModel.UserRoleName,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(10),
                     model.RememberMe,
                    userData
                    );
                string encTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie
                        (
                        FormsAuthentication.FormsCookieName,
                        encTicket
                        );
                Response.Cookies.Add(cookie);

                //return RedirectToAction("Login");
                return Redirect(FormsAuthentication.GetRedirectUrl(model.Email, model.RememberMe));
            }
        }

        // GET: /account/create-account
        [ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        // POST: /account/create-account
        [ActionName("create-account")]
        [HttpPost]
        public ActionResult CreateAccount(UserVM model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View("CreateAccount", model);
            }

            // Check if passwords match
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View("CreateAccount", model);
            }

            // Making sure username is unique
            if (userBussiness.IsEmailExist(model.EmailAddress))
            {
                ModelState.AddModelError("", "Username " + model.Username + " is taken.");
                model.Username = "";
                return View("CreateAccount", model);
            }

            // Create userDTO
            UserDomainModel userDm = new UserDomainModel()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.EmailAddress,
                Password = model.Password,
                RoleId = 2,

            };

            // Add the DTO
            userBussiness.SaveUser(userDm);

            // Create a TempData message
            TempData["SM"] = "You are now registered and can login.";

            // Redirect
            return Redirect("~/account/login");
        }

        // GET: /account/Logout
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/account/login");
        }

        [Authorize]
        public ActionResult UserNavPartial()
        {
            var userOBJ = User as CustomPrincipal;
            // Get username
            int userId = userOBJ.Id;

            // Declare model
            UserNavPartialVM model;

            // Get the user
            UserDomainModel UserDM = userBussiness.GetUserById(userId);
            // Build the model
            model = new UserNavPartialVM()
            {
                FirstName = UserDM.FirstName,
                LastName = UserDM.LastName
            };


            // Return partial view with model
            return PartialView(model);
        }

        // GET: /account/user-profile
        [HttpGet]
        [ActionName("user-profile")]
        [Authorize]
        public ActionResult UserProfile()
        {
            var userOBJ = User as CustomPrincipal;
            // Get username
            int useId = userOBJ.Id;

            // Declare model
            UserProfileVM userProfileVM;

            // Get user
            UserDomainModel userDM = userBussiness.GetUserById(useId);
            // Build model
            userProfileVM = new UserProfileVM(userDM);

            // Return view with model
            return View("UserProfile", userProfileVM);
        }

        // POST: /account/user-profile
        [HttpPost]
        [ActionName("user-profile")]
        [Authorize]
        public ActionResult UserProfile(UserProfileVM model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View("UserProfile", model);
            }

            // Check if passwords match if need be
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Passwords do not match.");
                    return View("UserProfile", model);
                }
            }

            UserDomainModel userDM = new UserDomainModel();

            userDM.FirstName = model.FirstName;
            userDM.LastName = model.LastName;
            userDM.Email = model.EmailAddress;
            userDM.Password = model.Password;

            // Making sure Email is unique
            if (!userBussiness.EditUser(userDM))
            {
                ModelState.AddModelError("", "Username " + model.Username + " already exists.");
                model.Username = "";
                return View("UserProfile", model);
            }

            // Set TempData message
            TempData["SM"] = "You have edited your profile!";

            // Redirect
            return Redirect("~/account/user-profile");
        }

        // GET: /account/Orders
        [Authorize(Roles = "User")]
        public ActionResult Orders()
        {
            var userOBJ = User as CustomPrincipal;
            // Init list of OrdersForUserVM
            List<OrdersForUserVM> ordersForUser = new List<OrdersForUserVM>();

            // Get user id
            int userId = userOBJ.Id;

            // Init list of OrderVM
            List<OrderVM> ordersDM = orderBussiness.GetOrders(userId).Select(x => new OrderVM(x)).ToList();

            // Loop through list of OrderVM
            foreach (var order in ordersDM)
            {
                // Init products dict
                Dictionary<string, int> productsAndQty = new Dictionary<string, int>();

                // Declare total
                decimal total = 0m;

                // Init list of OrderDetailsDTO
                List<OrderDetailDomainModel> orderDetailsDM = orderDetailBussiness.GetOrderDetailsById(order.OrderId);

                // Loop though list of OrderDetailsDTO
                foreach (var orderDetails in orderDetailsDM)
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

                // Add to OrdersForUserVM list
                ordersForUser.Add(new OrdersForUserVM()
                {
                    OrderNumber = order.OrderId,
                    Total = total,
                    ProductsAndQty = productsAndQty,
                    CreatedAt = order.CreatedAt
                });
            }
            // Return view with list of OrdersForUserVM
            return View(ordersForUser);
        }
    }
}