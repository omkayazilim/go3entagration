using Go3Interration.Models;
using LogoGo3Data;
using LogoGo3Data.Context;
using LogoGo3Data.DefineModel;
using LogoGo3Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using static LogoGo3Data.Extras.Utils;
using static LogoGo3Data.Soap_Models.Models;

namespace Go3Interration
{
    /// <summary>
    /// Summary description for Go3ent
    /// </summary>
    [WebService(Namespace = "http://cemasoft.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Object : System.Web.Services.WebService
    {
        JavaScriptSerializer jsonCreator = null;
        public Object() {
            jsonCreator = new JavaScriptSerializer();
            jsonCreator.MaxJsonLength = int.MaxValue;
        }

        #region CARİ KARTI İŞLEMLERİ
        [WebMethod(Description = "Cari Kart Listesi Metodu (Metod No 1)")]
        [ScriptMethod(UseHttpGet = true)]
        public string GETCARIKARTLAR(string WSPASS, string FILTERS)
        {
           
            List<Array_Result_Master> LOG = new List<Array_Result_Master>();
            Array_Result_Master Master = null;
          
               
                    List<Array_CariKartlar_Respons> CRES = new List<Array_CariKartlar_Respons>();
            MasterResult<List<Cari_Model>> MSTR= NQery.AdoFind<Cari_Model>(string.Format("LG_{0}_CLCARD", AppCommon.getConf().FirmaNo));
            if (MSTR.Result)
            {
                foreach (var item in MSTR.Data)
                {
                    CRES.Add(new Array_CariKartlar_Respons
                    {
                        CARI_ADRES = item.ADDR1 + " " + item.ADDR2,
                        CARI_IL = item.CITY,
                        CARI_ILCE = item.DISTRICT,
                        CARI_ISIM = item.DEFINITION_,
                        CARI_KOD = item.CODE,
                        CARI_TEL = item.TELNRS1,
                        CARI_TEL2 = item.TELNRS2,
                        CARI_TEL3 = "",
                        CARI_TIP = item.CARDTYPE.ToString(),
                        DETAY_KODU = "",
                        EMAIL = item.EMAILADDR,
                        FAX = item.FAXNR,
                        FAX2 = "",
                        GSM1 = "",
                        GSM2 = "",
                        ISLETME_KODU = 0,
                        M_KOD = "",
                        POSTAKODU = item.POSTCODE,
                        SUBE_KODU = 0,
                        VERGI_DAIRESI = item.TAXOFFICE,
                        VERGI_NUMARASI = item.TAXNR,
                        WEB = ""
                    });
                }

                Master = new Array_Result_Master { SONUC = false, DATA = jsonCreator.Serialize(CRES), ACIKLAMA = "" };
            }
            else {

                Master = new Array_Result_Master { SONUC = false, DATA = "[]", ACIKLAMA = MSTR.Message };
            }


       
                    





               

           

            return jsonCreator.Serialize(Master);

        }

        [WebMethod(Description = "Cari Kart Listesi Metodu (Metod No 2)")]
        [ScriptMethod(UseHttpGet = true)]
        public string CARIKARTLARARAMA(string WSPASS, string FILTERSSEARCH)
        {
        
            Array_Result_Master Master = null;

            Array_Filter_Request_Serach FRQ = jsonCreator.Deserialize<Array_Filter_Request_Serach>(FILTERSSEARCH);
            List<Array_CariKartlar_Respons> CRES = new List<Array_CariKartlar_Respons>();
            MasterResult<List<Cari_Model>> MSTR = NQery.AdoFind<Cari_Model>(string.Format("LG_{0}_CLCARD", AppCommon.getConf().FirmaNo), string.Format(" CODE like '%{0}%' or NAME like '%{0}%' or  DEFINITION_ like '%{0}%'  or  DEFINITION2 like '%{0}%'", FRQ.SEARCH));
            if (MSTR.Result)
            {
                foreach (var item in MSTR.Data)
                {
                    CRES.Add(new Array_CariKartlar_Respons
                    {
                        CARI_ADRES = item.ADDR1 + " " + item.ADDR2,
                        CARI_IL = item.CITY,
                        CARI_ILCE = item.DISTRICT,
                        CARI_ISIM = item.DEFINITION_,
                        CARI_KOD = item.CODE,
                        CARI_TEL = item.TELNRS1,
                        CARI_TEL2 = item.TELNRS2,
                        CARI_TEL3 = "",
                        CARI_TIP = item.CARDTYPE.ToString(),
                        DETAY_KODU = "",
                        EMAIL = item.EMAILADDR,
                        FAX = item.FAXNR,
                        FAX2 = "",
                        GSM1 = "",
                        GSM2 = "",
                        ISLETME_KODU = 0,
                        M_KOD = "",
                        POSTAKODU = item.POSTCODE,
                        SUBE_KODU = 0,
                        VERGI_DAIRESI = item.TAXOFFICE,
                        VERGI_NUMARASI = item.TAXNR,
                        WEB = ""
                    });
                }

                Master = new Array_Result_Master { SONUC = false, DATA = jsonCreator.Serialize(CRES), ACIKLAMA = "" };
            }
            else
            {

                Master = new Array_Result_Master { SONUC = false, DATA = "[]", ACIKLAMA = MSTR.Message };
            }


            Master = new Array_Result_Master { SONUC = false, DATA = "[]", ACIKLAMA = "" };

            

            return jsonCreator.Serialize(Master);

        }


        [WebMethod(Description = "Yeni Cari Kart (Metod No 3)")]
        [ScriptMethod(UseHttpGet = true)]
        public string YENICARIKART(string WSPASS, string ConnectionObject, string OBJECTS)

        {
            string CTABLE = string.Format("LG_{0}_CLCARD", AppCommon.getConf().FirmaNo);
            List<Array_Result_Master> LOG = new List<Array_Result_Master>();
            Array_Result_Master Master = null;
           
              
                 
              List<Array_Cari_Request> CRG = jsonCreator.Deserialize<List<Array_Cari_Request>>(OBJECTS);
               foreach (var item in CRG)
               {
                    Cari_Model CM = new Cari_Model {
                    ACTIVE = 1,
                    ADDR1 = item.CARI_ADRES,
                    ADDR2 = "",
                    CARDTYPE = 3,
                    CITY = item.CARI_IL

                           , CODE = item.CARI_KOD
                           , COUNTRY = "TR"


                           , DEFINITION_ = item.CARI_ISIM
                           , DISTRICT = item.CARI_ILCE

                           , EMAILADDR = item.EMAIL

                           , FAXNR = item.FAX
                           , GUID = Guid.NewGuid().ToString()

                           , LATITUTE = ""
                           , LOGICALREF = 0

                           , LONGITUDE = ""
                           , NAME = item.CARI_ISIM
                           , POSTCODE = item.POSTAKODU

                           , SPECODE = item.M_KOD
                           , SPECODE2 = ""
                           , SPECODE3 = ""
                           , SPECODE4 = ""
                           , SPECODE5 = ""
                           , SURNAME = ""
                           , TAXNR = item.VERGI_NUMARASI

                           , TAXOFFICE = item.VERGI_DAIRESI
                           , TCKNO = item.TcKimlikNo

                           , TELNRS1 = item.CARI_TEL
                           , TELNRS2 = item.CARI_TEL2
                           , TOWN = ""
                         
                           , WEBADDR = item.WEB

                      
                };
                if (NQery.AdoFind<Cari_Model>(CTABLE, string.Format(" CODE='{0}' or TAXNR='{1}' or TCKNO='{2}'", CM.CODE, CM.TAXNR, CM.TCKNO)).Result)
                {
                   Master= new Array_Result_Master { ACIKLAMA = "Cari daha önce tanımlanmış", DATA = null, SONUC = false };
                    break;
                }
                else {
                    MasterResult<LG_001_CLCARD> MCLS = LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_CLCARD>(CM, 0);
                    if (!MCLS.Result)
                    {
                        Master = new Array_Result_Master { ACIKLAMA = "Referans Model Oluşturulamadı", DATA = null, SONUC = false };
                        break;
                    }
                    else {
                        LG_001_CLCARD CLCARD = MCLS.Data;
                        var TFG= NExec.AdoInsert<LG_001_CLCARD>(CLCARD, CTABLE);
                        if (TFG.Result)
                            Master = new Array_Result_Master { ACIKLAMA = "Cari Oluşturuldu", DATA = null, SONUC = true };
                        else
                            Master = new Array_Result_Master { ACIKLAMA = TFG.Message, DATA = null, SONUC = false };

                    }
                }

                      
                 
              }
                   
                   return jsonCreator.Serialize(Master);

        }
        #endregion

        #region STOK KARTI İŞLEMLERİ

        //[WebMethod(Description = "StokKartı Grupları Listesi (Metod No 4)")]
        //[ScriptMethod(UseHttpGet = true)]
        //public string GETSTOKKARTI_GRUPLARI(string WSPASS, string FILTERS)
        //{
        //    List<Array_Result_Master> LOG = new List<Array_Result_Master>();
        //    Array_Result_Master Master = null;
        //  {
        //        try
        //        {
        //            Array_Filter_Request FRQ = jsonCreator.Deserialize<Array_Filter_Request>(FILTERS);
        //            List<Array_Stok_Guruplari_Respons> CRES = new List<Array_Stok_Guruplari_Respons>();

        //            string srg = string.Format("select ISLETME_KODU,SUBE_KODU,GRUP_KOD,GRUP_ISIM from TBLSTGRUP where ISLETME_KODU={0} and SUBE_KODU={1}", FRQ.ISLETME_KODU, FRQ.SUBE_KODU);
        //            Array_Sorgu_Reponses RESP = SQC.Sorgu(new Array_Sorgu_Request { Sorgu = srg, T_ADI = "Stok Grupları Listesi" });
        //            if (RESP.Sonuc)
        //            {
        //                foreach (DataRow i in RESP.T.Rows)
        //                {
        //                    CRES.Add(new Array_Stok_Guruplari_Respons
        //                    {
        //                        GRUP_ISIM = i["GRUP_ISIM"].ToString()
        //                         ,
        //                        GRUP_KOD = i["GRUP_KOD"].ToString()
        //                         ,
        //                        ISLETME_KODU = int.Parse(i["ISLETME_KODU"].ToString())
        //                         ,
        //                        SUBE_KODU = int.Parse(i["SUBE_KODU"].ToString())



        //                    });
        //                }

        //                Master = new Array_Result_Master { SONUC = true, DATA = jsonCreator.Serialize(CRES), ACIKLAMA = RESP.Aciklama };
        //                LOG.Add(Master);

        //            }
        //            else
        //            {
        //                Master = new Array_Result_Master { SONUC = false, DATA = "[]", ACIKLAMA = RESP.Aciklama };
        //                LOG.Add(Master);
        //            }





        //        }
        //        catch (Exception ex)
        //        {

        //            Master = new Array_Result_Master { SONUC = false, DATA = "[]", ACIKLAMA = ex.Message };
        //            LOG.Add(Master);
        //        }


         
        //    return jsonCreator.Serialize(Master);

        //}


        [WebMethod(Description = "Stok Kartları Listesi (Metod No 4)")]
        [ScriptMethod(UseHttpGet = true)]
        public string GETSTOKKARTLAR(string WSPASS, string FILTERS)
        {
          
            Array_Result_Master Master = null;
          
             
                    Array_Filter_Request FRQ = jsonCreator.Deserialize<Array_Filter_Request>(FILTERS);
                
                       List<Array_Stoklar_Response> LRES = new List<Array_Stoklar_Response>();
                      MasterResult<List<Malzeme_Model>> STK = NQery.AdoFind<Malzeme_Model>(string.Format("LG_{0}_ITEMS", AppCommon.getConf().FirmaNo), " not CARDTYPE=22");
            if (!STK.Result)
                return jsonCreator.Serialize(new Array_Result_Master { SONUC = false, DATA = "[]", ACIKLAMA = STK.Message });

            foreach (var item in STK.Data)
            {
                LRES.Add(new Array_Stoklar_Response {
                    ALIS_DOV_TIP = 0,
                    ALIS_KDV_KODU = 0,
                    BILESENMI = "",
                    BIRIM_AGIRLIK = 0,
                    DEPO_KODU = 0,
                    FIAT_BIRIMI = "",
                    FIKTIF_MAM = "",
                    GRUP_KODU = "",
                    ISLETME_KODU = item.LOGICALREF,
                    KDV_ORANI = item.RETURNVAT,
                    KILIT = "",
                    MAMULMU = "",
                    MUH_DETAYKODU = item.CARDTYPE,
                    MUSTERISIPKILIT = "",
                    OLCU_BR1 = item.UNITSETREF.ToString(),
                    ONAYNUM = 0,
                    ONAYTIPI = "",
                    PAY2 = 0,
                    PAYDA2 = 0,
                    PAYDA_1 = 0,
                    PAY_1 = 0,
                    PLANLANACAK = "",
                    SATICISIPKILIT = "",
                    SATINALMAKILIT = "",
                    SATISKILIT = "",
                    SAT_DOV_TIP = 0,
                    SBOMVARMI = "",
                    SIP_POLITIKASI = "",
                    STOK_ADI = item.NAME,
                    STOK_KODU = item.CODE,
                    SUBE_KODU = 0,
                    UPDATE_KODU = "",
                    YAPILANDIR = ""
                  });
            }
              Master = new Array_Result_Master { SONUC = true, DATA = jsonCreator.Serialize(LRES), ACIKLAMA ="" };
               return jsonCreator.Serialize(Master);

        }

        [WebMethod(Description = "Stok Kartları Listesi (Metod No 5)")]
        [ScriptMethod(UseHttpGet = true)]
        public string STOKKARTIARAMA(string WSPASS, string FILTERSSEARCH)
        {
            Array_Result_Master Master = null;


            Array_Filter_Request_Serach FRQ = jsonCreator.Deserialize<Array_Filter_Request_Serach>(FILTERSSEARCH);
            if(FRQ==null)
            return jsonCreator.Serialize(new Array_Result_Master { SONUC = false, DATA = "[]", ACIKLAMA ="Filtre Key Boş Olamaz !"});

            List<Array_Stoklar_Response> LRES = new List<Array_Stoklar_Response>();
            MasterResult<List<Malzeme_Model>> STK = NQery.AdoFind<Malzeme_Model>(string.Format("LG_{0}_ITEMS", AppCommon.getConf().FirmaNo), string.Format("CODE like '%{0}%' or NAME like '%{0}%' and not CARDTYPE=22", FRQ.SEARCH));
            if (!STK.Result)
                return jsonCreator.Serialize(new Array_Result_Master { SONUC = false, DATA = "[]", ACIKLAMA = STK.Message });

            foreach (var item in STK.Data)
            {
                LRES.Add(new Array_Stoklar_Response
                {
                    ALIS_DOV_TIP = 0,
                    ALIS_KDV_KODU = 0,
                    BILESENMI = "",
                    BIRIM_AGIRLIK = 0,
                    DEPO_KODU = 0,
                    FIAT_BIRIMI = "",
                    FIKTIF_MAM = "",
                    GRUP_KODU = "",
                    ISLETME_KODU = item.LOGICALREF,
                    KDV_ORANI = item.RETURNVAT,
                    KILIT = "",
                    MAMULMU = "",
                    MUH_DETAYKODU = item.CARDTYPE,
                    MUSTERISIPKILIT = "",
                    OLCU_BR1 = item.UNITSETREF.ToString(),
                    ONAYNUM = 0,
                    ONAYTIPI = "",
                    PAY2 = 0,
                    PAYDA2 = 0,
                    PAYDA_1 = 0,
                    PAY_1 = 0,
                    PLANLANACAK = "",
                    SATICISIPKILIT = "",
                    SATINALMAKILIT = "",
                    SATISKILIT = "",
                    SAT_DOV_TIP = 0,
                    SBOMVARMI = "",
                    SIP_POLITIKASI = "",
                    STOK_ADI = item.NAME,
                    STOK_KODU = item.CODE,
                    SUBE_KODU = 0,
                    UPDATE_KODU = "",
                    YAPILANDIR = ""
                });
            }
            Master = new Array_Result_Master { SONUC = true, DATA = jsonCreator.Serialize(LRES), ACIKLAMA = "" };
            return jsonCreator.Serialize(Master);



        }

        [WebMethod(Description = "Yeni Stok Kartı (Metod No 6)")]
        [ScriptMethod(UseHttpGet = true)]
        public string YENISTOKKARTI(string WSPASS, string ConnectionObject, string OBJECTS)

        {
            List<Array_Result_Master> LOG = new List<Array_Result_Master>();
            Array_Result_Master Master = null;
           
                
                    List<Array_Stoklar_Request> CRG = jsonCreator.Deserialize<List<Array_Stoklar_Request>>(OBJECTS);
                    if (CRG != null)
                    {
                       foreach (var item in CRG)
                        {
                    Malzeme_Model M = new Malzeme_Model {
                 
                         NAME=item.STOK_ADI
                       , CARDTYPE=1
                       , CODE=item.STOK_KODU
                       , GUID=Guid.NewGuid().ToString()
                       , NAME2=""
                       , NAME3=""
                       , NAME4=""
                       , RETURNVAT=int.Parse(item.ALIS_KDV_KODU.ToString())
                       , SELLVAT=0
                       , SPECODE=""
                       , SPECODE2=""
                       , SPECODE3=""
                       , LOGICALREF=0
                       , SPECODE4=""
                       , SPECODE5=""
                       , STGRPCODE=""
                       , UNITSETREF=int.Parse(item.OLCU_BR1)
                   
                       
                       };

                    string ITEMTABLENAME = string.Format("LG_{0}_ITEMS", AppCommon.getConf().FirmaNo);


                    if (NQery.AdoFind<Malzeme_Model>(ITEMTABLENAME, string.Format("CODE='{0}' ", M.CODE)).Result)
                    {

                        Master = new Array_Result_Master { SONUC = false, DATA = "[]", ACIKLAMA = " Malzeme Daha Önce Tanımlanmış" };
                        break;
                    }
                    MasterResult<LG_001_ITEM> MCLS = LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_ITEM>(M, 0);
                    if (!MCLS.Result)
                    {

                        Master = new Array_Result_Master { SONUC = false, DATA = "[]", ACIKLAMA = MCLS.Message };
                        break;
                    }

                    LG_001_ITEM ITEM = MCLS.Data; //LogoGo3Data.Tools.AppCommon.CreateAndFillObject<LG_001_ITEM>(P, ITEMTABLENAME,0);
                    ITEM.CAPIBLOCK_CREADEDDATE = DateTime.Now;
                    ITEM.CAPIBLOCK_CREATEDBY = 1;
                    ITEM.CAPIBLOCK_CREATEDHOUR = short.Parse(DateTime.Now.Hour.ToString());
                    ITEM.CAPIBLOCK_CREATEDMIN = short.Parse(DateTime.Now.Minute.ToString());
                    ITEM.CAPIBLOCK_CREATEDSEC = short.Parse(DateTime.Now.Second.ToString());
                    ITEM.CAPIBLOCK_MODIFIEDBY = 1;
                    ITEM.CAPIBLOCK_MODIFIEDDATE = DateTime.Now;
                    ITEM.CAPIBLOCK_MODIFIEDHOUR = short.Parse(DateTime.Now.Hour.ToString());
                    ITEM.CAPIBLOCK_MODIFIEDMIN = short.Parse(DateTime.Now.Minute.ToString());
                    ITEM.CAPIBLOCK_MODIFIEDSEC = short.Parse(DateTime.Now.Second.ToString());
                      var MB= NExec.AdoInsert<LG_001_ITEM>(ITEM, ITEMTABLENAME);
                    if (!MB.Result)
                    {
                        Master = new Array_Result_Master { SONUC = false, DATA = "[]", ACIKLAMA = MB.Message };
                        break;
                    }
                    else {
                        Master = new Array_Result_Master { SONUC = true, DATA = "[]", ACIKLAMA = MB.Message };
                    }


                }

                    }
                    else
                    {

                        Master = new Array_Result_Master { SONUC = false, DATA = "[]", ACIKLAMA = " Gönderilen Veride Kayıt İşlemi İçin Cari Bulunamadı" };
                     
                    }



            return jsonCreator.Serialize(Master);

        }

        [WebMethod(Description = "Depo Listesi (Metod No 7)")]
        [ScriptMethod(UseHttpGet = true)]
        public string GETDEPOLAR(string WSPASS, string FILTERS)
        {
    
            Array_Result_Master Master = null;
         

                   List<Array_Depolar_Response> LRES = new List<Array_Depolar_Response>();
                     var DEPS= NQery.Find<L_CAPIWHOUSE>(x => x.FIRMNR == int.Parse(AppCommon.getConf().FirmaNo));
                    if (!DEPS.Result)
                       Master = new Array_Result_Master { SONUC = false, DATA = "[]", ACIKLAMA = "Firmaya Ait Depo Bulunamadı" };


                         foreach (var i in DEPS.Data)
                        {
                            LRES.Add(new Array_Depolar_Response
                            {
                                DEPO_ISMI = i.NAME
                                 ,
                                DEPO_KODU = i.NR.ToString()
                                 ,
                                S_YEDEK1 = i.COSTGRP.ToString()
                            });

                        }

                        Master = new Array_Result_Master { SONUC = true, DATA = jsonCreator.Serialize(LRES), ACIKLAMA = "" };
                   
                 




            return jsonCreator.Serialize(Master);

        }
        #endregion



        #region İrsaliyeler
        [WebMethod(Description = "Yeni Satış İrsaliyesi (Metod No 8)")]
        [ScriptMethod(UseHttpGet = true)]
        public string YENI_SATIS_IRSALIYESI(string WSPASS, string ConnectionObject, string USTBILGILER, string KALEMLER)
        {
            ExceptionHelper.addRequestLog($"[&&PRELOAD][&& MODEL1]{USTBILGILER}[&& MODEL2]{KALEMLER}");
            var res= AppCommon.IRSALIYEOLUSTUR(USTBILGILER, KALEMLER, (int)EIrsaliyeTip.toptan_Satis_Irsaliyesi, (int)EBelgeGurupKodlari.Satis_irsaliyeleri, (int)EIoKod.Cikis);
            ExceptionHelper.addRequestLog($"[&& MODEL1]{USTBILGILER}[&& SONUC]{res}");
            return res;
        }


        [WebMethod(Description = "Yeni Satıştan İade İrsaliyesi (Metod No 9)")]
        [ScriptMethod(UseHttpGet = true)]
        public string YENI_SATISTAN_IADE_IRSALIYESI(string WSPASS, string ConnectionObject, string USTBILGILER, string KALEMLER)
        {

            ExceptionHelper.addRequestLog($"[&&PRELOAD][&& MODEL1]{USTBILGILER}[&& MODEL2]{KALEMLER}");
            var res= AppCommon.IRSALIYEOLUSTUR(USTBILGILER, KALEMLER, (int)EIrsaliyeTip.toptan_Satis_Iade_Irsaliyesi, (int)EBelgeGurupKodlari.Satis_irsaliyeleri, (int)EIoKod.Giris);
            ExceptionHelper.addRequestLog($"[&& MODEL1]{USTBILGILER}[&& SONUC]{res}");
            return res;


        }


        [WebMethod(Description = "Yeni Alış İrsaliyesi (Metod No 10)")]
        [ScriptMethod(UseHttpGet = true)]
        public string YENI_ALIS_IRSALIYESI(string WSPASS, string ConnectionObject, string USTBILGILER, string KALEMLER)
        {
            ExceptionHelper.addRequestLog($"[&&PRELOAD][&& MODEL1]{USTBILGILER}[&& MODEL2]{KALEMLER}");
            var res =AppCommon.IRSALIYEOLUSTUR(USTBILGILER, KALEMLER, (int)EIrsaliyeTip.alim_Irsaliyesi, (int)EBelgeGurupKodlari.Alim_irsaliyeleri, (int)EIoKod.Giris);
            ExceptionHelper.addRequestLog($"[&& MODEL1]{USTBILGILER}[&& SONUC]{res}");
            return res;
        }

        [WebMethod(Description = "Yeni Alıştan İade İrsaliyesi (Metod No 10)")]
        [ScriptMethod(UseHttpGet = true)]
        public string YENI_ALISTAN_IADE_IRSALIYESI(string WSPASS, string ConnectionObject, string USTBILGILER, string KALEMLER)
        {
            ExceptionHelper.addRequestLog($"[&&PRELOAD][&& MODEL1]{USTBILGILER}[&& MODEL2]{KALEMLER}");
            var res= AppCommon.IRSALIYEOLUSTUR(USTBILGILER, KALEMLER, (int)EIrsaliyeTip.alim_Iade_Irsaliyesi, (int)EBelgeGurupKodlari.Alim_irsaliyeleri, (int)EIoKod.Cikis);
            ExceptionHelper.addRequestLog($"[&& MODEL1]{USTBILGILER}[&& SONUC]{res}");
            return res;

        }

        #endregion



        #region FAturalar
        [WebMethod(Description = "Yeni Satış Faturası (Metod No 12)")]
        [ScriptMethod(UseHttpGet = true)]
        public string YENI_SATIS_FATURASI(string WSPASS, string ConnectionObject, string USTBILGILER, string KALEMLER)
        {
            ExceptionHelper.addRequestLog($"[&&PRELOAD][&& MODEL1]{USTBILGILER}[&& MODEL2]{KALEMLER}");
            var res= AppCommon.FATURAOLUSTUR(USTBILGILER, KALEMLER, (int)EFaturaTip.ToptanSatis_Faturasi, (int)EBelgeGurupKodlari.Satis_Faturalari, (int)EIoKod.Cikis);
            ExceptionHelper.addRequestLog($"[&& MODEL1]{USTBILGILER}[&& SONUC]{res}");
            return res;

       


        }


        [WebMethod(Description = "Yeni Satıştan İade Faturası (Metod No 13)")]
        [ScriptMethod(UseHttpGet = true)]
        public string YENI_SATISTAN_IADE_FATURASI(string WSPASS, string ConnectionObject, string USTBILGILER, string KALEMLER)
        {
            ExceptionHelper.addRequestLog($"[&&PRELOAD][&& MODEL1]{USTBILGILER}[&& MODEL2]{KALEMLER}");
            var res =AppCommon.FATURAOLUSTUR(USTBILGILER, KALEMLER, (int)EFaturaTip.Toptan_Satis_Iade_Faturasi, (int)EBelgeGurupKodlari.Satis_Faturalari, (int)EIoKod.Giris);
            ExceptionHelper.addRequestLog($"[&& MODEL1]{USTBILGILER}[&& SONUC]{res}");
            return res;

        }

        [WebMethod(Description = "Yeni Alım Faturası (Metod No 14)")]
        [ScriptMethod(UseHttpGet = true)]
        public string YENI_ALIS_FATURASI(string WSPASS, string ConnectionObject, string USTBILGILER, string KALEMLER)
        {


            ExceptionHelper.addRequestLog($"[&&PRELOAD][&& MODEL1]{USTBILGILER}[&& MODEL2]{KALEMLER}");
            var res= AppCommon.FATURAOLUSTUR(USTBILGILER, KALEMLER, (int)EFaturaTip.SatinAlma_faturasi, (int)EBelgeGurupKodlari.Alim_Faturalari, (int)EIoKod.Giris);
            ExceptionHelper.addRequestLog($"[&& MODEL1]{USTBILGILER}[&& SONUC]{res}");
            return res;



        }

        [WebMethod(Description = "Yeni Alımdan İade Faturası (Metod No 15)")]
        [ScriptMethod(UseHttpGet = true)]
        public string YENI_ALISTAN_IADE_FATURASI(string WSPASS, string ConnectionObject, string USTBILGILER, string KALEMLER)
        {

            ExceptionHelper.addRequestLog($"[&&PRELOAD][&& MODEL1]{USTBILGILER}[&& MODEL2]{KALEMLER}");
            var res= AppCommon.FATURAOLUSTUR(USTBILGILER, KALEMLER, (int)EFaturaTip.SatinAlmaIade_Faturasi, (int)EBelgeGurupKodlari.Alim_Faturalari, (int)EIoKod.Cikis);
            ExceptionHelper.addRequestLog($"[&& MODEL1]{USTBILGILER}[&& SONUC]{res}");
            return res;


        }




      

        #endregion


        #region Malzeme fiş
       
         [WebMethod(Description = "Ambar Giriş Fişi (Metod No 16)")]
        public string YENI_AMBAR_GIRIS_FISI(string WSPASS, string ConnectionObject, string USTBILGILER, string KALEMLER)
        {
            ExceptionHelper.addRequestLog($"[&&PRELOAD][&& MODEL1]{USTBILGILER}[&& MODEL2]{KALEMLER}");
            ExceptionHelper.addRequestLog($"{USTBILGILER}[&&]{KALEMLER}");
            return AppCommon.MALZEMEFISOLUSTUR(USTBILGILER, KALEMLER, (int)EMalzemeFisTips.UretimdenGiris_Fisi, (int)EBelgeGurupKodlari.Malzeme_Fisleri, (int)EIoKod.Giris);


        }

        [WebMethod(Description = "Ambar Çıkış Fişi (Metod No 17)")]
        [ScriptMethod(UseHttpGet = true)]
        public string YENI_AMBAR_CIKIS_FISI(string WSPASS, string ConnectionObject, string USTBILGILER, string KALEMLER)
        {
            ExceptionHelper.addRequestLog($"[&&PRELOAD][&& MODEL1]{USTBILGILER}[&& MODEL2]{KALEMLER}");
            ExceptionHelper.addRequestLog($"{USTBILGILER}[&&]{KALEMLER}");
            return AppCommon.MALZEMEFISOLUSTUR(USTBILGILER, KALEMLER, (int)EMalzemeFisTips.Sarf_Fisi, (int)EBelgeGurupKodlari.Malzeme_Fisleri, (int)EIoKod.Cikis);


        }

        [WebMethod(Description = "Depolar Arası Transfer Fişi (Metod No 18)")]
        [ScriptMethod(UseHttpGet = true)]
        public string YENI_AMBAR_TRANSFER_FISI(string WSPASS, string ConnectionObject, string USTBILGILER, string KALEMLER)
        {
            ExceptionHelper.addRequestLog($"[&&PRELOAD][&& MODEL1]{USTBILGILER}[&& MODEL2]{KALEMLER}");
            ExceptionHelper.addRequestLog($"{USTBILGILER}[&&]{KALEMLER}");
            return AppCommon.MALZEMETRANSFERFISOLUSTUR(USTBILGILER, KALEMLER, (int)EMalzemeFisTips.Ambar_Fisi, (int)EBelgeGurupKodlari.Malzeme_Fisleri, (int)EIoKod.Transfer,"");

        }
        #endregion

      


        [WebMethod(Description = "Kasalar Listesi (Metod No 19)")]
        [ScriptMethod(UseHttpGet = true)]
        public string GET_KASALAR(string WSPASS, string FILTERS)
        {

           List<LG_001_KSCARD> LKS= NQery.AdoFind<LG_001_KSCARD>(string.Format("LG_{0}_KSCARD", AppCommon.getConf().FirmaNo)).Data;
            List<Array_Kasalar_Respons> LRES = new List<Array_Kasalar_Respons>();
            foreach (var item in LKS)
            {
                LRES.Add(new Array_Kasalar_Respons { KSMAS_KOD=item.CODE, KSMAS_NAME=item.NAME});
              

            }
         
            return jsonCreator.Serialize(LRES);

        }


        [WebMethod(Description = "Yeni Tahsilat İşlemi (Metod No 20)")]
        [ScriptMethod(UseHttpGet = true)]
        public string YENI_TAHSILAT(string WSPASS, string ConnectionObject, string TAHSILAT)
        {

            return null;
        }


        [WebMethod(Description = "Yeni Cari ve Muhasebe  Dekont (Metod No 21)")]
        [ScriptMethod(UseHttpGet = true)]
        public string YENIDEKONT(string WSPASS, string ConnectionObject, string Dekont)
        {

            return "";
        }

        [ScriptMethod(UseHttpGet = true)]
        [WebMethod(Description = "APPUPDATE")]
        public string APPUPDATE() {
          var REP=  SvnHelper.getRepoRevision();

            return REP.Revision.ToString();
        }

        [WebMethod(Description = "Muhasebe Referans Kodları Listesi (Metod No 22)")]
        [ScriptMethod(UseHttpGet = true)]
        public string GET_MUHASEBE_REFERANS_KODLARI(string WSPASS, string FILTERS) {

            Array_Result_Master Master = null;
            List<Array_Muhasebe_Referans_kodlari> LRES = new List<Array_Muhasebe_Referans_kodlari>();
            List<LG_001_EMUHACC> EMUHAC = NQery.Find<LG_001_EMUHACC>().Data;
            foreach (var item in EMUHAC)
            {
                LRES.Add(new Array_Muhasebe_Referans_kodlari
                {
                    GRUP_ISIM = item.DEFINITION_,
                    GRUP_KOD = item.CODE
                }); 

            }

            Master=new Array_Result_Master { ACIKLAMA="", DATA=LRES.toJsonSerialize<List<Array_Muhasebe_Referans_kodlari>>(), SONUC=LRES.Count>0?true:false };
            return Master.toJsonSerialize<Array_Result_Master>();


        }

        [WebMethod(Description = "Muhasebe Referans Kodları Listesi Kod Arama (Metod No 22)")]
        [ScriptMethod(UseHttpGet = true)]
        public string GET_MUHASEBE_REFERANS_KODLARI_KOD_ARAMA(string WSPASS, string FILTERSSEARCH) {

            Array_Result_Master Master = null;
            List<Array_Muhasebe_Referans_kodlari> LRES = new List<Array_Muhasebe_Referans_kodlari>();

            Array_Filter_Request_Serach FRQ = FILTERSSEARCH.toJsonDeSerialize<Array_Filter_Request_Serach>();

            List<LG_001_EMUHACC> EMUHAC = NQery.Find<LG_001_EMUHACC>(x=>x.CODE.ToString().Contains(FRQ.SEARCH)||x.DEFINITION_.Contains(FRQ.SEARCH)).Data;
            foreach (var item in EMUHAC)
            {
                LRES.Add(new Array_Muhasebe_Referans_kodlari
                {
                    GRUP_ISIM = item.DEFINITION_,
                    GRUP_KOD = item.CODE
                });

            }

            Master = new Array_Result_Master { ACIKLAMA = "", DATA = LRES.toJsonSerialize<List<Array_Muhasebe_Referans_kodlari>>(), SONUC = LRES.Count > 0 ? true : false };
            return Master.toJsonSerialize<Array_Result_Master>();
        }



    }
}
