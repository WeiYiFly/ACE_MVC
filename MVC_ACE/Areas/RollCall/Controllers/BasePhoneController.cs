using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_ACE.Areas.RollCall.Controllers
{
    public class BasePhoneController : Controller
    {
        // GET: RollCall/BasePhone
        public string UserID { get { try { return Session["UserID"].ToString(); } catch { RedirectToAction("../Home/Login"); return ""; } } set { } }
        public string UserBU { get { try { return Session["UserBU"].ToString(); } catch { RedirectToAction("../Home/Login"); return ""; } } set { } }
        public string UserCLASS { get { try { return Session["UserCLASS"].ToString(); } catch { RedirectToAction("../Home/Login"); return ""; } } set { Session["UserCLASS"] = value; } }
        public string UserLINENAME { get { try { return Session["UserLINENAME"].ToString(); } catch { RedirectToAction("../Home/Login"); return ""; } } set { } }
    }
}