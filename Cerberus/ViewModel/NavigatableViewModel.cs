using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.ViewModel
{
    class NavigatableViewModel : BaseObservableViewModel
    {
        private MainViewModel navigator;
        public MainViewModel Navigator
        {
            get { return navigator; }
            set { navigator = value; }
        }
    }
}
