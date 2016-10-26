using System.Web;
using System.Web.Optimization;

namespace CMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/category.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/style.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/alertify.js",
                      "~/Scripts/alertify.min.js",
                      "~/Scripts/category.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/style.css",
                      "~/Content/Category.css",
                      "~/Content/alertifyjs/themes/alertify.css",
                      "~/Content/alertifyjs/themes/alertify.min.css",
                      "~/Content/alertifyjs/themes/alertify.rtl.css",
                      "~/Content/alertifyjs/themes/alertify.rtl.min.css",
                      "~/Content/font-awesome.css",
                      "~/Content/font-awesome.min.css"));
        }
    }
}
