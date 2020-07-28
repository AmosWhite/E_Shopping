using BLL.Interface;
using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.ViewModels.Cart;
using WebApp.Custom_Security;
using System.Net.Mail;
using System.Net;

namespace WebApp.Controllers
{
    public class CartController : Controller
    {
        ICategoryBussiness categoryBussiness;
        IProductBussiness productBussiness;
        IOrderBussiness orderBussiness;
        IOrderDetailBussiness orderDetailBussiness;

        // Constructor  PagesContriller
        public CartController(ICategoryBussiness _categoryBussiness,
                              IProductBussiness _ProductBussiness,
                              IOrderBussiness _orderBussiness,
                              IOrderDetailBussiness _orderDetailBussiness)
        {
            this.categoryBussiness = _categoryBussiness;
            this.productBussiness = _ProductBussiness;
            this.orderBussiness = _orderBussiness;
            this.orderDetailBussiness = _orderDetailBussiness;
        }

        // GET: Cart
        public ActionResult Index()
        {
            // Init the cart list
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            // Check if cart is empty
            if (cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return View();
            }

            // Calculate total and save to ViewBag

            decimal total = 0m;

            foreach (var item in cart)
            {
                total += item.Total;
            }

            ViewBag.GrandTotal = total;

            // Return view with list
            return View(cart);
        }
        public ActionResult CartPartial()
        {
            // Init CartVM
            CartVM model = new CartVM();

            // Init quantity
            int qty = 0;

            // Init price
            decimal price = 0m;

            // Check for cart session
            if (Session["cart"] != null)
            {
                // Get total qty and price
                var list = (List<CartVM>)Session["cart"];

                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.Price;
                }

                model.Quantity = qty;
                model.Price = price;

            }
            else
            {
                // Or set qty and price to 0
                model.Quantity = 0;
                model.Price = 0m;
            }

            // Return partial view with model
            return PartialView(model);
        }

        public ActionResult AddToCartPartial(int id)
        {
            // Init CartVM list
            List<CartVM> cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            // Init CartVM
            CartVM model = new CartVM();

            // Get the product
            ProductDomainModel productDM = productBussiness.GetProductById(id);

            // Check if the product is already in cart
            var productInCart = cart.FirstOrDefault(x => x.ProductId == id);

            // If not, add new
            if (productInCart == null)
            {
                cart.Add(new CartVM()
                {
                    ProductId = productDM.Id,
                    ProductName = productDM.Name,
                    Quantity = 1,
                    Price = (decimal)productDM.Price,
                    Image = productDM.ImageName
                });
            }
            else
            {
                // If it is, increment
                productInCart.Quantity++;
            }


            // Get total qty and price and add to model

            int qty = 0;
            decimal price = 0m;

            foreach (var item in cart)
            {
                qty += item.Quantity;
                price += item.Quantity * item.Price;
            }

            model.Quantity = qty;
            model.Price = price;

            // Save cart back to session
            Session["cart"] = cart;

            // Return partial view with model
            return PartialView(model);
        }

        // GET: /Cart/IncrementProduct
        public JsonResult IncrementProduct(int productId)
        {
            // Init cart list
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            // Get cartVM from list
            CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

            // Increment qty
            model.Quantity++;

            // Store needed data
            var result = new { qty = model.Quantity, price = model.Price };

            // Return json with data
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        // GET: /Cart/DecrementProduct
        public ActionResult DecrementProduct(int productId)
        {
            // Init cart
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            // Get model from list
            CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

            // Decrement qty
            if (model.Quantity > 1)
            {
                model.Quantity--;
            }
            else
            {
                model.Quantity = 0;
                cart.Remove(model);
            }

            // Store needed data
            var result = new { qty = model.Quantity, price = model.Price };

            // Return json
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        public void RemoveProduct(int productId)
        {
            // Init cart list
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            // Get model from list
            CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

            // Remove model from list
            cart.Remove(model);
        }

        public ActionResult PaypalPartial()
        {
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            return PartialView(cart);
        }

        // POST: /Cart/PlaceOrder
        [HttpPost]
        public void PlaceOrder()
        {
            // Get cart list
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            // Get username
            CustomPrincipal userOBJ = User as CustomPrincipal;
            int userId = userOBJ.Id;

            // Init OrderDM
            OrderDomainModel orderDM = new OrderDomainModel();
            orderDM.UserId = userId;
            orderDM.CreatedAt = DateTime.Now;

            // Save order and get order Id
            int orderId = orderBussiness.SaveOrder(orderDM).Id;

            // Init OrderDetailsDTO
            OrderDetailDomainModel orderDetailsDM = new OrderDetailDomainModel();

            // Add to OrderDetailsDTO
            foreach (var item in cart)
            {
                orderDetailsDM.OrderId = orderId;
                orderDetailsDM.UserId = userId;
                orderDetailsDM.ProductId = item.ProductId;
                orderDetailsDM.Quantity = item.Quantity;
                orderDetailBussiness.SaveOrderDetails(orderDetailsDM);

            }
            // Email admin
            var client = new SmtpClient("mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("21f57cbb94cf88", "e9d7055c69f02d"),
                EnableSsl = true
            };
            client.Send("admin@example.com", "admin@example.com", "New Order", "You have a new order. Order number " + orderId);

            // Reset session
            Session["cart"] = null;
        }


    }

}





