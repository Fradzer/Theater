using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Theater.Models.Attribute;
using Theater.Models.Plays;
using Theater.WorkingDb.Connections;
using Theater.WorkingDb.Interfaces;

namespace Theater.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            IPlayDao playsDb = PlaysTableConnection.Instance;
            ViewBag.Plays = playsDb.GetAllPlays();

            IAuthorDao authorsDb = AuthorsTableConnection.Instance;             
            ViewBag.Authors = authorsDb.GetAllAuthors();

            IGenreDao genresDb = GenresTableConnection.Instance;             
            ViewBag.Genres = genresDb.GetAllGenres();

            IDateDao datesDb = DatesTableConnection.Instance;
            ViewBag.Dates = datesDb.GetAllDates().OrderBy(x => x.Date).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult ChangeLang(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            Session["lang"] = lang;

            return Redirect(returnUrl);
        }


    }
}