using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Theater.Models.Attribute;
using Theater.Models.Plays;
using Theater.WorkingDb.Connections;
using Theater.WorkingDb.Interfaces;
using Theater.Models.Account;

namespace Theater.Controllers
{
    [Culture]
    [Authorize(Roles = "Сourier")]
    public class CourierController : Controller
    {
        // GET: Courier
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: Courier/Orders
        public ActionResult Orders()
        {
            IDateDao datesDb = DatesTableConnection.Instance;
            ViewBag.Dates = datesDb.GetAllDates();

            IOrderDao ordersDb = OrdersTableConnection.Instance;
            List<Order> orders = ordersDb.GetAllOrders().OrderBy(x => datesDb.GetDateById(x.DateId).Date).ToList();
            

            IPlayDao playsDb = PlaysTableConnection.Instance;
            ViewBag.Plays = playsDb.GetAllPlays();

            ILoginDao loginsDb = LoginsTableConnection.Instance;
            ViewBag.Logins = loginsDb.GetAllLogins();

            return View(orders);
        }

        [HttpPost]
        public ActionResult ChangeStatusOrder(int orderId, int? statusId)
        {
            if (statusId != null)
            {
                IOrderDao orders = OrdersTableConnection.Instance;
                orders.UpdateOrderStatusById(orderId, (int)statusId);
            }
            return RedirectToAction("Orders", "Courier");
        }
    }
}