using Cerberus.Views;
using Cerberus.Services;
using Cerberus.DataAcces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace Cerberus.ViewModel
{
    class SigningUpViewModel : NavigatableViewModel
    {
        public SigningUpViewModel(ref MainViewModel address)
        {
            Navigator = address;
            ErrorMessageText = "";
            AlreadyExist = false;
        }

        public void NavigateToSigning()
        {
            Navigator.NavigateTo<SigningPage, SigningViewModel>(Navigator);
        }

        public void NavigateToPwdManager()
        {
            Navigator.NavigateTo<PwdManagerPage, PwdManagerViewModel>(Navigator);
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

        public String Username
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public String Mail
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }


        public bool AlreadyExist
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
        }

        public bool Validation(SecureString tmpPassword, SecureString tmpConfirmPassword)
        {
            bool validation = true;
            ErrorMessageText = "";

            if (String.IsNullOrEmpty(Username))
            {
                ErrorMessageText = "-Username field is empty, you must fill it to sign up\n";
                validation = false;
            }
            if(String.IsNullOrEmpty(Mail))
            {
                ErrorMessageText += "-Mail field is empty, you must fill it to sign up\n";
                validation = false;
            }
            if(tmpPassword.Length==0)
            {
                ErrorMessageText += "-Password field is empty, you must fill it to sign up\n";
                validation = false;
            }
            if(tmpConfirmPassword.Length==0)
            {
                ErrorMessageText += "-Confirm password field is empty, you must fill it to sign up\n";
                validation = false;
            }

            if(validation && !ToolBox.IsEqualTo(tmpPassword,tmpConfirmPassword ))
            {
                ErrorMessageText += "-You must enter the same password twice to sign up\n";
                validation = false;
            }

            return validation;
        }

        private bool Exist(SecureString tmpPassword)
        {
            bool result = false;
            OracleDataReader reader = Navigator.Connection.Query("select * from cerberus_users where username='" + Username + "'");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string username = reader.GetString(1);

                    if (username.Equals(Username))
                        result = true;
                    else
                        result = false;
                }
            }
            return result;
        }

        private bool CreateNewAccount(SecureString tmpPassword)
        {
            bool result =false;
            if(!Exist(tmpPassword))
            {
                int rowChange = Navigator.Connection.NonQuery("insert into cerberus_users (username,password,mail) values('" + Username + "','" + Cypher.Encrypt(ToolBox.ToString(tmpPassword)) + "','" + Mail + "')");

                if (rowChange > 0)
                {
                    OracleDataReader reader = Navigator.Connection.Query("select * from cerberus_users where username='" + Username + "'");
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string username = reader.GetString(1);
                            SecureString pwd = ToolBox.ToSecureString(Cypher.Decrypt(reader.GetString(2)));
                            string mail = reader.GetString(3);

                            if (username.Equals(Username) && ToolBox.IsEqualTo(pwd, tmpPassword) && mail.Equals(Mail))
                                result = true;
                            else
                                result = false;
                        }
                    }
                }
            }
            else
            {
                AlreadyExist = true;
            }
            return result;
        }

        //############# COMMANDS ###############

        public Commands.BaseCommand Command_Button_Cancel
        {
            get
            {
                return new Commands.BaseCommand(Button_Cancel);
            }
        }

        private void Button_Cancel()
        {
            NavigateToSigning();
        }

        public Commands.BaseCommand<Cerberus.Views.SigningUpPage> Command_Button_Validation
        {
            get
            {
                return new Commands.BaseCommand<Cerberus.Views.SigningUpPage>(Button_Validation);
            }
        }

        private void Button_Validation(Cerberus.Views.SigningUpPage sup)
        {
            ErrorMessageText = "";
            bool valid = Validation(sup.PasswordBox.SecurePassword, sup.ConfirmPasswordBox.SecurePassword);
            if (valid)
            {
                if(CreateNewAccount(sup.PasswordBox.SecurePassword))
                    NavigateToSigning();
                else
                    ShowErrorAccount();
            }
        }

        private void ShowErrorAccount()
        {
            if(AlreadyExist)
            {
                MessageBox.Show("Error creating a new account\n This account already exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                AlreadyExist = false;
            }
            else
            {
                MessageBox.Show("Error creating a new account\nPlease try again or else contact our support service", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
