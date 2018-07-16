using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_ACE.Areas.SYS.Controllers
{
    public class IndexController : Controller
    {
        // GET: SYS/Index
        public ActionResult Index()
        {
            return RedirectToAction("../../SYS/Program/Index");
            //return View();
        }
    }
}