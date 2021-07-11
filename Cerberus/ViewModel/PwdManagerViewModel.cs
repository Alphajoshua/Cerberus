using Cerberus.Model;
using Cerberus.DataAcces;
using Cerberus.Views;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Collections.ObjectModel;
using Cerberus.Services;
using Microsoft.Win32;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Cerberus.ViewModel
{
    class PwdManagerViewModel : NavigatableViewModel
    {
        private ObservableCollection<String> _order = new ObservableCollection<String> { "Descendant", "Ascendant" };
        private String _selectedOrder = "Descendant";

        public PwdManagerViewModel( ref MainViewModel address, int id)
        {
            isMenuOpened = false;
            Navigator = address;
            ID = id;
            ListPwdModel = new ObservableCollection<pwdModel>();
            FullListPwdModel = new List<pwdModel>();
            loadPasswords();
            CurrentPwd = null;
            SelectedOrder = "Descendant";
            SearchedTag = String.Empty;
            SearchedTitle = String.Empty;
        }

        public ObservableCollection<String> Order
        {
            get { return _order; }
        }

        public String SelectedOrder
        {
            get { return _selectedOrder; }
            set { _selectedOrder = value; SortPwdObervableList(); }
        }

        public int ID
        {
            get => GetProperty<int>();
            set => SetProperty(value);
        }
        
        public pwdModel CurrentPwd
        {
            get => GetProperty<pwdModel>();
            set => SetProperty(value);
        }
        
        public String SearchedTag
        {
            get => GetProperty<String>();
            set => SetProperty(value);
        }
        
        public String SearchedTitle
        {
            get => GetProperty<String>();
            set => SetProperty(value);
        }

        public ObservableCollection<pwdModel> ListPwdModel
        {
            get => GetProperty<ObservableCollection<pwdModel>>();
            set => SetProperty(value);
        }

        public List<pwdModel> FullListPwdModel
        {
            get => GetProperty<List<pwdModel>>();
            set => SetProperty(value);
        }

        public bool isMenuOpened
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
        }

        public void NavigateToSignin()
        {
            Navigator.NavigateTo<SigningPage, SigningViewModel>(Navigator);
        }

        public void OpenWindowCreateNewPwd()
        {
            Navigator.ShowModalWindow<CreatePwdWindow, CreatePwdViewModel>();
        }

        public void OpenWindowGeneratePwd()
        {
            Navigator.ShowModalWindow<GeneratePwdWindow, GeneratePwdViewModel>();
        }

        public void loadPasswords()
        {
            OracleDataReader reader = Navigator.Connection.Query("select * from cerberus_passwords where user_id=" + ID);

            if (reader.HasRows)
                while (reader.Read())
                {
                    pwdModel model = new pwdModel(
                        reader.GetInt32(0),
                        reader.GetString(4),
                        reader.GetString(8),
                        Cypher.Decrypt(reader.GetString(2)),
                        reader.GetString(7),
                        reader.GetString(3),
                        ToolBox.ToDate(reader.GetOracleTimeStamp(5)),
                        ToolBox.ToDate(reader.GetOracleTimeStamp(6))
                    );
                    model.TagsAsString = loadTags(model);
                    model.Loaded = true;
                    FullListPwdModel.Add(model);
                }
            ListPwdModel = new ObservableCollection<pwdModel>(FullListPwdModel);
        }

        public String loadTags(pwdModel pwd)
        {
            OracleDataReader reader = Navigator.Connection.Query("select * from cerberus_tags where password_id=" + pwd.Id);

            String result=String.Empty;
            if (reader.HasRows)
                while (reader.Read())
                {
                    result += reader.GetString(2);
                }
            if (result.Length>0 && result[result.Length-1] == ',')
                result.Remove(result.Length - 1);
            return result;
        }

        public async Task addNewPwd ( pwdModel pwd)
        {
            string creation = pwd.Creation.ToString("yyyy-MM-dd HH:mm:ss");
            string lastModif = pwd.LastModif.ToString("yyyy-MM-dd HH:mm:ss");
            int result = await addNewTags(pwd);
            {
                string query = "INSERT INTO CERBERUS_PASSWORDS " +
                    "(USER_ID,PASSWORD,TITLE,MAIL,CREATION,LASTMODIF,URL,USERNAME)" +
                    " VALUES " +
                    "(" + ID + ",'" + Cypher.Encrypt(pwd.Password) + "','" + pwd.Title + "','" + pwd.AssociatedMail + "',TIMESTAMP '" + creation + "',TIMESTAMP '" + lastModif + "','" + pwd.Url + "','" + pwd.Username + "')";
                result = Navigator.Connection.NonQuery(query);

                if (result == -1)
                    MessageBox.Show("Error adding password to DataBase\nPlease try again or check your internet connection or else contact our support service", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    ListPwdModel.Add(pwd);
                    FullListPwdModel.Add(pwd);
                }
            }
        }

        public async Task<int> addNewTags(pwdModel pwd)
        {
            int result = 1;
            if (pwd.TagsAsString.Length > 0)
            {
                for (int index = 0; index < pwd.Tags.Count && result != -1; ++index)
                {
                    string query = "INSERT INTO CERBERUS_TAGS (PASSWORD_ID,TAG) VALUES (" + pwd.Id + ",'" + pwd.Tags[index] + "')";
                    result = Navigator.Connection.NonQuery(query);
                }
                if (result == -1)
                    MessageBox.Show("Error adding tags to DataBase\nPlease try again or check your internet connection or else contact our support service", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return result;
        }

        public async Task updatePwd (pwdModel pwd)
        {
            string lastModif = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int result = await updateTags(pwd);
            if (result != -1)
            {
                string query = "UPDATE CERBERUS_PASSWORDS SET" +
                    " USER_ID=" + ID + ",PASSWORD='" + Cypher.Encrypt(pwd.Password) + "',TITLE='" + pwd.Title + "',MAIL='" + pwd.AssociatedMail + "',LASTMODIF=TIMESTAMP '" + lastModif + "',URL='" + pwd.Url + "',USERNAME='" + pwd.Username + "'" +
                    " WHERE ID=" + pwd.Id;
                result = Navigator.Connection.NonQuery(query);

                if (result == -1)
                    MessageBox.Show("Error moddifying password to DataBase\nPlease try again or check your internet connection or else contact our support service", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private async Task<int> updateTags(pwdModel pwd)
        {
            int result = deleteTags(pwd);
            if (result != -1)
                result = await addNewTags(pwd);
            return result;
        }
        
        private int deleteTags(pwdModel pwd)
        {
            int result = 1;
            string tags = loadTags(pwd);
            if (tags.Split(',').Length > 0)
            {
                string query = "DELETE CERBERUS_TAGS WHERE" + " PASSWORD_ID=" + pwd.Id;
                result = Navigator.Connection.NonQuery(query);
                if (result == -1)
                {
                    MessageBox.Show("Error deleting tags to DataBase\nPlease try again or check your internet connection or else contact our support service", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return result;
        }

        public int deletePwd (pwdModel pwd)
        {
            int result = deleteTags(pwd);
            if(result!=-1)
            {
                string query = "DELETE CERBERUS_PASSWORDS WHERE" + " USER_ID=" + ID + " AND ID=" + pwd.Id;
                result = Navigator.Connection.NonQuery(query);
                if(result==-1)
                    MessageBox.Show("Error deleting password to DataBase\nPlease try again or check your internet connection or else contact our support service", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return result;
        }

        private void SortPwdObervableList()
        {
            List<pwdModel> result = ListPwdModel.ToList<pwdModel>();

            if (SelectedOrder == "Ascendant")
                result.Sort(CompareAscending);
            else
                result.Sort(CompareDescending);

            ListPwdModel = new ObservableCollection<pwdModel>(result);
        }

        public int CompareDescending(pwdModel x, pwdModel y)
        { 
            return x.Title.CompareTo(y.Title);
        }

        public int CompareAscending(pwdModel x, pwdModel y)
        {
            return x.Title.CompareTo(y.Title)*(-1);
        }

        private String getAllPwdAsString()
        {
            String result = String.Empty;

            result += "ID;Title;Username;Password;Mail;Url;Creation;Last modification;Tags\n";

            for(int index=0;index<FullListPwdModel.Count;++index)
            {
                result += FullListPwdModel[index].Id + ";";
                result += FullListPwdModel[index].Title + ";";
                result += FullListPwdModel[index].Username + ";";
                result += FullListPwdModel[index].Password + ";";
                result += FullListPwdModel[index].AssociatedMail + ";";
                result += FullListPwdModel[index].Url + ";";
                result += FullListPwdModel[index].Creation.ToString() + ";";
                result += FullListPwdModel[index].LastModif.ToString() + ";";
                result += FullListPwdModel[index].TagsAsString + "\n";
            }

            return result;
        }

        private System.Data.DataTable getAllPwdAsTable()
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("ID", typeof(String));
            table.Columns.Add("Title", typeof(String));
            table.Columns.Add("Username", typeof(String));
            table.Columns.Add("Password", typeof(String));
            table.Columns.Add("Mail", typeof(String));
            table.Columns.Add("Url", typeof(String));
            table.Columns.Add("Creation", typeof(String));
            table.Columns.Add("Last modification", typeof(String));
            table.Columns.Add("Tags", typeof(String));

            for (int index = 0; index < FullListPwdModel.Count; ++index)
            {
                table.Rows.Add( FullListPwdModel[index].Id.ToString(),
                                FullListPwdModel[index].Title,
                                FullListPwdModel[index].Username,
                                FullListPwdModel[index].Password,
                                FullListPwdModel[index].AssociatedMail,
                                FullListPwdModel[index].Url,
                                FullListPwdModel[index].Creation.ToString(),
                                FullListPwdModel[index].LastModif.ToString(),
                                FullListPwdModel[index].TagsAsString);
            }

            return table;
        }

        //######## COMMANDS #########

        public Commands.BaseCommand Command_Button_LogOut
        {
            get
            {
                return new Commands.BaseCommand(Button_LogOut);
            }
        }

        private void Button_LogOut()
        {
            NavigateToSignin();
        }

        public Commands.BaseCommand Command_Button_Generate
        {
            get
            {
                return new Commands.BaseCommand(OpenWindowGeneratePwd);
            }
        }

        public Commands.BaseCommand Command_Button_Add
        {
            get
            {
                return new Commands.BaseCommand(OpenWindowCreateNewPwd);
            }
        }

        public Commands.BaseCommand Command_Button_Export
        {
            get
            {
                return new Commands.BaseCommand(Button_Export_Pwd);
            }
        }

        private void Button_Export_Pwd()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = "xls";
            saveFileDialog.Filter = "Excel files (*.xls)|*.xls|Coma Separated Values files (*.csv)|*.csv";
            saveFileDialog.CreatePrompt = false;
            saveFileDialog.OverwritePrompt = false;

            if (saveFileDialog.ShowDialog() == true)
            {
                if (saveFileDialog.FileName.Contains(".csv"))
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, getAllPwdAsString());

                        MessageBox.Show("Export done");
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                else
                {
                    try
                    {
                        Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();

                        if (excelApp == null)
                        {
                            MessageBox.Show("Excel is not properly installed!!");
                        }
                        Workbook xlWorkBook = excelApp.Workbooks.Add();
                        Worksheet xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                        System.Data.DataTable table = getAllPwdAsTable();
                        int columnCount = 1;
                        xlWorkSheet.Cells[1, columnCount++] = "ID";
                        xlWorkSheet.Cells[1, columnCount++] = "Title";
                        xlWorkSheet.Cells[1, columnCount++] = "Username";
                        xlWorkSheet.Cells[1, columnCount++] = "Password";
                        xlWorkSheet.Cells[1, columnCount++] = "Mail";
                        xlWorkSheet.Cells[1, columnCount++] = "Url";
                        xlWorkSheet.Cells[1, columnCount++] = "Creation";
                        xlWorkSheet.Cells[1, columnCount++] = "Last modification";
                        xlWorkSheet.Cells[1, columnCount++] = "Tags";
                        Microsoft.Office.Interop.Excel.Range rng = xlWorkSheet.Cells[1, columnCount] as Range;
                        rng.EntireRow.Font.Bold = true;

                        for (int indexRow = 0; indexRow < table.Rows.Count; ++indexRow)
                        {
                            for (int indexColumn = 0; indexColumn < table.Columns.Count; ++indexColumn)
                            {
                                xlWorkSheet.Cells[indexRow+2,indexColumn+1] = table.Rows[indexRow].ItemArray[indexColumn].ToString();
                            }
                        }
                        xlWorkSheet.Columns.AutoFit();
                        xlWorkBook.SaveAs(saveFileDialog.FileName);

                        xlWorkBook.Close();
                        excelApp.Quit();

                        Marshal.ReleaseComObject(xlWorkBook);
                        Marshal.ReleaseComObject(excelApp);
                        MessageBox.Show("Export done");

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        public Commands.BaseCommand<pwdModel> Command_Button_Copy_Username
        {
            get
            {
                return new Commands.BaseCommand<pwdModel>(Button_Copy_Username);
            }
        }

        private void Button_Copy_Username(pwdModel pwd)
        {
            Clipboard.SetText(pwd.Username);
        }

        public Commands.BaseCommand<pwdModel> Command_Button_Copy_Pwd
        {
            get
            {
                return new Commands.BaseCommand<pwdModel>(Button_Copy_Pwd);
            }
        }

        private void Button_Copy_Pwd(pwdModel pwd)
        {
            Clipboard.SetText(pwd.Password);
        }

        public Commands.BaseCommand<pwdModel> Command_Button_Delete
        {
            get
            {
                return new Commands.BaseCommand<pwdModel>(Button_Delete);
            }
        }

        private void Button_Delete(pwdModel pwd)
        {
            int result = deletePwd(pwd);
            if (result != -1)
            {
                ListPwdModel.Remove(pwd);
                FullListPwdModel.Remove(pwd);
            }
        }


        public Commands.BaseCommand<pwdModel> Command_Button_Menu_Data_Invert
        {
            get
            {
                return new Commands.BaseCommand<pwdModel>(Button_Menu_Data_Invert);
            }
        }

        private void Button_Menu_Data_Invert(pwdModel pwd)
        {
           isMenuOpened = !isMenuOpened;
            if(isMenuOpened)
            {
                CurrentPwd = pwd;
            }
            else
            {
                /*Current pwd to update*/
                updatePwd(CurrentPwd);
                CurrentPwd = null;
            }
        }

        public Commands.BaseCommand Command_Button_Search_By_Tag
        {
            get
            {
                return new Commands.BaseCommand(Button_Search_By_Tag);
            }
        }

        private void Button_Search_By_Tag()
        {
            List<pwdModel> result = new List<pwdModel>();
            if(String.IsNullOrEmpty(SearchedTag))
            {
                ListPwdModel = new ObservableCollection<pwdModel>(FullListPwdModel);
            }
            else
            {
                ListPwdModel = new ObservableCollection<pwdModel>();
                for (int index = (FullListPwdModel.Count - 1); index >= 0; --index)
                {
                    bool hasTag = false;
                    String[] TabTag = SearchedTag.Split(',');
                    for (int indexTag =0; !hasTag && indexTag< TabTag.Length; ++indexTag)
                    {
                        if (FullListPwdModel[index].Tags.Contains(TabTag[indexTag]))
                        {
                            ListPwdModel.Add(FullListPwdModel[index]);
                            hasTag = true;
                        }
                    }
                    
                }
            }
            SortPwdObervableList();
        }

        public Commands.BaseCommand Command_Button_Search_By_Title
        {
            get
            {
                return new Commands.BaseCommand(Button_Search_By_Title);
            }
        }

        private void Button_Search_By_Title()
        {
            List<pwdModel> result = new List<pwdModel>();
            if (String.IsNullOrEmpty(SearchedTitle))
            {
                ListPwdModel = new ObservableCollection<pwdModel>(FullListPwdModel);
            }
            else
            {
                ListPwdModel = new ObservableCollection<pwdModel>();
                for (int index = (FullListPwdModel.Count-1); index >= 0; --index)
                {
                    if (FullListPwdModel[index].Title.Contains(SearchedTitle))
                        ListPwdModel.Add(FullListPwdModel[index]);
                }
            }
            SortPwdObervableList();
        }
    }

}
