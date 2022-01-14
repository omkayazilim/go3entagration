using LogoGo3Data.Context;
using LogoGo3Data.DefineModel;
using LogoGo3Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using static LogoGo3Data.Extras.Utils;


namespace LogoGo3Data.Tools
{
    public static class AppCommon
    {
        public static T CreateAndFillObject<T>(object o, string Tablename, int TrCode) where T : new()
        {
            T obj = new T();

            T DEFLGC = NQery.AdoGetDefModel<T>(Tablename, TrCode).Data.FirstOrDefault();
            foreach (var item in obj.GetType().GetProperties())
            {
                string pip = item.Name;

                try
                {
                    var itp = item.PropertyType;
                    object val = null;
                    if (Nullable.GetUnderlyingType(itp) != null) {
                        Type tpp = o.GetType();
                        PropertyInfo prinf = tpp.GetProperty(pip);
                        object but = prinf.GetValue(o, null);
                        val = AppCommon.ChangeType(but, itp);
                    }
                    else {
                        val = Convert.ChangeType(o.GetType().GetProperty(pip).GetValue(o, null), itp);
                    }
                    item.SetValue(obj, val);
                }
                catch (Exception ex)
                {
                    var itp = item.PropertyType;
                    var val = Nullable.GetUnderlyingType(itp) != null
                                   ? AppCommon.ChangeType(DEFLGC.GetType().GetProperty(pip).GetValue(DEFLGC, null), itp)
                                   : Convert.ChangeType(DEFLGC.GetType().GetProperty(pip).GetValue(DEFLGC, null), itp);
                    item.SetValue(obj, val);

                }


            }

            return obj;

        }

        public static T CreateAndFillObject<T>(object o, object def, string Tablename) where T : new()
        {
            T obj = new T();


            foreach (var item in obj.GetType().GetProperties())
            {
                string pip = item.Name;

                try
                {
                    var itp = item.PropertyType;
                    var val = Nullable.GetUnderlyingType(itp) != null
                                   ? ChangeType(o.GetType().GetProperty(pip).GetValue(o, null), itp)
                                   : Convert.ChangeType(o.GetType().GetProperty(pip).GetValue(o, null), itp);
                    item.SetValue(obj, val);
                }
                catch 
                {
                    var itp = item.PropertyType;
                    var val = Nullable.GetUnderlyingType(itp) != null
                                   ? ChangeType(def.GetType().GetProperty(pip).GetValue(def, null), itp)
                                   : Convert.ChangeType(def.GetType().GetProperty(pip).GetValue(def, null), itp);
                    item.SetValue(obj, val);

                }


            }

            return obj;

        }
        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }




        public static MasterResult<T> CreateAndFillObject<T>(object o, int Type) where T : new()
        { T obj = new T();


            MasterResult<T> GETDEF = SqliteContext.GetSqliteDefModel<T>(Type);
            if (!GETDEF.Result)
                return new MasterResult<T> { Result = false, Data = obj, Elapsed = 0, Message = GETDEF.Message };


            T DEFLGC = GETDEF.Data;
            foreach (var item in obj.GetType().GetProperties())
            {
                string pip = item.Name;

                if (!pip.Equals("LOGICALREF"))
                {
                    try
                    {
                        var itp = item.PropertyType;
                        object val = null;
                        if (Nullable.GetUnderlyingType(itp) != null)
                        {
                            Type tpp = o.GetType();
                          
                            PropertyInfo prinf = tpp.GetProperty(pip);  
                            if (prinf != null) { 
                            object but = prinf.GetValue(o, null);
                            val = AppCommon.ChangeType(but, itp);
                            }
                        }
                        else
                        {
                            Type tpp = o.GetType();

                            PropertyInfo prinf = tpp.GetProperty(pip);
                            if (prinf != null)
                            {
                                val = Convert.ChangeType(prinf.GetValue(o, null), itp);
                            }
                        }
                        item.SetValue(obj, val);
                    }
                    catch (Exception ex)
                    {
                        var itp = item.PropertyType;
                        var val = Nullable.GetUnderlyingType(itp) != null
                                       ? AppCommon.ChangeType(DEFLGC.GetType().GetProperty(pip).GetValue(DEFLGC, null), itp)
                                       : Convert.ChangeType(DEFLGC.GetType().GetProperty(pip).GetValue(DEFLGC, null), itp);
                        item.SetValue(obj, val);

                    }
                }


            }

            return new MasterResult<T> { Data = obj, Elapsed = 0, Message = "", Result = true };

        }



       

        public static T ChangeType<T>(object value)
        {
            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return default(T);
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return (T)Convert.ChangeType(value, t);
        }

        #region "TableToList"

        public static MasterResult<List<T>> ConvertTo<T>(DataTable datatable) where T : new()
        {
            List<T> Temp = new List<T>();
            try
            {
                List<string> columnsNames = new List<string>();
                foreach (DataColumn DataColumn in datatable.Columns)
                    columnsNames.Add(DataColumn.ColumnName);
                Temp = datatable.AsEnumerable().ToList().ConvertAll<T>(row => getObject<T>(row, columnsNames));
                return new MasterResult<List<T>> { Data = Temp, Result = true, Message = "", Elapsed = 0 };
            }
            catch
            {
                return new MasterResult<List<T>> { Data = null, Elapsed = 0, Message = "", Result = false };
            }

        }
        public static T getObject<T>(DataRow row, List<string> columnsName) where T : new()
        {
            T obj = new T();
            try
            {
                string columnname = "";
                string value = "";
                PropertyInfo[] Properties;
                Properties = typeof(T).GetProperties();
                foreach (PropertyInfo objProperty in Properties)
                {
                    columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());

                    if (!string.IsNullOrEmpty(columnname))
                    {
                        value = row[columnname].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            {
                                value = row[columnname].ToString().Replace("$", "").Replace(",", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[columnname].ToString().Replace("%", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                            }
                        }
                    }
                }
                return obj;
            }
            catch
            {
                return obj;
            }
        }



        /// <summary>
        /// Snuç tablosunu generic olarak listeye dönüştürüp doldurur
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static MasterResult<List<T>> convertToEnumerable<T>(DataTable table) where T : new() {

            List<T> retr = new List<T>();
            foreach (DataRow item in table.Rows)
            {
                T row = new T();
                foreach (var s in row.GetType().GetProperties())
                {
                    try
                    {

                        object colval = item[s.Name];
                        Type tp = s.PropertyType;
                        if (Nullable.GetUnderlyingType(tp) != null)
                            s.SetValue(row, Convert.ChangeType(colval, Type.GetType(Nullable.GetUnderlyingType(tp).ToString())), null);
                        else
                            s.SetValue(row, Convert.ChangeType(colval, tp), null);
                    }
                    catch (Exception ex)
                    {

                    }

                }

                retr.Add(row);

            }
            return new MasterResult<List<T>> { Data = retr, Elapsed = 0, Message = "", Result = true };

        }


        #endregion


        public static MasterResult<NTUPLE> YeniFatura(Belge_Post_Model P, string TABLE)
        {
            StackTrace stackTrace = new StackTrace();

            try
            {

                MasterResult<LG_001_01_INVOICE> MCLS = AppCommon.CreateAndFillObject<LG_001_01_INVOICE>(P, P.Belge.TRCODE);
                if (!MCLS.Result)
                    return new MasterResult<NTUPLE> { Data = null, Result = false, Elapsed = 0, Message = "Referans Model Bulunamadı" };


                LG_001_01_INVOICE FATBELGE = MCLS.Data;
                FATBELGE.CAPIBLOCK_CREADEDDATE = DateTime.Now;
                FATBELGE.CAPIBLOCK_MODIFIEDDATE = null;
                FATBELGE.CAPIBLOCK_CREATEDHOUR = short.Parse(DateTime.Now.Hour.ToString());
                FATBELGE.CAPIBLOCK_CREATEDMIN = short.Parse(DateTime.Now.Minute.ToString());
                FATBELGE.CAPIBLOCK_CREATEDSEC = short.Parse(DateTime.Now.Second.ToString());
                FATBELGE.GUID = Guid.NewGuid().ToString();
                FATBELGE.TRCODE = short.Parse(P.Belge.TRCODE.ToString());
                FATBELGE.GRPCODE = short.Parse(P.Belge.GRPCODE.ToString());
                return NExec.AdoInsert<LG_001_01_INVOICE>(FATBELGE, TABLE);





            }
            catch (Exception ex)
            {
                SqliteContext.addLog(AppCommon.LogModelBuild<Exception>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = ex.StackTrace, stat = 0 }, Elapsed = 0, Message = ex.Message, Result = false };

            }


        }





        public static MasterResult<NTUPLE> YeniIrsaliye(Belge_Post_Model P)
        {
            StackTrace stackTrace = new StackTrace();
#warning irsaliye
            try
            {

                MasterResult<LG_001_01_STFICHE> MCLS = AppCommon.CreateAndFillObject<LG_001_01_STFICHE>(P.Belge, P.Belge.TRCODE);
                if (!MCLS.Result)
                    return new MasterResult<NTUPLE> { Data = null, Result = false, Elapsed = 0, Message = "Referans Model Bulunamadı" };


                LG_001_01_STFICHE IRSBELGE = MCLS.Data;
                IRSBELGE.CAPIBLOCK_CREADEDDATE = DateTime.Now;
                IRSBELGE.CAPIBLOCK_MODIFIEDDATE = null;
                IRSBELGE.CAPIBLOCK_CREATEDHOUR = short.Parse(DateTime.Now.Hour.ToString());
                IRSBELGE.CAPIBLOCK_CREATEDMIN = short.Parse(DateTime.Now.Minute.ToString());
                IRSBELGE.CAPIBLOCK_CREATEDSEC = short.Parse(DateTime.Now.Second.ToString());
                IRSBELGE.GUID = Guid.NewGuid().ToString();
                IRSBELGE.TRCODE = short.Parse(P.Belge.TRCODE.ToString());
                IRSBELGE.GRPCODE = short.Parse(P.Belge.GRPCODE.ToString());
                IRSBELGE.SOURCEINDEX = short.Parse(P.Belge.SOURCEINDEX.ToString());
                IRSBELGE.SOURCECOSTGRP = short.Parse(P.Belge.SOURCECOSTGRP.ToString());
                IRSBELGE.DESTINDEX= short.Parse(P.Belge.DESTINDEX.ToString());
                IRSBELGE.DESTCOSTGRP = short.Parse(P.Belge.DESTCOSTGRP.ToString());
                IRSBELGE.FICHENO = P.Belge.IRSFICHENO;
                IRSBELGE.BILLED = P.Belge.BILLED;
                IRSBELGE.INVNO = P.Belge.INVNO;
                IRSBELGE.INVOICEREF =P.Belge.INVOICEREF;

              
                IRSBELGE.ACCOUNTREF = 0;
                IRSBELGE.ACCOUNTED = 0;
                IRSBELGE.TOTALDISCOUNTED = P.Line.Where(x=>x.LINETYPE==2).Sum(x=>x.TOTAL);
                IRSBELGE.TOTALVAT = P.Line.Sum(x=>x.VATAMNT);
                IRSBELGE.GROSSTOTAL = P.Line.Where(x => x.LINETYPE == 0).Sum(x=>x.TOTAL);
                IRSBELGE.NETTOTAL = P.Line.Sum(x => x.LINENET);
                IRSBELGE.REPORTRATE = 1;
                IRSBELGE.REPORTNET = IRSBELGE.NETTOTAL;
                IRSBELGE.SHIPDATE = null;
                IRSBELGE.APPROVEDATE = null;
                IRSBELGE.PRINTDATE = null;
                IRSBELGE.DOCDATE = DateTime.Parse(P.Belge.IRSDATE);
                IRSBELGE.CANCELDATE = null;
                IRSBELGE.GRPCODE = short.Parse(P.Belge.GRPCODE.ToString());
                IRSBELGE.IOCODE=short.Parse(P.Belge.IOCODE.ToString());
                IRSBELGE.DOCODE = "";
                IRSBELGE.TRACKNR = "";
                IRSBELGE.CYPHCODE = "";
                IRSBELGE.TRADINGGRP = "";
                IRSBELGE.DOCTRACKINGNR = "";
                IRSBELGE.DELIVERYCODE = "";
                IRSBELGE.SPECODE = ""; 
                IRSBELGE.DATE_= DateTime.Parse(P.Belge.IRSDATE);




                MasterResult<NTUPLE> islem= NExec.AdoInsert<LG_001_01_STFICHE>(IRSBELGE,P.HEADTABLE);
                if (!islem.Result) return new MasterResult<NTUPLE> { Data =islem.Data, Elapsed = 0, Message = islem.Message, Result = false };


                var belge = NQery.AdoFind<LG_001_01_STFICHE>(P.HEADTABLE, string.Format(" FICHENO='{0}'", P.Belge.IRSFICHENO));
                List<NTUPLE> ntp = new List<NTUPLE>();

                List<LG_001_01_STLINE> LINES = new List<LG_001_01_STLINE>();

                short k = 0;
                foreach (var item in P.Line.OrderBy(x=>x.LINENO))
                {
                    k++;
                    MasterResult<LG_001_01_STLINE> LINEREF = AppCommon.CreateAndFillObject<LG_001_01_STLINE>(item, P.Belge.TRCODE);
                    if (!LINEREF.Result) {
                        ntp.Add( new NTUPLE { rec="", stat=0 });
                        break;
                    }

                    LG_001_01_STLINE KL = LINEREF.Data;
                    KL.STFICHEREF = belge.Data.First().LOGICALREF;
                    KL.STFICHELNNO = k;
                    KL.BILLED = P.Belge.BILLED;
                    KL.DATE_ = belge.Data.First().DATE_;
                    KL.CLIENTREF = belge.Data.First().CLIENTREF;
                    KL.GUID = Guid.NewGuid().ToString();
                    KL.TRCODE = short.Parse(P.Belge.TRCODE.ToString());
                    KL.INVOICEREF = item.INVOICEREF;
                    KL.INVOICELNNO = short.Parse(item.INVOICELNNO.ToString());
                    KL.PRPRICE = item.PRICE;
                    KL.VATAMNT = item.VATAMNT;
                    KL.LINENET = item.LINENET;
                    KL.TOTAL = item.TOTAL;
                    KL.VATINC = P.Belge.KDV_DAHILMI.toShortparse<int>();
                    KL.REPORTRATE = 1;
                    KL.RECSTATUS = 1;
                    KL.AFFECTRISK = 1;
                    KL.ACCOUNTREF = item.ACCOUNTREF; 
                    KL.VATACCREF = item.VATACCREF;
                    KL.PURCHACCREF = 0;
                    KL.PURCHACCREFUFRS = 0;
                    KL.SOURCEINDEX = IRSBELGE.SOURCEINDEX;
                    KL.SOURCECOSTGRP = IRSBELGE.SOURCECOSTGRP;
                    KL.DESTINDEX = IRSBELGE.DESTINDEX;
                    KL.DESTCOSTGRP = IRSBELGE.DESTCOSTGRP;
                    LINES.Add(KL);
                    MasterResult<NTUPLE> islemi = NExec.AdoInsert<LG_001_01_STLINE>(KL, P.LISTTABLE);
                    ntp.Add(islemi.Data);

                }

                if (ntp.Where(x=>x.stat==0).Count()>0)
                {
                    NExec.AdoDelete<LG_001_01_STFICHE>(P.HEADTABLE, string.Format(" where LOGICALREF={0}", belge.Data.First().LOGICALREF));
                    NExec.AdoDelete<LG_001_01_STLINE>(P.LISTTABLE, string.Format(" where STFICHEREF={0}", belge.Data.First().LOGICALREF));
                    return new MasterResult<NTUPLE> { Data = new NTUPLE{ rec="", stat=0 }, Elapsed = 0, Message ="İrsaliye Oluşturulurken Bazı işlemler Hata Aldı Kayıt Geri Alındı !", Result = false };
                }





             return new MasterResult<NTUPLE> { Data = new NTUPLE{ rec = "", stat = 0 }, Elapsed = 0, Message = "İrsaliye Oluşturuldu !", Result = true };


            }
            catch (Exception ex)
            {
                SqliteContext.addLog(AppCommon.LogModelBuild<Exception>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = ex.StackTrace, stat = 0 }, Elapsed = 0, Message = ex.Message, Result = false };

            }


        }



       

        public static MasterResult<NTUPLE> YeniTransfer(Belge_Post_Model P)
        {
            StackTrace stackTrace = new StackTrace();

            try
            {

                MasterResult<LG_001_01_STFICHE> MCLS = AppCommon.CreateAndFillObject<LG_001_01_STFICHE>(P.Belge, P.Belge.TRCODE);
                if (!MCLS.Result)
                    return new MasterResult<NTUPLE> { Data = null, Result = false, Elapsed = 0, Message = "Referans Model Bulunamadı" };


                LG_001_01_STFICHE IRSBELGE = MCLS.Data;
                IRSBELGE.CAPIBLOCK_CREADEDDATE = DateTime.Now;
                IRSBELGE.CAPIBLOCK_MODIFIEDDATE = null;
                IRSBELGE.CAPIBLOCK_CREATEDHOUR = short.Parse(DateTime.Now.Hour.ToString());
                IRSBELGE.CAPIBLOCK_CREATEDMIN = short.Parse(DateTime.Now.Minute.ToString());
                IRSBELGE.CAPIBLOCK_CREATEDSEC = short.Parse(DateTime.Now.Second.ToString());
                IRSBELGE.GUID = Guid.NewGuid().ToString();
                IRSBELGE.TRCODE = (int)EMalzemeFisTips.Ambar_Fisi;
                IRSBELGE.GRPCODE = (int)EBelgeGurupKodlari.Malzeme_Fisleri;
                IRSBELGE.SOURCEINDEX = short.Parse(P.Belge.SOURCEINDEX.ToString());
                IRSBELGE.SOURCECOSTGRP = short.Parse(P.Belge.SOURCECOSTGRP.ToString());
                IRSBELGE.DESTINDEX = short.Parse(P.Belge.DESTINDEX.ToString());
                IRSBELGE.DESTCOSTGRP = short.Parse(P.Belge.DESTCOSTGRP.ToString());

                IRSBELGE.BILLED = 0;
                IRSBELGE.INVNO = "";
                IRSBELGE.INVOICEREF = 0;

                IRSBELGE.FICHENO = P.Belge.FICHENO;
                IRSBELGE.ACCOUNTREF = 0;
                IRSBELGE.ACCOUNTED = 0;
                IRSBELGE.TOTALDISCOUNTED = 0;
                IRSBELGE.TOTALVAT = P.Line.Where(x=>x.IOCODE==2).Sum(x => x.VATAMNT);
                IRSBELGE.GROSSTOTAL = P.Line.Where(x => x.IOCODE == 2).Sum(x => x.TOTAL);
                IRSBELGE.NETTOTAL = P.Line.Where(x => x.IOCODE == 2).Sum(x => x.LINENET);
                IRSBELGE.REPORTRATE = 0;
                IRSBELGE.REPORTNET = IRSBELGE.NETTOTAL;
                IRSBELGE.SHIPDATE = null;
                IRSBELGE.APPROVEDATE = null;
                IRSBELGE.PRINTDATE = null;
                IRSBELGE.DOCDATE = DateTime.Now;
                IRSBELGE.CANCELDATE = null;
                IRSBELGE.GRPCODE = 3;
                IRSBELGE.IOCODE = 2;
                IRSBELGE.DOCODE = "";
                IRSBELGE.TRACKNR = "";
                IRSBELGE.CYPHCODE = "";
                IRSBELGE.TRADINGGRP = "";
                IRSBELGE.DOCTRACKINGNR = "";
                IRSBELGE.DELIVERYCODE = "";


                MasterResult<NTUPLE> islem = NExec.AdoInsert<LG_001_01_STFICHE>(IRSBELGE, P.HEADTABLE);
                if (!islem.Result) return new MasterResult<NTUPLE> { Data = islem.Data, Elapsed = 0, Message = islem.Message, Result = false };


                var belge = NQery.AdoFind<LG_001_01_STFICHE>(P.HEADTABLE, string.Format(" FICHENO='{0}'", P.Belge.FICHENO));
                List<NTUPLE> ntp = new List<NTUPLE>();

                List<LG_001_01_STLINE> LINES = new List<LG_001_01_STLINE>();

                short k = 0;
                foreach (var item in P.Line)
                {
                    k++;
                    MasterResult<LG_001_01_STLINE> LINEREF = AppCommon.CreateAndFillObject<LG_001_01_STLINE>(item, P.Belge.TRCODE);
                    if (!LINEREF.Result)
                    {
                        ntp.Add(new NTUPLE { rec = "", stat = 0 });
                        break;
                    }

                    LG_001_01_STLINE KL = LINEREF.Data;
                    KL.STFICHEREF = belge.Data.First().LOGICALREF;
                    KL.STFICHELNNO = k;
                    KL.VATINC = 0;
                    KL.BILLED = 0;
                    KL.DATE_ = belge.Data.First().DATE_;
                    KL.CLIENTREF = 0;
                    KL.GUID = Guid.NewGuid().ToString();
                    KL.TRCODE = 25;
                    KL.INVOICEREF = 0;
                    KL.INVOICELNNO =0;
                    KL.PRPRICE = item.PRICE;
                    KL.VATAMNT = item.VATAMNT;
                    KL.LINENET = item.LINENET;
                    KL.TOTAL = item.TOTAL;
                    KL.ACCOUNTREF = 0;
                    KL.VATACCREF = 0;
                    KL.PURCHACCREF = 0;
                    KL.PURCHACCREFUFRS = 0;
                    KL.SOURCEINDEX = IRSBELGE.SOURCEINDEX;
                    KL.SOURCECOSTGRP = IRSBELGE.SOURCECOSTGRP;
                    KL.DESTINDEX = IRSBELGE.DESTINDEX;
                    KL.DESTCOSTGRP = IRSBELGE.DESTCOSTGRP;
                    KL.IOCODE = 3;
                    LINES.Add(KL);

                    LG_001_01_STLINE KL2 = LINEREF.Data;
                    KL2.STFICHEREF = belge.Data.First().LOGICALREF;
                    KL2.STFICHELNNO = k;
                    KL2.BILLED = 0;
                    KL2.DATE_ = belge.Data.First().DATE_;
                    KL2.CLIENTREF = 0;
                    KL2.GUID = Guid.NewGuid().ToString();
                    KL2.TRCODE = 25;
                    KL2.INVOICEREF = 0;
                    KL2.INVOICELNNO = 0;
                    KL2.PRPRICE = item.PRICE;
                    KL2.VATAMNT = item.VATAMNT;
                    KL2.LINENET = item.LINENET;
                    KL2.TOTAL = item.TOTAL;
                    KL2.ACCOUNTREF = 0;
                    KL2.VATACCREF = 0;
                    KL2.PURCHACCREF = 0;
                    KL2.PURCHACCREFUFRS = 0;
                    KL2.SOURCEINDEX = IRSBELGE.DESTINDEX;
                    KL2.SOURCECOSTGRP = IRSBELGE.DESTCOSTGRP;
                    KL2.DESTINDEX = IRSBELGE.SOURCEINDEX;
                    KL2.DESTCOSTGRP = IRSBELGE.SOURCECOSTGRP;
                    LINES.Add(KL2);




                    MasterResult<NTUPLE> islemi = NExec.AdoInsert<LG_001_01_STLINE>(KL, P.LISTTABLE);
                    MasterResult<NTUPLE> islemi2 = NExec.AdoInsert<LG_001_01_STLINE>(KL2, P.LISTTABLE);
                    ntp.Add(islemi.Data);
                    ntp.Add(islemi2.Data);

                }

                if (ntp.Where(x => x.stat == 0).Count() > 0)
                {
                    NExec.AdoDelete<LG_001_01_STFICHE>(P.HEADTABLE, string.Format(" where LOGICALREF={0}", belge.Data.First().LOGICALREF));
                    NExec.AdoDelete<LG_001_01_STLINE>(P.LISTTABLE, string.Format(" where STFICHEREF={0}", belge.Data.First().LOGICALREF));
                    return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = "", stat = 0 }, Elapsed = 0, Message = "İrsaliye Oluşturulurken Bazı işlemler Hata Aldı Kayıt Geri Alındı !", Result = false };
                }





                return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = "", stat = 0 }, Elapsed = 0, Message = "İrsaliye Oluşturuldu !", Result = true };


            }
            catch (Exception ex)
            {
                SqliteContext.addLog(AppCommon.LogModelBuild<Exception>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = ex.StackTrace, stat = 0 }, Elapsed = 0, Message = ex.Message, Result = false };

            }


        }


        public static MasterResult<NTUPLE> YeniFatura(Belge_Post_Model P)
        {
#warning fatura Kayıt
            StackTrace stackTrace = new StackTrace();

            try
            {

                MasterResult<LG_001_01_INVOICE> MCLS = AppCommon.CreateAndFillObject<LG_001_01_INVOICE>(P.Belge, P.Belge.TRCODE);
                if (!MCLS.Result)
                    return new MasterResult<NTUPLE> { Data = null, Result = false, Elapsed = 0, Message = "Referans Model Bulunamadı" };


                LG_001_01_INVOICE FATBELGE = MCLS.Data;
                FATBELGE.CAPIBLOCK_CREADEDDATE = DateTime.Now;
                FATBELGE.CAPIBLOCK_MODIFIEDDATE = null;
                FATBELGE.CAPIBLOCK_CREATEDHOUR = short.Parse(DateTime.Now.Hour.ToString());
                FATBELGE.CAPIBLOCK_CREATEDMIN = short.Parse(DateTime.Now.Minute.ToString());
                FATBELGE.CAPIBLOCK_CREATEDSEC = short.Parse(DateTime.Now.Second.ToString());
                FATBELGE.GUID = Guid.NewGuid().ToString();
                FATBELGE.TRCODE = short.Parse(P.Belge.TRCODE.ToString());
                FATBELGE.GRPCODE = short.Parse(P.Belge.GRPCODE.ToString());
                FATBELGE.FICHENO = P.Belge.FICHENO;
                FATBELGE.ACCOUNTREF = 0;
                FATBELGE.ACCOUNTED = 0;
                FATBELGE.TOTALDISCOUNTED = P.Line.Where(x=>x.LINETYPE==double.Parse("2")).Sum(x=>x.TOTAL);
                FATBELGE.TOTALVAT = P.Line.Sum(x => x.VATAMNT);
                FATBELGE.SPECODE = "";
                FATBELGE.ENTEGSET = 247;
                if (P.Belge.TRCODE == (int)EFaturaTip.ToptanSatis_Faturasi)
                {
                    FATBELGE.GROSSTOTAL = P.Line.Where(x=>x.LINETYPE==0).Sum(x => x.TOTAL) + P.Line.Sum(x => x.VATAMNT);
                    FATBELGE.NETTOTAL = P.Line.Where(x => x.LINETYPE == 0).Sum(x => x.LINENET) + P.Line.Sum(x => x.VATAMNT);
                }
                else
                {
                    FATBELGE.GROSSTOTAL = P.Line.Where(x => x.LINETYPE == 0).Sum(x => x.TOTAL) ;
                    FATBELGE.NETTOTAL = P.Line.Where(x => x.LINETYPE == 0).Sum(x => x.LINENET) ;

                }
              
                FATBELGE.REPORTRATE = 1;
                FATBELGE.REPORTNET = FATBELGE.NETTOTAL;
                FATBELGE.TRNET=FATBELGE.NETTOTAL;
                FATBELGE.DATE_ = P.Belge.DATE_;
                
       
                FATBELGE.APPROVEDATE = null;
                FATBELGE.CAPIBLOCK_MODIFIEDDATE = null;
                FATBELGE.PRINTDATE = null;
                FATBELGE.DOCDATE = DateTime.Now;
                FATBELGE.CANCELDATE = null;
                FATBELGE.EINVOICE = P.Belge.EINVOICE;
                FATBELGE.GRPCODE = short.Parse(P.Belge.GRPCODE.ToString());
                FATBELGE.DOCODE = P.Belge.FICHENO;
                FATBELGE.DEDUCTIONPART1 = 2;
                FATBELGE.DEDUCTIONPART2 = 3;
                FATBELGE.RECSTATUS = 1;
               
                FATBELGE.TRACKNR = "";
                MasterResult<NTUPLE> islem = NExec.AdoInsert<LG_001_01_INVOICE>(FATBELGE, P.HEADTABLE);
                    if (!islem.Result) return new MasterResult<NTUPLE> { Data = islem.Data, Elapsed = 0, Message = islem.Message, Result = false };
                    return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = "", stat = 0 }, Elapsed = 0, Message = "Fatura Oluşturuldu !", Result = true };


            }
            catch (Exception ex)
            {
                SqliteContext.addLog(AppCommon.LogModelBuild<Exception>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = ex.StackTrace, stat = 0 }, Elapsed = 0, Message = ex.Message, Result = false };

            }


        }





   





        public static SqlCommand inserCommandCreator<T>(object document, string Table)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbp = new StringBuilder();
            sb.Append(string.Format("insert into {0} (", Table));
            sbp.Append(string.Format("values  ("));
            int K = 1;
            int count = typeof(T).GetProperties().Count();
            foreach (var item in typeof(T).GetProperties())
            {
              string sti = item.Name;
             if (!sti.Equals("LOGICALREF"))
              {

               
                if (count > K)
                {
                    sb.Append(string.Format("{0},", sti));
                    sbp.Append(string.Format("@{0},", sti));
               
                }
                else if(count==K)
                {
                    sb.Append(string.Format("{0})", sti));
                    sbp.Append(string.Format("@{0})", sti));
                  
                }
             }
                K++;



            }

            sb.Append(sbp.ToString());

            SqlCommand cm = new SqlCommand(sb.ToString());
            cm.Parameters.Clear();
            foreach (var item in typeof(T).GetProperties())
            {
                if (!item.Name.Equals("LOGICALREF"))
                {
                    Type tip = item.PropertyType;
                    var val = document.GetType().GetProperty(item.Name).GetValue(document, null);
                    if (val == null)
                    {
                        if (tip == typeof(Int32) || tip == typeof(Int16) || tip == typeof(Int64) || tip == typeof(float) || tip == typeof(Double) || tip == typeof(Boolean)){ val = 0; }
                        else if (tip == typeof(DateTime)) { val = DateTime.Now; }
                        else { val = ""; }
                    }
                     cm.Parameters.Add(new SqlParameter(string.Format("@{0}", item.Name), val));
                }

            }

            return cm;
        }

        public static SqlCommand inserCommandCreatorT<T>(this object document, string Table)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbp = new StringBuilder();
            sb.Append(string.Format("insert into {0} (", Table));
            sbp.Append(string.Format("values  ("));
            int K = 1;
            int count = typeof(T).GetProperties().Count();
            foreach (var item in typeof(T).GetProperties())
            {
                string sti = item.Name;
                if (!sti.Equals("LOGICALREF"))
                {


                    if (count > K)
                    {
                        sb.Append(string.Format("{0},", sti));
                        sbp.Append(string.Format("@{0},", sti));

                    }
                    else if (count == K)
                    {
                        sb.Append(string.Format("{0})", sti));
                        sbp.Append(string.Format("@{0})", sti));

                    }
                }
                K++;



            }

            sb.Append(sbp.ToString());

            SqlCommand cm = new SqlCommand(sb.ToString());
            cm.Parameters.Clear();
            foreach (var item in typeof(T).GetProperties())
            {
                if (!item.Name.Equals("LOGICALREF"))
                {
                    Type tip = item.PropertyType;
                    var val = document.GetType().GetProperty(item.Name).GetValue(document, null);
                    if (val == null)
                    {
                        if (tip == typeof(Int32) || tip == typeof(Int16) || tip == typeof(Int64) || tip == typeof(float) || tip == typeof(Double) || tip == typeof(Boolean)) { val = 0; }
                        else if (tip == typeof(DateTime)) { val = DateTime.Now; }
                        else { val = ""; }
                    }
                    cm.Parameters.Add(new SqlParameter(string.Format("@{0}", item.Name), val));
                }

            }

            return cm;
        }


        public static SqlCommand insertCommandCreator<T>(this object document)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbp = new StringBuilder();
            sb.Append(string.Format("insert into {0} (", typeof(T).Name));
            sbp.Append(string.Format("values  ("));
            int K = 1;
            int count = typeof(T).GetProperties().Count();
            foreach (var item in typeof(T).GetProperties())
            {
                string sti = item.Name;
                if (!sti.Equals("LOGICALREF"))
                {
                    if (count > K)
                    {
                        sb.Append(string.Format("{0},", sti));
                        sbp.Append(string.Format("@{0},", sti));

                    }
                    else if (count == K)
                    {
                        sb.Append(string.Format("{0})", sti));
                        sbp.Append(string.Format("@{0})", sti));

                    }

              
                }

                 K++;

            }

            sb.Append(sbp.ToString());

            SqlCommand cm = new SqlCommand(sb.ToString());
            cm.Parameters.Clear();
            foreach (var item in typeof(T).GetProperties())
            {
               
                    Type tip = item.PropertyType;
                    var val = document.GetType().GetProperty(item.Name).GetValue(document, null);
                    if (val == null)
                    {
                        if (tip == typeof(Int32) || tip == typeof(Int16) || tip == typeof(Int64) || tip == typeof(float) || tip == typeof(Double) || tip == typeof(Boolean)) { val = 0; }
                        else if (tip == typeof(DateTime)) { val = DateTime.Now; }
                        else { val = ""; }
                    }
                    cm.Parameters.Add(new SqlParameter(string.Format("@{0}", item.Name), val));
                

            }

            return cm;
        }


        public static SqlCommand UpdateCommandCreator<T>(object document, string Table,string conditions)
        {
            StringBuilder sb = new StringBuilder();
           
            sb.Append(string.Format("Update {0} set ", Table));
        
            int K = 1;
            int count = typeof(T).GetProperties().Count();
            foreach (var item in typeof(T).GetProperties())
            {
                string sti = item.Name;
                if (!sti.Equals("LOGICALREF"))
                {


                    if (count > K)
                    {
                        sb.Append(string.Format("{0}=@{0},", sti));
                      

                    }
                    else if (count == K)
                    {
                        sb.Append(string.Format("{0}=@{0} ", sti));
                 

                    }
                }
                K++;
                


            }

       sb.Append(conditions);

            SqlCommand cm = new SqlCommand(sb.ToString());
            cm.Parameters.Clear();
            foreach (var item in typeof(T).GetProperties())
            {
                if (!item.Name.Equals("LOGICALREF"))
                {
                    Type tip = item.PropertyType;
                    var val = document.GetType().GetProperty(item.Name).GetValue(document, null);
                    if (val == null)
                    {
                        if (tip == typeof(Int32) || tip == typeof(Int16) || tip == typeof(Int64) || tip == typeof(float) || tip == typeof(Double) || tip == typeof(Boolean)) { val = 0; }
                        else if (tip == typeof(DateTime)) { val = DateTime.Now; }
                        else { val = ""; }
                    }
                    cm.Parameters.Add(new SqlParameter(string.Format("@{0}", item.Name), val));
                }

            }

            return cm;
        }
        public static SqlCommand DeleteCommandCreator<T>(string Table, string conditions)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("Delete from  {0} {1} ", Table,conditions));
            SqlCommand cm = new SqlCommand(sb.ToString());
       

            return cm;
        }


        public static SQLiteCommand sqlIteInsertCommandCreator<T>(object document, string Table)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbp = new StringBuilder();
            sb.Append(string.Format("insert into {0} (", Table));
            sbp.Append(string.Format("values  ("));
            int K = 1;
            int count = typeof(T).GetProperties().Count();
            foreach (var item in typeof(T).GetProperties())
            {
                string sti = item.Name;
                if (!sti.Equals("id"))
                {


                    if (count > K)
                    {
                        sb.Append(string.Format("{0},", sti));
                        sbp.Append(string.Format("@{0},", sti));

                    }
                    else if (count == K)
                    {
                        sb.Append(string.Format("{0})", sti));
                        sbp.Append(string.Format("@{0})", sti));

                    }
                }
                K++;



            }

            sb.Append(sbp.ToString());

            SQLiteCommand cm = new SQLiteCommand(sb.ToString());
            cm.Parameters.Clear();
            foreach (var item in typeof(T).GetProperties())
            {
                if (!item.Name.Equals("id"))
                {
                    Type tip = item.PropertyType;
                    var val = document.GetType().GetProperty(item.Name).GetValue(document, null);
                    if (val == null)
                    {
                        if (tip == typeof(Int32) || tip == typeof(Int16) || tip == typeof(Int64) || tip == typeof(float) || tip == typeof(Double) || tip == typeof(Boolean)) { val = 0; }
                        else if (tip == typeof(DateTime)) { val = DateTime.Now; }
                        else { val = ""; }
                    }
                   // cm.Parameters.Add(new SQLiteParameter(string.Format("@{0}", item.Name), val));
                    cm.Parameters.AddWithValue(string.Format("@{0}", item.Name), val);
                }

            }

            return cm;
        }

        public static Log_Model LogModelBuild<T>(T ex,int stat, MethodBase MB) {

            string Type = ex.GetType().ToString();
            string Message = ex.GetType().GetProperty("Message").GetValue(ex, null).ToString();
            string StackTrace = ex.GetType().GetProperty("StackTrace").GetValue(ex, null).ToString();
            var error = new Dictionary<string, string>
         {
        {"Type", Type},
        {"Message", Message},
        {"StackTrace", StackTrace}
        };
            return new Log_Model { date=string.Format("{0:yyyy-MM-dd HH:mm:ss}",DateTime.Now), desc=Message, procname=MB.Name, details= "", status=stat, location= MB.DeclaringType.AssemblyQualifiedName };
             
        }

        public static string SerializeJson<T>(T data) {
            JavaScriptSerializer jsoncreator = new JavaScriptSerializer();
            jsoncreator.MaxJsonLength = int.MaxValue;
            try
            {
                return jsoncreator.Serialize(data);
            }
            catch (Exception ex)
            {
                return ex.Message;
      
            }
          

        }

      



       

    }

}
