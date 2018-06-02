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
           
            return View();
        }
        public ActionResult JqGrid()
        {
           
            return View();
        }

        public ActionResult test()
        {


            string sdate = Request["sdate"];



            return Json("");
        }
        public ActionResult getJison()
        {
            string grid_data = @"
               [
                   { id: '1', name: 'Desktop Computer', age: 'note ' },
                   { id: '2', name: 'Laptop', age: 'Long text ' },
                   { id: '3', name: 'LCD Monitor', age: 'note3' },
                   { id: '4', name: 'Speakers', age: 'note' },
                   { id: '5', name: 'Laser Printer', age: 'note2' },
                   { id: '6', name: 'Play Station', age: 'note3' }
               ];";
            return Json(grid_data);
        }
    }
}