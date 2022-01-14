using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogoGo3Data
{
  public  class RequestModels
    {

        public class ConnectionModel
        {
            public string reqkey { get; set; }
            public string firmaNo { get; set; }
            public string donemNo { get; set; }
        }

        public class SearchModel {
       
            public string likeKey { get; set; }
        }

       public enum Modeltype { 
          Firma=1,
          Firma_Donem=3
        }


        public class modelconf { 
          public string ModelName { get; set; }
            public Modeltype Type { get; set; }
        }

        public class createmodeldata {
          public string Model { get; set; }
        }

       
    }
}
