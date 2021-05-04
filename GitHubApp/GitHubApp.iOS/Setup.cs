
using GitHubApp.Interface;
using GitHubApp.Service;
using GitHubApp.Utils.BusyManager;
using MvvmCross;
using MvvmCross.Forms.Platforms.Ios.Core;
using MvvmCross.Forms.Presenters;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

using Octokit;

namespace GitHubApp.iOS
{
    public class Setup : MvxFormsIosSetup<CoreApp, GitHubApp.App>
    {
        protected override Xamarin.Forms.Application CreateFormsApplication()
        {
            return new GitHubApp.App();
        }
        protected override IMvxApplication CreateApp()
        {
            return new CoreApp();
        }
        protected override IMvxFormsPagePresenter CreateFormsPagePresenter(IMvxFormsViewPresenter viewPresenter)
        {
            IMvxFormsPagePresenter formsPresenter = base.CreateFormsPagePresenter(viewPresenter);
            Mvx.IoCProvider.RegisterSingleton(formsPresenter);
            return formsPresenter;
        }
        /// <summary>
        /// Register implementations to IoC.
        /// </summary>
        /// <returns>The ioc provider.</returns>
        protected override IMvxIoCProvider InitializeIoC()
        {
            IMvxIoCProvider ioc = base.InitializeIoC();
            ioc.RegisterSingleton<IGitHubClient>(new GitHubClient(new ProductHeaderValue("RecrutationApp")));
            ioc.LazyConstructAndRegisterSingleton<IBusyManager, BusyManager>();
            ioc.LazyConstructAndRegisterSingleton<IGithubService, GithubWebService>();
            ioc.LazyConstructAndRegisterSingleton<IPopupNavigationService, PopupNavigationService>();
            ioc.LazyConstructAndRegisterSingleton<ISecureStorageService, SecureStorageService>();
            ioc.LazyConstructAndRegisterSingleton<ICacheService, CacheService>();
            return ioc;
        }
    }
}