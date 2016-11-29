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
        private static int pageSize = 10;
        private static IPlayDao playsDb;

        public AdminController()
        {
            authorsDb = AuthorsTableConnection.Instance;
            genresDb = GenresTableConnection.Instance;
            playsDb = PlaysTableConnection.Instance;

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

        public ActionResult GenresTable()
        {
            return View();
        }

        public ActionResult PlaysTable()
        {
            return View();
        }

        public ActionResult DatesTable()
        {
            return View();
        }
    }
}