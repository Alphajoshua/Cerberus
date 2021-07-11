using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using Cerberus.Services;

namespace Cerberus.Model
{
    public class pwdModel : Cerberus.ViewModel.BaseObservableViewModel
    {
        public enum source
        {
            Cerberus =0,
            Amazon =1,
            Chrome =2,
            Disney =3,
            Github =4,
            Gmail =5,
            Netflix =6,
            Spotify =7,
            Steam =8,
            Twitch =9
        }

        public pwdModel()
        {
            Id = -1;
            Title = "";
            AssociatedMail = "";
            Url = "";
            Username = "";
            Password = "";
            Creation = DateTime.Now;
            LastModif = Creation;
            Tags = new List<string>();
        }

        public pwdModel(int rawId, String rawAssociatedMail, String rawUsername, String rawPassword, String rawUrl, String rawTitle, DateTime rawCreation, DateTime rawLastModif)
        {
            Id = rawId;
            AssociatedMail = rawAssociatedMail;
            Username = rawUsername;
            Password = rawPassword;
            Url = rawUrl;
            Title = rawTitle;
            Creation = rawCreation;
            LastModif = rawLastModif;
            Tags = new List<string>();
        }

        public int Id
        {
            get => GetProperty<int>();
            set => SetProperty(value);
        }
        
        public String AssociatedMail
        {
            get => GetProperty<string>();
            set => SetProperty(ToolBox.CleanStringForQuery(value));
        }

        public String Username
        {
            get => GetProperty<string>();
            set => SetProperty(ToolBox.CleanStringForQuery(value));
        }

        public String Password
        {
            get => GetProperty<string>();
            set { SetProperty(ToolBox.CleanStringForQuery(value));if(Loaded)LastModif=DateTime.Now ; }
        }

        public String Url
        {
            get => GetProperty<string>();
            set { SetProperty(ToolBox.CleanStringForQuery(value)); setPath(); }
            }

        public String Title
        {
            get => GetProperty<string>();
            set => SetProperty(ToolBox.CleanStringForQuery(value));
        }

        public String TagsAsString
        {
            get
            {
                String result=String.Empty;
                for(int index = 0; index < Tags.Count; ++index)
                {
                    result += ToolBox.CleanStringForQuery(Tags[index]);
                    if (index != Tags.Count - 1)
                        result += ",";
                }
                return result;
            }
            set {
                String toProcess = value;
                Tags=toProcess.Split(',').ToList<String>();
            }
        }

        public List<String> Tags
        {
            get => GetProperty<List<String>>();
            set => SetProperty(value);
        }

        public DateTime Creation
        {
            get => GetProperty<DateTime>();
            set => SetProperty(value);
        }

        public bool Loaded
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
        }

        public DateTime LastModif
        {
            get => GetProperty<DateTime>();
            set { SetProperty(value);
                int result=-1;
                TimeSpan six_month = TimeSpan.FromDays(182);
                TimeSpan year = TimeSpan.FromDays(365);
                TimeSpan elapsedTime = DateTime.Now.Subtract(value);
                result = elapsedTime.CompareTo(six_month);
                if (result >= 0)
                {
                    result = elapsedTime.CompareTo(year);
                    if (result >= 0)
                        result = 1;
                    else
                        result = 0;
                }
                String resultAsStr = result.ToString();
                isSafe = resultAsStr;
            }
        }

        public String Path
        {
            get => GetProperty<String>();
            set => SetProperty(value);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// return an int
        /// -1 less than 6 month
        /// 0 between 6 months and a year
        /// 1 more than a year
        /// </returns>
        public String isSafe
        {
            get => GetProperty<String>();
            set => SetProperty(value);
        }

        public void setPath()
        {
            string result = String.Empty;
            switch(searchSource(Url))
            {
            case source.Amazon:
                    result+= "amazon";
                    break;
            case source.Cerberus:
                    result += "cerberus";
                    break;
                case source.Chrome:
                    result += "google";
                    break;
                case source.Disney:
                    result += "disney";
                    break;
                case source.Github:
                    result += "github";
                    break;
                case source.Gmail:
                    result += "gmail";
                    break;
                case source.Netflix:
                    result += "netflix";
                    break;
                case source.Spotify:
                    result += "spotify";
                    break;
                case source.Steam:
                    result += "steam";
                    break;
                case source.Twitch:
                    result += "twitch";
                    break;
                default:
                    result+= "cerberus";
                    break;
            }
            Path = result + "DrawingImage";
        }

        public source searchSource(String url)
        {
            if(url.Contains("amazon"))
            {
                return source.Amazon;
            }
            else if(url.Contains("disney"))
            {
                return source.Disney;
            }
            else if(url.Contains("github"))
            {
                return source.Github;
            }
            else if(url.Contains("gmail"))
            {
                return source.Gmail;
            }
            else if(url.Contains("netflix"))
            {
                return source.Netflix;
            }
            else if(url.Contains("spotify"))
            {
                return source.Spotify;
            }
            else if(url.Contains("steam"))
            {
                return source.Steam;
            }
            else if(url.Contains("twitch"))
            {
                return source.Twitch;
            }
            else if(url.Contains("http://") || url.Contains("https://") || url.Contains(".com") 
                || url.Contains(".fr") || url.Contains("chrome") || url.Contains("google"))
            {
                return source.Chrome;
            }
            return source.Cerberus;
        }
    }
}
