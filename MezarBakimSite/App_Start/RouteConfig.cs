using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MezarBakimSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "FiyatListesi",
            url: "fiyat-listesi",
            defaults: new { controller = "FiyatListesi", action = "Index" }
            );

            routes.MapRoute(
            name: "Iletisim",
            url: "iletisim",
            defaults: new { controller = "Iletisim", action = "Index" }
            );

            routes.MapRoute(
            name: "Hakkimizda",
            url: "hakkimizda",
            defaults: new { controller = "Hakkimizda", action = "Index" }
            );



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "AnaSayfa", action = "Index", id = UrlParameter.Optional }
            );
        }


     
    }
}
