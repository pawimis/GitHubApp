using GitHubApp.PageModel.Base;

using MvvmCross.Forms.Views;

using System.Runtime.CompilerServices;

namespace GitHubApp.Page.Base
{
    public class BaseContentPage<TViewModel> : MvxContentPage<TViewModel> where TViewModel : class, MvvmCross.ViewModels.IMvxViewModel
    {
        /// <summary>
        /// Filed for binding context
        /// </summary>
        private BasePageModel _basePageModel;

        public BaseContentPage()
        {
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }

        /// <summary>
        /// Method used for update binding contex
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext != null && BindingContext.DataContext != null)
            {
                _basePageModel = BindingContext.DataContext as BasePageModel;
            }
        }
        /// <summary>
        /// Method called after changed any property page
        /// </summary>
        /// <param name="propertyName">Changed name of property</param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (_basePageModel != null)
            {
                _basePageModel.CanNavigateBack = Navigation.NavigationStack.Count > 1;
            }
        }
    }
}
