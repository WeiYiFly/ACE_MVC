using MVC_ACE.Areas.SYS.Common;
using MVC_DATA;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;

namespace MVC_ACE.Areas.SYS.Controllers
{
    public class ProgramController : Controller
    {
        // GET: SYS/Program
        ACE_MVCEntities Database = new ACE_MVCEntities();
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult JqGrid()
        {
            //for (int x = 0; x < 10000; x++)
            //{
            //    SYS_Program Program_table = new SYS_Program();
            //    Program_table.ChinaName = "測試" + x.ToString();
            //    Program_table.EnglishName = "Test" + x.ToString();

            //    Program_table.CreatorID = "DG170542";
            //    Program_table.CreatedTime = DateTime.Now;
            //    Database.SYS_Program.Add(Program_table);

            //}
            //Database.SaveChanges();
            return View();
        }

        public ActionResult CRUD()
        {
            Program Program_calss = new Program();
            NameValueCollection forms = HttpContext.Request.Form;
            string action = forms.Get("Oper");
            string ID= forms.Get("Id");
            string CName = forms.Get("ChinaName");
            string EName = Request["EnglishName"];
            string CID = Request["CreatorID"];
            string CCID = Request["Id"];
            if (action == "add")
            { 
                int number = Program_calss.add(CName, EName, CID);
                if (number == 1)
                    HttpContext.Response.Write("添加成功");
                else
                    HttpContext.Response.Write("添加失敗");
            }
            if (action == "edit")
            {
                string[] id = ID.Trim().Split(',');
               int number= Program_calss.Update(int.Parse(id[0]), CName, EName, CID);
                if (number == 1)
                    HttpContext.Response.Write("修改成功");
                else
                    HttpContext.Response.Write("修改失敗");

            }
            if (action == "del")
            {
                string[] id = ID.Trim().Split(',');
                int number = Program_calss.del(id);
                if (number >= 1)
                    HttpContext.Response.Write("=刪除成功");
                else
                    HttpContext.Response.Write("刪除失敗");

            }
            return Content("");
        }
        public ActionResult GetProgram()
        {
            var program=Database.SYS_Program.Where(u =>u.Del!="" ).ToList();
            var jsonResult= Json(program, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}