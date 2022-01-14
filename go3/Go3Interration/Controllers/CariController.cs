using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Go3Interration.Models;
using LogoGo3Data;
using LogoGo3Data.Context;
using LogoGo3Data.DefineModel;
using static LogoGo3Data.Extras.Utils;
using static LogoGo3Data.RequestModels;

namespace Go3Interration.Controllers
{
    public class CariController : ApiController
    {



        //[AllowAnonymous]
        //[HttpPost]
        //[Route("api/Cari/getCariList")]
        //public MasterResult<List<LG_001_CLCARD>> getCariList([FromBody] ConnectionModel P)
        //{
        //    return NQery.Find<LG_001_CLCARD>();
        //}

      


        [AllowAnonymous]
        [HttpPost]
        [Route("api/Cari/getCariList")]
        public MasterResult<List<Cari_Model>> getCariListAdo()
        {
          
            //LogoGo3Data.Context.SqliteContext.getConnectionStringSqlite();
            return NQery.AdoFind<Cari_Model>(string.Format("LG_{0}_CLCARD",AppCommon.getConf().FirmaNo));
        }


        //320.61.00001
        [AllowAnonymous]
        [HttpPost]
        [Route("api/Cari/searchCariList")]
        public MasterResult<List<Cari_Model>> searchCariList([FromBody] SearchModel P)
        {
           return NQery.AdoFind<Cari_Model>(string.Format("LG_{0}_CLCARD",AppCommon.getConf().FirmaNo),string.Format(" CODE like '%{0}%' or NAME like '%{0}%' or  DEFINITION_ like '%{0}%'  or  DEFINITION2 like '%{0}%'", P.likeKey));
  
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Cari/addNewCari")]
        public MasterResult<NTUPLE> addNewCari([FromBody] Cari_Model P)

        {
            P.GUID = Guid.NewGuid().ToString();
            string CTABLE = string.Format("LG_{0}_CLCARD", AppCommon.getConf().FirmaNo);
            if (NQery.AdoFind<Cari_Model>(CTABLE,string.Format(" CODE='{0}' or TAXNR='{1}'",P.CODE,P.TAXNR)).Result)
                 return new MasterResult<NTUPLE> { Data=new NTUPLE { rec="Cari Daha Önce Tanımlanmış", stat=0 }, Elapsed=0, Message="Cari Daha Önce Tanımlanmış", Result=false };

            MasterResult<LG_001_CLCARD> MCLS= LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_CLCARD>(P,0);
            if (!MCLS.Result)
                return new MasterResult<NTUPLE> { Data=null, Result=false, Elapsed=0, Message="Referans Model Bulunamadı" };

            LG_001_CLCARD CLCARD = MCLS.Data;
            return NExec.AdoInsert<LG_001_CLCARD>(CLCARD,CTABLE);
        
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("api/Cari/updateCari")]
        public MasterResult<NTUPLE> updateCari([FromBody] Cari_Model P)

        {
            string CTABLE = string.Format("LG_{0}_CLCARD", AppCommon.getConf().FirmaNo);
            LG_001_CLCARD Def = NQery.AdoFind<LG_001_CLCARD>(CTABLE, " LOGICALREF="+P.LOGICALREF).Data.First();
            LG_001_CLCARD CLCARD = LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_CLCARD>(P, CTABLE,0);
            return NExec.AdoUpdate<LG_001_CLCARD>(CLCARD, CTABLE, string.Format(" where LOGICALREF={0}", P.LOGICALREF));

        }
    }
}
