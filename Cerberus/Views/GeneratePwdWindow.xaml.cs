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
using System.Windows.Shapes;
using Cerberus.ViewModel;
using Cerberus.Model;
using System.ComponentModel;

namespace Cerberus.Views
{
    /// <summary>
    /// Logique d'interaction pour GeneratePwdWindow.xaml
    /// </summary>
    public partial class GeneratePwdWindow : Window
    {
        private GeneratePwdViewModel PwdViewModel = new GeneratePwdViewModel();

        public GeneratePwdWindow()
        {
            InitializeComponent();
        }
    }
}
