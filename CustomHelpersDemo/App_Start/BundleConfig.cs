
using System.Web.Optimization;
namespace CustomHelpersDemo
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region JS

            //  jQuery Framework
            bundles.Add(new ScriptBundle("~/js/jquery")
                .Include("~/Scripts/jquery/jquery-1.10.2.js"));

            //  AjaxManager
            bundles.Add(new ScriptBundle("~/js/base")
                .Include(   "~/Scripts/AjaxManager.js",
                            "~/Scripts/App.js"));


            //  Custom Helpers Scripts
            bundles.Add(new ScriptBundle("~/js/controlHelper")
                .Include("~/Scripts/CustomHelper/CustomHelperForm.js",
                            "~/Scripts/CustomHelper/jqgrid/jquery.jqGrid.js",
                            "~/Scripts/CustomHelper/jqgrid/i18n/grid.locale-es.js"
                ));
            

            //  Pages
            bundles.Add(new ScriptBundle("~/js/demo-form")
                .Include("~/Scripts/pages/home/DemoForm.js"));

            #endregion JS


            #region CSS

            //  Base CSS
            bundles.Add(new StyleBundle("~/css/app")
                            .Include("~/Content/Main.css"));

            //  jQGrid use CSS from JqueryUI
            bundles.Add(new StyleBundle("~/css/controlHelper")
                            .Include("~/Content/jquery/jquery-ui-1.10.3.css",
                                        "~/Content/jqgrid/ui.jqgrid.css"));
            #endregion CSS


            //  Minify JS and CSS
            BundleTable.EnableOptimizations = true;
        }
    }
}