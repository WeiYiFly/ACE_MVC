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
    public class RoleController : Controller
    {
        ACE_MVCEntities Database = new ACE_MVCEntities();
        // GET: SYS/Role
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CRUD()
        {
            Role Role_calss = new Role();
            NameValueCollection forms = HttpContext.Request.Form;
            string action = forms.Get("Oper");
            string ID = forms.Get("Id");
            int PrID=0 ;
            if (!string.IsNullOrEmpty(forms.Get("ProgramId")))
            { 
                 PrID = int.Parse(forms.Get("ProgramId").ToString());
            }
            string Name = Request["Name"];
            string Remark = Request["Remark"];
            string CID = Request["CreatorID"];

            
            if (action == "add")
            {
                int number = Role_calss.add(PrID,Name, Remark, CID);
                if (number == 1)
                    HttpContext.Response.Write("添加成功");
                else
                    HttpContext.Response.Write("添加失敗");
            }
            if (action == "edit")
            {
                string[] id = ID.Trim().Split(',');
                int number = Role_calss.Update(int.Parse(id[0]), PrID, Name, Remark, CID);
                if (number == 1)
                    HttpContext.Response.Write("修改成功");
                else
                    HttpContext.Response.Write("修改失敗");

            }
            if (action == "del")
            {
                string[] id = ID.Trim().Split(',');
                int number = Role_calss.del(id);
                if (number >= 1)
                    HttpContext.Response.Write("刪除成功");
                else
                    HttpContext.Response.Write("刪除失敗");

            }
            return Content("");
        }

        public ActionResult GetRole()
        {
            var program = Database.SYS_Role.Where(u => u.Del != "").ToList();

            var jsonResult = Json(program, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult GetProgram()
        {
            string Jsonn = "0:共用角色";
            var program = Database.SYS_Program.Where(u => u.Del != "" || u.Del != null).ToList();
            foreach (var i in program)
            {
                Jsonn = Jsonn + ";" + i.Id + ":" + i.ChinaName;
            }
            //var jsonResult = Json(program, JsonRequestBehavior.AllowGet);

            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
            return Json( new { json= Jsonn });
        }
    }
}