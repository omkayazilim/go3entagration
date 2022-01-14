using LogoGo3Data;
using LogoGo3Data.Context;
using LogoGo3Data.DefineModel;
using LogoGo3Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using static LogoGo3Data.Extras.Utils;
using static LogoGo3Data.Model.DataModels;
using static LogoGo3Data.Soap_Models.Models;

namespace Go3Interration.Models
{
    public class AppCommon
    {




        public static Conf getConf()
        {


            return LogoGo3Data.GenerateProcess.getAppConf();



        }


        public static string IRSALIYEOLUSTUR(string USTBILGILER, string KALEMLER, short TRCODE, short GRPCODE, short IOCODE) {
            JavaScriptSerializer jsonCreator = JsonCreator();
            Array_Result_Master Master = null;

            Array_Fatura_Irsaliye_Ust_Min_Request UST = jsonCreator.Deserialize<Array_Fatura_Irsaliye_Ust_Min_Request>(USTBILGILER);
            if (UST == null) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "İrsaliye Başlığı Boş", DATA = "[]", SONUC = false });

            //UST.FIS_NO = UST.KOD2.Split('#')[0];

            List<Array_Fatura_Irsaliye_Kalemler_Min_Request> LKL = jsonCreator.Deserialize<List<Array_Fatura_Irsaliye_Kalemler_Min_Request>>(KALEMLER);
            if (LKL == null) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "İrsaliye Kalemleri Bulunamadı", DATA = "[]", SONUC = false });

            var IRS = NQery.AdoFind<LG_001_01_STFICHE>(string.Format("LG_{0}_{1}_STFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo), string.Format(" FICHENO='{0}'", UST.FIS_NO));
            if (IRS.Result) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Fiş No Daha önce Kaydedilmiş ", DATA = "[]", SONUC = false });


            var DEP = NQery.AdoFind<L_CAPIWHOUSE>("L_CAPIWHOUSE", string.Format(" NR={0}", LKL.First().DEPO_KODU));
            if (!DEP.Result) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Depo Bulunamadı", DATA = "[]", SONUC = false });


            //if (!string.IsNullOrEmpty(UST.PROJE_KODU))
            //{
            //    var MUH = NQery.AdoFind<LG_001_EMUHACC>(string.Format("LG_{0}_EMUHACC", AppCommon.getConf().FirmaNo), string.Format(" CODE='{0}'", UST.PROJE_KODU));
            //    if (!MUH.Result) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Muhasebe Hesabı Bulunamadı", DATA = "[]", SONUC = false });
            //}


            var cari = NQery.AdoFind<LG_001_CLCARD>(string.Format("LG_{0}_CLCARD", AppCommon.getConf().FirmaNo), string.Format(" CODE='{0}'", UST.CARI_KODU));
            if (!cari.Result) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Cari kaydı Bulunamadı", DATA = "[]", SONUC = false });

            List<Line_Ks> LNKS = new List<Line_Ks>();
            foreach (var item in LKL)
            {
                var MLZ = NQery.AdoFind<LG_001_ITEM>(string.Format("LG_{0}_ITEMS", AppCommon.getConf().FirmaNo), string.Format(" CODE='{0}'", item.STOK_KODU)) ;
                if(!MLZ.Result)
                    return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Malzeme Bulunamadı", DATA = "[]", SONUC = false });
                var UNL = NQery.AdoFind<LG_001_UNITSETL>(string.Format("LG_{0}_UNITSETL", AppCommon.getConf().FirmaNo), string.Format(" UNITSETREF={0} and LINENR=1", MLZ.Data[0].UNITSETREF, 1));
                if (!UNL.Result)
                    return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Birim Seti Bulunamadı", DATA = "[]", SONUC = false });

                var Accountrefs = NQery.RawQuery<LG_001_02_STLINE>($"select top 1  ACCOUNTREF,VATACCREF from LG_001_02_STLINE where TRCODE={TRCODE} and STOCKREF={MLZ.Data[0].LOGICALREF}  order by DATE_ desc");
                if (!Accountrefs.Result)
                    return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Muhasebe Referans Kodları Bulunamadı", DATA = "[]", SONUC = false });




                Line_Ks LK = new Line_Ks
                {
                    AMOUNT = item.STHAR_GCMIK,
                    LINEEXP = "",

                    PRICE = item.STHAR_BF,
                    VAT = int.Parse(MLZ.Data[0].SELLPRVAT.ToString()),
                    SPECODE = "",
                    STOCKREF = MLZ.Data[0].LOGICALREF,


                    LINETYPE = item.ODEGUN,
                    UOMREF = UNL.Data[0].LOGICALREF,
                    UINFO1 = 1,
                    UINFO2 = 1,
                    USREF = int.Parse(UNL.Data[0].UNITSETREF.ToString()),


                    VATAMNT = 0,
                    VATMATRAH = 0,
                    TOTAL = 0,
                    LINENET = 0,
                    IOCODE = IOCODE,
                    TRCODE = TRCODE,
                    ACCOUNTREF = Accountrefs.Data[0].ACCOUNTREF.nullableToRequired<int>(),
                    VATACCREF= Accountrefs.Data[0].VATACCREF.nullableToRequired<int>(),



                };

                AppCommon.CalculeteVatIrsKalem(ref LK,item.ODEGUN);
                LNKS.Add(LK);
            }


            Belge_Post_Model BPM = new Belge_Post_Model
            {
                Belge = new Belge_Ks
                {
                    ACCOUNTREF = 0,//MUH.LOGICALREF,
                    CLIENTREF = cari.Data[0].LOGICALREF,
                    AFFECTRISK = 1,
                    DATE_ = DateTime.Parse(UST.TARIH),
                    DOCODE = "",
                    EINVOICE = short.Parse(UST.TIPI.ToString()),
                    FICHENO = UST.FIS_NO,
                    GENEXP1 = "",
                    GENEXP2 = "",
                    GENEXP3 = "",
                    GENEXP4 = "",
                    GENEXP5 = "",
                    GENEXP6 = "",
                    GROSSTOTAL = LNKS.Sum(x => x.TOTAL),
                    GRPCODE = GRPCODE,
                    NETTOTAL = LNKS.Sum(x => x.LINENET),
                    SOURCEINDEX = int.Parse(DEP.Data[0].NR.ToString()),
                    IRSFICHENO = "",
                    SOURCECOSTGRP = int.Parse(DEP.Data[0].COSTGRP.ToString()),
                    SPECODE = UST.KOD1,
                    TOTALDISCOUNTED = 0,
                    TOTALVAT = LNKS.Sum(x => x.VATAMNT),
                    TRACKNR = "",
                    TRCODE = TRCODE,
                    TRNET = LNKS.Sum(x => x.LINENET),
                    FATID = 0,
                    VAT = 0,
                    IOCODE = IOCODE
                },
                Line = LNKS,
                HEADTABLE = string.Format("LG_{0}_{1}_STFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo),
                LISTTABLE = string.Format("LG_{0}_{1}_STLINE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo)
            };

            MasterResult<NTUPLE> ADG = LogoGo3Data.Tools.AppCommon.YeniIrsaliye(BPM);
            Master = new Array_Result_Master { ACIKLAMA = ADG.Message, DATA = "", SONUC = ADG.Result };


            return jsonCreator.Serialize(Master);

        }
        public static double unVatPrice(double price,int Vat)
        {

            double net = 0;

            switch (Vat)
            {
                case 8: { net = price / 1.08; break; }
                case 18: { net = price / 1.18; break; }
                case 1: { net = price / 1.01; break; }
              
            }
            return net ;
        }

        public static string FATURAOLUSTUR(string USTBILGILER, string KALEMLER, short TRCODE, short GRPCODE, short IOCODE) {
            Array_Result_Master Master = null;
            JavaScriptSerializer jsonCreator = JsonCreator();
            Array_Fatura_Irsaliye_Ust_Min_Request UST = jsonCreator.Deserialize<Array_Fatura_Irsaliye_Ust_Min_Request>(USTBILGILER);
            if (UST == null) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "İrsaliye Başlığı Boş", DATA = "[]", SONUC = false });
            //UST.FIS_NO = UST.KOD2.Split('#')[0];
            List<Array_Fatura_Irsaliye_Kalemler_Min_Request> LKL = jsonCreator.Deserialize<List<Array_Fatura_Irsaliye_Kalemler_Min_Request>>(KALEMLER);
            if (LKL == null) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "İrsaliye Kalemleri Bulunamadı", DATA = "[]", SONUC = false });

            var IRS = NQery.AdoFind<LG_001_01_INVOICE>(string.Format("LG_{0}_{1}_INVOICE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo), string.Format(" FICHENO='{0}'", UST.FIS_NO));
            if (IRS.Result) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Fiş No Daha önce Kaydedilmiş ", DATA = "[]", SONUC = false });


            var DEP = NQery.AdoFind<L_CAPIWHOUSE>("L_CAPIWHOUSE", string.Format(" NR={0}", LKL.First().DEPO_KODU)).Data.First();
            if (DEP == null) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Depo Bulunamadı", DATA = "[]", SONUC = false });

            var cari = NQery.AdoFind<LG_001_CLCARD>(string.Format("LG_{0}_CLCARD", AppCommon.getConf().FirmaNo), string.Format(" CODE='{0}'", UST.CARI_KODU)).Data.First();
            if (cari == null) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Cari kaydı Bulunamadı", DATA = "[]", SONUC = false });

            List<Line_Ks> LNKS = new List<Line_Ks>();
            int K = 0;
            foreach (var item in LKL)
            {
                K++;
                var MLZMS = NQery.AdoFind<LG_001_ITEM>(string.Format("LG_{0}_ITEMS", AppCommon.getConf().FirmaNo), string.Format(" CODE='{0}'", item.STOK_KODU));
                if (!MLZMS.Result)
                    return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Malzeme  kaydı Bulunamadı", DATA = item.toJsonSerialize(), SONUC = false });
                var MLZ = MLZMS.Data.First();
                
                var UNL = NQery.AdoFind<LG_001_UNITSETL>(string.Format("LG_{0}_UNITSETL", AppCommon.getConf().FirmaNo), string.Format(" UNITSETREF={0} and LINENR=1", MLZ.UNITSETREF, 1)).Data.First();

         
                var Accountrefs = NQery.RawQuery<LV_001_02_STLINE_GRP>($"select top 1  ACCOUNTREF,VATACCREF from LV_{AppCommon.getConf().FirmaNo}_{AppCommon.getConf().DonemNo}_STLINE_GRP where TRCODE={TRCODE} and VAT={MLZ.VAT} and STGRPCODE={MLZ.STGRPCODE}   order by DATE_ desc");

                var ACountCode = NQery.RawQuery<LV_001_MUHESEBE_STOK_GRP_KODLARI>($"select top 1  ACOOUNTREF from LV_{AppCommon.getConf().FirmaNo}_MUHESEBE_STOK_GRP_KODLARI where TRCODE={TRCODE} and VAT={MLZ.VAT} and GRPCODE={MLZ.SPECODE} ");
                if (!ACountCode.Result)
                {
                    ACountCode = NQery.RawQuery<LV_001_MUHESEBE_STOK_GRP_KODLARI>($"select top 1  ACOOUNTREF from LV_{AppCommon.getConf().FirmaNo}_MUHESEBE_STOK_GRP_KODLARI where TRCODE={TRCODE} and VAT={MLZ.VAT} and GRPCODE={MLZ.STGRPCODE} ");
                }
                var VatACountCode=NQery.RawQuery<LV_001_MUHESEBE_STOK_GRP_KODLARI>($"select top 1  ACOOUNTREF from LV_{AppCommon.getConf().FirmaNo}_MUHESEBE_KDV_STOK_GRP_KODLARI where TRCODE={TRCODE} and VAT={MLZ.VAT} and GRPCODE={MLZ.STGRPCODE} ");
            


                if (!Accountrefs.Result)
                    Accountrefs.Data = new List<LV_001_02_STLINE_GRP> {
                    new LV_001_02_STLINE_GRP{ ACCOUNTREF=0, VATACCREF=0  }
                    };
                if (!ACountCode.Result)
                    ACountCode.Data = new List<LV_001_MUHESEBE_STOK_GRP_KODLARI> {
                    new LV_001_MUHESEBE_STOK_GRP_KODLARI{ ACOOUNTREF=0  }
                    };  
                
                if (!VatACountCode.Result)
                    VatACountCode.Data = new List<LV_001_MUHESEBE_STOK_GRP_KODLARI> {
                    new LV_001_MUHESEBE_STOK_GRP_KODLARI{ ACOOUNTREF=0  }
                    };

                Line_Ks LK = new Line_Ks
                {
                    AMOUNT = item.STHAR_GCMIK,
                    LINEEXP = "",

                    //PRICE = item.STHAR_BF,
                    PRICE = TRCODE == (int)EFaturaTip.SatinAlma_faturasi ? unVatPrice(item.STHAR_BF, int.Parse(MLZ.SELLVAT.ToString())) : item.STHAR_BF,
                    VAT = int.Parse(MLZ.SELLVAT.ToString()),
                    SPECODE = "",
                    STOCKREF = MLZ.LOGICALREF,


                    LINETYPE = 0,
                    UOMREF = UNL.LOGICALREF,
                    UINFO1 = 1,
                    UINFO2 = 1,
                    USREF = int.Parse(UNL.UNITSETREF.ToString()),


                    VATAMNT = 0,
                    VATMATRAH = 0,
                    TOTAL = 0,
                    LINENET = 0,
                    IOCODE = IOCODE,
                    TRCODE = TRCODE,
                    ACCOUNTREF = ACountCode.Data.FirstOrDefault().ACOOUNTREF.nullableToRequired<int>(),
                    VATACCREF = VatACountCode.Data.FirstOrDefault().ACOOUNTREF.nullableToRequired<int>(),
                    KDVDahilmi = UST.KDV_DAHILMI ? KdvStatu.Dahil : KdvStatu.Haric,
                    DISTCOST = 0,
                    DISCPER = 0,
                    DISTDISC = 0,
                    LINENO = K,
                   
                 };

                if (item.ODEGUN > 0)
                {
                    K++;
                    Line_Ks ISK = new Line_Ks
                    {

                       
                        LINETYPE = 2,
                        DISCPER = item.ODEGUN,
                        IOCODE=IOCODE,
                        TRCODE=TRCODE,
                        LINENO=K
                       
                        
                       };
                    AppCommon.CalculateDiscount(ref LK, ISK);
                    ISK.TOTAL = LK.DISTCOST;
                    LNKS.Add(ISK);
                }
               

                AppCommon.CalculeteVatIrsKalem(ref LK,item.ODEGUN);
               LNKS.Add(LK);
            }


            Belge_Post_Model BPM = new Belge_Post_Model
            {
                Belge = new Belge_Ks
                {
                    ACCOUNTREF = 0,//MUH.LOGICALREF,
                    CLIENTREF = cari.LOGICALREF,
                    AFFECTRISK = 1,
                    DATE_ = DateTime.Parse(UST.TARIH),


                    DOCODE = "",
                    EINVOICE = short.Parse(UST.TIPI.ToString()),
                    FICHENO = UST.FIS_NO,
                    GENEXP1 = "",
                    GENEXP2 = "",
                    GENEXP3 = "",
                    GENEXP4 = "",
                    GENEXP5 = "",
                    GENEXP6 = "",


                    GROSSTOTAL = LNKS.Where(x => x.LINETYPE == 0).Sum(x => x.TOTAL) + LNKS.Sum(x => x.VATAMNT),
                    GRPCODE = GRPCODE,
                    NETTOTAL = LNKS.Sum(x => x.LINENET),
                    SOURCEINDEX = int.Parse(DEP.NR.ToString()),
                    IRSFICHENO = UST.KOD2.Split('#')[0],
                    SOURCECOSTGRP = int.Parse(DEP.COSTGRP.ToString()),
                    SPECODE = UST.KOD1,
                    TOTALDISCOUNTED = LNKS.Where(x => x.LINETYPE == 2).Sum(x => x.TOTAL),
                    TOTALVAT = LNKS.Sum(x => x.VATAMNT),
                    TRACKNR = "",
                    TRCODE = TRCODE,
                    TRNET = LNKS.Sum(x => x.LINENET),
                    FATID = 0,
                    VAT = 0,
                    IOCODE = IOCODE,
                    KDV_DAHILMI = UST.KDV_DAHILMI ? 1 : 0,
                    IRSDATE = UST.KOD2.Split('#')[1],



                },
                Line = LNKS,
                HEADTABLE = string.Format("LG_{0}_{1}_INVOICE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo),
                LISTTABLE = string.Format("LG_{0}_{1}_STLINE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo)
            };

            MasterResult<NTUPLE> ADG = LogoGo3Data.Tools.AppCommon.YeniFatura(BPM);
            if (!ADG.Result)
                return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = ADG.Message, DATA = "[]", SONUC = false });

            var belge = NQery.AdoFind<LG_001_01_STFICHE>(BPM.HEADTABLE, string.Format(" FICHENO='{0}'", BPM.Belge.FICHENO));
            if (!belge.Result)
                return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = belge.Message, DATA = "[]", SONUC = false });

            BPM.Belge.BILLED = 1;
            BPM.Belge.INVNO = BPM.Belge.FICHENO;
            BPM.Belge.INVOICEREF = belge.Data.First().LOGICALREF;
            BPM.Line.ForEach(x => { x.INVOICEREF = BPM.Belge.INVOICEREF; x.INVOICELNNO++; x.IOCODE = BPM.Belge.IOCODE; x.TRCODE = BPM.Belge.TRCODE;  });

            BPM.HEADTABLE = string.Format("LG_{0}_{1}_STFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);

            MasterResult<NTUPLE> YNIRS = LogoGo3Data.Tools.AppCommon.YeniIrsaliye(BPM);


            BPM.HEADTABLE = string.Format("LG_{0}_{1}_INVOICE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
            if (!YNIRS.Result)
            {
                NExec.AdoDelete<LG_001_01_STFICHE>(BPM.HEADTABLE, string.Format(" where LOGICALREF={0}", BPM.Belge.INVOICEREF));
                BPM.HEADTABLE = string.Format("LG_{0}_{1}_STFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo);
                NExec.AdoDelete<LG_001_01_STFICHE>(BPM.HEADTABLE, string.Format(" where INVOICEREF={0}", BPM.Belge.INVOICEREF));

                NExec.AdoDelete<LG_001_01_STLINE>(BPM.LISTTABLE, string.Format(" where INVOICEREF={0}", BPM.Belge.INVOICEREF));

                return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Fatura Girişi Esnasında Hatalar oluştu İşlemler Geri Alındı", DATA = "[]", SONUC = false });
            }




            Master = new Array_Result_Master { ACIKLAMA = ADG.Message, DATA = jsonCreator.Serialize(ADG.Data), SONUC = ADG.Result };




            return jsonCreator.Serialize(Master);

        }


        public static string MALZEMEFISOLUSTUR(string USTBILGILER, string KALEMLER, short TRCODE, short GRPCODE, short IOCODE)
        {
            JavaScriptSerializer jsonCreator = JsonCreator();
            Array_Result_Master Master = null;

            Array_Fatura_Irsaliye_Ust_Min_Request UST = jsonCreator.Deserialize<Array_Fatura_Irsaliye_Ust_Min_Request>(USTBILGILER);
            if (UST == null) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Fiş Başlığı Boş", DATA = "[]", SONUC = false });
            UST.FIS_NO = UST.KOD2.Split('#')[0];
            List<Array_Fatura_Irsaliye_Kalemler_Min_Request> LKL = jsonCreator.Deserialize<List<Array_Fatura_Irsaliye_Kalemler_Min_Request>>(KALEMLER);
            if (LKL == null) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Fiş Kalemleri Bulunamadı", DATA = "[]", SONUC = false });

            var IRS = NQery.AdoFind<LG_001_01_STFICHE>(string.Format("LG_{0}_{1}_STFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo), string.Format(" FICHENO='{0}'", UST.FIS_NO));
            if (IRS.Result) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Fiş No Daha önce Kaydedilmiş ", DATA = "[]", SONUC = false });


            var DEP = NQery.AdoFind<L_CAPIWHOUSE>("L_CAPIWHOUSE", string.Format(" NR={0}", LKL.First().DEPO_KODU)).Data.First();
            if (DEP == null) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Depo Bulunamadı", DATA = "[]", SONUC = false });


            //var MUH = NQery.AdoFind<LG_001_EMUHACC>(string.Format("LG_{0}_EMUHACC", AppCommon.getConf().FirmaNo), string.Format(" CODE='{0}'", UST.PROJE_KODU)).Data.First();

            //if (MUH == null) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Muhasebe Hesabı Bulunamadı", DATA = "[]", SONUC = false });


            //var cari = NQery.AdoFind<LG_001_CLCARD>(string.Format("LG_{0}_CLCARD", AppCommon.getConf().FirmaNo), string.Format(" CODE='{0}'", UST.CARI_KODU)).Data.First();
            //if (cari == null) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Cari kaydı Bulunamadı", DATA = "[]", SONUC = false });

            List<Line_Ks> LNKS = new List<Line_Ks>();
            foreach (var item in LKL)
            {
                var MLZ = NQery.AdoFind<LG_001_ITEM>(string.Format("LG_{0}_ITEMS", AppCommon.getConf().FirmaNo), string.Format(" CODE='{0}'", item.STOK_KODU)).Data.First();
                var UNL = NQery.AdoFind<LG_001_UNITSETL>(string.Format("LG_{0}_UNITSETL", AppCommon.getConf().FirmaNo), string.Format(" UNITSETREF={0} and LINENR=1", MLZ.UNITSETREF, 1)).Data.First();

                Line_Ks LK = new Line_Ks
                {
                    AMOUNT = item.STHAR_GCMIK,
                    LINEEXP = "",

                    PRICE = item.STHAR_BF,
                    VAT = int.Parse(MLZ.SELLVAT.ToString()),
                    SPECODE = "",
                    STOCKREF = MLZ.LOGICALREF,


                    LINETYPE = item.ODEGUN,
                    UOMREF = UNL.LOGICALREF,
                    UINFO1 = 1,
                    UINFO2 = 1,
                    USREF = int.Parse(UNL.UNITSETREF.ToString()),


                    VATAMNT = 0,
                    VATMATRAH = 0,
                    TOTAL = 0,
                    LINENET = 0,
                    IOCODE = IOCODE,
                    TRCODE = TRCODE

                };
                AppCommon.CalculeteVatIrsKalem(ref LK, item.ODEGUN);
                LNKS.Add(LK);
            }


            Belge_Post_Model BPM = new Belge_Post_Model
            {
                Belge = new Belge_Ks
                {
                    ACCOUNTREF = 0,
                    CLIENTREF = 0,
                    AFFECTRISK = 1,
                    DATE_ = DateTime.Parse(UST.TARIH),
                    DOCODE = "",
                    EINVOICE = short.Parse(UST.TIPI.ToString()),
                    FICHENO = UST.FIS_NO,
                    GENEXP1 = "",
                    GENEXP2 = "",
                    GENEXP3 = "",
                    GENEXP4 = "",
                    GENEXP5 = "",
                    GENEXP6 = "",
                    GROSSTOTAL = LNKS.Sum(x => x.TOTAL),
                    GRPCODE = GRPCODE,
                    NETTOTAL = LNKS.Sum(x => x.LINENET),
                    SOURCEINDEX = int.Parse(DEP.NR.ToString()),
                    IRSFICHENO = "",
                    SOURCECOSTGRP = int.Parse(DEP.COSTGRP.ToString()),
                    SPECODE = UST.KOD1,
                    TOTALDISCOUNTED = 0,
                    TOTALVAT = LNKS.Sum(x => x.VATAMNT),
                    TRACKNR = "",
                    TRCODE = TRCODE,
                    TRNET = LNKS.Sum(x => x.LINENET),
                    FATID = 0,
                    VAT = LNKS.First().VAT,
                    IOCODE = IOCODE
                },
                Line = LNKS,
                HEADTABLE = string.Format("LG_{0}_{1}_STFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo),
                LISTTABLE = string.Format("LG_{0}_{1}_STLINE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo)
            };

            MasterResult<NTUPLE> ADG = LogoGo3Data.Tools.AppCommon.YeniIrsaliye(BPM);
            Master = new Array_Result_Master { ACIKLAMA = ADG.Message, DATA = jsonCreator.Serialize(ADG.Data), SONUC = ADG.Result };


            return jsonCreator.Serialize(Master);

        }


        public static string MALZEMETRANSFERFISOLUSTUR(string USTBILGILER, string KALEMLER, short TRCODE, short GRPCODE, short IOCODE, string girdepokod)
        {
            JavaScriptSerializer jsonCreator = JsonCreator();
            Array_Result_Master Master = null;

            Array_Fatura_Irsaliye_Ust_Min_Request UST = jsonCreator.Deserialize<Array_Fatura_Irsaliye_Ust_Min_Request>(USTBILGILER);
            if (UST == null) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Fiş Başlığı Boş", DATA = "[]", SONUC = false });
            UST.FIS_NO = UST.KOD2.Split('#')[0];
            List<Array_Fatura_Irsaliye_Kalemler_Min_Request> LKL = jsonCreator.Deserialize<List<Array_Fatura_Irsaliye_Kalemler_Min_Request>>(KALEMLER);
            if (LKL == null) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Fiş Kalemleri Bulunamadı", DATA = "[]", SONUC = false });

            var IRS = NQery.AdoFind<LG_001_01_STFICHE>(string.Format("LG_{0}_{1}_STFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo), string.Format(" FICHENO='{0}'", UST.FIS_NO));
            if (IRS.Result) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Fiş No Daha önce Kaydedilmiş ", DATA = "[]", SONUC = false });


            var CIKDEP = NQery.AdoFind<L_CAPIWHOUSE>("L_CAPIWHOUSE", string.Format(" NR={0}", LKL.First().DEPO_KODU)).Data.First();
            if (CIKDEP == null) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Çıkış Depo Bulunamadı", DATA = "[]", SONUC = false });

            var GIRDEP = NQery.AdoFind<L_CAPIWHOUSE>("L_CAPIWHOUSE", string.Format(" NR={0}", LKL.First().GIR_DEPO_KODU)).Data.First();
            if (GIRDEP == null) return jsonCreator.Serialize(new Array_Result_Master { ACIKLAMA = "Giriş Depo Bulunamadı", DATA = "[]", SONUC = false });


            List<Line_Ks> LNKS = new List<Line_Ks>();
            foreach (var item in LKL)
            {
                var MLZ = NQery.AdoFind<LG_001_ITEM>(string.Format("LG_{0}_ITEMS", AppCommon.getConf().FirmaNo), string.Format(" CODE='{0}'", item.STOK_KODU)).Data.First();
                var UNL = NQery.AdoFind<LG_001_UNITSETL>(string.Format("LG_{0}_UNITSETL", AppCommon.getConf().FirmaNo), string.Format(" UNITSETREF={0} and LINENR=1", MLZ.UNITSETREF, 1)).Data.First();

                Line_Ks LK = new Line_Ks
                {
                    AMOUNT = item.STHAR_GCMIK,
                    LINEEXP = "",

                    PRICE = item.STHAR_BF,
                    VAT = int.Parse(MLZ.SELLVAT.ToString()),
                    SPECODE = "",
                    STOCKREF = MLZ.LOGICALREF,
                    LINETYPE = item.ODEGUN,
                    UOMREF = UNL.LOGICALREF,
                    UINFO1 = 1,
                    UINFO2 = 1,
                    USREF = int.Parse(UNL.UNITSETREF.ToString()),


                    VATAMNT = 0,
                    VATMATRAH = 0,
                    TOTAL = 0,
                    LINENET = 0,
                    IOCODE = (int)EIoKod.Transfer,
                    TRCODE = TRCODE,




                };
                AppCommon.CalculeteVatIrsKalem(ref LK,item.ODEGUN);




                LNKS.Add(LK);
            }


            Belge_Post_Model BPM = new Belge_Post_Model
            {
                Belge = new Belge_Ks
                {
                    ACCOUNTREF = 0,
                    CLIENTREF = 0,
                    AFFECTRISK = 1,
                    DATE_ = DateTime.Parse(UST.TARIH),
                    DOCODE = "",
                    EINVOICE = short.Parse(UST.TIPI.ToString()),
                    FICHENO = UST.FIS_NO,
                    GENEXP1 = "",
                    GENEXP2 = "",
                    GENEXP3 = "",
                    GENEXP4 = "",
                    GENEXP5 = "",
                    GENEXP6 = "",
                    GROSSTOTAL = LNKS.Sum(x => x.TOTAL),
                    GRPCODE = GRPCODE,
                    NETTOTAL = LNKS.Sum(x => x.LINENET),
                    SOURCEINDEX = int.Parse(CIKDEP.NR.ToString()),
                    IRSFICHENO = "",
                    SOURCECOSTGRP = int.Parse(CIKDEP.COSTGRP.ToString()),
                    DESTINDEX = int.Parse(GIRDEP.NR.ToString()),
                    DESTCOSTGRP = int.Parse(GIRDEP.COSTGRP.ToString()),

                    SPECODE = UST.KOD1,
                    TOTALDISCOUNTED = 0,
                    TOTALVAT = LNKS.Sum(x => x.VATAMNT),
                    TRACKNR = "",
                    TRCODE = TRCODE,
                    TRNET = LNKS.Sum(x => x.LINENET),
                    FATID = 0,
                    VAT = LNKS.First().VAT,
                    IOCODE = IOCODE
                },
                Line = LNKS,
                HEADTABLE = string.Format("LG_{0}_{1}_STFICHE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo),
                LISTTABLE = string.Format("LG_{0}_{1}_STLINE", AppCommon.getConf().FirmaNo, AppCommon.getConf().DonemNo)
            };

            MasterResult<NTUPLE> ADG = LogoGo3Data.Tools.AppCommon.YeniTransfer(BPM);


            Master = new Array_Result_Master { ACIKLAMA = ADG.Message, DATA = jsonCreator.Serialize(ADG.Data), SONUC = ADG.Result };


            return jsonCreator.Serialize(Master);

        }


        public static JavaScriptSerializer JsonCreator() {
            JavaScriptSerializer jsonCreator = new JavaScriptSerializer();
            jsonCreator.MaxJsonLength = int.MaxValue;
            return jsonCreator;
        }

        public static void setInvoiceType(ref Belge_Model Myrec) {
            int TR = Myrec.belge.TRCODE;
            long INVREC = Myrec.belge.LOGICALREF;
            switch (TR)
            {


                case (int)EFaturaTip.SatinAlma_faturasi: { Myrec.belge.GRPCODE = (int)EBelgeGurupKodlari.Alim_Faturalari; break; }
                case (int)EFaturaTip.AlinanHizmet_Faturasi: { Myrec.belge.GRPCODE = (int)EBelgeGurupKodlari.Alim_Faturalari; break; }
                case (int)EFaturaTip.AlinanProforma_Faturasi: { Myrec.belge.GRPCODE = (int)EBelgeGurupKodlari.Alim_Faturalari; break; }
                case (int)EFaturaTip.SatinAlmaIade_Faturasi: { Myrec.belge.GRPCODE = (int)EBelgeGurupKodlari.Alim_Faturalari; break; }
                case (int)EFaturaTip.SatinAlmaFiyatFarki_Faturasi: { Myrec.belge.GRPCODE = (int)EBelgeGurupKodlari.Alim_Faturalari; break; }
                case (int)EFaturaTip.Mustahsil_Makbuzu: { Myrec.belge.GRPCODE = (int)EBelgeGurupKodlari.Alim_Faturalari; break; }



                case (int)EFaturaTip.PerakendeSatis_Faturasi: { Myrec.belge.GRPCODE = (int)EBelgeGurupKodlari.Satis_Faturalari; break; }
                case (int)EFaturaTip.ToptanSatis_Faturasi: { Myrec.belge.GRPCODE = (int)EBelgeGurupKodlari.Satis_Faturalari; break; }
                case (int)EFaturaTip.SatisFiyatFarki_Faturasi: { Myrec.belge.GRPCODE = (int)EBelgeGurupKodlari.Satis_Faturalari; break; }
                case (int)EFaturaTip.VerilenHizmet_Faturasi: { Myrec.belge.GRPCODE = (int)EBelgeGurupKodlari.Satis_Faturalari; break; }
                case (int)EFaturaTip.VerilenProforma_Faturasi: { Myrec.belge.GRPCODE = (int)EBelgeGurupKodlari.Satis_Faturalari; break; }

            }

        }

        public static void setIrsaliyeType(ref Belge_Model Myrec)
        {
            int TR = Myrec.belge.TRCODE;
            long INVREC = Myrec.belge.LOGICALREF;
            switch (TR)
            {


                case (int)EIrsaliyeTip.alim_Iade_Irsaliyesi: { Myrec.belge.GRPCODE = (int)EBelgeGurupKodlari.Alim_irsaliyeleri; break; }
                case (int)EIrsaliyeTip.alim_Irsaliyesi: { Myrec.belge.GRPCODE = (int)EBelgeGurupKodlari.Alim_irsaliyeleri; break; }
                case (int)EIrsaliyeTip.toptan_Satis_Iade_Irsaliyesi: { Myrec.belge.GRPCODE = (int)EBelgeGurupKodlari.Satis_irsaliyeleri; break; }
                case (int)EIrsaliyeTip.toptan_Satis_Irsaliyesi: { Myrec.belge.GRPCODE = (int)EBelgeGurupKodlari.Satis_irsaliyeleri; break; }

            }

        }


        public static void CalculeteVatIrsKalem(ref Line_Ks KS,double ISK)
        {

            switch (KS.TRCODE)
            {
                case (int)EFaturaTip.SatinAlma_faturasi: {
                       KS.TOTAL = KS.PRICE * KS.AMOUNT;
                        switch (KS.VAT)
                        {
                            case 8:
                                {
                                    KS.TOTAL = (KS.AMOUNT * KS.PRICE); //-(KS.TOTAL - (ISK > 0 ? KS.DISTDISC : 0)); 

                                    double Total = KS.TOTAL * 1.08;

                                    KS.VATMATRAH = KS.TOTAL -(ISK>0?KS.DISTCOST:0);
                                    KS.VATAMNT = Total - KS.VATMATRAH;
                                    KS.LISTETOPLAM = Total;
                                    KS.LINENET = Total;
                                    

                                    break;
                                }

                            case 18:
                                {
                                    KS.TOTAL = (KS.AMOUNT * KS.PRICE); //- (KS.TOTAL - (ISK > 0 ? KS.DISTDISC : 0));
                                    double Total = KS.TOTAL * 1.18;

                                    KS.VATMATRAH = KS.TOTAL - (ISK > 0 ? KS.DISTCOST : 0);
                                    KS.VATAMNT = Total - KS.VATMATRAH;
                                    KS.LISTETOPLAM = Total;
                                    KS.LINENET = Total;


                                    break;
                                }

                            case 1:
                                {
                                    KS.TOTAL = (KS.AMOUNT * KS.PRICE);// - (KS.TOTAL - (ISK > 0 ? KS.DISTDISC : 0));
                                    double Total = KS.TOTAL * 1.01;

                                    KS.VATMATRAH = KS.TOTAL - (ISK > 0 ? KS.DISTCOST : 0);
                                    KS.VATAMNT = Total - KS.VATMATRAH;
                                    KS.LISTETOPLAM = Total;
                                    KS.LINENET = Total;


                                    break;
                                }

                           
                        }


                        break;
                    
                    }


                default:
                    KS.TOTAL = KS.PRICE * KS.AMOUNT;
                    switch (KS.VAT)
                    {
                        case 8:
                            {
                                KS.LINENET = (KS.TOTAL-(ISK>0?KS.DISTDISC:0));
                                double Total = KS.LINENET * 1.08;
                                KS.VATMATRAH = KS.LINENET;
                                KS.VATAMNT = Total - KS.VATMATRAH;
                                KS.LISTETOPLAM = Total;

                                break;
                            }

                        case 18:
                            {
                                KS.LINENET = (KS.TOTAL - (ISK > 0 ? KS.DISTDISC : 0));
                                double TOTAL = KS.LINENET * 1.18;
                                KS.VATMATRAH = KS.LINENET;
                                KS.VATAMNT = TOTAL - KS.VATMATRAH;
                                KS.LISTETOPLAM = TOTAL;
                                break;
                            }

                        case 1:
                            {
                                KS.LINENET = (KS.TOTAL - (ISK > 0 ? KS.DISTDISC : 0));
                                double TOTAL = KS.LINENET * 1.01;
                                KS.VATMATRAH = KS.LINENET;
                                KS.VATAMNT = TOTAL - KS.VATMATRAH;
                                KS.LISTETOPLAM = TOTAL;
                                break;
                            }

                        default:
                            {
                                KS.LINENET = (KS.TOTAL - (ISK > 0 ? KS.DISTDISC : 0));
                                KS.VATMATRAH = KS.LINENET;
                                KS.VATAMNT = KS.TOTAL - KS.VATMATRAH;
                                KS.LISTETOPLAM = KS.TOTAL;
                                break;
                            }
                    }
                    break;
            }

          

            


        


        }

        public static void CalculateDiscount(ref Line_Ks KALEM,Line_Ks DISCOUNT) {
            KALEM.TOTAL = KALEM.AMOUNT * KALEM.PRICE;
           KALEM.DISTCOST = (KALEM.TOTAL / 100)*DISCOUNT.DISCPER;
           KALEM.DISTDISC = KALEM.DISTCOST;

        }

        public static string Filter<T, R>(R Filter) where T : new()
        {


            return "";

        }



    }


   




   
}