using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogoGo3Data.DefineModel
{
    public class Log_Model
    {
     public int id { get; set; }
	public int status { get; set; }
	public string date	{ get; set; }
        public string desc { get; set; }
        public string procname { get; set; }
        public string details{ get; set; }
        public string location { get; set; }
    }

    public class LogModel_ref<T> {
       public int stat { get; set; }
       public T Ref { get; set; }
    }
}
