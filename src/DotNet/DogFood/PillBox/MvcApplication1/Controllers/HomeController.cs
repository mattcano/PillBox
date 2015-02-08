using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {

        static List<Book> books = new List<Book>()
        {
            new Book { Id = 1, Author = "John", Title="Johns Book", DatePublished = DateTime.Now },
            new Book { Id = 2, Author = "Tom", Title="Toms Book", DatePublished = DateTime.Now },
            new Book { Id = 3, Author = "Ann", Title="Anns Book", DatePublished = DateTime.Now },
        };

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UpdateProducts()
        {
            return View(books);
        }

        //[HttpPost]
        //public ActionResult UpdateProducts(ICollection<Book> books)
        //{
        //    return View(books);
        //}

        //[HttpPost]
        //public ActionResult UpdateProducts(FormCollection c)
        //{

        //    var temp = c.Keys.Count;
        //    return View(books);
        //}

    }
}
