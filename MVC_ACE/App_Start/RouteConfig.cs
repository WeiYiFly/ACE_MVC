using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_ACE
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional }
                   //defaults: new {Areas= "SYS/Program/jqGrid", controller = "Home", action = "Index", id = UrlParameter.Optional }
            //  new { area = "SYS", controller = "Home", action = "Index", id = UrlParameter.Optional },
            // new string[] { "MVC_ACE.Areas." + this.AreaName + ".Controllers" }
            );
           
        }
    }
}
