using System.Web;
using System.Web.Optimization;

namespace MentorConnect
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                        "~/Scripts/login.js"));

            bundles.Add(new ScriptBundle("~/bundles/mentorIndex").Include(
                        "~/Scripts/mentorIndex.js"));

            bundles.Add(new ScriptBundle("~/bundles/MentorshipList").Include(
                         "~/Scripts/MentorshipList.js"));

            bundles.Add(new ScriptBundle("~/bundles/mentorSchedule").Include(
                        "~/Scripts/mentorSchedule.js"));

            bundles.Add(new ScriptBundle("~/bundles/editmentorSchedule").Include(
                        "~/Scripts/EditMentorProfile.js"));

            bundles.Add(new ScriptBundle("~/bundles/mentorHomeIndex").Include(
                       "~/Scripts/MentorHomeIndex.js"));

            bundles.Add(new ScriptBundle("~/bundles/writeReview").Include(
                "~/Scripts/WriteReview.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/Content/mentorship").Include(
                        "~/Content/style.css"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/login").Include("~/Content/login.css"));
            
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/MentorIndex").Include("~/Content/MentorIndex.css"));

            bundles.Add(new StyleBundle("~/Content/MentorHome").Include("~/Content/MentorHome.css"));

            bundles.Add(new StyleBundle("~/Content/register").Include("~/Content/register.css"));

            bundles.Add(new StyleBundle("~/Content/MentorSchedule").Include("~/Content/MentorSchedule.css"));

            bundles.Add(new StyleBundle("~/Content/MentorshipList").Include("~/Content/MentorshipList.css"));

            bundles.Add(new StyleBundle("~/Content/writeReview").Include("~/Content/writeReview.css"));

            bundles.Add(new StyleBundle("~/Content/MentorScheduleDialog").Include("~/Content/MentorScheduleDialog.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}