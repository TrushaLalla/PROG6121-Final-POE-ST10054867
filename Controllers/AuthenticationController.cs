using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Trial.DatabaseContent;
using Trial.Models;

namespace Trial.Controllers
{
    public class AuthenticationController : Controller
    {
        PROG6212Database progdbs = new PROG6212Database();
        SingletonStudentIDHolder IDHolder = SingletonStudentIDHolder.Instance();
        // GET: AuthenticationController
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AuthenticateLogin(Authenticate authenticate)
        {

            string userId = progdbs.GetUser(authenticate.Username, authenticate.Password);

            if (userId != "")
            {
                IDHolder.setid(userId);
                return RedirectToAction("Index","Modules");
              
            }
            
            return View("Login");
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AuthenticateRegister(Authenticate authenticate)
        {
            Random ran = new Random();
            string hashed = getHashofPass(authenticate.Password.ToString());
            progdbs.UserRegistrationInsert(ran.Next(0, 1001), authenticate.Username, hashed);
            return View("Login");

        }

        public string getHashofPass(string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }
        // GET: AuthenticationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuthenticationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthenticationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthenticationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuthenticationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthenticationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuthenticationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
