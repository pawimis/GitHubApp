
using GitHubApp.CustomViews.CustomEntry;
using GitHubApp.iOS.CustomRenderer.CustomEntry;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]

namespace GitHubApp.iOS.CustomRenderer.CustomEntry
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                e.NewElement.SizeChanged += (obj, args) =>
                {


                    UITextField entry = Control;
                    if (entry == null)
                    {
                        return;
                    }

                    entry.Layer.MasksToBounds = true;
                    entry.BorderStyle = UITextBorderStyle.None;
                    entry.BackgroundColor = UIColor.Clear;
                };
            }
        }

    }
}