using LogoGo3Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogoGo3Data.DefineModel
{
   public class Belge_Model
    {

        public Belge belge { get; set; }
        public List<BelgeLine> belgeLines { get; set; }


    }

   

    public class Alim_Fatura
    {

        public AlimBelge belge { get; set; }
        public List<BelgeLine> belgeLines { get; set; }


    }

    public class Satis_Fatura
    {

        public AlimBelge belge { get; set; }
        public List<BelgeLine> belgeLines { get; set; }


    }

    public class Belge_Model2
    {

        public LG_001_01_STFICHE belge { get; set; }
        public List<LG_001_01_STLINE> belgeLines { get; set; }


    }

    public class Belge
        {
        public int LOGICALREF { get; set; }
        public int GRPCODE { get; set; }
        public int TRCODE { get; set; }
        public int IOCODE { get; set; }
        public string FICHENO { get; set; }
        public DateTime DATE_ { get; set; }
        public int FTIME { get; set; }
        public string DOCODE { get; set; }
        public string INVNO { get; set; }
        public string SPECODE { get; set; }
        public string CYPHCODE { get; set; }
        public int INVOICEREF { get; set; }
        public int CLIENTREF { get; set; }
        public int RECVREF { get; set; }
        public int ACCOUNTREF { get; set; }
        public int CENTERREF { get; set; }
        public int PRODORDERREF { get; set; }
        public string PORDERFICHENO { get; set; }
        public int SOURCETYPE { get; set; }
        public int SOURCEINDEX { get; set; }
        public int SOURCEWSREF { get; set; }
        public int SOURCEPOLNREF { get; set; }
        public int SOURCECOSTGRP { get; set; }
        public int DESTTYPE { get; set; }
        public int DESTINDEX { get; set; }
        public int DESTWSREF { get; set; }
        public int DESTPOLNREF { get; set; }
        public int DESTCOSTGRP { get; set; }
        public int FACTORYNR { get; set; }
        public int CANCELLED { get; set; }
        public int BILLED { get; set; }
        public int ACCOUNTED { get; set; }
        public int UPDCURR { get; set; }
        public int INUSE { get; set; }
        public int INVKIND { get; set; }
        public double ADDDISCOUNTS { get; set; }
        public double TOTALDISCOUNTS { get; set; }
        public double TOTALDISCOUNTED { get; set; }
        public double ADDEXPENSES { get; set; }
        public double TOTALEXPENSES { get; set; }
        public double TOTALDEPOZITO { get; set; }
        public double TOTALPROMOTIONS { get; set; }
        public double TOTALVAT { get; set; }
        public double GROSSTOTAL { get; set; }
        public double NETTOTAL { get; set; }
        public string GENEXP1 { get; set; }
        public string GENEXP2 { get; set; }
        public string GENEXP3 { get; set; }
        public string GENEXP4 { get; set; }
        public string GENEXP5 { get; set; }
        public string GENEXP6 { get; set; }
        public double REPORTRATE { get; set; }
        public double REPORTNET { get; set; }
        public int EXTENREF { get; set; }
        public int PAYDEFREF { get; set; }
        public int PRINTCNT { get; set; }
        public int FICHECNT { get; set; }
        public int ACCFICHEREF { get; set; }
        public int CAPIBLOCK_CREATEDBY { get; set; }
        public DateTime CAPIBLOCK_CREADEDDATE { get; set; }
        public int CAPIBLOCK_CREATEDHOUR { get; set; }
        public int CAPIBLOCK_CREATEDMIN { get; set; }
        public int CAPIBLOCK_CREATEDSEC { get; set; }
        public int CAPIBLOCK_MODIFIEDBY { get; set; }
        public DateTime CAPIBLOCK_MODIFIEDDATE { get; set; }
        public int CAPIBLOCK_MODIFIEDHOUR { get; set; }
        public int CAPIBLOCK_MODIFIEDMIN { get; set; }
        public int CAPIBLOCK_MODIFIEDSEC { get; set; }
        public int SALESMANREF { get; set; }
        public int GENEXCTYP { get; set; }
        public int LINEEXCTYP { get; set; }
        public string TRADINGGRP { get; set; }
        public int TEXTINC { get; set; }
        public int SITEID { get; set; }
        public int RECSTATUS { get; set; }
        public int ORGLOGICREF { get; set; }
        public int WFSTATUS { get; set; }
        public int SHIPINFOREF { get; set; }
        public int DISTORDERREF { get; set; }
        public string DOCTRACKINGNR { get; set; }
        public double ADDTAXCALC { get; set; }
        public double TOTALADDTAX { get; set; }
        public double DEDUCTIONPART1 { get; set; }
        public double DEDUCTIONPART2 { get; set; }
        public int GRPFIRMTRANS { get; set; }
        public int AFFECTRISK { get; set; }
        public int DISPSTATUS { get; set; }
        public string DELIVERYCODE { get; set; }
        public string GUID { get; set; }
        public int  EINVOICE { get; set; }
    }

     public class BelgeLine {
        public int LOGICALREF { get; set; }
        public int STOCKREF { get; set; }
        public int LINETYPE { get; set; }
        public int TRCODE { get; set; }
        public DateTime DATE_ { get; set; }
        public int FTIME { get; set; }
        public int GLOBTRANS { get; set; }
        public int CALCTYPE { get; set; }
        public int SOURCETYPE { get; set; }
        public int SOURCEINDEX { get; set; }
        public int SOURCECOSTGRP { get; set; }
        public int SOURCEWSREF { get; set; }
        public int SOURCEPOLNREF { get; set; }
        public int DESTTYPE { get; set; }
        public int DESTINDEX { get; set; }
        public int DESTCOSTGRP { get; set; }
        public int DESTWSREF { get; set; }
        public int DESTPOLNREF { get; set; }
        public int FACTORYNR { get; set; }
        public int IOCODE { get; set; }
        public int STFICHEREF { get; set; }
        public int STFICHELNNO { get; set; }
        public long INVOICEREF { get; set; }
        public int INVOICELNNO { get; set; }
        public int CLIENTREF { get; set; }
        public int ORDTRANSREF { get; set; }
        public int ORDFICHEREF { get; set; }
        public int CENTERREF { get; set; }
        public int ACCOUNTREF { get; set; }
        public int PAYDEFREF { get; set; }
        public string SPECODE { get; set; }
        public string DELVRYCODE { get; set; }
        public double AMOUNT { get; set; }
        public double PRICE { get; set; }
        public double TOTAL { get; set; }
        public double PRCURR { get; set; }
        public double PRPRICE { get; set; }
        public double TRCURR { get; set; }
        public double TRRATE { get; set; }
        public double REPORTRATE { get; set; }
        public double DISTCOST { get; set; }
        public double DISTDISC { get; set; }
        public double DISTEXP { get; set; }
        public int DISTPROM { get; set; }
        public int DISCPER { get; set; }
        public string LINEEXP { get; set; }
        public int UOMREF { get; set; }
        public int USREF { get; set; }
        public int UINFO1 { get; set; }
        public int UINFO2 { get; set; }
        public int UINFO3 { get; set; }
        public int UINFO4 { get; set; }
        public int UINFO5 { get; set; }
        public int UINFO6 { get; set; }
        public int UINFO7 { get; set; }
        public int UINFO8 { get; set; }
        public int VAT { get; set; }
        public double VATAMNT { get; set; }
        public double VATMATRAH { get; set; }
        public int BILLEDITEM { get; set; }
        public int BILLED { get; set; }
        public int LINENET { get; set; }
        public int SALESMANREF { get; set; }
        public int RECSTATUS { get; set; }
        public string GUID { get; set; }
        public string SPECODE2 { get; set; }
    }


    public class AlimBelge
    {
        //public int LOGICALREF { get; set; }
        //public int GRPCODE { get; set; }
        //public int TRCODE { get; set; }
        //public int IOCODE { get; set; }
        public string FICHENO { get; set; }
        public DateTime DATE_ { get; set; }
        public string DOCODE { get; set; }
        //public string INVNO { get; set; }
        public string SPECODE { get; set; }
        //public string CYPHCODE { get; set; }
        //public int INVOICEREF { get; set; }
        public int CLIENTREF { get; set; }
        //public int RECVREF { get; set; }
        //public int ACCOUNTREF { get; set; }
        //public int CENTERREF { get; set; }
        //public int PRODORDERREF { get; set; }
        //public string PORDERFICHENO { get; set; }
        //public int SOURCETYPE { get; set; }
        public int SOURCEINDEX { get; set; }
        //public int SOURCEWSREF { get; set; }
        //public int SOURCEPOLNREF { get; set; }
        public int SOURCECOSTGRP { get; set; }
        //public int DESTTYPE { get; set; }
        public int DESTINDEX { get; set; }
        //public int DESTWSREF { get; set; }
        //public int DESTPOLNREF { get; set; }
        public int DESTCOSTGRP { get; set; }
        //public int FACTORYNR { get; set; }
        //public int CANCELLED { get; set; }
        //public int BILLED { get; set; }
        public double TOTALDISCOUNTED { get; set; }
        public double TOTALVAT { get; set; }
        public double GROSSTOTAL { get; set; }
        public double NETTOTAL { get; set; }
        public string GENEXP1 { get; set; }
        public string GENEXP2 { get; set; }
        public string GENEXP3 { get; set; }
        public string GENEXP4 { get; set; }
        public string GENEXP5 { get; set; }
        public string GENEXP6 { get; set; }
        public double REPORTRATE { get; set; }
        public double REPORTNET { get; set; }
        //public int EXTENREF { get; set; }
        //public int PAYDEFREF { get; set; }
        //public int PRINTCNT { get; set; }
        //public int FICHECNT { get; set; }
        //public int ACCFICHEREF { get; set; }
        //public int CAPIBLOCK_CREATEDBY { get; set; }
        //public DateTime CAPIBLOCK_CREADEDDATE { get; set; }
        //public int CAPIBLOCK_CREATEDHOUR { get; set; }
        //public int CAPIBLOCK_CREATEDMIN { get; set; }
        //public int CAPIBLOCK_CREATEDSEC { get; set; }
        //public int CAPIBLOCK_MODIFIEDBY { get; set; }
        //public DateTime CAPIBLOCK_MODIFIEDDATE { get; set; }
        //public int CAPIBLOCK_MODIFIEDHOUR { get; set; }
        //public int CAPIBLOCK_MODIFIEDMIN { get; set; }
        //public int CAPIBLOCK_MODIFIEDSEC { get; set; }
        public int SALESMANREF { get; set; }
        //public int GENEXCTYP { get; set; }
        //public int LINEEXCTYP { get; set; }
        //public string TRADINGGRP { get; set; }
        //public int TEXTINC { get; set; }
        //public int SITEID { get; set; }
        //public int RECSTATUS { get; set; }
        //public int ORGLOGICREF { get; set; }
        //public int WFSTATUS { get; set; }
        //public int SHIPINFOREF { get; set; }
        //public int DISTORDERREF { get; set; }
        public string DOCTRACKINGNR { get; set; }
        public int AFFECTRISK { get; set; }
        public int DISPSTATUS { get; set; }
        public string GUID { get; set; }
        public int EINVOICE { get; set; }
        public string IRSFICHENO { get; set; }
    }
    public class SatisBelge
    {
        //public int LOGICALREF { get; set; }
        //public int GRPCODE { get; set; }
        //public int TRCODE { get; set; }
        //public int IOCODE { get; set; }
        public string FICHENO { get; set; }
        public DateTime DATE_ { get; set; }
        public string DOCODE { get; set; }
        //public string INVNO { get; set; }
        public string SPECODE { get; set; }
        //public string CYPHCODE { get; set; }
        //public int INVOICEREF { get; set; }
        public int CLIENTREF { get; set; }
        //public int RECVREF { get; set; }
        //public int ACCOUNTREF { get; set; }
        //public int CENTERREF { get; set; }
        //public int PRODORDERREF { get; set; }
        //public string PORDERFICHENO { get; set; }
        //public int SOURCETYPE { get; set; }
        public int SOURCEINDEX { get; set; }
        //public int SOURCEWSREF { get; set; }
        //public int SOURCEPOLNREF { get; set; }
        public int SOURCECOSTGRP { get; set; }
        //public int DESTTYPE { get; set; }
        public int DESTINDEX { get; set; }
        //public int DESTWSREF { get; set; }
        //public int DESTPOLNREF { get; set; }
        public int DESTCOSTGRP { get; set; }
        //public int FACTORYNR { get; set; }
        //public int CANCELLED { get; set; }
        //public int BILLED { get; set; }
        public double TOTALDISCOUNTED { get; set; }
        public double TOTALVAT { get; set; }
        public double GROSSTOTAL { get; set; }
        public double NETTOTAL { get; set; }
        public string GENEXP1 { get; set; }
        public string GENEXP2 { get; set; }
        public string GENEXP3 { get; set; }
        public string GENEXP4 { get; set; }
        public string GENEXP5 { get; set; }
        public string GENEXP6 { get; set; }
        public double REPORTRATE { get; set; }
        public double REPORTNET { get; set; }
        //public int EXTENREF { get; set; }
        //public int PAYDEFREF { get; set; }
        //public int PRINTCNT { get; set; }
        //public int FICHECNT { get; set; }
        //public int ACCFICHEREF { get; set; }
        //public int CAPIBLOCK_CREATEDBY { get; set; }
        //public DateTime CAPIBLOCK_CREADEDDATE { get; set; }
        //public int CAPIBLOCK_CREATEDHOUR { get; set; }
        //public int CAPIBLOCK_CREATEDMIN { get; set; }
        //public int CAPIBLOCK_CREATEDSEC { get; set; }
        //public int CAPIBLOCK_MODIFIEDBY { get; set; }
        //public DateTime CAPIBLOCK_MODIFIEDDATE { get; set; }
        //public int CAPIBLOCK_MODIFIEDHOUR { get; set; }
        //public int CAPIBLOCK_MODIFIEDMIN { get; set; }
        //public int CAPIBLOCK_MODIFIEDSEC { get; set; }
        public int SALESMANREF { get; set; }
        //public int GENEXCTYP { get; set; }
        //public int LINEEXCTYP { get; set; }
        //public string TRADINGGRP { get; set; }
        //public int TEXTINC { get; set; }
        //public int SITEID { get; set; }
        //public int RECSTATUS { get; set; }
        //public int ORGLOGICREF { get; set; }
        //public int WFSTATUS { get; set; }
        //public int SHIPINFOREF { get; set; }
        //public int DISTORDERREF { get; set; }
        public string DOCTRACKINGNR { get; set; }
        public int AFFECTRISK { get; set; }
        public int DISPSTATUS { get; set; }
        public string GUID { get; set; }
        public int EINVOICE { get; set; }
        public string IRSFICHENO { get; set; }
    }



    public class Belge_Post_Model {
        public Belge_Ks Belge { get; set; }
        public List<Line_Ks> Line { get; set; }
        public string HEADTABLE { get; set; }
        public string LISTTABLE { get; set; }
    }

    public class Belge_Ks {
        public int TRCODE { get; set; }
        public string FICHENO { get; set; }
        public DateTime DATE_ { get; set; }
        public string DOCODE { get; set; }
        public string SPECODE { get; set; }
        public int CLIENTREF { get; set; }
        public int ACCOUNTREF { get; set; }
        public int SOURCEINDEX { get; set; }
        public int SOURCECOSTGRP { get; set; }
        public int DESTINDEX { get; set; }
        public int DESTCOSTGRP { get; set; }
        public int VAT { get; set; }
        public double TOTALDISCOUNTED { get; set; }
        public double TOTALVAT { get; set; }
        public double GROSSTOTAL { get; set; }
        public double NETTOTAL { get; set; }
        public string GENEXP1 { get; set; }
        public string GENEXP2 { get; set; }
        public string GENEXP3 { get; set; }
        public string GENEXP4 { get; set; }
        public string GENEXP5 { get; set; }
        public string GENEXP6 { get; set; }
        public double TRNET { get; set; }
        public int AFFECTRISK { get; set; }
        public short EINVOICE { get; set; }
        public int GRPCODE { get; set; }
        public string TRACKNR { get; set; }
        public string IRSFICHENO { get; set; }
        public int FATID { get; set; }
        public short IOCODE { get; set; }
        public short BILLED { get; set; }
        public int INVOICEREF { get; set; }
        public string INVNO { get; set; }
        public int KDV_DAHILMI { get; set; }
        public static int ACCREF { get; set; }
        public  string IRSDATE { get; set; }

    }

    public class Line_Ks {
        public int STOCKREF { get; set; }
        public double LINETYPE { get; set; }
        public string SPECODE { get; set; }
        public double AMOUNT { get; set; }
        public double PRICE { get; set; }
        public double TOTAL { get; set; }
        public string LINEEXP { get; set; }
        public int UOMREF { get; set; }
        public int USREF { get; set; }
        public int UINFO1 { get; set; }
        public int UINFO2 { get; set; }
        public int VAT { get; set; }
        public double VATAMNT { get; set; }
        public double VATMATRAH { get; set; }
        public double LINENET { get; set; }
        public short IOCODE { get; set; }
        public int TRCODE { get; set; }
        public int INVOICEREF { get; set; }
        public int INVOICELNNO { get; set; }
        public int ACCOUNTREF { get; set; }
        public int VATACCREF { get; set; }
        public KdvStatu KDVDahilmi { get; set; }
        public double LISTETOPLAM { get; set; }
        public double DISCPER { get; set; }
        public double DISTDISC { get; set; }
        public double DISTCOST { get; set; }
        public int LINENO { get; set; }
        public string  IRSDATE { get; set; }

    }
    public class LV_001_02_STLINE_GRP
    {
        public int LINEID { get; set; }

        public int STOCKID { get; set; }

        public string STOCKCODE { get; set; }

        public string NAME { get; set; }

        public int? ACCOUNTREF { get; set; }

        public int? VATACCREF { get; set; }

        public int? CLIENTREF { get; set; }

        public string DEFINITION_ { get; set; }

        public float? VAT { get; set; }

        public int? INVOICEREF { get; set; }

        public DateTime? DATE_ { get; set; }

        public string STGRPCODE { get; set; }

        public short? SOURCEINDEX { get; set; }

        public short? SOURCECOSTGRP { get; set; }

        public short? DESTINDEX { get; set; }

        public short? DESTCOSTGRP { get; set; }

        public short? IOCODE { get; set; }

        public int? STFICHEREF { get; set; }

        public short? STFICHELNNO { get; set; }

        public short? TRCODE { get; set; }

    }

    public class LV_001_MUHESEBE_STOK_GRP_KODLARI { public int LOGICALREF { get; set; } public string CODE { get; set; } public string DEFINITION_ { get; set; } public Guid ID { get; set; } public long ACOOUNTREF { get; set; } public string ACOOUNTCODE { get; set; } public short? VAT { get; set; } public short? TRCODE { get; set; } public string GRPCODE { get; set; } }

    public enum KdvStatu { 
    Dahil=1,
    Haric=0
    }
}
