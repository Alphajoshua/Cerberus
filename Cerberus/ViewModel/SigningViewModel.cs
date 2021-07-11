using Cerberus.Views;
using Cerberus.Services;
using Cerberus.DataAcces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Security;
using System.Runtime.InteropServices;

namespace Cerberus.ViewModel
{
    class SigningViewModel : NavigatableViewModel
    {

        public SigningViewModel(ref MainViewModel address)
        {
            Navigator = address;
            ErrorMessageText = "";
            Id = -1;
        }

        public void NavigateToPwdManager()
        {
            Navigator.NavigateTo<PwdManagerPage, PwdManagerViewModel>(Navigator,Id);
        }

        public void NavigateToSigningUp()
        {
            Navigator.NavigateTo<SigningUpPage, SigningUpViewModel>(Navigator);
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

        public int Id
        {
            get => GetProperty<int>();
            set => SetProperty(value);
        }

        public String Username
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public bool validation(SecureString tmpPassword)
        {
            bool valid = true;
            ErrorMessageText = "";
            if (String.IsNullOrEmpty(Username))
            {
                ErrorMessageText = "-Username field is empty, you must fill it to sign in\n";
                valid = false;
            }
            if(tmpPassword.Length==0)
            {
                ErrorMessageText += "-Password field is empty, you must fill it to sign in\n";
                valid = false;
            }
            if(valid)
            {
                OracleDataReader reader = Navigator.Connection.Query("select * from cerberus_users where username='"+Username+"'");

                if (reader.HasRows)
                    while (reader.Read())
                    {
                        Id = reader.GetInt32(0);
                        string username = reader.GetString(1);
                        string encryptedPassword = reader.GetString(2);
                        string password = Cypher.Decrypt(encryptedPassword);
                        SecureString tmppass = ToolBox.ToSecureString(password);
                        valid = ToolBox.IsEqualTo(tmpPassword, tmppass);
                        if (!valid)
                            ErrorMessageText = "-Username or Password invalid";
                    }
                else
                {
                    ErrorMessageText = "-Username invalid or non existent account";
                    valid = false;
                }
            }
            return valid;
        }

        //############ COMMANDS ##################

        public Commands.BaseCommand Command_Button_Sign_Up
        {
            get
            {
                return new Commands.BaseCommand(Button_Sign_Up);
            }
        }

        private void Button_Sign_Up()
        {
            NavigateToSigningUp();
        }

        public Commands.BaseCommand<Cerberus.Views.SigningPage> Command_Button_Sign_In
        {
            get
            {
                return new Commands.BaseCommand<Cerberus.Views.SigningPage>(Button_Sign_In);
            }
        }

        private void Button_Sign_In(Cerberus.Views.SigningPage sp)
        {
            bool val = validation(sp.PasswordBox.SecurePassword);
            if ( val && Id >-1 )
            {
                NavigateToPwdManager();
            }
        }

    }
}
