using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace LogoGo3Data.Extras
{
   public static class Utils
    {
     
        //public static string Cnn = @"Data Source=185.86.4.254,33690;Initial Catalog=GO3;Persist Security Info=True;User ID=sa;Password=2096Cema;";
      // public static string Cnn = @"Data Source=26.170.52.214,33669;Initial Catalog=GO3;Persist Security Info=True;User ID=go3ent;Password=2096Go3;";
        public static string Cnn = @"Data Source=192.168.1.185\SQLEXPRESS;Initial Catalog=GO3;Persist Security Info=True;User ID=go3ent;Password=2096Go3;";
        public static void CastModelProps<T,R>( ref T Xmodel,R Ymodel) {
                 var xModel = typeof(T);
                 var yModel = typeof(R);
            foreach (PropertyInfo i in typeof(T).GetType().GetProperties())
            {
              

                object a = xModel.GetType().GetProperty(i.Name).GetValue(yModel);
                xModel.GetType().GetProperty(i.Name).SetValue(yModel, Convert.ChangeType(a, i.PropertyType), null);

              

            }

          
        }

        public static string CreateHash(string fdata) {
          
            using (var sha = SHA256.Create())
            {
             return  Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(fdata)));
            }

        }

        public class MasterResult { 
            public bool Result { get; set; }
            public string Message { get; set; }        
            public long Elapsed { get; set; }
        }

        public class MasterResult<T>:MasterResult
        {
            
            public T Data { get; set; }
   
        }
    
        public class NTUPLE
        {
            public dynamic rec { get; set; }
            public int stat { get; set; }
        }

    }


 
   
}
