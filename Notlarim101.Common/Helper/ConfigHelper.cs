using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim101.Common.Helper
{
    public class ConfigHelper
    {
        //public static string Get(string key)
        //{
        //    //Configuration manager Web.Config dosyasi icinde appSettings icinde olusturdugumuz mail dosyalarinin keylerine ulasmak icin kullanacagiz.
        //    return ConfigurationManager.AppSettings[key];
        //}

        public static T Get<T>(string key)
        {
            //port numarasi gibi int bir geri donus istenirse bunun icin metodu generic hale getirerek gelen tipi istenen tipe degistirerek gondeririz.
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }
    }
}
