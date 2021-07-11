using Cerberus.ViewModel;
using Cerberus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Cerberus.Views
{
    /// <summary>
    /// Logique d'interaction pour SigningPage.xaml
    /// </summary>
    public partial class SigningPage : Page
    {

        private SigningViewModel ViewModelEntryPoint => (SigningViewModel)DataContext;

        public SigningPage()
        {
            InitializeComponent();
        }
        ~SigningPage()
        {

        }
    }
}
