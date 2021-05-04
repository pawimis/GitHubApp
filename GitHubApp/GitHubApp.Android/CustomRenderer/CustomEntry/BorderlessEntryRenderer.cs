using Android.Content;
using Android.Graphics.Drawables;

using GitHubApp.CustomViews.CustomEntry;
using GitHubApp.Droid.CustomRenderer.CustomEntry;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]

namespace GitHubApp.Droid.CustomRenderer.CustomEntry
{
    public class BorderlessEntryRenderer : EntryRenderer
    {

        public BorderlessEntryRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
            }
        }
    }
}