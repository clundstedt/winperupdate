using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace WinPerUpdateAdmin
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/bootstrap-datepicker.min.js",
                      "~/Scripts/locales/bootstrap-datepicker.es.min.js",
                      "~/Scripts/jasny-bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-datepicker.min.css",
                      "~/Content/jasny-bootstrap.min.css",
                      "~/Content/Site.css").Include("~/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                       "~/Scripts/angular.min.js"
                      ,"~/Scripts/angular-route.min.js"
                      ,"~/Scripts/angular-file-upload.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                      "~/js/Login/app.js",
                      "~/js/Login/login.js",
                      "~/js/Login/serviceLogin.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                      "~/js/Version/app.js"
                      ,"~/js/Version/admin.js"
                      ,"~/js/Version/version.js"
                      ,"~/js/Version/componente.js"
                      ,"~/js/Version/editcomponente.js"
                      ,"~/js/Version/serviceAdmin.js"
                      ));
        }
    }
}