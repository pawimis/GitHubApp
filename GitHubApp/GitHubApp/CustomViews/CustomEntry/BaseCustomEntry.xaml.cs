using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GitHubApp.CustomViews.CustomEntry
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BaseCustomEntry : ContentView
    {
        #region Contructors
        public BaseCustomEntry()
        {
            InitializeComponent();
            Content.BindingContext = this;
        }
        #endregion Contructors

        #region Fields
        private bool _isEntryAPassword = true;
        #endregion Fields

        #region Bindable Properties
        public static readonly BindableProperty IsPasswordProperty = BindableProperty
            .Create(nameof(IsPassword), typeof(bool), typeof(BaseCustomEntry), propertyChanged: IsPasswordPropertyChanged);

        public static readonly BindableProperty MaxEntryLengthProperty = BindableProperty
          .Create(nameof(MaxEntryLength), typeof(int), typeof(BaseCustomEntry), int.MaxValue);

        public static readonly BindableProperty IsMandatoryProperty = BindableProperty
            .Create(nameof(IsMandatory), typeof(bool), typeof(BaseCustomEntry));

        public static readonly BindableProperty IsNotValidProperty = BindableProperty
            .Create(nameof(IsNotValid), typeof(bool), typeof(BaseCustomEntry));

        public static readonly BindableProperty TitleProperty = BindableProperty
            .Create(nameof(Title), typeof(string), typeof(BaseCustomEntry));

        public static readonly BindableProperty PlaceholderProperty = BindableProperty
            .Create(nameof(Placeholder), typeof(string), typeof(BaseCustomEntry), "");

        public static readonly BindableProperty TextProperty = BindableProperty
            .Create(nameof(Text), typeof(string), typeof(BaseCustomEntry), null, BindingMode.TwoWay);

        public static readonly BindableProperty ValidityPlaceholderProperty = BindableProperty
            .Create(nameof(ValidityPlaceholder), typeof(string), typeof(BaseCustomEntry));

        public static readonly BindableProperty SelectedKeyboardProperty = BindableProperty
          .Create(nameof(SelectedKeyboard), typeof(Keyboard), typeof(BaseCustomEntry), Keyboard.Default);

        public static readonly BindableProperty FrameColorProperty = BindableProperty
          .Create(nameof(FrameColor), typeof(Color), typeof(BaseCustomEntry), (Color)Application.Current.Resources["SecondaryLight"]);
        #endregion Bindable Properties

        #region Properties
        public bool IsMandatory { get => (bool)GetValue(IsMandatoryProperty); set => SetValue(IsMandatoryProperty, value); }
        public int MaxEntryLength { get => (int)GetValue(MaxEntryLengthProperty); set => SetValue(MaxEntryLengthProperty, value); }
        public Keyboard SelectedKeyboard { get => (Keyboard)GetValue(SelectedKeyboardProperty); set => SetValue(SelectedKeyboardProperty, value); }
        public bool IsNotValid { get => (bool)GetValue(IsNotValidProperty); set => SetValue(IsNotValidProperty, value); }
        public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
        public string ValidityPlaceholder { get => (string)GetValue(ValidityPlaceholderProperty); set => SetValue(ValidityPlaceholderProperty, value); }
        public bool IsPassword { get => (bool)GetValue(IsPasswordProperty); set => SetValue(IsPasswordProperty, value); }
        public Color FrameColor { get => (Color)GetValue(FrameColorProperty); set => SetValue(FrameColorProperty, value); }
        public bool IsEntryAPassword
        {
            get => _isEntryAPassword && IsPassword;
            set
            {
                _isEntryAPassword = value;
                OnPropertyChanged(nameof(IsEntryAPassword));
            }
        }
        #endregion Properties

        #region Private Methods
        private void ShowHidePassword(object sender, EventArgs e)
        {
            IsEntryAPassword = !_isEntryAPassword;
        }

        private static void IsPasswordPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as BaseCustomEntry).IsEntryAPassword = (bool)newValue;
        }
        #endregion Private Methods

    }
}