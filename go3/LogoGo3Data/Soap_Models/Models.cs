using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogoGo3Data.Soap_Models
{
   public class Models
    {
        public class Array_Filter_Request
        {
            public int SUBE_KODU { get; set; }
            public int ISLETME_KODU { get; set; }
        }

        public class Array_CariKartlar_Respons
        {
            public int SUBE_KODU { get; set; }
            public int ISLETME_KODU { get; set; }
            public string CARI_KOD { get; set; }
            public string CARI_ADRES { get; set; }
            public string CARI_IL { get; set; }
            public string CARI_ILCE { get; set; }
            public string CARI_ISIM { get; set; }
            public string CARI_TEL { get; set; }
            public string CARI_TEL2 { get; set; }
            public string CARI_TEL3 { get; set; }
            public string CARI_TIP { get; set; }
            public string VERGI_DAIRESI { get; set; }
            public string VERGI_NUMARASI { get; set; }
            public string FAX { get; set; }
            public string FAX2 { get; set; }
            public string POSTAKODU { get; set; }
            public string DETAY_KODU { get; set; }
            public string M_KOD { get; set; }
            public string EMAIL { get; set; }
            public string WEB { get; set; }
            public string GSM1 { get; set; }
            public string GSM2 { get; set; }

        }

        public class Array_Result_Master
        {
            public bool SONUC { get; set; }
            public string DATA { get; set; }
            public string ACIKLAMA { get; set; }
        }

        public class Array_Filter_Request_Serach
        {
            public int SUBE_KODU { get; set; }
            public int ISLETME_KODU { get; set; }
            public string SEARCH { get; set; }
        }

           public class Array_Cari_Request
    {
        public int ISLETME_KODU { get; set; }
        public int Sube_Kodu { get; set; }
        public string CARI_KOD { get; set; }
        public string CARI_ISIM { get; set; }
        public string CARI_TIP { get; set; }
        public string CARI_ADRES { get; set; }
        public string ULKE_KODU { get; set; }
        public string CARI_IL { get; set; }
        public string CARI_ILCE { get; set; }
        public string CARI_TEL { get; set; }
        public string CARI_TEL2 { get; set; }
        public string CARI_TEL3 { get; set; }
        public string VERGI_DAIRESI { get; set; }
        public string VERGI_NUMARASI { get; set; }
        public string EMAIL { get; set; }
        public string WEB { get; set; }
        public string M_KOD { get; set; }
        public string DOVIZLIMI { get; set; }
        public int DOVIZ_TIPI { get; set; }
        public int DOVIZ_TURU { get; set; }
        public int VADE_GUNU { get; set; }
        public int CM_BORCT { get; set; }
        public int CM_ALACT { get; set; }
        public string HESAPTUTMASEKLI { get; set; }
        public string Update_Kodu { get; set; }
        public string C_Yedek1 { get; set; }
        public int B_Yedek1 { get; set; }
        public int ODEMETIPI { get; set; }
        public string OnayTipi { get; set; }
        public int OnayNum { get; set; }
        public string MUSTERIBAZIKDV { get; set; }
        public int DETAY_KODU { get; set; }
        public string FAX { get; set; }
        public string POSTAKODU { get; set; }
        public string TcKimlikNo { get; set; }
        public int L_Yedek1 { get; set; }

    }
        public class Array_Stoklar_Response
        {
            public int SUBE_KODU { get; set; }
            public int ISLETME_KODU { get; set; }
            public string STOK_KODU { get; set; }
            public string STOK_ADI { get; set; }
            public string GRUP_KODU { get; set; }
            public int MUH_DETAYKODU { get; set; }
            public float KDV_ORANI { get; set; }
            public float ALIS_KDV_KODU { get; set; }
            public string OLCU_BR1 { get; set; }
            public float PAY_1 { get; set; }
            public float PAYDA_1 { get; set; }
            public float PAY2 { get; set; }
            public float PAYDA2 { get; set; }
            public string FIAT_BIRIMI { get; set; }
            public int SAT_DOV_TIP { get; set; }
            public float BIRIM_AGIRLIK { get; set; }
            public int ALIS_DOV_TIP { get; set; }
            public int DEPO_KODU { get; set; }
            public string BILESENMI { get; set; }
            public string MAMULMU { get; set; }
            public string UPDATE_KODU { get; set; }
            public string KILIT { get; set; }
            public string SIP_POLITIKASI { get; set; }
            public string PLANLANACAK { get; set; }
            public string SATICISIPKILIT { get; set; }
            public string MUSTERISIPKILIT { get; set; }
            public string SATINALMAKILIT { get; set; }
            public string SATISKILIT { get; set; }
            public string ONAYTIPI { get; set; }
            public int ONAYNUM { get; set; }
            public string FIKTIF_MAM { get; set; }
            public string YAPILANDIR { get; set; }
            public string SBOMVARMI { get; set; }

        }

        public class Array_Stoklar_Request
        {
            public int SUBE_KODU { get; set; }
            public int ISLETME_KODU { get; set; }
            public string STOK_KODU { get; set; }
            public string STOK_ADI { get; set; }
            public string GRUP_KODU { get; set; }
            public int MUH_DETAYKODU { get; set; }
            public float KDV_ORANI { get; set; }
            public float ALIS_KDV_KODU { get; set; }
            public string OLCU_BR1 { get; set; }
            public float PAY_1 { get; set; }
            public float PAYDA_1 { get; set; }
            public float PAY2 { get; set; }
            public float PAYDA2 { get; set; }
            public string FIAT_BIRIMI { get; set; }
            public int SAT_DOV_TIP { get; set; }
            public float BIRIM_AGIRLIK { get; set; }
            public int ALIS_DOV_TIP { get; set; }
            public int DEPO_KODU { get; set; }
            public string BILESENMI { get; set; }
            public string MAMULMU { get; set; }
            public string UPDATE_KODU { get; set; }
            public string KILIT { get; set; }
            public string SIP_POLITIKASI { get; set; }
            public string PLANLANACAK { get; set; }
            public string SATICISIPKILIT { get; set; }
            public string MUSTERISIPKILIT { get; set; }
            public string SATINALMAKILIT { get; set; }
            public string SATISKILIT { get; set; }
            public string ONAYTIPI { get; set; }
            public int ONAYNUM { get; set; }
            public string FIKTIF_MAM { get; set; }
            public string YAPILANDIR { get; set; }
            public string SBOMVARMI { get; set; }
        }

        public class Array_Depolar_Response
        {
            public string DEPO_KODU { get; set; }
            public string DEPO_ISMI { get; set; }
            public string S_YEDEK1 { get; set; }
        }

        public class Array_Muhasebe_Referans_kodlari
        {
            public string GRUP_KOD { get; set; }
            public string GRUP_ISIM { get; set; }

        }

      
        public class Array_Fatura_Irsaliye_Ust_Min_Request
        {
            public string CARI_KODU { get; set; }
            public string TARIH { get; set; }
            public string SIPARIS_TEST { get; set; }
            public double GEN_ISK1O { get; set; }
            public bool KDV_DAHILMI { get; set; }
            public string PLA_KODU { get; set; }
            public string PROJE_KODU { get; set; }
            public int TIPI { get; set; }
            public string FIS_NO { get; set; }
            public string KOD1 { get; set; }
            public string KOD2 { get; set; }
            public int ODEMEGUNU { get; set; }
            public double F_YEDEK4 { get; set; }
        }

        public class Array_Fatura_Irsaliye_Kalemler_Min_Request
        {
            public string STOK_KODU { get; set; }
            public int DEPO_KODU { get; set; }
            public double STHAR_GCMIK { get; set; }
            public string STHAR_TARIH { get; set; }
            public double STHAR_NF { get; set; }
            public double STHAR_BF { get; set; }
            public double ODEGUN { get; set; }
            public double GIR_DEPO_KODU { get; set; }
            public string REFERANS_KODU { get; set; }

        }


        public class Array_Kasalar_Respons
        {
            public string KSMAS_KOD { get; set; }
            public string KSMAS_NAME { get; set; }
        }

    }
}
