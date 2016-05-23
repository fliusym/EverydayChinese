using System.Web;
using System.Web.Optimization;

namespace EDCWebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
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

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                     "~/Scripts/angular.js",
                     "~/Scripts/angular-sanitize.js",
                     "~/Scripts/angular-route.js",
                     "~/Scripts/angular-resource.js",
                     "~/Scripts/angular-messages.js",
                     "~/Scripts/angular-animate.js",
                     "~/Scripts/angular-touch.js",
                     "~/Scripts/angular-ui/ui-bootstrap.min.js",
                     "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/appjs").Include(
                "~/Client/app.js",
                "~/Client/controllers/controllerBase.js",
                 "~/Client/controllers/defaultController.js",
                  "~/Client/services/serviceBase.js",
                  "~/Client/services/basicFactory.js",
                  "~/Client/services/wordFactory.js",
                  "~/Client/services/errorFactory.js",
                  "~/Client/services/authenticationFactory.js",
                  "~/Client/services/learnRequestFactory.js",
                  "~/Client/services/loginUserFactory.js",
                  "~/Client/filters/filterBase.js",
                  "~/Client/filters/trustedFilter.js",
                  "~/Client/filters/dateShortFilter.js",
                  "~/Client/directives/directiveBase.js",
                  "~/Client/directives/characterDirective.js",
                  "~/Client/directives/dateDirective.js",
                  "~/Client/directives/dismissibleErrorDirective.js",
                  "~/Client/directives/everydayDirective.js",
                  "~/Client/directives/headerDirective.js",
                  "~/Client/directives/imagelistDirective.js",
                  "~/Client/directives/phraseDirective.js",
                  "~/Client/directives/pwdCompareDirective.js",
                  "~/Client/directives/quoteDirective.js",
                  "~/Client/directives/svgDirective.js",
                  "~/Client/directives/userItemContainerDirective.js",
                  "~/Client/directives/userLearnRequestDirective.js",
                  "~/Client/directives/userWordDirective.js",
                  "~/Client/directives/wordBasicDirective.js"
                ));

        }
    }
}
