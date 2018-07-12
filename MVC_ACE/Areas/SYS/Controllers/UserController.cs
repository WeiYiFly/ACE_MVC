using MVC_ACE.Areas.SYS.Common;
using MVC_DATA;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_ACE.Areas.SYS.Controllers
{
    public class UserController : Controller
    {
        ACE_MVCEntities Database = new ACE_MVCEntities();
        // GET: SYS/User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CRUD()
        {
            User User_calss = new User();
            NameValueCollection forms = HttpContext.Request.Form;
            string action = forms.Get("Oper");

            string ID= Request["Id"];
            if(ID.Contains("_empty"))
                ID = ID.Substring(0, ID.Length - 7);
            string Name = Request["Name"];
            string Bu = Request["Bu"];
            string Email = Request["Email"];
            string Tel = Request["Tel"];
            string Passw = Request["Passw"];

            if (action == "add")
            {
                int number = User_calss.add( ID,Name,  Bu,  Email,  Tel,  Passw);
                if (number == 1)
                    HttpContext.Response.Write("添加成功");
                else
                    HttpContext.Response.Write("添加失敗");
            }
            if (action == "edit")
            {
                string[] id = ID.Trim().Split(',');
                int number = User_calss.Update(ID, Name, Bu, Email, Tel, Passw);
                if (number == 1)
                    HttpContext.Response.Write("修改成功");
                else
                    HttpContext.Response.Write("修改失敗");

            }
            if (action == "del")
            {
                string[] id = ID.Trim().Split(',');
                int number = User_calss.del(id);
                if (number >= 1)
                    HttpContext.Response.Write("=刪除成功");
                else
                    HttpContext.Response.Write("刪除失敗");

            }
            return Content("");
        }

        public ActionResult GetUser()
        {
            var program = Database.SYS_User.Where(u => u.Del != ""|| u.Del!=null).ToList();

            var jsonResult = Json(program, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

    
    }
}