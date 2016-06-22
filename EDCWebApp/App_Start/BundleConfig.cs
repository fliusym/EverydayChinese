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
                      "~/Scripts/bootstrap-select.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                    "~/Scripts/jquery.signalR-2.1.2.js"

            ));

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
                /*controllers*/
                "~/Client/controllers/controllerBase.js",
                 "~/Client/controllers/defaultController.js",
                 "~/Client/controllers/loginController.js",
                 "~/Client/controllers/studentController.js",
                 "~/Client/controllers/addLearnRequestController.js",
                 "~/Client/controllers/learnRequestController.js",
                 "~/Client/controllers/teacherController.js",
                 "~/Client/controllers/addNewWordController.js",
                 "~/Client/controllers/addNewScenarioController.js",
                 "~/Client/controllers/scenarioDetailController.js",
                 /*factories*/
                  "~/Client/services/serviceBase.js",
                  "~/Client/services/basicFactory.js",
                  "~/Client/services/wordFactory.js",
                  "~/Client/services/errorFactory.js",
                  "~/Client/services/authenticationFactory.js",
                  "~/Client/services/learnRequestFactory.js",
                  "~/Client/services/loginUserFactory.js",
                  "~/Client/services/signalRFactory.js",
                  "~/Client/services/canvasDrawFactory.js",
                  "~/Client/services/scenarioFactory.js",
                  /*filters*/
                  "~/Client/filters/filterBase.js",
                  "~/Client/filters/trustedFilter.js",
                  "~/Client/filters/dateShortFilter.js",
                  "~/Client/filters/directoryFilter.js",
                  /*directives*/
                  "~/Client/directives/directiveBase.js",
                  "~/Client/directives/audioDirective.js",
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
                  "~/Client/directives/wordBasicDirective.js",
                  "~/Client/directives/addLearnRequestDirective.js",
                  "~/Client/directives/teacherCanvasDirective.js",
                  "~/Client/directives/studentCanvasDirective.js",
                  "~/Client/directives/addWordDirective.js",
                  "~/Client/directives/addWordPhraseDirective.js",
                  "~/Client/directives/addWordSlangDirective.js",
                  "~/Client/directives/addWordPhraseExampleDirective.js",
                  "~/Client/directives/scenarioDirective.js",
                  "~/Client/directives/slangDirective.js",
                  "~/Client/directives/addScenarioDirective.js",
                  "~/Client/directives/addScenarioImageDirective.js",
                  "~/Client/directives/addScenarioWordDirective.js",
                  "~/Client/directives/userScenarioDirective.js"
                ));

        }
    }
}
