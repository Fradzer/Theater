﻿using System;
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
using PagedList;

namespace Theater.Controllers
{
    [Culture]
    public class HomeController : Controller
    {

        private static IDateDao datesDb;
        private static IPlayDao playsDb;
        private static IAuthorDao authorsDb;
        private static IGenreDao genresDb;

        private static int pageSize = 5;

        public HomeController()
        {
            authorsDb = AuthorsTableConnection.Instance;
            genresDb = GenresTableConnection.Instance;
            datesDb = DatesTableConnection.Instance;
            playsDb = PlaysTableConnection.Instance;
        }
        // GET: Home
        public ActionResult Index(int page = 1)
        {
            ViewBag.Authors = authorsDb.GetAllAuthors();

            ViewBag.Genres = genresDb.GetAllGenres();

            List<DatePlay> sortedDates = datesDb.GetAllDates().OrderBy(x => x.Date).ToList();
            ViewBag.Dates = sortedDates;

            var sortedPlays = playsDb.GetAllPlays().OrderBy(a => a.Name).ToList()
                            .Where(play => sortedDates.Where(date=> date.PlayId == play.Id).Count() > 0);

            page = truePage(page, (int)Math.Ceiling((double)sortedPlays.Count() / pageSize));

            return View(sortedPlays.ToPagedList(page, pageSize));
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


        [HttpPost]
        public ActionResult ChangeLang(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            Session["lang"] = lang;

            return Redirect(returnUrl);
        }


    }
}