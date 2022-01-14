using LogoGo3Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LogoGo3Data.Extras.Utils;
using static LogoGo3Data.RequestModels;

namespace LogoGo3Data
{
   public class GenerateProcess
    {

        public static string getPath(string cp) {

            return $"{System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)}\\{cp}".Replace("file:\\","");;
        }
        public static string ReadSqlScript() {

            return File.ReadAllText(getPath("Config\\design.sql"));

        }

        public static List<modelconf> getModelList() {

           string Js= File.ReadAllText(getPath("Config\\modelconf.json"));
            var LST=Js.deserializeJson<List<modelconf>>();
            foreach (var item in LST)
            {
                switch (item.Type) {
                    case Modeltype.Firma: {
                            item.ModelName = $"LG_{getAppConf().FirmaNo}_{item.ModelName}";
                            break; }
                    case Modeltype.Firma_Donem: {
                            item.ModelName = $"LG_{getAppConf().FirmaNo}_{getAppConf().DonemNo}_{item.ModelName}";
                            break; 
                        }
                }
            }

            return LST;
        }

        public static DataModels.Conf getAppConf() {

            string Js = File.ReadAllText(getPath("Config\\appconfig.json"));

            return Js.deserializeJson<DataModels.Conf>();

        }

        public static string createModels()  {

            StringBuilder sbb = new StringBuilder();
            var ModelNameList = getModelList();
            foreach (var item in ModelNameList)
            {
                string sql = ReadSqlScript().Replace("{0}",item.ModelName);
                var res = NQery.RawQuery<createmodeldata>(sql);
                sbb.Append(res.Result?res.Data[0].Model:"");
                sbb.Append("\n");
          
            
            

          

           
            }

            string model = sbb.ToString();

            File.WriteAllText(getPath("Config\\design.mdl"), sbb.ToString());

        



                return File.ReadAllText(getPath("Config\\design.mdl")); 

            
        }

        public static string readSvnConf() {
            return File.ReadAllText(getPath("Config\\SvnConfig.json"));
        }


     
    }
}
