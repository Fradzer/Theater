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
using PagedList;

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
        private static int pageSize = 10;
        private static int normalyPageSize = 10;
        private static int pageSizeForFilter = 1000;


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
        public ActionResult Orders(int page = 1)
        {
            ViewBag.Dates = datesDb.GetAllDates();

            List<Order> filterOrder = (TempData["OrderList"] as List<Order>);
            List<Order> orders = (filterOrder ?? ordersDb.GetAllOrders()).Where(order => order.Status != StatusOrder.Completed)
                                .OrderBy(x => datesDb.GetDateById(x.DateId).Date).ToList();

            int sizeForPage = pageSize;
            pageSize = normalyPageSize;

            page = truePage(page, (int)Math.Ceiling((double)orders.Count / sizeForPage));

            ViewBag.Plays = playsDb.GetAllPlays();

            ViewBag.Logins = loginsDb.GetAllLogins();

            return View(orders.ToPagedList(page, sizeForPage));
        }


        [HttpPost]
        public ActionResult GetOrders(int? countTickets)
        {
            try
            {
                if (countTickets != null)
                {
                    TempData["orderList"] = ordersDb.GetOrderByCountTickets((int)countTickets);
                    pageSize = pageSizeForFilter;
                }
                return RedirectToAction("Orders", "Courier");
            }
            catch
            {
                return RedirectToAction("Orders", "Courier");
            }
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

        private int truePage(int page, int pageCount)
        {
            if (page > pageCount)
            {
                page = pageCount;
            }
            if (page < 1)
            {
                page = 1;
            }
            return page;
        }

    }
}