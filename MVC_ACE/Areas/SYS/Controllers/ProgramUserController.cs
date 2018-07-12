using MVC_DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_ACE.Areas.SYS.Controllers
{
    public class ProgramUserController : Controller
    {
        ACE_MVCEntities Database = new ACE_MVCEntities();
        // GET: SYS/ProgramUser
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetUser()
        {
            var User = Database.SYS_User.Where(u => true);
            return Json(User);
        }
        public ActionResult GetRoleNameList()
        {
            var program = (from s in Database.SYS_Program
                           select new { ProgramId = s.Id, ProgramName = s.ChinaName }).ToList();

            return Json(program);
        }
        public ActionResult GetHasRoleUserList()
        {
            int roleId = int.Parse(Request["RoleId"].ToString().Trim());
            var user =  (from x in Database.SYS_UserSYS_Program
                         from s in Database.SYS_User
                         where x.SYS_ProgramID== roleId&& x.SYS_UserID==s.Id
                         select new { UserId = s.Id, UserName = s.Name, UserBu= s.Bu }).ToList();

            return Json(user);
        }
        public ActionResult GetNoRoleUserList()
        {
            int roleId = int.Parse(Request["RoleId"].ToString().Trim());
            var temp = from s in Database.SYS_UserSYS_Program
                        where s.SYS_ProgramID == roleId
                        select s.SYS_UserID;
                var user = (from s in Database.SYS_User                      
                          where !temp.Contains(s.Id)
                        select new { UserId = s.Id, UserName = s.Name, UserBu = s.Bu }).ToList();
            return Json(user);
        }
        public ActionResult SetProgramUserList()
        {
            string ids = Request["Ids"].ToString();
            int roleId = Convert.ToInt32(Request["RoleId"].ToString());
            string[] a = ids.Split(',');
            foreach (string b in a)
            {
                if (!string.IsNullOrEmpty(b))
                {
                    SYS_UserSYS_Program up = new SYS_UserSYS_Program();
                    up.SYS_UserID = b;
                    up.SYS_ProgramID = roleId;
                    Database.SYS_UserSYS_Program.Add(up);
                }
              
             }
            int k=Database.SaveChanges();
            if(k>0)
               return Content("ok:角色分配成功");
            else
                return Content("no:角色分配失敗");
        }
        public ActionResult ProgramUserDel()
        {
            string roleId = Request["RoleId"].ToString();//程式ID
            string UserId = Request["EmpNo"].ToString();//工號
            int programId = int.Parse(roleId);

            var temp = Database.SYS_UserSYS_Program.FirstOrDefault(u => u.SYS_UserID == UserId && u.SYS_ProgramID == programId);
            Database.SYS_UserSYS_Program.Remove(temp);
 
      
            int k = Database.SaveChanges();
            if (k>0)
            {
                return Content("ok:刪除成功");
            }
            else
            {
                return Content("no:刪除失敗");
            }
        }

    }
}