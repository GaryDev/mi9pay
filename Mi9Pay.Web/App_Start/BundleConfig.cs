using System.Web;
using System.Web.Optimization;

namespace Mi9Pay.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryloading").Include(
                        "~/Scripts/jquery.showLoading.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/gateway").Include(
                      "~/Scripts/gateway/paymentSetting.js",
                      "~/Scripts/gateway/order.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
