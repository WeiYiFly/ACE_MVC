using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_ACE.Areas.SYS.Controllers
{
    public class ProgramController : Controller
    {
        // GET: SYS/Program
        public ActionResult Index()
        {
            ViewBag.Title = "";
            return View();
        }
    }
}