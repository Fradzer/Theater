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
            List<Author> authors = authorsDb.GetAllAuthors();
            List<Play> plays = playsDb.GetAllPlays();
            Hashtable idAndCountInPlays = new Hashtable();
            authors.ForEach(author => idAndCountInPlays.Add(author.Id, plays.Where(play => play.AuthorId == author.Id).Count()));
            ViewBag.idAndCountInPlay = idAndCountInPlays;

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
                    ModelState.AddModelError("Completed", Resources.Resource.Completed);

                }
                else
                {
                    ModelState.AddModelError("Not found author", Resources.Resource.ErrorFound);
                }
                return RedirectToAction("AuthorsTable", "Admin");
            }
            catch
            {
                ModelState.AddModelError("Error Delete", Resources.Resource.Error);
                return RedirectToAction("AuthorsTable", "Admin");
            }
        }

        [HttpPost]
        public ActionResult CreateAuthor(Author author)
        {
            try
            {
                authorsDb.Create(author);
                return RedirectToAction("AuthorsTable", "Admin");
            }
            catch
            {
                ModelState.AddModelError("Error Create", Resources.Resource.Error);
                return RedirectToAction("AuthorsTable", "Admin");
            }
        }

        [HttpPost]
        public ActionResult AuthorUpdate(Author author)
        {
            try
            {
                authorsDb.Update(author);
                return RedirectToAction("AuthorsTable", "Admin");
            }
            catch
            {
                ModelState.AddModelError("Error Update", Resources.Resource.Error);
                return RedirectToAction("AuthorsTable", "Admin");
            }
        }
        #endregion

        #region GenresCrud
        public ActionResult GenresTable(int page = 1)
        {            
            List<Genre> genres = genresDb.GetAllGenres();
            List<Play> plays = playsDb.GetAllPlays();
            Hashtable idAndCountInPlays = new Hashtable();
            genres.ForEach(genre => idAndCountInPlays.Add(genre.Id, plays.Where(play => play.GenreId == genre.Id).Count()));
            ViewBag.idAndCountInPlay = idAndCountInPlays;

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
                    ModelState.AddModelError("Completed", Resources.Resource.Completed);

                }
                else
                {
                    ModelState.AddModelError("Not found genre", Resources.Resource.ErrorFound);
                }
                return RedirectToAction("GenresTable", "Admin");
            }
            catch
            {
                ModelState.AddModelError("Error Delete", Resources.Resource.Error);
                return RedirectToAction("GenresTable", "Admin");
            }
        }

        [HttpPost]
        public ActionResult CreateGenre(Genre genre)
        {
            try
            {
                genresDb.Create(genre);
                return RedirectToAction("GenresTable", "Admin");
            }
            catch
            {
                ModelState.AddModelError("Error Create", Resources.Resource.Error);
                return RedirectToAction("GenresTable", "Admin");
            }
        }
        [HttpPost]
        public ActionResult GenreUpdate(Genre genre)
        {
            try
            {
                genresDb.Update(genre);
                return RedirectToAction("GenresTable", "Admin");
            }
            catch
            {
                ModelState.AddModelError("Error Update", Resources.Resource.Error);
                return RedirectToAction("GenresTable", "Admin");
            }
        }
        #endregion

        #region PlaysCrud
        public ActionResult PlaysTable(int page = 1)
        {
            List<Play> plays = playsDb.GetAllPlays();
            Hashtable idAndCountInPlays = new Hashtable();
            plays.ForEach(play => idAndCountInPlays.Add(play.Id, datesDb.GetDatesByIdPlay(play.Id).Count()));
            ViewBag.idAndCountInPlay = idAndCountInPlays;

            page = truePage(page, (int)Math.Ceiling((double)plays.Count / pageSize));

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
                    ModelState.AddModelError("Completed", Resources.Resource.Completed);

                }
                else
                {
                    ModelState.AddModelError("Not found play", Resources.Resource.ErrorFound);
                }
                return RedirectToAction("PlaysTable", "Admin");
            }
            catch
            {
                ModelState.AddModelError("Error Delete", Resources.Resource.Error);
                return RedirectToAction("PlaysTable", "Admin");
            }
        }

        [HttpPost]
        public ActionResult CreatePlay(Play play)
        {
            try
            {
                playsDb.Create(play);
                return RedirectToAction("PlaysTable", "Admin");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Error Create", Resources.Resource.Error);
                return RedirectToAction("PlaysTable", "Admin");
            }
        }
        [HttpPost]
        public ActionResult PlayUpdate(Play play)
        {
            try
            {
                playsDb.Update(play);
                return RedirectToAction("PlaysTable", "Admin");
            }
            catch
            {
                ModelState.AddModelError("Error Update", Resources.Resource.Error);
                return RedirectToAction("PlaysTable", "Admin");
            }
        }
        #endregion

        #region DatePlaysCrud
        public ActionResult DatePlaysTable(int page = 1)
        {
            List<DatePlay> dates = datesDb.GetAllDates();            

            page = truePage(page, (int)Math.Ceiling((double)dates.Count / pageSize));

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
                    ModelState.AddModelError("Completed", Resources.Resource.Completed);

                }
                else
                {
                    ModelState.AddModelError("Not found date", Resources.Resource.ErrorFound);
                }
                return RedirectToAction("DatePlaysTable", "Admin");
            }
            catch
            {
                ModelState.AddModelError("Error Delete", Resources.Resource.Error);
                return RedirectToAction("DatePlaysTable", "Admin");
            }
        }

        [HttpPost]
        public ActionResult CreateDatePlay(DatePlay date, string dateCheck)
        {
            try
            {
                DateTime newDate;
                if (DateTime.TryParse(dateCheck, out newDate) &&
                    datesDb.GetAllDates().Where(datePlay => datePlay.Date == newDate).Count() == 0)
                {
                    date.Date = newDate;
                    datesDb.Create(date);
                    return RedirectToAction("DatePlaysTable", "Admin");
                }
                else
                {
                    ModelState.AddModelError("Error Create", Resources.Resource.Error);
                    return RedirectToAction("DatePlaysTable", "Admin");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error Create", Resources.Resource.Error);
                return RedirectToAction("DatePlaysTable", "Admin");
            }
        }
        [HttpPost]
        public ActionResult DatePlayUpdate(DatePlay date, string dateCheck)
        {
            try
            {
                DateTime newDate;
                if (DateTime.TryParse(dateCheck, out newDate) &&
                    datesDb.GetAllDates().Where(datePlay => datePlay.Date == newDate).Count() == 0)
                {
                    date.Date = newDate;
                    datesDb.Update(date);
                    return RedirectToAction("DatePlaysTable", "Admin");
                }
                else
                {
                    ModelState.AddModelError("Error Create", Resources.Resource.Error);
                    return RedirectToAction("DatePlaysTable", "Admin");
                }
            }
            catch(Exception e)
            {
                ModelState.AddModelError("Error Update", Resources.Resource.Error);
                return RedirectToAction("DatePlaysTable", "Admin");
            }
        }
        #endregion


        private int truePage(int page, int pageCount)
        {
            if (page < 1)
            {
                page = 1;
            }
            else if (page > pageCount)
            {
                page = pageCount;
            }
            return page;
        }
    }
}