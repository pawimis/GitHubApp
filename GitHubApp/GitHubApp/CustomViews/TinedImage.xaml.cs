using FFImageLoading.Forms;
using FFImageLoading.Transformations;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GitHubApp.CustomViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TinedImage : ContentView
    {
        #region Contructors
        public TinedImage()
        {
            InitializeComponent();
            Content.BindingContext = this;
        }
        #endregion Contructors

        #region Bindable Properties
        public static readonly BindableProperty TintColorProperty = BindableProperty
            .Create(nameof(TintColor), typeof(Color), typeof(Color), Color.Transparent, BindingMode.TwoWay, propertyChanged: TintColorChanged);
        public static readonly BindableProperty SourceProperty = BindableProperty
            .Create(nameof(Source), typeof(ImageSource), typeof(ImageSource));
        #endregion Bindable Properties

        #region Properties
        public Color TintColor { get => (Color)GetValue(TintColorProperty); set => SetValue(TintColorProperty, value); }
        public ImageSource Source { get => (ImageSource)GetValue(SourceProperty); set => SetValue(SourceProperty, value); }
        #endregion Properties

        #region Private Methods
        private static void TintColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CachedImage image = (bindable as TinedImage).CachedImage;
            image.Transformations.Clear();
            image.Transformations.Add(new TintTransformation()
            {
                R = (int)(((Color)newValue).R * 255),
                G = (int)(((Color)newValue).G * 255),
                B = (int)(((Color)newValue).B * 255),
                A = 255,
                EnableSolidColor = true
            });
            image.ReloadImage();
        }
        #endregion Private Methods
    }
}