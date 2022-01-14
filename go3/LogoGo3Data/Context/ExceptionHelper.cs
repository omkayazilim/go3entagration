using LogoGo3Data.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogoGo3Data
{
   public class ExceptionHelper
    {
        public static void addRequestLog(string MSG) {
            StackTrace staktrace = new StackTrace();
            var MT = staktrace.GetFrame(1).GetMethod();
            SqliteContext.CreateTable<ReqLog>();

            ReqLog RG = new ReqLog { MetodName=MT.Name, ReqModel= MSG};


            SqliteContext.addReqLog(RG);

          

        }

      
    }

    public class ReqLog
    {
        public string Date { get; set; } = DateTime.Now.ToString();
        public string ReqModel { get; set; } = "";
        public string MetodName { get; set; } = "";
       
    }

    public class XReqLog:ReqLog {
    public  DateTime dateH { get; set; }
    }
}
