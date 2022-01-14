using Go3Interration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Go3Interration.Controllers
{
    public class MalzemeController : ApiController
    {
      
        string STLINES;
        string FISDETAY;
        public MalzemeController()
        {
           STLINES = string.Format("LG_{0}_{1}_STLINE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
           FISDETAY = string.Format("LG_{0}_{1}_STLFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
        }



    }
}
