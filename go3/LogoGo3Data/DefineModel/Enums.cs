using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace LogoGo3Data.DefineModel
{
  

    public  enum EIrsaliyeTip
    {
        alim_Irsaliyesi = 1,
        toptan_Satis_Iade_Irsaliyesi = 3,
        alim_Iade_Irsaliyesi = 6,
        toptan_Satis_Irsaliyesi =8,

   
    

    }

    public enum EBelgeGurupKodlari
    {
        Alim_irsaliyeleri = 1,
        Satis_irsaliyeleri = 2,
          Satis_Faturalari = 2,
        Alim_Faturalari = 1,
        Malzeme_Fisleri = 3,

    }

    public enum EIoKod
    {
        Bos = 0,
        Giris = 1,
        Transfer = 2,
        Cikis = 3,


    }

    public enum ELineTypes {
        malzeme=0,
        indirim=2,
        hizmet = 4,

    }

   public  enum EFaturaTip
    {
        SatinAlma_faturasi=1,
        Perakende_Satis_Iade_Faturasi = 2,
        Toptan_Satis_Iade_Faturasi = 3,
        AlinanHizmet_Faturasi =4,
        AlinanProforma_Faturasi=5,
        SatinAlmaIade_Faturasi = 6,
        SatinAlmaFiyatFarki_Faturasi =13,
        Mustahsil_Makbuzu = 26,
        PerakendeSatis_Faturasi = 7,
        ToptanSatis_Faturasi = 8,
        VerilenHizmet_Faturasi=9,
        VerilenProforma_Faturasi = 10,
        SatisFiyatFarki_Faturasi = 14,
    }

    public enum ECariTipleri {
        ALICI_SATICI = 3,
    }

    public enum EMalzemeTipleri
    {
        TicariMal = 1,
    }

    public enum EMalzemeFisTips
    {
        Fire_Fisi = 11,
        Sarf_Fisi = 12,
       UretimdenGiris_Fisi = 13,
        Devir_Fisi = 14,
        Ambar_Fisi = 25,
        SayimFazlasi_Fisi = 50,
        SayimEksigiFisi = 51,

    }


    public enum ECariHareket
    {
        Nakit_Tahsilat = 1,
        Nakit_Odeme = 2,
        Borc_Dekontu = 3,
        Alacak_Dekontu = 4,
        Virman_Fisi = 5,
        Kur_Farkı_Fisi = 6,
         OzelFis = 12,
        Acilis_Fisi = 14,
        Verilen_Vade_Farki_Faturasi = 41,
        Alinan_Vade_Farki_Faturasi = 42,
        Verilen_Serbest_Meslek_Makbuzu = 45,
        Alinan_Serbest_Meslek_Makbuzu = 46,
        Kredi_Karti_Fisi = 70,
        Kredi_Karti_iade_Fisi = 71,
        Firma_Kredi_Karti_Fisi = 72,
        Firma_Kredi_Karti_iade_Fisi = 73,

    }



}
