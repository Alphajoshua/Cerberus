using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cerberus.Services;
using Cerberus.Views;

namespace Cerberus.ViewModel
{
    class GeneratePwdViewModel : NavigatableViewModel
    {
        public GeneratePwdViewModel()
        {
            Length = 8;
        }

        public String Password
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public int Length
        {
            get => GetProperty<int>();
            set { SetProperty(value); LengthAsStr=value.ToString(); Button_Generate(); }
        }

        public String LengthAsStr
        {
            get => GetProperty<String>();
            set => SetProperty(value);
        }

        //######## COMMANDS #########

        public Commands.BaseCommand<GeneratePwdWindow> Command_Button_Close
        {
            get
            {
                return new Commands.BaseCommand<GeneratePwdWindow>(Button_Close);
            }
        }

        private void Button_Close(GeneratePwdWindow mvw)
        {
            mvw.Close();
        }
        
        public Commands.BaseCommand Command_Button_Copy_Pwd
        {
            get
            {
                return new Commands.BaseCommand(Button_Copy_Pwd);
            }
        }

        private void Button_Copy_Pwd()
        {
            Clipboard.SetText(Password);
        }

        public Commands.BaseCommand Command_Button_Generate
        {
            get
            {
                return new Commands.BaseCommand(Button_Generate);
            }
        }
        private char generateLetter(ref Random randomizer)
        {
            char result;
            
            if(randomizer.Next()%2==0)
            {
                char first = 'a';
                first += (char)(randomizer.Next() % 26);
                result = first;
            }
            else
            {
                char second = 'A';
                second += (char)(randomizer.Next() % 26);
                result = second;
            }

            return result;
        }
        private char generateNumber(ref Random randomizer)
        {
            char result;
            char start = '0';
            start += (char)(randomizer.Next() % 10);
            result = start;
            return result;
        }
        private char generateSpecial(ref Random randomizer)
        {
            char result;
            Dictionary<int, char> map = new Dictionary<int, char>();
            int mapIndex = 0;
            map.Add(mapIndex++,'!');
            map.Add(mapIndex++,'"');
            map.Add(mapIndex++,'#');
            map.Add(mapIndex++,'$');
            map.Add(mapIndex++,'%');
            map.Add(mapIndex++,'&');
            map.Add(mapIndex++,'(');
            map.Add(mapIndex++,')');
            map.Add(mapIndex++,'*');
            map.Add(mapIndex++,'+');
            map.Add(mapIndex++,',');
            map.Add(mapIndex++,'-');
            map.Add(mapIndex++,'.');
            map.Add(mapIndex++,'/');
            map.Add(mapIndex++,':');
            map.Add(mapIndex++,';');
            map.Add(mapIndex++,'<');
            map.Add(mapIndex++,'=');
            map.Add(mapIndex++,'>');
            map.Add(mapIndex++,'?');
            map.Add(mapIndex++,'@');
            map.Add(mapIndex++,'[');
            map.Add(mapIndex++,']');
            map.Add(mapIndex++,'^');
            map.Add(mapIndex++,'_');
            map.Add(mapIndex++,'`');
            map.Add(mapIndex++,'{');
            map.Add(mapIndex++,'}');
            map.Add(mapIndex++,'|');
            map.Add(mapIndex++,'~');
            map.Add(mapIndex++,'é');
            map.Add(mapIndex++,'è');
            map.Add(mapIndex++,'à');
            map.Add(mapIndex++,'ç');
            map.Add(mapIndex++,'£');
            map.Add(mapIndex++,'ù');
            int mapSize = map.Count;
            result = map[randomizer.Next() % mapSize];
            return result;
        }
        private void Button_Generate()
        {
            String result = String.Empty;
            Dictionary<int, bool> map = new Dictionary<int, bool>();
            Random randomizer = new Random();
            for(int index =0; index<Length;++index)
            {
                int rand = randomizer.Next() % 3;
                switch(rand)
                {
                    case 0:
                        result += generateLetter(ref randomizer);
                        break;
                    case 1:
                        result += generateNumber(ref randomizer);
                        break;
                    case 2:
                        result += generateSpecial(ref randomizer);
                        break;
                    default:
                        break;
                }
                
            }


            Password = result;
        }
    }
}
