using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cerberus.Model;
using Cerberus.Services;
using Cerberus.Views;


namespace Cerberus.ViewModel
{
    class CreatePwdViewModel : NavigatableViewModel
    {

        pwdModel tmp = new pwdModel();

        String visibleSource = "visibleDrawingImage";
        String notVisibleSource = "notVisibleDrawingImage";

        bool visibility = false;

        public CreatePwdViewModel()
        {
            ImageSource = notVisibleSource;
            ErrorMessageText = "";
        }

        public pwdModel getModel()
        {
            return tmp;
        }

        public String ImageSource
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public bool Visibility
        {
            get { return visibility; }
            set
            {
                visibility = value;
                if (visibility)
                    ImageSource = visibleSource;
                else
                    ImageSource = notVisibleSource;
            }
        }

        public String AssociatedMail
        {
            get => tmp.AssociatedMail;
            set => tmp.AssociatedMail = value;
        }

        public String Username
        {
            get => tmp.Username;
            set => tmp.Username = value;
        }

        public String Password
        {
            get => tmp.Password;
            set => tmp.Password = value;
        }

        public String ConfirmationPassword
        {
            get => GetProperty<String>();
            set => SetProperty(value); 
        }

        public String Url
        {
            get => tmp.Url;
            set => tmp.Url = value;
        }

        public String Title
        {
            get => tmp.Title;
            set => tmp.Title = value;
        }

        public String TagsAsString
        {
            get
            {
                String result = String.Empty;
                for (int index = 0; index < tmp.Tags.Count; ++index)
                {
                    result += ToolBox.CleanStringForQuery(tmp.Tags[index]);
                    if (index != tmp.Tags.Count - 1)
                        result += ",";
                }
                return result;
            }
            set
            {
                String toProcess = value;
                tmp.Tags = toProcess.Split(',').ToList<String>();
            }
        }

        public List<String> Tags
        {
            get => tmp.Tags;
            set => tmp.Tags=value;
        }

        public String ErrorMessageText
        {
            get => GetProperty<string>();
            set { SetProperty(value); isErrorMessageVisible = String.IsNullOrEmpty(value) ? "Hidden" : "Visible"; }
        }

        public String isErrorMessageVisible
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public bool Validation()
        {
            bool result = true;
            ErrorMessageText = "";
            if (String.IsNullOrEmpty(Title))
            {
                result = false;
                ErrorMessageText += "-Title field is mandatory\n";
            }
            if(String.IsNullOrEmpty(AssociatedMail))
            {
                result = false;
                ErrorMessageText += "-Mail field is mandatory\n";
            }
            if(String.IsNullOrEmpty(Password))
            {
                result = false;
                ErrorMessageText += "-Password field is mandatory\n";
            }
            if(String.IsNullOrEmpty(ConfirmationPassword))
            {
                result = false;
                ErrorMessageText += "-Confirmation password field is mandatory\n";
            }
            if (!Password.Equals(ConfirmationPassword))
            {
                result = false;
                ErrorMessageText += "-Password and Confimation password are not equals\n";
            }
            
            return result;
        }

        //########## COMMANDS ############
        
        public Commands.BaseCommand Command_Button_Visibility
        {
            get
            {
                return new Commands.BaseCommand(Button_Visibility);
            }
        }

        private void Button_Visibility()
        {
            Visibility = !Visibility;
        }

        public Commands.BaseCommand<CreatePwdWindow> Command_Button_Close
        {
            get
            {
                return new Commands.BaseCommand<CreatePwdWindow>(Button_Close);
            }
        }

        private void Button_Close(CreatePwdWindow mvw)
        {
            mvw.Close();
        }

        public Commands.BaseCommand<CreatePwdWindow> Command_Button_Validate
        {
            get
            {
                return new Commands.BaseCommand<CreatePwdWindow>(Button_Validate);
            }
        }

        private void Button_Validate(CreatePwdWindow mvw)
        {
            bool valid = Validation();
            if (valid)
            {
                Window mainWindow = Application.Current.MainWindow;
                (((mainWindow.DataContext as MainViewModel).CurrentPage as PwdManagerPage).DataContext as PwdManagerViewModel).addNewPwd((mvw.DataContext as CreatePwdViewModel).getModel());
                mvw.Close();
            }
        }
    }
}
