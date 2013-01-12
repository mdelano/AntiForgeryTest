using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AntiForgeryTest.Controllers
{
    public class AntiForgeryController : Controller
    {
        [HttpGet]
        public ActionResult AntiForgeryForm()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AntiForgeryFormPost()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UnsafeForm()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UnsafeFormPost()
        {
            return View();
        }

    }
}
