using System.Web.Optimization;

namespace Marketplace.Admin
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                       "~/Scripts/site.js",
                       "~/Scripts/jquery.mCustomScrollbar.concat.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/gmap").Include(
                      "~/Scripts/jquery.gmap.js",
                      "~/Scripts/jquery.gmap_init.js"));

            bundles.Add(new ScriptBundle("~/bundles/services").Include(
               "~/Scripts/Marketplace/image-helper.js",
               "~/Scripts/Marketplace/datepicker-helper.js",
               "~/Scripts/Marketplace/services.js",
               "~/Scripts/Marketplace/location.js",
               "~/Scripts/Marketplace/product.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/serviceList").Include(
                "~/Scripts/Marketplace/serviceList.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/export").Include(
                "~/Scripts/Marketplace/export.js"));

            bundles.Add(new ScriptBundle("~/bundles/settings").Include(
                "~/Scripts/Marketplace/settings.js"));

            bundles.Add(new ScriptBundle("~/bundles/user").Include(
                "~/Scripts/Marketplace/users.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/bootstrap.css",
                      "~/Content/datepicker.css",
                      "~/Content/jquery.mCustomScrollbar.css",
                      "~/Content/site.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
