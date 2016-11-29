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
    [Authorize(Roles = "Сourier, Admin")]
    public class CourierController : Controller
    {
        private static ILoginDao loginsDb;
        private static IDateDao datesDb;
        private static IOrderDao ordersDb;
        private static IPlayDao playsDb;

        public CourierController()
        {
            loginsDb = LoginsTableConnection.Instance;
            datesDb = DatesTableConnection.Instance;
            ordersDb = OrdersTableConnection.Instance;
            playsDb = PlaysTableConnection.Instance;
        }

        // GET: Courier
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: Courier/Orders
        public ActionResult Orders()
        {
            ViewBag.Dates = datesDb.GetAllDates();

            List<Order> orders = ordersDb.GetAllOrders().OrderBy(x => datesDb.GetDateById(x.DateId).Date).ToList();            

            ViewBag.Plays = playsDb.GetAllPlays();

            ViewBag.Logins = loginsDb.GetAllLogins();

            return View(orders);
        }

        [HttpPost]
        public ActionResult ChangeStatusOrder(int orderId, int? statusId)
        {
            if (statusId != null)
            {
                ordersDb.UpdateOrderStatusById(orderId, (int)statusId);
            }
            return RedirectToAction("Orders", "Courier");
        }
    }
}