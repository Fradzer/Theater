using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Theater.Models;
using Theater.Models.Attribute;
using Theater.Models.Plays;
using Theater.Models.Theater;
using Theater.WorkingDb;
using Theater.WorkingDb.Connections;
using Theater.WorkingDb.Interfaces;

namespace Theater.Controllers
{
    [Culture]
    public class PlaysController : Controller
    {
        private static ILoginDao loginsDb;
        private static IDateDao datesDb;
        private static IOrderDao ordersDb;
        private static IPlayDao playsDb;
        private static IAuthorDao authorsDb;
        private static IGenreDao genresDb;

        public PlaysController()
        {
            loginsDb = LoginsTableConnection.Instance;
            datesDb = DatesTableConnection.Instance;
            ordersDb = OrdersTableConnection.Instance;
            playsDb = PlaysTableConnection.Instance;
            authorsDb = AuthorsTableConnection.Instance;
            genresDb = GenresTableConnection.Instance;
        }

        // GET: Plays
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: Plays/Description
        public ActionResult Description(int id)
        {
            try
            {
                Play play = playsDb.GetPlayById(id);
                ViewBag.Play = play;

                ViewBag.Genre = genresDb.GetGenreById(play.GenreId);

                ViewBag.Author = authorsDb.GetAuthorById(play.AuthorId);

                ViewBag.Dates = datesDb.GetDatesByIdPlay(play.Id).OrderBy(x => x.Date).ToList();
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //Plays/Order/[id]
        [HttpGet]
        public ActionResult Order(int id)
        {
            try
            {
                getViewBagForOrderPage(id);
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Order(Order order, int dateId)
        {            
            DatePlay date = datesDb.GetDateById(dateId);
            Play play = PlaysTableConnection.Instance.GetPlayById(date.PlayId);

            getViewBagForOrderPage(dateId);

            if (date.Date < DateTime.Now)
            {
                ModelState.AddModelError("Error order time!", Resources.Resource.ErrorOrderTime);
                return View();
            }

            if (!isTrueOrder(order, dateId))
            {
                ModelState.AddModelError("Error order!", Resources.Resource.ErrorOrderNumber);
                return View();
            }
            else
            {
                try
                {
                    ordersDb.AddOrder(new Order(0,
                                            dateId,
                                    CurrentUserService.GetCurrentUser().Id,
                                    (int)order.Category,
                                    order.Quantity,
                                    TheaterInformation.GetPriceByCategoryId((int)order.Category) * order.Quantity,
                                    0));
                    return View("OrderAccepted");
                }
                catch (InvalidOperationException)
                {
                    ModelState.AddModelError("Error order!", Resources.Resource.ErrorOrder);
                    return View();
                }

            }
        }


        [HttpPost]
        [Authorize]
        public ActionResult DeleteOrder(int orderId)
        {
            ordersDb.DeleteOrderById(orderId);
            return RedirectToAction("Cart", "Account");
        }

        //Plays/OrderAccepted
        [Authorize]
        public ActionResult OrderAccepted()
        {
            return View();
        }

        /// <summary>
        /// Gets all need information in ViewBag
        /// </summary>
        /// <param name="id">Date id</param>
        private void getViewBagForOrderPage(int id)
        {
            ViewBag.DateId = id;


            DatePlay currentDate = datesDb.GetDateById(id);

            Play play = playsDb.GetPlayById(currentDate.PlayId);
            ViewBag.Play = play;

            ViewBag.Genre = genresDb.GetGenreById(play.GenreId);

            ViewBag.Author = authorsDb.GetAuthorById(play.AuthorId);

            ViewBag.Dates = datesDb.GetDatesByIdPlay(play.Id).OrderBy(x => x.Date).ToList();

            ViewBag.TotalCountBalconySeats = TheaterInformation.TotalCountBalconySeats;
            ViewBag.PriceBalconySeats = TheaterInformation.PriceBalcony;
            ViewBag.FreeBalconySeats = (TheaterInformation.TotalCountBalconySeats -
                                    ordersDb.GetCountBusySeetsByDateIdAndCategory(id, 0));

            ViewBag.TotalCountParterreSeats = TheaterInformation.TotalCountParterreSeats;
            ViewBag.PriceParterreSeats = TheaterInformation.PriceParterre;
            ViewBag.FreeParterreSeats = (TheaterInformation.TotalCountParterreSeats -
                                    ordersDb.GetCountBusySeetsByDateIdAndCategory(id, 1));
        }

        /// <summary>
        /// Checks order on correct
        /// </summary>
        /// <param name="order">Order, which need check</param>
        /// <param name="orders">databas, where saves orders</param>
        /// <param name="dateId">date id, when will be play</param>
        /// <returns>If order correct return true, else false</returns>
        private bool isTrueOrder(Order order, int dateId)
        {
            int count1 = TheaterInformation.TotalCountBalconySeats - ordersDb.GetCountBusySeetsByDateIdAndCategory(dateId, 0);
            int count2 = TheaterInformation.TotalCountParterreSeats - ordersDb.GetCountBusySeetsByDateIdAndCategory(dateId, 1);

            return (((order.Category == Category.Balcony && order.Quantity < count1) ||
                    (order.Category == Category.Parterre && order.Quantity < count2))
                        && order.Quantity > 0);


        }

    }
}