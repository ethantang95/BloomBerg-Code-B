using System;
using System.Web;
using System.Web.Optimization;

namespace BloomBerg_Code_B {
    public class BundleConfig {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/site.css"));

            AddAngularScriptBundles(bundles);
            AddAngularStyleBundles(bundles);

            AddHomeScriptBundles(bundles);
        }

        private static void AddHomeScriptBundles(BundleCollection bundles) {
            var bundle = new ScriptBundle("~/bundles/home").Include(
                    "~/Scripts/home/home.js");
            bundles.Add(bundle);
        }

        private static void AddAngularScriptBundles(BundleCollection bundles) {
            var bundle = new ScriptBundle("~/bundles/angular").Include(
                    "~/Scripts/angular/angular.js",
                    "~/Scripts/angular-route.js",
                    "~/Scripts/angular-animate/angular-animate.js",
                    "~/Scripts/angular-aria/angular-aria.js",
                    "~/Scripts/angular-material/angular-material.js");
            bundles.Add(bundle);
        }

        private static void AddAngularStyleBundles(BundleCollection bundles) {
            var bundle = new StyleBundle("~/Content/angular").Include(
                    "~/Content/angular-material.css");
            bundles.Add(bundle);
        }
    }
}
