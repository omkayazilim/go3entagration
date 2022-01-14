using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LogoGo3Data.Model
{
     public class DataModels
     {
        public class Conf
        {
            public string FirmaNo { get; set; }
            public string DonemNo { get; set; }
      
        }
    }


    public static class Extensions {

        public static short toShortparse<T>(this T val) {

            short s = 0;
            short.TryParse(val.ToString(),out s);
            return s;
        
        }

        public static string toJsonSerialize<T>(this T val) {
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = int.MaxValue;
            return js.Serialize(val);
        
        }

        public static T toJsonDeSerialize<T>(this string val)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = int.MaxValue;
            return js.Deserialize<T>(val);

        }

        public static T nullableToRequired<T>(this object val) {


            return (T)Convert.ChangeType(val, typeof(T));
           
        }

    }
    
}
