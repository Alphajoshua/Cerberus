using System;
using System.Windows;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Cerberus.DataAcces
{
    public class DbConnection
    {
        String oradb = /*ASK ME FOR CONNECTION STRING*/

        OracleConnection connexion;
        OracleCommand cmd;

        public DbConnection()
        {
            connectToDb();
        }

        void connectToDb()
        {
            connexion = new OracleConnection(oradb);
            try
            {
                connexion.Open();
                cmd = new OracleCommand();
                cmd.Connection = connexion;
            }
            catch
            {
                MessageBox.Show("Error connecting to DataBase\nPlease check your internet connection or else contact our support service", "Error",MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        public OracleDataReader Query ( String sqlQuery)
        {
            //cleanQuery();
            cmd.CommandText = sqlQuery;
            return cmd.ExecuteReader();
        }

        public int NonQuery(String sqlNonQuery)
        {
            cmd.CommandText = sqlNonQuery;
            return cmd.ExecuteNonQuery(); 
        }

        void cleanQuery()
        {
            if (cmd != null)
                cmd.Dispose();
        }

        public void StopConnection()
        {
            connexion.Dispose();
        }
    }
}
