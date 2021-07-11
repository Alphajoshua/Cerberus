using Cerberus.ViewModel;
using Cerberus.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.IO;
using System.Security;

//Animation
using System.Windows.Media.Animation;

namespace Cerberus.Views
{
    /// <summary>
    /// Logique d'interaction pour PwdManager.xaml
    /// </summary>
    public partial class PwdManagerPage : Page
    {
        private PwdManagerViewModel ViewModelEntryPoint => (PwdManagerViewModel)DataContext;

        public PwdManagerPage()
        {
            InitializeComponent();
        }
    }
}
