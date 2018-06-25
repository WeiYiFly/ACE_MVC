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
   
    public class ModuleController : Controller
    {
        ACE_MVCEntities DB = new ACE_MVCEntities();
        // GET: SYS/Module
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CRUD()
        {
            Module Module_calss = new Module();
            NameValueCollection forms = HttpContext.Request.Form;
            string action = forms.Get("Oper");
            string Id = forms.Get("Id");
            string Name = forms.Get("Name");
            string Lv = forms.Get("Lv");
            string Controller = forms.Get("Controller");
            string View_ = forms.Get("View_");
            string Url = forms.Get("Url");
            string Icon = Request["Icon"];
            string UpId = Request["UpId"];
            string ProgramId = Request["ProgramId"];
         
            if (action == "add")
            {
                int number = Module_calss.add(Name,Lv, Controller, View_,Url,Icon,UpId,ProgramId);
                if (number == 1)
                    HttpContext.Response.Write("添加成功");
                else
                    HttpContext.Response.Write("添加失敗");
            }
            if (action == "edit")
            {
                string[] id = Id.Trim().Split(',');
                int ID = int.Parse(id[0]);
                int number = Module_calss.Update(ID,Name, Lv, Controller, View_, Url, Icon, UpId, ProgramId);
                if (number == 1)
                    HttpContext.Response.Write("修改成功");
                else
                    HttpContext.Response.Write("修改失敗");

            }
            if (action == "del")
            {
                string[] id = Id.Trim().Split(',');
                int number = Module_calss.del(id);
                if (number >= 1)
                    HttpContext.Response.Write("=刪除成功");
                else
                    HttpContext.Response.Write("刪除失敗");

            }
            return Content("");
        }

        public ActionResult GetModule()
        {
            //var Module = DB.SYS_Module.Where(u => true).ToList();
            var Module = (from i in DB.SYS_Module
                          join   b in DB.SYS_Program on i.ProgramId equals b.Id into bb from bbb in bb.DefaultIfEmpty()
                          join c in DB.SYS_Module on i.UpId equals c.Id into cc  from ccc in cc.DefaultIfEmpty()

                              //join c in DB.SYS_Module on i.UpId equals bb.Id
                          select new
                          {
                              Id = i.Id,
                              Name = i.Name,
                              Controller = i.Controller,
                              View_ = i.View_,
                              Url = i.Url,
                              Icon = i.Icon,
                              UpId =ccc.Name,
                              ProgramId = bbb.ChinaName,
                              Lv= i.Lv
                              //UpId=i.UpId

                          }).ToList();
            var jsonResult = Json(Module, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public string  getlv(string lv)
        {
            string l = "啊";

            //switch (lv)
            //{
            //    case "0":l = "模塊";break;
            //    case "1": l = "菜單"; break;
            //    case "2": l = "子菜單"; break;
            //}
            return l;
        }

        //獲取所有程式
        public ActionResult Get_ProgarmId()
        {
            // string json = "  6: One;2: Two ";
            string json = "";
            var PrgId = from t in DB.SYS_Program
                        select new { t.Id, t.ChinaName };
            foreach (var i in PrgId)
            {
                string temp = i.Id + ":" + i.ChinaName;
                if(json =="")
                    json = temp;
                else
                    json = json + ";" + temp;
            }
            return Json(new
            {
                json = json
            });
        }
        //獲取上級菜單
        public ActionResult Get_UP(string id)
        {
            
            List<string> list_Json = new List<string>();
           // string json = "0:無";
            string json = "<option value='0'>無</option>";
             list_Json.Add(json);
            if (id != "" && id!=null )
            {
                int ID = int.Parse(id);
                var UpId = from t in DB.SYS_Module
                           where t.ProgramId== ID
                           select new { t.Id, t.Name };
                foreach (var i in UpId)
                {
                    //string temp = i.Id + ":" + i.Name;
                    //json = json + ";" + temp;
                    string temp = "<option value='" + i.Id + "'>"+ i.Name + "</option>";
                    list_Json.Add(temp);
                }

            }

            return Json(list_Json);
        }

        //獲取上級菜單
        public ActionResult Get_Lv(string id)
        {
            string json = "";           
            if (id != "" && id !=null)
            {
                if (id.Trim() == "0")
                {
                    json = "1";
                }
                int ID = int.Parse(id);
                var UpId = from t in DB.SYS_Module
                           where t.Id==ID
                           select new { t.Lv };
                
                foreach (var i in UpId)
                {
                    int  temp = int.Parse(i.Lv);
                    temp = temp + 1;
                    if (json == "")
                        json = temp.ToString();

                }

            }

            return Json(new
            {
                json = json
            });
        }
    }
}