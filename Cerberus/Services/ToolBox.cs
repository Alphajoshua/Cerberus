using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Oracle.ManagedDataAccess.Types;

namespace Cerberus.Services
{
    static class ToolBox
    {
        static Regex allowedChars = new Regex("[*\\?'\"]");

        public static DateTime ToDate(OracleTimeStamp ts)
        {
            DateTime result = new DateTime(ts.Year,ts.Month,ts.Day,ts.Hour,ts.Minute,ts.Second);
            return result;
        }

        public static OracleTimeStamp ToTimeStamp(DateTime dt)
        {
            OracleTimeStamp result = new OracleTimeStamp(dt);
            return result;
        }

        public static SecureString ToSecureString(string rawPwd)
        {
            SecureString result = new System.Net.NetworkCredential("", rawPwd).SecurePassword;
            return result;
        }

        public static String ToString(SecureString rawPwd)
        {
            String result = new System.Net.NetworkCredential("", rawPwd).Password;
            return result;
        }

        public static bool IsEqualTo(this SecureString ss1, SecureString ss2)
        {
            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;
            try
            {
                bstr1 = Marshal.SecureStringToBSTR(ss1);
                bstr2 = Marshal.SecureStringToBSTR(ss2);
                int length1 = Marshal.ReadInt32(bstr1, -4);
                int length2 = Marshal.ReadInt32(bstr2, -4);
                if (length1 == length2)
                {
                    for (int x = 0; x < length1; ++x)
                    {
                        byte b1 = Marshal.ReadByte(bstr1, x);
                        byte b2 = Marshal.ReadByte(bstr2, x);
                        if (b1 != b2) return false;
                    }
                }
                else return false;
                return true;
            }
            finally
            {
                if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
            }
        }

        public static String CleanStringForQuery (String rawString)
        {
            String result=String.Empty;

            result=allowedChars.Replace(rawString, String.Empty);

            return result;
        }
    }
}
