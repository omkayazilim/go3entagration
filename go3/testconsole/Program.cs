using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using static LogoGo3Data.Soap_Models.Models;

namespace testconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //js.MaxJsonLength = int.MaxValue;
            //ServiceReference1.ObjectSoapClient clnt = new ServiceReference1.ObjectSoapClient("ObjectSoap");
            //string CRT= clnt.GETCARIKARTLAR("", "");
            //Array_Result_Master M = js.Deserialize<Array_Result_Master>(CRT);
            //List<Array_CariKartlar_Respons> CRL = js.Deserialize<List<Array_CariKartlar_Respons>>(M.DATA);
            //string STR = clnt.GETSTOKKARTLAR("", "");
            //Array_Result_Master M1 = js.Deserialize<Array_Result_Master>(CRT);
            getModels();
          //  irsaliye();
            Console.ReadLine();
        }


        static void irsaliye() {
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = int.MaxValue;
            ServiceReference1.ObjectSoapClient clnt = new ServiceReference1.ObjectSoapClient("ObjectSoap");
           string deps= clnt.GETDEPOLAR("", "");
            //string CRT = clnt.GETCARIKARTLAR("", "");

            Array_Fatura_Irsaliye_Ust_Min_Request AV = new Array_Fatura_Irsaliye_Ust_Min_Request {
                CARI_KODU = "TDR00087", //Cariğ Kodu
                FIS_NO="TEST000000090", // Fiş no
                F_YEDEK4=0, //0 kullanılmıyor
                GEN_ISK1O=0,//0 kullanılmıyor
                KDV_DAHILMI=true,// Kdv Durumu
                KOD1="", // boş kullanılmıyor
                KOD2 ="",// boş kullanılmıyor
                ODEMEGUNU =0,// 0 kullanılmıyor
                PLA_KODU="", // // boş kullanılmıyor
                PROJE_KODU ="",// boş kullanılmıyor
                SIPARIS_TEST ="",// boş kullanılmıyor
                TARIH ="2020-03-31", // Fiş Tarihi
                TIPI = 0 //0 kullanılmıyor
            };

            List<Array_Fatura_Irsaliye_Kalemler_Min_Request> LKL = new List<Array_Fatura_Irsaliye_Kalemler_Min_Request> {
                new Array_Fatura_Irsaliye_Kalemler_Min_Request{
                       DEPO_KODU=0//depo No
                     , GIR_DEPO_KODU=0 // 0 boş
                     , ODEGUN=0  //0 Boş
                     , REFERANS_KODU=""//Boş
                     , STHAR_BF=3.80,// Birim Fiyat
                         STHAR_GCMIK=1,//Miktar
                         STHAR_NF=0// 0 Boş
                       , STHAR_TARIH="2020-03-31"// Hareket Tarihi
                       , STOK_KODU="1002000800080001"// Stok Kodu
                   
                }
              

            };

            string crt = clnt.YENI_ALIS_IRSALIYESI("", "", js.Serialize(AV), js.Serialize(LKL));

            Console.WriteLine(crt);


    }

        static void getModels() {

           Console.WriteLine(LogoGo3Data.GenerateProcess.createModels());

            Console.ReadLine();        
        }
    }
}
