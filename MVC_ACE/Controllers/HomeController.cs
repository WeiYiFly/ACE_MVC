using MVC_ACE.Models;
using MVC_DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_ACE.Controllers
{
    public class HomeController : Controller
    {
        ACE_MVCEntities DB = new ACE_MVCEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(string UserID1, string Password1)
        {
            if (!string.IsNullOrEmpty(UserID1) && !string.IsNullOrEmpty(Password1))
            {
                //驗證密碼
                var temp = DB.SYS_User.FirstOrDefault(u => u.Id == UserID1 && u.Passw == Password1);
                if (temp != null)
                {
                    Session["UserID"] = temp.Id;
                    //查詢有多少個系統權限
                    var tempUserPro = DB.SYS_UserSYS_Program.Where(u => u.SYS_UserID == temp.Id);
                    if (tempUserPro.Count() == 1)
                    {
                        foreach (var t in tempUserPro)
                        {
                            var tempPro = DB.SYS_Program.FirstOrDefault(u => u.Id == t.SYS_ProgramID);
                            string url = "../" + tempPro.EnglishName + "/Index/Index";
                            return RedirectToAction(url);
                        }
                        //查詢系統路徑跳轉                     
                    }
                    if (tempUserPro.Count() > 1)
                    {



                        return RedirectToAction("../Home/ChoosePro");
                    }
                }
                else
                {
                    string msg = "验证失败";
                    var script = string.Format("alert('{0}');", msg);
                    //return JavaScript(script);
                    return Content("<script>alert('验证失败');history.go(-1);</script>");
                }
            }
            return View();
        }

        public ActionResult ChoosePro()
        {
            string SYS_UserID = Session["UserID"].ToString();
            var tempUserPro = DB.SYS_UserSYS_Program.Where(u => u.SYS_UserID == SYS_UserID);
            List<ChoosePro> Chp = new List<ChoosePro>();
            //查詢系統路徑跳轉
            foreach (var t in tempUserPro)
            {
                ChoosePro chp = new ChoosePro();
                var tempPro = DB.SYS_Program.FirstOrDefault(u => u.Id == t.SYS_ProgramID);
                chp.Id = tempPro.Id;
                chp.Name = tempPro.ChinaName;
                chp.Url = "../" + tempPro.EnglishName + "/Index/Index";
                Chp.Add(chp);
            }
            ViewData["ChoosePro"] = Chp;
            return View();
        }

    }
}