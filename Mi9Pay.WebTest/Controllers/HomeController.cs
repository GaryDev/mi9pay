using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mi9Pay.ViewModel.Test;

namespace Mi9Pay.WebTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new OrderViewModel());
        }
    }
}