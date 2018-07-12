using MVC_ACE.Models;
using MVC_DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_ACE.Areas.SYS.Controllers
{
    public class RolePermissionController : Controller
    {
        ACE_MVCEntities DB = new ACE_MVCEntities();
        // GET: SYS/RolePermission
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetRoleTreeData()
        {
            List<AceTree> tree = new List<AceTree>();
            string tree_data1 = "{";
            var tree_data = DB.SYS_Role.Where(u => true);
            foreach (var i in tree_data)
            {
                AceTree tr = new AceTree();
                tr.id = i.Id;
                tr.name = i.Name;
                tr.type = "item";
            }
            tree_data1 = tree_data1.Substring(0, tree_data1.Length - 1);
            tree_data1 = tree_data1+"}";
          //  return Json(tree_data1);
           return Json(new {
              
           });
        }
        
    }
}