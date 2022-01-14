using Go3Interration.Models;
using LogoGo3Data;
using LogoGo3Data.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using static LogoGo3Data.Extras.Utils;
using static LogoGo3Data.RequestModels;

namespace Go3Interration.Controllers
{
    public class DepoController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("api/Depo/getDepoList")]
        public MasterResult<List<L_CAPIWHOUSE>> getDepoList()
        {
            return NQery.Find<L_CAPIWHOUSE>(x=>x.FIRMNR==int.Parse(AppCommon.getConf().FirmaNo));
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("api/Depo/getPrintersd")]
        public bool getPrint()
        {
            string path = HttpContext.Current.Server.MapPath(@"~\prntemplate\sablon1.prn");
            string etk = File.ReadAllText(path);
            byte[] bytefile = File.ReadAllBytes(path);
            string stringfile = File.ReadAllText(path);
            PrinterHelper ph = new PrinterHelper(bytefile, "10.120.106.201");
            RawPrinterHelper.printzzpRaw(stringfile); 
            return true;
          
        }

    }
}
