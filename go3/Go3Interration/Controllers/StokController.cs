using Go3Interration.Models;
using LogoGo3Data;
using LogoGo3Data.Context;
using LogoGo3Data.DefineModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static LogoGo3Data.Extras.Utils;
using static LogoGo3Data.RequestModels;

namespace Go3Interration.Controllers
{
    public class StokController : ApiController
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("api/Stok/getStokList")]
        public MasterResult<List<Malzeme_Model>> getStokList()
        {
            MasterResult < List < Malzeme_Model >> STK=NQery.AdoFind<Malzeme_Model>(string.Format("LG_{0}_ITEMS",AppCommon.getConf().FirmaNo)," not CARDTYPE=22");
            return STK;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Stok/searchStokList")]
        public MasterResult<List<Malzeme_Model>> searchStokList([FromBody] SearchModel P)
        {
            return NQery.AdoFind<Malzeme_Model>(string.Format("LG_{0}_ITEMS", AppCommon.getConf().FirmaNo),string.Format( "CODE like '%{0}%' or NAME like '%{0}%' and not CARDTYPE=22", P.likeKey));
           
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Stok/addNewMalzeme")]
        public MasterResult<NTUPLE> addNewMalzeme([FromBody] Malzeme_Model P)

        {
            string ITEMTABLENAME = string.Format("LG_{0}_ITEMS", AppCommon.getConf().FirmaNo);

            
            if (NQery.AdoFind<Malzeme_Model> (ITEMTABLENAME,string.Format("CODE='{0}' ", P.CODE)).Result)
                return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = "Malzeme Daha Önce Tanımlanmış", stat = 0 }, Elapsed = 0, Message = "Malzeme Daha Önce Tanımlanmış", Result = false };

            MasterResult<LG_001_ITEM> MCLS = LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_ITEM>(P,0);
            if (!MCLS.Result)
                return new MasterResult<NTUPLE> { Data = null, Result = false, Elapsed = 0, Message = "Referans Model Bulunamadı" };

            LG_001_ITEM ITEM = MCLS.Data; //LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_ITEM>(P, ITEMTABLENAME,0);
            ITEM.CAPIBLOCK_CREADEDDATE = DateTime.Now;
            ITEM.CAPIBLOCK_CREATEDBY = 1;
            ITEM.CAPIBLOCK_CREATEDHOUR =short.Parse(DateTime.Now.Hour.ToString());
            ITEM.CAPIBLOCK_CREATEDMIN = short.Parse(DateTime.Now.Minute.ToString());
            ITEM.CAPIBLOCK_CREATEDSEC = short.Parse(DateTime.Now.Second.ToString());
            ITEM.CAPIBLOCK_MODIFIEDBY = 1;
            ITEM.CAPIBLOCK_MODIFIEDDATE = DateTime.Now;
            ITEM.CAPIBLOCK_MODIFIEDHOUR = short.Parse(DateTime.Now.Hour.ToString());
            ITEM.CAPIBLOCK_MODIFIEDMIN = short.Parse(DateTime.Now.Minute.ToString());
            ITEM.CAPIBLOCK_MODIFIEDSEC = short.Parse(DateTime.Now.Second.ToString());
            return NExec.AdoInsert<LG_001_ITEM>(ITEM, ITEMTABLENAME);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Stok/updateNewMalzeme")]
        public MasterResult<NTUPLE> updateNewMalzeme([FromBody] Malzeme_Model P)

        {
            string ITEMTABLENAME = string.Format("LG_{0}_ITEMS", AppCommon.getConf().FirmaNo);
            LG_001_ITEM STF = NQery.AdoFind<LG_001_ITEM>(ITEMTABLENAME, string.Format("CODE='{0}'", P.CODE)).Data.First();
            LG_001_ITEM ITEM = LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_ITEM>(P,STF, ITEMTABLENAME);
            return NExec.AdoUpdate<LG_001_ITEM>(ITEM, ITEMTABLENAME," where LOGICALREF="+P.LOGICALREF);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Stok/getUnitSetList")]
        public MasterResult<List<LG_001_UNITSETF>> getUnitSetList()
        {
              return NQery.AdoFind<LG_001_UNITSETF>(string.Format("LG_{0}_UNITSETF", AppCommon.getConf().FirmaNo));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Stok/getUnitSetLineList")]
        public MasterResult<List<LG_001_UNITSETL>> getUnitSetLineList()
        {
            return NQery.AdoFind<LG_001_UNITSETL>(string.Format("LG_{0}_UNITSETL", AppCommon.getConf().FirmaNo));
        }


    }
}
