using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepo;
        private readonly Cart _cartRepo;

        public OrderController(IOrderRepository orderRepo, Cart cartRepo)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
        }

        [Authorize]
        public ViewResult List() => View(_orderRepo.Orders.Where(o => !o.Shipped));

        [HttpPost]
        [Authorize]
        public IActionResult MarkShipped(int orderID)
        {
            Order order = _orderRepo.Orders.FirstOrDefault(o => o.OrderID == orderID);
            if (order != null)
            {
                order.Shipped = true;
                _orderRepo.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }

        public IActionResult Checkout()
        {
            return View(new Order());
        }
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (_cartRepo.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                order.Lines = _cartRepo.Lines.ToArray();
                _orderRepo.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }

        public ViewResult Completed()
        {
            _cartRepo.Clear();
            return View();
        }
    }
}
