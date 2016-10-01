using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Theater.Models;
using Theater.Models.Account;
using Theater.Models.Attribute;
using Theater.Models.AuthorizeAttributes;
using Theater.Models.Plays;
using Theater.WorkingDb.Connections;
using Theater.WorkingDb.Interfaces;

namespace Theater.Controllers
{
    [Culture]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Account/Register
        [HttpGet]
        [NoAuthorize]
        public ActionResult Register()
        {
            //var newUser = new User();
            return View();
        }

        [HttpPost]
        [NoAuthorize]
        public ActionResult Register(User user, string confirmPassword)
        {
            if (isTrueUser(user, confirmPassword))
            {
                ILoginDao logins = LoginsTableConnection.Instance;
                logins.AddAccount(user);

                CreateAuthCookie(user);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }

        }

        // GET: /Account/Login
        [HttpGet]
        [NoAuthorize]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [NoAuthorize]
        public ActionResult Login(string email, string password)
        {
            ILoginDao logins = LoginsTableConnection.Instance;
            User user = logins.GetUserByEmailAndPassword(email, password);
            if (user == null)
            {
                ModelState.AddModelError("Incorrectrly entered", Resources.Resource.ErrorLoginInput);
                return View();
            }
            else
            {
                CreateAuthCookie(user);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: /Account/Logout
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Cart
        [HttpGet]
        [Authorize]
        public ActionResult Cart()
        {
            IDateDao datesDb = DatesTableConnection.Instance;
            ViewBag.Dates = datesDb.GetAllDates();

            IOrderDao ordersDb = OrdersTableConnection.Instance;
            List<Order> orders = ordersDb.GetOrdersByIdLogin(CurrentUserService.GetCurrentUser().Id)
                .OrderBy( x=> datesDb.GetDateById(x.DateId).Date).ToList();

            IPlayDao playsDb = PlaysTableConnection.Instance;
            ViewBag.Plays = playsDb.GetAllPlays();


            return View(orders);
        }

        /// <summary>
        /// Creates cookie of authorize for person
        /// </summary>
        /// <param name="user">user, who authorize</param>
        /// <param name="timeout">time out for cookie</param>
        /// <param name="persistent">works cookie for close browser</param>
        private void CreateAuthCookie(User user)
        {
            DateTime timeoutCookie = DateTime.Now.AddMinutes(15);
            bool persistentCookie = false;

            var ticket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now,
                                            timeoutCookie, persistentCookie, user.Role.ToString());
            var encTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            cookie.Expires = timeoutCookie;
            Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Checks user on correct
        /// </summary>
        /// <param name="user">User, who need check</param>
        /// <param name="confirmPassword">password for confirm</param>
        /// <returns>If user correct return true, else return false</returns>
        private bool isTrueUser(User user, string confirmPassword)
        {
            bool trueUser = true;
            if (!ValidationAccount.IsTrueName(user.Name))
            {
                ModelState.AddModelError("Name", Resources.Resource.ErrorName);
                trueUser = false;
            }

            if (!ValidationAccount.IsTruePhone(user.Phone))
            {
                ModelState.AddModelError("Phone", Resources.Resource.ErrorPhone);
                trueUser = false;
            }

            if (!ValidationAccount.IsTrueEmail(user.Email))
            {
                ModelState.AddModelError("Email", Resources.Resource.ErrorEmail);
                trueUser = false;
            }

            if (!ValidationAccount.IsTruePassword(user.Password))
            {
                ModelState.AddModelError("Password", Resources.Resource.ErrorPassword);
                trueUser = false;
            }
            else
            {
                if (!ValidationAccount.IsTrueConfirmPassword(user.Password, confirmPassword))
                {
                    ModelState.AddModelError("ConfirmPassword", Resources.Resource.ErrorConfirmPassword);

                    trueUser = false;
                }
            }

            return trueUser;
        }
    }
}