
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

namespace Go3Interration.Controllers
{
    public class FauraController : ApiController
    {
        string FATURABELGE;
        string FATURALINES;
        string IRSALIYEBELGE;
        public FauraController()
        {

            FATURABELGE = string.Format("LG_{0}_{1}_INVOICE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            FATURALINES = string.Format("LG_{0}_{1}_STLINE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            IRSALIYEBELGE = string.Format("LG_{0}_{1}_STLFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
        }



        [AllowAnonymous]
        [HttpGet]
        [Route("api/Fatura/getFaturaList")]
        public LG_001_01_STLINE getFaturaList()
        {



            List<Belge_Model> bm = new List<Belge_Model>();
            LG_001_01_STLINE g= NQery.AdoFind<LG_001_01_STLINE>(FATURALINES, " TRCODE=8 and  LOGICALREF=16" ).Data.First();

            //foreach (var item in blg)
            //{

            //    List<BelgeLine> ln = NQery.AdoFind<BelgeLine>(FATURALINES, string.Format(" INVOICEREF={0}", item.LOGICALREF)).Data;
            //    bm.Add(new Belge_Model { belge = item, belgeLines = ln });

            //}



            return g;
        }


    


        #region Alım Faturaları
        [AllowAnonymous]
        [HttpPost]
        [Route("api/Fatura/addNewAlimFaturasi")]
        public MasterResult<List<NTUPLE>> addNewAlimFaturasi([FromBody] Alim_Fatura P)

        {


            List<NTUPLE> tuples = new List<NTUPLE>();

            #region checkperiods
            if (P == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Boş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "Fatura Boş İşlem Yapılamaz", Result = false };
            if (P.belge == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Boş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "Fatura Boş İşlem Yapılamaz", Result = false };
            //if (P.belge.TRCODE == 0)
            //    return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Tipi Belirtilmemiş", stat = 0 } }, Elapsed = 0, Message = "Fatura Tipi Belirtilmemiş", Result = false };


            if (P.belgeLines == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Kalemleri Girilmemiş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "Fatura Kalemleri Girilmemiş İşlem Yapılamaz", Result = false };

            if (P.belgeLines.Count == 0)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Kalemleri Girilmemiş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "Fatura Kalemleri Girilmemiş İşlem Yapılamaz", Result = false };
            #endregion

          
            P.belge.DATE_ = P.belge.DATE_;
            L_CAPIWHOUSE WRH = NQery.AdoFind<L_CAPIWHOUSE>(string.Format("L_CAPIWHOUSE", " FIRMNR={0} and NR={1}", int.Parse(AppCommon.getConf().FirmaNo), P.belge.SOURCEINDEX)).Data.First();
            P.belge.SOURCECOSTGRP = int.Parse(WRH.COSTGRP.ToString());


            if (NQery.AdoFind<LG_001_01_INVOICE>(FATURABELGE, string.Format("FICHENO='{0}' ", P.belge.FICHENO)).Result)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Daha Önce Tanımlanmış", stat = 0 } }, Elapsed = 0, Message = "Fatura Daha Önce Tanımlanmış", Result = false };


            MasterResult<LG_001_01_INVOICE> MCLS = LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_01_INVOICE>(P, (int)EIrsaliyeTip.alim_Irsaliyesi);
            if (!MCLS.Result)
                 return new MasterResult<List<NTUPLE>> { Data = null, Result = false, Elapsed = 0, Message = "Referans Model Bulunamadı" };

            LG_001_01_INVOICE FATBELGE = MCLS.Data; //LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_01_INVOICE>(P.belge, FATURABELGE,1); //new LG_001_01_INVOICE();

            FATBELGE.CAPIBLOCK_CREADEDDATE = DateTime.Now;
            FATBELGE.CAPIBLOCK_MODIFIEDDATE =null;
            FATBELGE.CAPIBLOCK_CREATEDHOUR =short.Parse( DateTime.Now.Hour.ToString());
            FATBELGE.CAPIBLOCK_CREATEDMIN = short.Parse(DateTime.Now.Minute.ToString());
            FATBELGE.CAPIBLOCK_CREATEDSEC = short.Parse(DateTime.Now.Second.ToString());
            FATBELGE.GUID = Guid.NewGuid().ToString();
            FATBELGE.TRCODE = (int)EIrsaliyeTip.alim_Irsaliyesi;
            FATBELGE.GRPCODE = (int)EBelgeGurupKodlari.Alim_Faturalari;
            
            MasterResult<NTUPLE> FP = NExec.AdoInsert<LG_001_01_INVOICE>(FATBELGE, FATURABELGE);


            Belge Blgfat = NQery.AdoFind<Belge>(FATURABELGE, string.Format("FICHENO='{0}' ", P.belge.FICHENO)).Data.First();
            Belge_Model BM = new Belge_Model { belge=Blgfat, belgeLines=P.belgeLines };
            BM.belge.INVNO = Blgfat.FICHENO;
            BM.belge.FICHENO = P.belge.IRSFICHENO;
            BM.belge.GUID = Guid.NewGuid().ToString();
            BM.belge.INVOICEREF = Blgfat.LOGICALREF;
            BM.belge.BILLED = 1;
            P.belgeLines.ForEach(x => { x.INVOICELNNO ++; x.INVOICEREF = Blgfat.LOGICALREF; ;x.IOCODE = (int)EIoKod.Giris; x.STFICHELNNO++; });

            ///////////////////////////////irsaliye //////////////////////////
            IrsaliyeController ICL = new IrsaliyeController();
             MasterResult<List<NTUPLE>> res = ICL.addNewAlimIrsaliye(BM);
            foreach (NTUPLE item in res.Data)
            {
                tuples.Add(item);

           }
            bool stat = tuples.Where(x => x.stat == 0).Count() > 0 ? false : true;
            if (!stat)
            {
                NExec.AdoDelete<LG_001_01_INVOICE>(FATURABELGE, string.Format(" where LOGICALREF={0}", Blgfat.LOGICALREF));
                NExec.AdoDelete<LG_001_01_STFICHE>(IRSALIYEBELGE, string.Format(" where INVOICEREF={0}", Blgfat.LOGICALREF));
                NExec.AdoDelete<LG_001_01_STLINE>(FATURALINES, string.Format(" where INVOICEREF={0}", Blgfat.LOGICALREF));
            }



            return new MasterResult<List<NTUPLE>> { Data = tuples, Elapsed = 0, Message = stat ? "Fatura Kaydı Oluşturuldu" : "Bazı Hatalar oluştu işlem Geri Alındı Hata detayı Mesajın Ekindedir.", Result = stat };
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Fatura/addNewAlimIedeFaturasi")]
        public MasterResult<List<NTUPLE>> addNewAlimIedeFaturasi([FromBody] Alim_Fatura P)

        {


            List<NTUPLE> tuples = new List<NTUPLE>();

            #region checkperiods
            if (P == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Boş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "Fatura Boş İşlem Yapılamaz", Result = false };
            if (P.belge == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Boş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "Fatura Boş İşlem Yapılamaz", Result = false };
            //if (P.belge.TRCODE == 0)
            //    return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Tipi Belirtilmemiş", stat = 0 } }, Elapsed = 0, Message = "Fatura Tipi Belirtilmemiş", Result = false };


            if (P.belgeLines == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Kalemleri Girilmemiş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "Fatura Kalemleri Girilmemiş İşlem Yapılamaz", Result = false };

            if (P.belgeLines.Count == 0)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Kalemleri Girilmemiş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "Fatura Kalemleri Girilmemiş İşlem Yapılamaz", Result = false };
            #endregion


            P.belge.DATE_ = P.belge.DATE_;
            L_CAPIWHOUSE WRH = NQery.AdoFind<L_CAPIWHOUSE>(string.Format("L_CAPIWHOUSE", " FIRMNR={0} and NR={1}", int.Parse(AppCommon.getConf().FirmaNo), P.belge.SOURCEINDEX)).Data.First();
            P.belge.SOURCECOSTGRP = int.Parse(WRH.COSTGRP.ToString());


            if (NQery.AdoFind<LG_001_01_INVOICE>(FATURABELGE, string.Format("FICHENO='{0}' ", P.belge.FICHENO)).Result)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Daha Önce Tanımlanmış", stat = 0 } }, Elapsed = 0, Message = "Fatura Daha Önce Tanımlanmış", Result = false };


            MasterResult<LG_001_01_INVOICE> MCLS = LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_01_INVOICE>(P, (int)EIrsaliyeTip.alim_Iade_Irsaliyesi);
            if (!MCLS.Result)
                return new MasterResult<List<NTUPLE>> { Data = null, Result = false, Elapsed = 0, Message = "Referans Model Bulunamadı" };

            LG_001_01_INVOICE FATBELGE = MCLS.Data; //LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_01_INVOICE>(P.belge, FATURABELGE,1); //new LG_001_01_INVOICE();

            FATBELGE.CAPIBLOCK_CREADEDDATE = DateTime.Now;
            FATBELGE.CAPIBLOCK_MODIFIEDDATE = null;
            FATBELGE.CAPIBLOCK_CREATEDHOUR = short.Parse(DateTime.Now.Hour.ToString());
            FATBELGE.CAPIBLOCK_CREATEDMIN = short.Parse(DateTime.Now.Minute.ToString());
            FATBELGE.CAPIBLOCK_CREATEDSEC = short.Parse(DateTime.Now.Second.ToString());
            FATBELGE.GUID = Guid.NewGuid().ToString();
            FATBELGE.TRCODE = (int)EIrsaliyeTip.alim_Iade_Irsaliyesi;
            FATBELGE.GRPCODE = (int)EBelgeGurupKodlari.Alim_Faturalari;

            MasterResult<NTUPLE> FP = NExec.AdoInsert<LG_001_01_INVOICE>(FATBELGE, FATURABELGE);


            Belge Blgfat = NQery.AdoFind<Belge>(FATURABELGE, string.Format("FICHENO='{0}' ", P.belge.FICHENO)).Data.First();
            Belge_Model BM = new Belge_Model { belge = Blgfat, belgeLines = P.belgeLines };
            BM.belge.INVNO = Blgfat.FICHENO;
            BM.belge.FICHENO = P.belge.IRSFICHENO;
            BM.belge.GUID = Guid.NewGuid().ToString();
            BM.belge.INVOICEREF = Blgfat.LOGICALREF;
            BM.belge.BILLED = 1;
            P.belgeLines.ForEach(x => { x.INVOICELNNO++; x.INVOICEREF = Blgfat.LOGICALREF; ; x.IOCODE = (int)EIoKod.Cikis; x.STFICHELNNO++; });

            ///////////////////////////////irsaliye //////////////////////////
            IrsaliyeController ICL = new IrsaliyeController();
            MasterResult<List<NTUPLE>> res = ICL.addNewAlimIadeIrsaliye(BM);
            foreach (NTUPLE item in res.Data)
            {
                tuples.Add(item);

            }
            bool stat = tuples.Where(x => x.stat == 0).Count() > 0 ? false : true;
            if (!stat)
            {
                NExec.AdoDelete<LG_001_01_INVOICE>(FATURABELGE, string.Format(" where LOGICALREF={0}", Blgfat.LOGICALREF));
                NExec.AdoDelete<LG_001_01_STFICHE>(IRSALIYEBELGE, string.Format(" where INVOICEREF={0}", Blgfat.LOGICALREF));
                NExec.AdoDelete<LG_001_01_STLINE>(FATURALINES, string.Format(" where INVOICEREF={0}", Blgfat.LOGICALREF));
            }



            return new MasterResult<List<NTUPLE>> { Data = tuples, Elapsed = 0, Message = stat ? "Fatura Kaydı Oluşturuldu" : "Bazı Hatalar oluştu işlem Geri Alındı Hata detayı Mesajın Ekindedir.", Result = stat };
        }


        #endregion

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Fatura/YeniToptanSatisFaturasi")]
        public MasterResult<List<NTUPLE>> YeniToptanSatisFaturasi([FromBody] Belge_Post_Model P)

        {


            List<NTUPLE> tuples = new List<NTUPLE>();
            #region checkperiods
            if (P == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Boş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "Fatura Boş İşlem Yapılamaz", Result = false };
            if (P.Belge == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Boş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "Fatura Boş İşlem Yapılamaz", Result = false };
            //if (P.belge.TRCODE == 0)
            //    return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Tipi Belirtilmemiş", stat = 0 } }, Elapsed = 0, Message = "Fatura Tipi Belirtilmemiş", Result = false };


            if (P.Line == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Kalemleri Girilmemiş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "Fatura Kalemleri Girilmemiş İşlem Yapılamaz", Result = false };

            if (P.Line.Count == 0)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura Kalemleri Girilmemiş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "Fatura Kalemleri Girilmemiş İşlem Yapılamaz", Result = false };

            if (P.Belge.IRSFICHENO.Length == 0)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye No Girilmemiş", stat = 0 } }, Elapsed = 0, Message = "Fatura Kalemleri Girilmemiş İşlem Yapılamaz", Result = false };

            if (NQery.AdoFind<LG_001_01_INVOICE>(FATURABELGE, string.Format("FICHENO='{0}' ", P.Belge.FICHENO)).Result)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura No Daha Önce Tanımlanmış", stat = 0 } }, Elapsed = 0, Message = "Fatura No Daha Önce Tanımlanmış", Result = false };

            if (NQery.AdoFind<LG_001_01_INVOICE>(IRSALIYEBELGE, string.Format("FICHENO='{0}' ", P.Belge.IRSFICHENO)).Result)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Irsaliye No Daha Önce Tanımlanmış", stat = 0 } }, Elapsed = 0, Message = "Irsaliye No Daha Önce Tanımlanmış", Result = false };
            #endregion

           L_CAPIWHOUSE WRH = NQery.AdoFind<L_CAPIWHOUSE>(string.Format("L_CAPIWHOUSE", " FIRMNR={0} and NR={1}", int.Parse(Go3Interration.Models.AppCommon.getConf().FirmaNo), P.Belge.SOURCEINDEX)).Data.First();
            if(WRH==null) new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Depo Bilgisi Bulunamadı", stat = 0 } }, Elapsed = 0, Message = "Depo Bilgisi Bulunamadı", Result = false };


            P.Belge.SOURCECOSTGRP=short.Parse(WRH.COSTGRP.ToString());
            P.Belge.TRCODE = (int)EFaturaTip.ToptanSatis_Faturasi;

            MasterResult<NTUPLE> Ntp= LogoGo3Data.Tools.AppCommon.YeniFatura(P, FATURABELGE);
            if (!Ntp.Result)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fatura İşenemedi", stat = 0 } }, Elapsed = 0, Message = "Fatura İşlenemedi", Result = false };


            LG_001_01_INVOICE Blgfat = NQery.AdoFind<LG_001_01_INVOICE>(FATURABELGE, string.Format("FICHENO='{0}' ", P.Belge.FICHENO)).Data.First();
            if (Blgfat == null)
            {
                NExec.AdoDelete<LG_001_01_INVOICE>(FATURABELGE, string.Format(" where FICHENO='{0}'", P.Belge.FICHENO));
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Referans Fatura Bulunamadı", stat = 0 } }, Elapsed = 0, Message = "Referans Fatura Bulunamadı", Result = false };
            }

            P.Line.ForEach(x=>{
              
            });

            bool stat = tuples.Where(x => x.stat == 0).Count() > 0 ? false : true;
            if (!stat)
            {
                NExec.AdoDelete<LG_001_01_INVOICE>(FATURABELGE, string.Format(" where LOGICALREF={0}", Blgfat.LOGICALREF));
                NExec.AdoDelete<LG_001_01_STFICHE>(IRSALIYEBELGE, string.Format(" where INVOICEREF={0}", Blgfat.LOGICALREF));
                NExec.AdoDelete<LG_001_01_STLINE>(FATURALINES, string.Format(" where INVOICEREF={0}", Blgfat.LOGICALREF));
            }



            return new MasterResult<List<NTUPLE>> { Data = tuples, Elapsed = 0, Message = stat ? "Fatura Kaydı Oluşturuldu" : "Bazı Hatalar oluştu işlem Geri Alındı Hata detayı Mesajın Ekindedir.", Result = stat };
        }


    }
}
