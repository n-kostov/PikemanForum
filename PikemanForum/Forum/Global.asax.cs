using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Forum
{
    using System.Data.Entity;

    using Forum.Data;
    //using WebMatrix.WebData;

    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/fwlink/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer<ForumDbContext>(new MigrateDatabaseToLatestVersion<ForumDbContext, Forum.Data.Migrations.Configuration>());
            //var db = new ForumDbContext();
            //db.Database.Initialize(true);

            ControllerBuilder.Current.SetControllerFactory(new NonvalidatedInputControllerFactory());
        }
    }
}
