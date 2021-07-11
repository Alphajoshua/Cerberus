using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Model
{
    class userModel
    {
        private int id;
        private String mailAddress;
        private String identifier;
        private String masterPwd;
        private List<pwdModel> pwdList = new List<pwdModel>();

        public userModel()
        {
            Id = -1;
            MailAddress = "";
            Identifier = "";
            MasterPwd = "";
        }

        public userModel(int rawId, String rawMailAddress, String rawIdentifier, String rawMasterPwd)
        {
            Id = rawId;
            MailAddress = rawMailAddress;
            Identifier = rawIdentifier;
            MasterPwd = rawMasterPwd;
        }

        public userModel(int rawId, String rawMailAddress, String rawIdentifier, String rawMasterPwd, List<pwdModel> rawList)
        {
            Id = rawId;
            MailAddress = rawMailAddress;
            Identifier = rawIdentifier;
            MasterPwd = rawMasterPwd;
            PwdList = rawList;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public String MailAddress
        {
            get { return mailAddress; }
            set { mailAddress = value; }
        }

        public String Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        public String MasterPwd
        {
            get { return masterPwd; }
            set { masterPwd = value; }
        }

        public List<pwdModel> PwdList
        {
            get { return pwdList; }
            set { pwdList = value; }
        }
    }
}
