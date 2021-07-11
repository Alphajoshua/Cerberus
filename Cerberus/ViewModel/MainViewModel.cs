using Cerberus.Views;
using Cerberus.DataAcces;
using Cerberus.Services;
using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Cerberus.ViewModel
{
    public class MainViewModel : BaseObservableViewModel
    {
        public MainViewModel()
        {
            Connection = new DbConnection();
            NavigateTo<SigningPage, SigningViewModel>(this);
        }

        public Page CurrentPage
        {
            get => GetProperty<Page>();
            set => SetProperty(value);
        }

        public DbConnection Connection
        {
            get => GetProperty<DbConnection>();
            set => SetProperty(value);
        }

       //public void NavigateToPwdManager()
       //{
       //    CurrentPage = NavigationService.GetPage<PwdManagerPage, PwdManagementViewModel>();
       //}
       //
       //public void NavigateToSigning()
       //{
       //    CurrentPage = NavigationService.GetPage<SigningPage, SigningViewModel>();
       //}

        public void NavigateTo<TView, TViewModel>(params object[] viewModelParameters)
            where TView : Page
            where TViewModel : BaseObservableViewModel
        {
            CurrentPage = NavigationService.GetPage<TView, TViewModel>(false, viewModelParameters);
        }

        public bool? ShowModalWindow<TView, TViewModel>(params object[] viewModelParameters)
             where TView : Window
            where TViewModel : BaseObservableViewModel
        {
            return NavigationService.ShowDialog<TView, TViewModel>(viewModelParameters);
        }

        public void ShowNonModalWindow<TView, TViewModel>(params object[] viewModelParameters)
            where TView : Window
            where TViewModel : BaseObservableViewModel
        {
            NavigationService.Show<TView, TViewModel>(viewModelParameters);
        }
    }
}
