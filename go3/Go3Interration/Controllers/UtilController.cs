using Go3Interration.Models;
using LogoGo3Data;
using LogoGo3Data.Context;
using LogoGo3Data.DefineModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web.Http;
using static LogoGo3Data.Extras.Utils;
using static LogoGo3Data.LogoGo3Data;
using static LogoGo3Data.RequestModels;

namespace Go3Interration.Controllers
{
    public class UtilController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("api/Util/getDefModels")]
        public MasterResult<List<LG_001_01_STFICHE>> getCariListAdo()
        {
            //  MasterResult<List<LogoGo3Data.Context.LG_001_CLCARD>> MDL= NQery.AdoFind<LogoGo3Data.Context.LG_001_CLCARD>(string.Format("LG_{0}_CLCARD", AppCommon.getConf().FirmaNo));

            //LogoGo3Data.Context.SqliteContext.getConnectionStringSqlite();
            return NQery.AdoFind<LG_001_01_STFICHE>("LG_002_02_STFICHE"," TRCODE=13");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Util/InsertIsk")]
        public MasterResult InsertIsk() {

            LogoGo3Data.LogoGo3Data.LG_001_02_STLINE_ISK ISK = new LogoGo3Data.LogoGo3Data.LG_001_02_STLINE_ISK();
            ISK.LINETYPE = 2;
            ISK.TRCODE = 8;
            ISK.DATE_ = DateTime.Parse("2020-07-22");
            ISK.IOCODE = 4;
            ISK.STFICHEREF = 30169;
            ISK.STFICHELNNO = 0;
            ISK.INVOICEREF = 41941;
            ISK.INVOICELNNO = 2;
            ISK.CLIENTREF = 10813;
            ISK.TOTAL = float.Parse("249,75");
            ISK.DISCPER = float.Parse("99,9");
            ISK.BILLED = 1;
            ISK.RECSTATUS = 1;
            ISK.YEAR_ = 2020;
            ISK.MONTH_ = 7;
            ISK.PARENTLNREF = 62331;
            ISK.AFFECTRISK = 1;
            ISK.DEDUCTIONPART1 = 2;
            ISK.DEDUCTIONPART2 = 3;

            var de=  NExec.AdoInsert<LG_001_02_STLINE_ISK>(ISK,"LG_001_02_STLINE");


            return new MasterResult();
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("api/Util/CreateDefModels")]
        public MasterResult<List<NTUPLE>> CreateDefModels()
        {
            string FATURABELGE = string.Format("LG_{0}_{1}_INVOICE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            string LINES = string.Format("LG_{0}_{1}_STLINE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            string IRSALIYEBELGE = string.Format("LG_{0}_{1}_STFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            string CARIHAREKET = string.Format("LG_{0}_{1}_CLFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            string CARIHAREKETLINE = string.Format("LG_{0}_{1}_CLFLINE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
       
            List<NTUPLE> ntpl = new List<NTUPLE>();


            ntpl.Add(SqliteContext.CreateTable<LG_001_01_INVOICE>());
            //ntpl.Add(SqliteContext.CreateTable<LG_001_CLCARD>());
            ntpl.Add(SqliteContext.CreateTable<LG_001_ITEM>());
            ntpl.Add(SqliteContext.CreateTable<LG_001_01_STFICHE>());
            ntpl.Add(SqliteContext.CreateTable<LG_001_01_STLINE>());
            ntpl.Add(SqliteContext.CreateTable<LG_001_01_CLFICHE>());
            ntpl.Add(SqliteContext.CreateTable<LG_001_01_CLFLINE>());


            //LG_001_01_STFICHE sarf_Fisi = NQery.AdoFind<LG_001_01_STFICHE>(IRSALIYEBELGE, string.Format(" TRCODE={0}  order by DATE_ desc", (int)EMalzemeFisTips.Sarf_Fisi), 1).Data.First();
            //ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_STFICHE>(sarf_Fisi, (int)EMalzemeFisTips.Sarf_Fisi));

            //LG_001_01_STLINE sarfkalem = NQery.AdoFind<LG_001_01_STLINE>(LINES, string.Format(" TRCODE={0} and LINETYPE=0  ", (int)EMalzemeFisTips.Sarf_Fisi), 1).Data.First();
            //ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_STLINE>(sarfkalem, (int)EMalzemeFisTips.Sarf_Fisi));

            //LG_001_01_STFICHE uretim_fisi = NQery.AdoFind<LG_001_01_STFICHE>(IRSALIYEBELGE, string.Format(" TRCODE={0}  order by DATE_ desc", (int)EMalzemeFisTips.UretimdenGiris_Fisi), 1).Data.First();
            //ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_STFICHE>(uretim_fisi, (int)EMalzemeFisTips.UretimdenGiris_Fisi));
            //LG_001_01_STLINE uretimkalem = NQery.AdoFind<LG_001_01_STLINE>(LINES, string.Format(" TRCODE={0} and LINETYPE=0  ", (int)EMalzemeFisTips.UretimdenGiris_Fisi), 1).Data.First();
            //ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_STLINE>(uretimkalem, (int)EMalzemeFisTips.UretimdenGiris_Fisi));

            //LG_001_01_STFICHE ambar = NQery.AdoFind<LG_001_01_STFICHE>(IRSALIYEBELGE, string.Format(" TRCODE={0}  order by DATE_ desc", (int)EMalzemeFisTips.Ambar_Fisi), 1).Data.First();
            //ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_STFICHE>(ambar, (int)EMalzemeFisTips.Ambar_Fisi));

            //LG_001_01_STLINE ambarkalem = NQery.AdoFind<LG_001_01_STLINE>(LINES, string.Format(" TRCODE={0} and LINETYPE=0  ", (int)EMalzemeFisTips.Ambar_Fisi), 1).Data.First();
            //ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_STLINE>(ambarkalem, (int)EMalzemeFisTips.Ambar_Fisi));


      


            LG_001_01_INVOICE toptansatisfaturasi = NQery.AdoFind<LG_001_01_INVOICE>(FATURABELGE, string.Format(" TRCODE={0}  order by DATE_ desc", (int)EFaturaTip.ToptanSatis_Faturasi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_INVOICE>(toptansatisfaturasi, (int)EFaturaTip.ToptanSatis_Faturasi));

      
            LG_001_01_INVOICE toptansatisIadefaturasi = NQery.AdoFind<LG_001_01_INVOICE>(FATURABELGE, string.Format(" TRCODE={0}  order by DATE_ desc", (int)EFaturaTip.Toptan_Satis_Iade_Faturasi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_INVOICE>(toptansatisIadefaturasi, (int)EFaturaTip.Toptan_Satis_Iade_Faturasi));


            LG_001_01_INVOICE verilenhizmetfaturasi = NQery.AdoFind<LG_001_01_INVOICE>(FATURABELGE, string.Format(" TRCODE={0}  order by DATE_ desc", (int)EFaturaTip.VerilenHizmet_Faturasi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_INVOICE>(verilenhizmetfaturasi, (int)EFaturaTip.VerilenHizmet_Faturasi));

            LG_001_01_INVOICE alimfaturasi = NQery.AdoFind<LG_001_01_INVOICE>(FATURABELGE, string.Format(" TRCODE={0}  order by DATE_ desc",(int)EFaturaTip.SatinAlma_faturasi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_INVOICE>(alimfaturasi, (int)EFaturaTip.SatinAlma_faturasi));

            LG_001_01_INVOICE alimiadefaturasi = NQery.AdoFind<LG_001_01_INVOICE>(FATURABELGE, string.Format(" TRCODE={0}  order by DATE_ desc", (int)EFaturaTip.SatinAlmaIade_Faturasi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_INVOICE>(alimiadefaturasi, (int)EFaturaTip.SatinAlmaIade_Faturasi));


            LG_001_01_INVOICE alinanhizmetfaturasi = NQery.AdoFind<LG_001_01_INVOICE>(FATURABELGE, string.Format(" TRCODE={0}  order by DATE_ desc", (int)EFaturaTip.AlinanHizmet_Faturasi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_INVOICE>(alinanhizmetfaturasi, (int)EFaturaTip.AlinanHizmet_Faturasi));


            LG_001_01_STFICHE toptanSatisIrsaliyesi = NQery.AdoFind<LG_001_01_STFICHE>(IRSALIYEBELGE, string.Format(" TRCODE={0}  order by DATE_ desc", (int)EIrsaliyeTip.toptan_Satis_Irsaliyesi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_STFICHE>(toptanSatisIrsaliyesi, (int)EIrsaliyeTip.toptan_Satis_Irsaliyesi));

            LG_001_01_STFICHE toptanSatisIadeIrsaliyesi = NQery.AdoFind<LG_001_01_STFICHE>(IRSALIYEBELGE, string.Format(" TRCODE={0}  order by DATE_ desc", (int)EIrsaliyeTip.toptan_Satis_Iade_Irsaliyesi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_STFICHE>(toptanSatisIadeIrsaliyesi, (int)EIrsaliyeTip.toptan_Satis_Iade_Irsaliyesi));

            LG_001_01_STFICHE alimIrsaliyesi = NQery.AdoFind<LG_001_01_STFICHE>(IRSALIYEBELGE, string.Format(" TRCODE={0}  order by DATE_ desc", (int)EIrsaliyeTip.alim_Irsaliyesi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_STFICHE>(alimIrsaliyesi, (int)EIrsaliyeTip.alim_Irsaliyesi));

            LG_001_01_STFICHE alimIadeIrsaliyesi = NQery.AdoFind<LG_001_01_STFICHE>(IRSALIYEBELGE, string.Format(" TRCODE={0}  order by DATE_ desc", (int)EIrsaliyeTip.alim_Iade_Irsaliyesi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_STFICHE>(alimIadeIrsaliyesi, (int)EIrsaliyeTip.alim_Iade_Irsaliyesi));

            LG_001_01_STLINE toptanSatisIrsaliyesikalemi = NQery.AdoFind<LG_001_01_STLINE>(LINES, string.Format(" TRCODE={0} and LINETYPE=0 ", (int)EIrsaliyeTip.toptan_Satis_Irsaliyesi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_STLINE>(toptanSatisIrsaliyesikalemi, (int)EIrsaliyeTip.toptan_Satis_Irsaliyesi));

            LG_001_01_STLINE toptanSatisIadeIrsaliyesikalem = NQery.AdoFind<LG_001_01_STLINE>(LINES, string.Format(" TRCODE={0} and LINETYPE=0  ", (int)EIrsaliyeTip.toptan_Satis_Iade_Irsaliyesi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_STLINE>(toptanSatisIadeIrsaliyesikalem, (int)EIrsaliyeTip.toptan_Satis_Iade_Irsaliyesi));


            LG_001_01_STLINE alimIrsaliyesikalem = NQery.AdoFind<LG_001_01_STLINE>(LINES, string.Format(" TRCODE={0} and LINETYPE=0  ", (int)EIrsaliyeTip.alim_Irsaliyesi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_STLINE>(alimIrsaliyesikalem, (int)EIrsaliyeTip.alim_Irsaliyesi));

            LG_001_01_STLINE alimIadeIrsaliyesikalem = NQery.AdoFind<LG_001_01_STLINE>(LINES, string.Format(" TRCODE={0} and LINETYPE=0  ", (int)EIrsaliyeTip.alim_Iade_Irsaliyesi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_STLINE>(alimIadeIrsaliyesikalem, (int)EIrsaliyeTip.alim_Iade_Irsaliyesi));


            LG_001_ITEM MALZEME = NQery.AdoFind<LG_001_ITEM>("LG_001_ITEMS", string.Format(" CARDTYPE={0} order by LOGICALREF desc", (int)EMalzemeTipleri.TicariMal), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_ITEM>(MALZEME,0));

            LG_001_01_CLFICHE NAKITTAHSILAT = NQery.AdoFind<LG_001_01_CLFICHE>(CARIHAREKET, string.Format(" TRCODE={0} order by LOGICALREF desc", (int)ECariHareket.Nakit_Tahsilat), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_CLFICHE>(NAKITTAHSILAT, 0));
            LG_001_01_CLFLINE NAKITTAHSILATLINE = NQery.AdoFind<LG_001_01_CLFLINE>(CARIHAREKETLINE, string.Format(" TRCODE={0} order by LOGICALREF desc", (int)ECariHareket.Nakit_Tahsilat), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_CLFLINE>(NAKITTAHSILATLINE, 0));


            LG_001_01_CLFICHE KREDIKARTITAHSILAT= NQery.AdoFind<LG_001_01_CLFICHE>(CARIHAREKET, string.Format(" TRCODE={0} order by LOGICALREF desc", (int)ECariHareket.Kredi_Karti_Fisi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_CLFICHE>(KREDIKARTITAHSILAT, 0));
            LG_001_01_CLFLINE KREDIKARTITAHSILATLINE = NQery.AdoFind<LG_001_01_CLFLINE>(CARIHAREKETLINE, string.Format(" TRCODE={0} order by LOGICALREF desc", (int)ECariHareket.Kredi_Karti_Fisi), 1).Data.First();
            ntpl.Add(SqliteContext.InsertModelSqlite<LG_001_01_CLFLINE>(KREDIKARTITAHSILATLINE, 0));


            return new MasterResult<List<NTUPLE>> { Data=ntpl, Elapsed=0, Message="", Result=true };
        }



        [AllowAnonymous]
        [HttpGet]
        [Route("api/Util/readlocations")]
        public string readlocations()
        {
            return LogoGo3Data.GenerateProcess.createModels();
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("api/Util/getlog")]
        public MasterResult<List<ReqLog>> getlog()
        {
            return SqliteContext.GetSqliteData<ReqLog>();
        }


    }
}
