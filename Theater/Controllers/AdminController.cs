using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Theater.Models.Attribute;
using Theater.Models.Plays;
using Theater.WorkingDb.Connections;
using Theater.WorkingDb.Interfaces;

namespace Theater.Controllers
{
    [Culture]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private static IAuthorDao authorsDb;
        private static IGenreDao genresDb;
        private static IPlayDao playsDb;
        private static IDateDao datesDb;
        private static int pageSize = 10;
        private static int normalyPageSize = 10;
        private static int pageSizeForFilter = 1000;

        private static string messagesAuthorTable;
        private static string messagesGenreTable;
        private static string messagesPlayTable;
        private static string messagesDatePlayTable;

        public AdminController()
        {
            authorsDb = AuthorsTableConnection.Instance;
            genresDb = GenresTableConnection.Instance;
            playsDb = PlaysTableConnection.Instance;
            datesDb = DatesTableConnection.Instance;
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View("Admin");
        }
        #region AuthorsCrud
        public ActionResult AuthorsTable(int page = 1)
        {
            ModelState.AddModelError("messages", messagesAuthorTable);
            messagesAuthorTable = null;

            List<Author> filterAuthors = TempData["AuthorList"] as List<Author>;

            List<Author> authors = filterAuthors ?? authorsDb.GetAllAuthors();
            List<Play> plays = playsDb.GetAllPlays();
            Hashtable idAndCountInPlays = new Hashtable();
            authors.ForEach(author => idAndCountInPlays.Add(author.Id, plays.Where(play => play.AuthorId == author.Id).Count()));
            ViewBag.idAndCountInPlay = idAndCountInPlays;

            int sizeForPage = pageSize;
            pageSize = normalyPageSize;

            page = truePage(page, (int)Math.Ceiling((double)authors.Count / pageSize));


            return View(authors.ToPagedList(page, pageSize));
        }

        [HttpPost]
        public ActionResult AuthorDelete(int id)
        {
            try
            {
                if (authorsDb.GetAuthorById(id) != null)
                {

                    authorsDb.DeleteById(id);
                    messagesAuthorTable = Resources.Resource.MessageDeleted;
                }
                else
                {
                    messagesAuthorTable = Resources.Resource.ErrorFound;
                }
                return RedirectToAction("AuthorsTable", "Admin");
            }
            catch
            {
                messagesAuthorTable = Resources.Resource.Error;
                return RedirectToAction("AuthorsTable", "Admin");
            }
        }

        [HttpPost]
        public ActionResult CreateAuthor(Author author)
        {
            try
            {
                authorsDb.Create(author);
                messagesAuthorTable = Resources.Resource.MessageCreated;
                return RedirectToAction("AuthorsTable", "Admin");
            }
            catch
            {
                messagesAuthorTable = Resources.Resource.Error;
                return RedirectToAction("AuthorsTable", "Admin");
            }
        }

        [HttpPost]
        public ActionResult AuthorUpdate(Author author)
        {
            try
            {
                authorsDb.Update(author);
                messagesAuthorTable = Resources.Resource.MessageUpdated;
                return RedirectToAction("AuthorsTable", "Admin");
            }
            catch
            {
                messagesAuthorTable = Resources.Resource.Error;
                return RedirectToAction("AuthorsTable", "Admin");
            }
        }

        [HttpPost]
        public ActionResult GetAuthorsByName(string name)
        {
            try
            {
                TempData["AuthorList"] = authorsDb.GetAuthorsByName(name);
                pageSize = pageSizeForFilter;
                return RedirectToAction("AuthorsTable", "Admin");
            }
            catch
            {
                return RedirectToAction("AuthorsTable", "Admin");
            }
        }
        #endregion

        #region GenresCrud
        public ActionResult GenresTable(int page = 1)
        {
            ModelState.AddModelError("messages", messagesGenreTable);
            messagesGenreTable = null;

            List<Genre> filterGenres = TempData["GenreList"] as List<Genre>;

            List<Genre> genres = filterGenres ?? genresDb.GetAllGenres();

            List<Play> plays = playsDb.GetAllPlays();
            Hashtable idAndCountInPlays = new Hashtable();
            genres.ForEach(genre => idAndCountInPlays.Add(genre.Id, plays.Where(play => play.GenreId == genre.Id).Count()));
            ViewBag.idAndCountInPlay = idAndCountInPlays;

            int sizeForPage = pageSize;
            pageSize = normalyPageSize;

            page = truePage(page, (int)Math.Ceiling((double)genres.Count / pageSize));

            return View(genres.ToPagedList(page, pageSize));
        }

        [HttpPost]
        public ActionResult GenreDelete(int id)
        {
            try
            {
                if (genresDb.GetGenreById(id) != null)
                {
                    genresDb.DeleteById(id);
                    messagesGenreTable = Resources.Resource.MessageDeleted;

                }
                else
                {
                    messagesGenreTable = Resources.Resource.ErrorFound;
                }
                return RedirectToAction("GenresTable", "Admin");
            }
            catch
            {
                messagesGenreTable = Resources.Resource.Error;
                return RedirectToAction("GenresTable", "Admin");
            }
        }

        [HttpPost]
        public ActionResult CreateGenre(Genre genre)
        {
            try
            {
                genresDb.Create(genre);
                messagesGenreTable = Resources.Resource.MessageCreated;
                return RedirectToAction("GenresTable", "Admin");
            }
            catch
            {
                messagesGenreTable = Resources.Resource.Error;
                return RedirectToAction("GenresTable", "Admin");
            }
        }
        [HttpPost]
        public ActionResult GenreUpdate(Genre genre)
        {
            try
            {
                genresDb.Update(genre);
                messagesGenreTable = Resources.Resource.MessageUpdated;
                return RedirectToAction("GenresTable", "Admin");
            }
            catch
            {
                messagesGenreTable = Resources.Resource.Error;
                return RedirectToAction("GenresTable", "Admin");
            }
        }

        [HttpPost]
        public ActionResult GetGenresByName(string name)
        {
            try
            {
                TempData["GenreList"] = genresDb.GetGenresByName(name);
                pageSize = pageSizeForFilter;
                return RedirectToAction("GenresTable", "Admin");
            }
            catch
            {
                return RedirectToAction("GenresTable", "Admin");
            }
        }
        #endregion

        #region PlaysCrud
        public ActionResult PlaysTable(int page = 1)
        {
            ModelState.AddModelError("messages", messagesPlayTable);
            messagesPlayTable = null;

            List<Play> filterPlays = TempData["PlayList"] as List<Play>;

            List<Play> plays = filterPlays ?? playsDb.GetAllPlays();
            Hashtable idAndCountInPlays = new Hashtable();
            plays.ForEach(play => idAndCountInPlays.Add(play.Id, datesDb.GetDatesByIdPlay(play.Id).Count()));
            ViewBag.idAndCountInPlay = idAndCountInPlays;

            int sizeForPage = pageSize;
            pageSize = normalyPageSize;

            page = truePage(page, (int)Math.Ceiling((double)plays.Count / pageSize));


            ViewBag.Authors = authorsDb.GetAllAuthors();
            ViewBag.Genres = genresDb.GetAllGenres();

            return View(plays.ToPagedList(page, pageSize));
        }



        [HttpPost]
        public ActionResult PlayDelete(int id)
        {
            try
            {
                if (playsDb.GetPlayById(id) != null)
                {
                    playsDb.DeleteById(id);
                    messagesPlayTable = Resources.Resource.MessageDeleted;

                }
                else
                {
                    messagesPlayTable = Resources.Resource.ErrorFound;
                }
                return RedirectToAction("PlaysTable", "Admin");
            }
            catch
            {
                messagesPlayTable = Resources.Resource.Error;
                return RedirectToAction("PlaysTable", "Admin");
            }
        }

        [HttpPost]
        public ActionResult CreatePlay(Play play, string authorName, string genreName)
        {
            try
            {
                play.AuthorId = authorsDb.GetAuthorsByName(authorName).First().Id;
                play.GenreId = genresDb.GetGenresByName(genreName).First().Id;

                playsDb.Create(play);
                messagesPlayTable = Resources.Resource.MessageCreated;
                return RedirectToAction("PlaysTable", "Admin");
            }
            catch (Exception ex)
            {
                messagesPlayTable = Resources.Resource.Error;
                return RedirectToAction("PlaysTable", "Admin");
            }
        }
        [HttpPost]
        public ActionResult PlayUpdate(Play play, string authorName, string genreName)
        {
            try
            {
                play.AuthorId = authorsDb.GetAuthorsByName(authorName).First().Id;
                play.GenreId = genresDb.GetGenresByName(genreName).First().Id;

                playsDb.Update(play);
                messagesPlayTable = Resources.Resource.MessageUpdated;

                return RedirectToAction("PlaysTable", "Admin");
            }
            catch
            {
                messagesPlayTable = Resources.Resource.Error;
                return RedirectToAction("PlaysTable", "Admin");
            }
        }

        [HttpPost]
        public ActionResult GetPlaysByName(string name)
        {
            try
            {
                TempData["PlayList"] = playsDb.GetPlaysByName(name);
                pageSize = pageSizeForFilter;

                return RedirectToAction("PlaysTable", "Admin");
            }
            catch
            {
                return RedirectToAction("PlaysTable", "Admin");
            }
        }
        #endregion

        #region DatePlaysCrud
        public ActionResult DatePlaysTable(int page = 1)
        {
            ModelState.AddModelError("messages", messagesDatePlayTable);
            messagesDatePlayTable = null;

            List<DatePlay> filterDate = TempData["Date"] as List<DatePlay>;

            List<DatePlay> dates = filterDate ?? datesDb.GetAllDates();

            int sizeForPage = pageSize;
            pageSize = normalyPageSize;

            page = truePage(page, (int)Math.Ceiling((double)dates.Count / pageSize));

            ViewBag.Plays = playsDb.GetAllPlays();

            return View(dates.ToPagedList(page, pageSize));
        }

        [HttpPost]
        public ActionResult DatePlayDelete(int id)
        {
            try
            {
                if (datesDb.GetDateById(id) != null)
                {
                    datesDb.DeleteById(id);
                    messagesDatePlayTable = Resources.Resource.MessageDeleted;

                }
                else
                {
                    messagesDatePlayTable = Resources.Resource.ErrorFound;
                }
                return RedirectToAction("DatePlaysTable", "Admin");
            }
            catch
            {
                messagesDatePlayTable = Resources.Resource.Error;
                return RedirectToAction("DatePlaysTable", "Admin");
            }
        }

        [HttpPost]
        public ActionResult DatePlayCreate(DatePlay date, string dateCheck, string playName)
        {
            try
            {
                DateTime newDate;
                if (DateTime.TryParse(dateCheck, out newDate) &&
                    datesDb.GetAllDates().Where(datePlay => datePlay.Date == newDate).Count() == 0)
                {
                    date.PlayId = playsDb.GetPlaysByName(playName).First().Id;

                    date.Date = newDate;
                    datesDb.Create(date);
                    messagesDatePlayTable = Resources.Resource.MessageCreated;
                    return RedirectToAction("DatePlaysTable", "Admin");
                }
                else
                {
                    messagesDatePlayTable = Resources.Resource.Error;
                    return RedirectToAction("DatePlaysTable", "Admin");
                }
            }
            catch (Exception ex)
            {
                messagesDatePlayTable = Resources.Resource.Error;
                return RedirectToAction("DatePlaysTable", "Admin");
            }
        }
        [HttpPost]
        public ActionResult DatePlayUpdate(DatePlay date, string dateCheck, string playName)
        {
            try
            {
                DateTime newDate;
                if (DateTime.TryParse(dateCheck, out newDate) &&
                    (datesDb.GetAllDates().Where(datePlay => datePlay.Date == newDate).Count() == 0 ||
                    datesDb.GetAllDates().First(datePlay => datePlay.Date == newDate).Id == date.Id))
                {
                    date.PlayId = playsDb.GetPlaysByName(playName).First().Id;


                    date.Date = newDate;
                    datesDb.Update(date);
                    messagesDatePlayTable = Resources.Resource.MessageUpdated;
                    return RedirectToAction("DatePlaysTable", "Admin");
                }
                else
                {
                    messagesDatePlayTable = Resources.Resource.Error;
                    return RedirectToAction("DatePlaysTable", "Admin");
                }
            }
            catch (Exception e)
            {
                messagesDatePlayTable = Resources.Resource.Error;
                return RedirectToAction("DatePlaysTable", "Admin");
            }
        }

        [HttpPost]
        public ActionResult GetDatesByPlayName(string playName)
        {
            try
            {
                TempData["Date"] = datesDb.GetDatesByPlayIds(playsDb.GetPlaysByName(playName).Select(a => a.Id).ToList());
                pageSize = pageSizeForFilter;

                return RedirectToAction("DatePlaysTable", "Admin");
            }
            catch
            {
                return RedirectToAction("DatePlaysTable", "Admin");
            }
        }
        #endregion

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