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
                IPlayDao playsDb = PlaysTableConnection.Instance;
                Play play = playsDb.GetPlayById(id);
                ViewBag.Play = play;

                IGenreDao genresDb = GenresTableConnection.Instance;
                ViewBag.Genre = genresDb.GetGenreById(play.GenreId);

                IAuthorDao authorsDb = AuthorsTableConnection.Instance;
                ViewBag.Author = authorsDb.GetAuthorById(play.AuthorId);

                IDateDao datesDb = DatesTableConnection.Instance;
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

            IDateDao dates = DatesTableConnection.Instance;
            IOrderDao orders = OrdersTableConnection.Instance;

            DatePlay date = dates.GetDateById(dateId);
            Play play = PlaysTableConnection.Instance.GetPlayById(date.PlayId);

            getViewBagForOrderPage(dateId);

            if (date.Date < DateTime.Now)
            {
                ModelState.AddModelError("Error order time!", Resources.Resource.ErrorOrderTime);
                return View();
            }

            if (!isTrueOrder(order, orders, dateId))
            {
                ModelState.AddModelError("Error order!", Resources.Resource.ErrorOrderNumber);
                return View();
            }
            else
            {
                try
                {
                    orders.AddOrder(new Order(0,
                                            dateId,
                                    CurrentUserService.GetCurrentUser().Id,
                                    (int)order.Category,
                                    order.Quantity,
                                    TheaterInformation.Prices.GetPricesByNameAndCategoryId(play.Name, (int)order.Category) * order.Quantity,
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
            IOrderDao orders = OrdersTableConnection.Instance;
            orders.DeleteOrderById(orderId);
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

            IOrderDao orders = OrdersTableConnection.Instance;

            IDateDao datesDb = DatesTableConnection.Instance;
            DatePlay currentDate = datesDb.GetDateById(id);

            IPlayDao playsDb = PlaysTableConnection.Instance;
            Play play = playsDb.GetPlayById(currentDate.PlayId);
            ViewBag.Play = play;

            IGenreDao genresDb = GenresTableConnection.Instance;
            ViewBag.Genre = genresDb.GetGenreById(play.GenreId);

            IAuthorDao authorsDb = AuthorsTableConnection.Instance;
            ViewBag.Author = authorsDb.GetAuthorById(play.AuthorId);

            ViewBag.Dates = datesDb.GetDatesByIdPlay(play.Id).OrderBy(x => x.Date).ToList();

            ViewBag.TotalCountBalconySeats = TheaterInformation.Balcony.CountSeats;
            ViewBag.PriceBalconySeats = TheaterInformation.Prices.GetPricesByName(play.Name).PriceBalcony;
            ViewBag.FreeBalconySeats = (TheaterInformation.Balcony.CountSeats -
                                    orders.GetCountBusySeetsByDateIdAndCategory(id, 0));

            ViewBag.TotalCountParterreSeats = TheaterInformation.Parterre.CountSeats;
            ViewBag.PriceParterreSeats = TheaterInformation.Prices.GetPricesByName(play.Name).PriceParterre;
            ViewBag.FreeParterreSeats = (TheaterInformation.Parterre.CountSeats -
                                    orders.GetCountBusySeetsByDateIdAndCategory(id, 1));
        }

        /// <summary>
        /// Checks order on correct
        /// </summary>
        /// <param name="order">Order, which need check</param>
        /// <param name="orders">databas, where saves orders</param>
        /// <param name="dateId">date id, when will be play</param>
        /// <returns>If order correct return true, else false</returns>
        private bool isTrueOrder(Order order, IOrderDao orders, int dateId)
        {
            int count1 = TheaterInformation.Balcony.CountSeats - orders.GetCountBusySeetsByDateIdAndCategory(dateId, 0);
            int count2 = TheaterInformation.Parterre.CountSeats - orders.GetCountBusySeetsByDateIdAndCategory(dateId, 1);

            return (((order.Category == Category.Balcony && order.Quantity < count1) ||
                    (order.Category == Category.Parterre && order.Quantity < count2))
                        && order.Quantity > 0);


        }

    }
}