using LogoGo3Data;
using LogoGo3Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LogoGo3Data.Utils;

namespace go3test
{
    class Program
    {
        static void Main(string[] args)
        {

         var LST= NQery.Find<LG_001_ITEM>();
            if (LST.Result)
            {
                foreach (var item in LST.Data)
                {
                    Console.WriteLine(item.NAME);
                    
                }
            }

            Console.ReadKey();
        }
    }
}
