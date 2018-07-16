using RollCall.BLL;
using RollCall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_ACE.Areas.RollCall.Controllers
{
    public class IndexController : Controller
    {
        Sys_UsersService Sys_user = new Sys_UsersService();
        // GET: RollCall/Index
        public ActionResult Index()
        {
            string UserID = Session["UserID"].ToString();
            //
            //驗證PhoneUser 上有沒有數數據        
            var temp1 = Sys_user.DbSession.StaffDal.GetEntity(u => u.ID == UserID);
            string power = "";
            foreach (Staff a in temp1)
            {
                //Session["Users"] = temp1 as Staff;
                Session["UserID"] = a.ID;
                Session["UserBU"] = a.BU;
                Session["UserCLASS"] = a.CLASS;
                Session["UserLINENAME"] = a.LINENAME;
                power = a.POSITION;
            }
            //線長
            if (power == "線長")
            {
                return RedirectToAction("../../RollCall/Phone/CallNameSelectClass");
                // return RedirectToAction("/RollCall/Phone/CallNameSelectClass");
            }
            else
            {

                return RedirectToAction("../../RollCall/details/QCdetail");
            }
            //查看明細
         
            //return View();
        }
    }
}