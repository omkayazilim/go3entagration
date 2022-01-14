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
    public class IrsaliyeController : ApiController
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("api/Irsaliye/getIrsaliyeList")]
        public MasterResult<List<Belge_Model2>> getIrsaliyeList()
        {
            
            string BELGETABLENAME = string.Format("LG_{0}_{1}_STFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            string ITEMTABLENAME = string.Format("LG_{0}_{1}_STLINE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);

            List<Belge_Model2> bm = new List<Belge_Model2>();
            List<LG_001_01_STFICHE> blg= NQery.AdoFind<LG_001_01_STFICHE>(BELGETABLENAME).Data;

            foreach (var item in blg)
            {
              
              List<LG_001_01_STLINE> ln= NQery.AdoFind<LG_001_01_STLINE>(ITEMTABLENAME,string.Format(" STFICHEREF={0}", item.LOGICALREF)).Data;
                bm.Add(new Belge_Model2 { belge=item, belgeLines=ln });
           
            }
          


           return new MasterResult <List<Belge_Model2> >{ Data= bm, Elapsed=0, Message="", Result=true  };
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Irsaliye/deleteIrsaliye")]
        public MasterResult<NTUPLE> deleteIrsaliye([FromBody] Belge B)
        {
            string BELGETABLENAME = string.Format("LG_{0}_{1}_STFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            string ITEMTABLENAME = string.Format("LG_{0}_{1}_STLINE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            NExec.AdoDelete<LG_001_01_STFICHE>(BELGETABLENAME," where LOGICALREF="+B.LOGICALREF);
            return NExec.AdoDelete<LG_001_01_STLINE>(ITEMTABLENAME, " where STFICHEREF=" + B.LOGICALREF);

        }




      
        public MasterResult<List<NTUPLE>> addNewIrsaliye([FromBody] Belge_Model P)
        {
         
       
            List<NTUPLE> tuples = new List<NTUPLE>();

            if (P == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Boş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Boş İşlem Yapılamaz", Result = false };



            if (P.belge==null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Boş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Boş İşlem Yapılamaz", Result = false };


            if (P.belge.TRCODE==0)
               return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Tipi Belirtilmemiş", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Tipi Belirtilmemiş", Result = false };


            if (P.belgeLines == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", Result = false };
            if (P.belgeLines.Count == 0)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", Result = false };




            string BELGETABLENAME = string.Format("LG_{0}_{1}_STFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            string ITEMTABLENAME = string.Format("LG_{0}_{1}_STLINE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            P.belge.GUID = Guid.NewGuid().ToString();
            P.belge.DATE_ = DateTime.Now;
            P.belge.CAPIBLOCK_CREADEDDATE = DateTime.Now;
            P.belge.CAPIBLOCK_MODIFIEDDATE = DateTime.Now;
            P.belge.IOCODE = P.belge.TRCODE ==(int)EIrsaliyeTip.toptan_Satis_Irsaliyesi
                || P.belge.TRCODE == (int)EIrsaliyeTip.alim_Iade_Irsaliyesi ? (int)EIoKod.Cikis

                   :P.belge.TRCODE==(int)EIrsaliyeTip.toptan_Satis_Iade_Irsaliyesi
                || P.belge.TRCODE == (int)EIrsaliyeTip.alim_Irsaliyesi ? (int)EIoKod.Giris :
                0;

               P.belge.GRPCODE = P.belge.TRCODE == (int)EIrsaliyeTip.alim_Iade_Irsaliyesi || P.belge.TRCODE == (int)EIrsaliyeTip.alim_Irsaliyesi ? (int)EBelgeGurupKodlari.Alim_irsaliyeleri 
             : P.belge.TRCODE == (int)EIrsaliyeTip.toptan_Satis_Iade_Irsaliyesi || P.belge.TRCODE == (int)EIrsaliyeTip.toptan_Satis_Irsaliyesi ? (int)EBelgeGurupKodlari.Satis_irsaliyeleri: 0;

            L_CAPIWHOUSE WRH = NQery.AdoFind<L_CAPIWHOUSE>(string.Format("L_CAPIWHOUSE", " FIRMNR={0} and NR={1}",int.Parse(AppCommon.getConf().FirmaNo),P.belge.SOURCEINDEX)).Data.First();
            P.belge.SOURCECOSTGRP = int.Parse(WRH.COSTGRP.ToString());
      
           


            if (NQery.AdoFind<LG_001_01_STFICHE>(BELGETABLENAME, string.Format("FICHENO='{0}' ", P.belge.FICHENO)).Result)
                   return  new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fiş No Daha Önce Tanımlanmış", stat = 0 } }, Elapsed = 0, Message = "Fiş No Daha Önce Tanımlanmış", Result = false };

            LG_001_01_STFICHE BELGE = LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_01_STFICHE>(P.belge, BELGETABLENAME,(int)EIrsaliyeTip.toptan_Satis_Irsaliyesi); //new LG_001_01_STFICHE();
            MasterResult<NTUPLE> TP = NExec.AdoInsert<LG_001_01_STFICHE>(BELGE, BELGETABLENAME);
            if (!TP.Result)
                return  new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = TP.Data.rec, stat = 0 } }, Elapsed = 0, Message = TP.Message, Result = false };

            ////////////////////////////////////////////--0--///////////////////////////////////////////////////////////////////////////

            Belge Blg = NQery.AdoFind<Belge>(BELGETABLENAME, string.Format("FICHENO='{0}' ", P.belge.FICHENO)).Data.First();
        

            int K = 1;
            foreach (BelgeLine line in P.belgeLines)
            {


                line.GUID = Guid.NewGuid().ToString();
                line.STFICHEREF = Blg.LOGICALREF;
                line.STFICHELNNO = K;
                line.DATE_ = DateTime.Now;
                line.TRCODE = Blg.TRCODE;
                line.CLIENTREF = Blg.CLIENTREF;
                line.DATE_ = Blg.DATE_;
                line.IOCODE = Blg.IOCODE;
                line.SOURCECOSTGRP = int.Parse(WRH.COSTGRP.ToString());
                line.SOURCEINDEX = int.Parse(WRH.NR.ToString());
                
                
                K++;

               LG_001_01_STLINE NEWLINE = LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_01_STLINE>(line, ITEMTABLENAME,0);
               MasterResult<NTUPLE> sc=NExec.AdoInsert<LG_001_01_STLINE>(NEWLINE,ITEMTABLENAME);
               tuples.Add(sc.Data);

            }


            bool stat = tuples.Where(x => x.stat == 0).Count() > 0 ? false : true;
            if (!stat)
            {
                NExec.AdoDelete<LG_001_01_STFICHE>(BELGETABLENAME, string.Format(" where LOGICALREF={0}", Blg.LOGICALREF));
                NExec.AdoDelete<LG_001_01_STLINE>(ITEMTABLENAME, string.Format(" where STFICHEREF={0}", Blg.LOGICALREF));
            }

            

            return new MasterResult<List<NTUPLE>> { Data=tuples, Elapsed=0, Message=stat?"İrsaliye Kaydı Oluşturuldu":"Bazı Hatalar oluştu işlem Geri Alındı Hata detayı Mesajın Ekindedir.",Result=stat };
        }




        [AllowAnonymous]
        [HttpPost]
        [Route("api/Irsaliye/addNewAlimIrsaliye")]
        public MasterResult<List<NTUPLE>> addNewAlimIrsaliye([FromBody] Belge_Model P)

        {


            List<NTUPLE> tuples = new List<NTUPLE>();

            if (P == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Boş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Boş İşlem Yapılamaz", Result = false };



            if (P.belge == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Boş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Boş İşlem Yapılamaz", Result = false };


            if (P.belge.TRCODE == 0)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Tipi Belirtilmemiş", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Tipi Belirtilmemiş", Result = false };


            if (P.belgeLines == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", Result = false };
            if (P.belgeLines.Count == 0)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", Result = false };




            string BELGETABLENAME = string.Format("LG_{0}_{1}_STFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            string ITEMTABLENAME = string.Format("LG_{0}_{1}_STLINE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            L_CAPIWHOUSE WRH = NQery.AdoFind<L_CAPIWHOUSE>(string.Format("L_CAPIWHOUSE", " FIRMNR={0} and NR={1}", int.Parse(AppCommon.getConf().FirmaNo), P.belge.SOURCEINDEX)).Data.First();
            P.belge.SOURCECOSTGRP = int.Parse(WRH.COSTGRP.ToString());




            if (NQery.AdoFind<LG_001_01_STFICHE>(BELGETABLENAME, string.Format("FICHENO='{0}' ", P.belge.FICHENO)).Result)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fiş No Daha Önce Tanımlanmış", stat = 0 } }, Elapsed = 0, Message = "Fiş No Daha Önce Tanımlanmış", Result = false };

            MasterResult<LG_001_01_STFICHE> MCLS = LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_01_STFICHE>(P,(int)EIrsaliyeTip.alim_Irsaliyesi);
            if (!MCLS.Result)
                return new MasterResult<List<NTUPLE>> { Data = null, Result = false, Elapsed = 0, Message = "Referans Model Bulunamadı" };


            LG_001_01_STFICHE BELGE = MCLS.Data;//LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_01_STFICHE>(P.belge, BELGETABLENAME, (int)EIrsaliyeTip.alim_Irsaliyesi); //new LG_001_01_STFICHE();



            BELGE.CAPIBLOCK_CREADEDDATE = DateTime.Now;
            BELGE.CAPIBLOCK_MODIFIEDDATE = null;
            BELGE.CAPIBLOCK_CREATEDHOUR = short.Parse(DateTime.Now.Hour.ToString());
            BELGE.CAPIBLOCK_CREATEDMIN = short.Parse(DateTime.Now.Minute.ToString());
            BELGE.CAPIBLOCK_CREATEDSEC = short.Parse(DateTime.Now.Second.ToString());
            BELGE.GUID = Guid.NewGuid().ToString();
            BELGE.TRCODE = (int)EIrsaliyeTip.alim_Irsaliyesi;
            BELGE.GRPCODE = (int)EBelgeGurupKodlari.Alim_irsaliyeleri;
            MasterResult<NTUPLE> TP = NExec.AdoInsert<LG_001_01_STFICHE>(BELGE, BELGETABLENAME);
            if (!TP.Result)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = TP.Data.rec, stat = 0 } }, Elapsed = 0, Message = TP.Message, Result = false };

            ////////////////////////////////////////////--0--///////////////////////////////////////////////////////////////////////////

            Belge Blg = NQery.AdoFind<Belge>(BELGETABLENAME, string.Format("FICHENO='{0}' ", P.belge.FICHENO)).Data.First();


            int K = 1;
            foreach (BelgeLine line in P.belgeLines)
            {


                line.GUID = Guid.NewGuid().ToString();
                line.STFICHEREF = Blg.LOGICALREF;
                line.STFICHELNNO = K;
                line.DATE_ = DateTime.Now;
                line.TRCODE = Blg.TRCODE;
                line.CLIENTREF = Blg.CLIENTREF;
                line.DATE_ = Blg.DATE_;
                line.IOCODE = Blg.IOCODE;
                line.SOURCECOSTGRP = int.Parse(WRH.COSTGRP.ToString());
                line.SOURCEINDEX = int.Parse(WRH.NR.ToString());


                K++;


                MasterResult<LG_001_01_STLINE> MCLSI = LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_01_STLINE>(P, (int)EIrsaliyeTip.alim_Irsaliyesi);
                tuples.Add(new NTUPLE { rec=null, stat=MCLSI.Result?1:0 });

                LG_001_01_STLINE NEWLINE = MCLSI.Data; //LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_01_STLINE>(line, ITEMTABLENAME, 0);
                MasterResult<NTUPLE> sc = NExec.AdoInsert<LG_001_01_STLINE>(NEWLINE, ITEMTABLENAME);
                tuples.Add(sc.Data);

            }


            bool stat = tuples.Where(x => x.stat == 0).Count() > 0 ? false : true;
            if (!stat)
            {
                NExec.AdoDelete<LG_001_01_STFICHE>(BELGETABLENAME, string.Format(" where LOGICALREF={0}", Blg.LOGICALREF));
                NExec.AdoDelete<LG_001_01_STLINE>(ITEMTABLENAME, string.Format(" where STFICHEREF={0}", Blg.LOGICALREF));
            }



            return new MasterResult<List<NTUPLE>> { Data = tuples, Elapsed = 0, Message = stat ? "İrsaliye Kaydı Oluşturuldu" : "Bazı Hatalar oluştu işlem Geri Alındı Hata detayı Mesajın Ekindedir.", Result = stat };
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("api/Irsaliye/addNewSatisIrsaliye")]
        public MasterResult<List<NTUPLE>> addNewSatisIrsaliye([FromBody] Belge_Model P)

        {


            List<NTUPLE> tuples = new List<NTUPLE>();

            if (P == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Boş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Boş İşlem Yapılamaz", Result = false };



            if (P.belge == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Boş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Boş İşlem Yapılamaz", Result = false };


            if (P.belge.TRCODE == 0)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Tipi Belirtilmemiş", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Tipi Belirtilmemiş", Result = false };


            if (P.belgeLines == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", Result = false };
            if (P.belgeLines.Count == 0)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", Result = false };




            string BELGETABLENAME = string.Format("LG_{0}_{1}_STFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            string ITEMTABLENAME = string.Format("LG_{0}_{1}_STLINE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            L_CAPIWHOUSE WRH = NQery.AdoFind<L_CAPIWHOUSE>(string.Format("L_CAPIWHOUSE", " FIRMNR={0} and NR={1}", int.Parse(AppCommon.getConf().FirmaNo), P.belge.SOURCEINDEX)).Data.First();
            P.belge.SOURCECOSTGRP = int.Parse(WRH.COSTGRP.ToString());




            if (NQery.AdoFind<LG_001_01_STFICHE>(BELGETABLENAME, string.Format("FICHENO='{0}' ", P.belge.FICHENO)).Result)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fiş No Daha Önce Tanımlanmış", stat = 0 } }, Elapsed = 0, Message = "Fiş No Daha Önce Tanımlanmış", Result = false };

            LG_001_01_STFICHE BELGE = LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_01_STFICHE>(P.belge, BELGETABLENAME, (int)EIrsaliyeTip.toptan_Satis_Irsaliyesi); //new LG_001_01_STFICHE();
            BELGE.CAPIBLOCK_CREADEDDATE = DateTime.Now;
            BELGE.CAPIBLOCK_MODIFIEDDATE = null;
            BELGE.CAPIBLOCK_CREATEDHOUR = short.Parse(DateTime.Now.Hour.ToString());
            BELGE.CAPIBLOCK_CREATEDMIN = short.Parse(DateTime.Now.Minute.ToString());
            BELGE.CAPIBLOCK_CREATEDSEC = short.Parse(DateTime.Now.Second.ToString());
            BELGE.GUID = Guid.NewGuid().ToString();
            BELGE.TRCODE = (int)EIrsaliyeTip.toptan_Satis_Irsaliyesi;
            BELGE.GRPCODE = (int)EBelgeGurupKodlari.Satis_irsaliyeleri;
            MasterResult<NTUPLE> TP = NExec.AdoInsert<LG_001_01_STFICHE>(BELGE, BELGETABLENAME);
            if (!TP.Result)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = TP.Data.rec, stat = 0 } }, Elapsed = 0, Message = TP.Message, Result = false };

            ////////////////////////////////////////////--0--///////////////////////////////////////////////////////////////////////////

            Belge Blg = NQery.AdoFind<Belge>(BELGETABLENAME, string.Format("FICHENO='{0}' ", P.belge.FICHENO)).Data.First();


            int K = 1;
            foreach (BelgeLine line in P.belgeLines)
            {


                line.GUID = Guid.NewGuid().ToString();
                line.STFICHEREF = Blg.LOGICALREF;
                line.STFICHELNNO = K;
                line.DATE_ = DateTime.Now;
                line.TRCODE = Blg.TRCODE;
                line.CLIENTREF = Blg.CLIENTREF;
                line.DATE_ = Blg.DATE_;
                line.IOCODE = Blg.IOCODE;
                line.SOURCECOSTGRP = int.Parse(WRH.COSTGRP.ToString());
                line.SOURCEINDEX = int.Parse(WRH.NR.ToString());


                K++;

                LG_001_01_STLINE NEWLINE = LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_01_STLINE>(line, ITEMTABLENAME, 0);
                MasterResult<NTUPLE> sc = NExec.AdoInsert<LG_001_01_STLINE>(NEWLINE, ITEMTABLENAME);
                tuples.Add(sc.Data);

            }


            bool stat = tuples.Where(x => x.stat == 0).Count() > 0 ? false : true;
            if (!stat)
            {
                NExec.AdoDelete<LG_001_01_STFICHE>(BELGETABLENAME, string.Format(" where LOGICALREF={0}", Blg.LOGICALREF));
                NExec.AdoDelete<LG_001_01_STLINE>(ITEMTABLENAME, string.Format(" where STFICHEREF={0}", Blg.LOGICALREF));
            }



            return new MasterResult<List<NTUPLE>> { Data = tuples, Elapsed = 0, Message = stat ? "İrsaliye Kaydı Oluşturuldu" : "Bazı Hatalar oluştu işlem Geri Alındı Hata detayı Mesajın Ekindedir.", Result = stat };
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("api/Irsaliye/addNewAlimIadeIrsaliye")]
        public MasterResult<List<NTUPLE>> addNewAlimIadeIrsaliye([FromBody] Belge_Model P)

        {


            List<NTUPLE> tuples = new List<NTUPLE>();

            if (P == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Boş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Boş İşlem Yapılamaz", Result = false };



            if (P.belge == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Boş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Boş İşlem Yapılamaz", Result = false };


            if (P.belge.TRCODE == 0)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Tipi Belirtilmemiş", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Tipi Belirtilmemiş", Result = false };


            if (P.belgeLines == null)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", Result = false };
            if (P.belgeLines.Count == 0)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", stat = 0 } }, Elapsed = 0, Message = "İrsaliye Kalemleri Girilmemiş İşlem Yapılamaz", Result = false };




            string BELGETABLENAME = string.Format("LG_{0}_{1}_STFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            string ITEMTABLENAME = string.Format("LG_{0}_{1}_STLINE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            L_CAPIWHOUSE WRH = NQery.AdoFind<L_CAPIWHOUSE>(string.Format("L_CAPIWHOUSE", " FIRMNR={0} and NR={1}", int.Parse(AppCommon.getConf().FirmaNo), P.belge.SOURCEINDEX)).Data.First();
            P.belge.SOURCECOSTGRP = int.Parse(WRH.COSTGRP.ToString());




            if (NQery.AdoFind<LG_001_01_STFICHE>(BELGETABLENAME, string.Format("FICHENO='{0}' ", P.belge.FICHENO)).Result)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = "Fiş No Daha Önce Tanımlanmış", stat = 0 } }, Elapsed = 0, Message = "Fiş No Daha Önce Tanımlanmış", Result = false };

            LG_001_01_STFICHE BELGE = LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_01_STFICHE>(P.belge, BELGETABLENAME, (int)EIrsaliyeTip.alim_Iade_Irsaliyesi); //new LG_001_01_STFICHE();
            BELGE.CAPIBLOCK_CREADEDDATE = DateTime.Now;
            BELGE.CAPIBLOCK_MODIFIEDDATE = null;
            BELGE.CAPIBLOCK_CREATEDHOUR = short.Parse(DateTime.Now.Hour.ToString());
            BELGE.CAPIBLOCK_CREATEDMIN = short.Parse(DateTime.Now.Minute.ToString());
            BELGE.CAPIBLOCK_CREATEDSEC = short.Parse(DateTime.Now.Second.ToString());
            BELGE.GUID = Guid.NewGuid().ToString();
            BELGE.TRCODE = (int)EIrsaliyeTip.alim_Iade_Irsaliyesi;
            BELGE.GRPCODE = (int)EBelgeGurupKodlari.Alim_irsaliyeleri;
            MasterResult<NTUPLE> TP = NExec.AdoInsert<LG_001_01_STFICHE>(BELGE, BELGETABLENAME);
            if (!TP.Result)
                return new MasterResult<List<NTUPLE>> { Data = new List<NTUPLE> { new NTUPLE { rec = TP.Data.rec, stat = 0 } }, Elapsed = 0, Message = TP.Message, Result = false };

            ////////////////////////////////////////////--0--///////////////////////////////////////////////////////////////////////////

            Belge Blg = NQery.AdoFind<Belge>(BELGETABLENAME, string.Format("FICHENO='{0}' ", P.belge.FICHENO)).Data.First();


            int K = 1;
            foreach (BelgeLine line in P.belgeLines)
            {


                line.GUID = Guid.NewGuid().ToString();
                line.STFICHEREF = Blg.LOGICALREF;
                line.STFICHELNNO = K;
                line.DATE_ = DateTime.Now;
                line.TRCODE = Blg.TRCODE;
                line.CLIENTREF = Blg.CLIENTREF;
                line.DATE_ = Blg.DATE_;
                line.IOCODE = Blg.IOCODE;
                line.SOURCECOSTGRP = int.Parse(WRH.COSTGRP.ToString());
                line.SOURCEINDEX = int.Parse(WRH.NR.ToString());


                K++;

                LG_001_01_STLINE NEWLINE = LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_01_STLINE>(line, ITEMTABLENAME, 0);
                MasterResult<NTUPLE> sc = NExec.AdoInsert<LG_001_01_STLINE>(NEWLINE, ITEMTABLENAME);
                tuples.Add(sc.Data);

            }


            bool stat = tuples.Where(x => x.stat == 0).Count() > 0 ? false : true;
            if (!stat)
            {
                NExec.AdoDelete<LG_001_01_STFICHE>(BELGETABLENAME, string.Format(" where LOGICALREF={0}", Blg.LOGICALREF));
                NExec.AdoDelete<LG_001_01_STLINE>(ITEMTABLENAME, string.Format(" where STFICHEREF={0}", Blg.LOGICALREF));
            }



            return new MasterResult<List<NTUPLE>> { Data = tuples, Elapsed = 0, Message = stat ? "İrsaliye Kaydı Oluşturuldu" : "Bazı Hatalar oluştu işlem Geri Alındı Hata detayı Mesajın Ekindedir.", Result = stat };
        }
    }
}
