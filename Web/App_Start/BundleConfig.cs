using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/jquery").Include(
                        "~/Content/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/jqueryval").Include(
                        "~/Content/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备就绪，请使用 https://modernizr.com 上的生成工具仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/modernizr").Include(
                        "~/Content/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bootstrap").Include(
                      "~/Content/Scripts/bootstrap.js",
                      "~/Content/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/CSS").Include(
                      "~/Content/Css/bootstrap.css",
                      "~/Content/Css/Site.css"));

            bundles.Add(new ScriptBundle("~/Script").Include(
                    "~/Content/Scripts/jquery-1.10.2.min.js",
                    "~/Content/Scripts/bootstrap.min.js",
                    "~/Content/Scripts/metisMenu/jquery.metisMenu.js",
                    "~/Content/Scripts/slimscroll/jquery.slimscroll.min.js",
                    "~/Content/Scripts/flot/jquery.flot.js" ,
                    "~/Content/Scripts/flot/jquery.flot.tooltip.min.js",
                    "~/Content/Scripts/flot/jquery.flot.spline.js",
                    "~/Content/Scripts/flot/jquery.flot.resize.js",
                    "~/Content/Scripts/flot/jquery.flot.pie.js",
                    "~/Content/Scripts/jquery-ui/jquery-ui.min.js",
                    "~/Content/Scripts/gritter/jquery.gritter.min.js",
                    "~/Content/Scripts/toastr/toastr.min.js",
                    "~/Content/Scripts/jquery.extend.base.js",
                    "~/Content/Scripts/bootbox.min.js",
                    "~/Content/Scripts/pace/pace.min.js",
                    "~/Content/Scripts/Core.js"
                ));

            bundles.Add(new StyleBundle("~/Style").Include(
                   "~/Content/Css/bootstrap.min.css",
                   "~/Content/fonts/font-awesome/css/font-awesome.min.css",
                   "~/Content/CSS/style.css",
                   "~/Content/Css/Site.css",
                   "~/Content/Css/loaders.min.css"
                ));
        }
    }
}
