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
            //查看出勤明細
            DataModelContainer DB1 = new DataModelContainer();
             var power_ = DB1.Power_.FirstOrDefault(u => u.id == UserID);
            if (power_ != null)
            {
                if (power_ != null && (power_.QC == "Y" || power_.BUx.Length>6))
                {
                    return RedirectToAction("../../RollCall/details/QCdetail");
                }
                if (power_.QC != "Y" && power_.BUx.Length <= 6)
                {
                    return RedirectToAction("../../RollCall/details/BUdetail/" + power_.BUx + ",0");
                }
            }
            //線長點名
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
            
            if (power == "線長")
            {
                return RedirectToAction("../../RollCall/Phone/CallNameSelectClass");
                // return RedirectToAction("/RollCall/Phone/CallNameSelectClass");
            }   
         
            return View();
        }
    }
}