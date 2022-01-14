using LogoGo3Data.DefineModel;
using LogoGo3Data.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static LogoGo3Data.Extras.Utils;

namespace LogoGo3Data.Context
{
   public class SqliteContext
    {
       
       // SQLiteConnection cnn = new SQLiteConnection("");


        public static SQLiteConnection  getConnectionSqlite() {

            SQLiteConnectionStringBuilder connSB = new SQLiteConnectionStringBuilder();
           
            //string dbLocation= HttpContext.Current.Server.MapPath(@"~\bin\Context\Log\Log.db");
            connSB.DataSource = GenerateProcess.getPath(@"\Context\Log\Log.db");
            connSB.FailIfMissing = false;
         
     

         
            return new SQLiteConnection(connSB.ConnectionString);
            
          
        }

        public static void addLog(Log_Model M) {
            using (var connect = getConnectionSqlite()) {
                SQLiteCommand command = AppCommon.sqlIteInsertCommandCreator<Log_Model>(M,"T_Log");
                try
                {

                    command.Connection = connect;
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }

                catch (SQLiteException ex)
                {

                }
                catch (Exception ex)
                {


                }
                finally {
                    command.Connection.Close();
                }
                
            }

        }

        public static void addReqLog(ReqLog M)
        {
            using (var connect = getConnectionSqlite())
            {
                SQLiteCommand command = AppCommon.sqlIteInsertCommandCreator<ReqLog>(M, "ReqLog");
                try
                {

                    command.Connection = connect;
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }

                catch (SQLiteException ex)
                {

                }
                catch (Exception ex)
                {


                }
                finally
                {
                    command.Connection.Close();
                }

            }

        }

        public static NTUPLE InsertModelSqlite<T>(object data,int trcode) {

            NTUPLE dlt = DeleteRefModel<T>(trcode);
          using (var connect = getConnectionSqlite())
            {
                SQLiteCommand command = AppCommon.sqlIteInsertCommandCreator<T>(data, typeof(T).Name);
                try
                {

                    command.Connection = connect;
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                    return new NTUPLE { stat = 1, rec = string.Format("Referans Oluşturuldu {0}", typeof(T).Name) };
                }

                catch (SQLiteException ex)
                {
                    return new NTUPLE { stat = 0, rec = string.Format("{0} - {1}", ex.Message, typeof(T).Name) };
                }
                catch (Exception ex)
                {
                    return new NTUPLE { stat = 0, rec = string.Format("{0} - {1}", ex.Message, typeof(T).Name) };

                }
                finally
                {
                    command.Connection.Close();
                }


            }


        }

        public static NTUPLE DeleteRefModel<T>(int trcode) {

            using (var connect = getConnectionSqlite())
            { SQLiteCommand command = null;
                if (trcode != 0) { command = new SQLiteCommand(string.Format("delete from {0} where TRCODE={1}", typeof(T).Name, trcode)); }
                else {command = new SQLiteCommand(string.Format("delete from {0}", typeof(T).Name)); }
                
                try
                {

                    command.Connection = connect;
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                    return new NTUPLE { stat = 1, rec = string.Format("Referans silindi {0}", typeof(T).Name) };
                }

                catch (SQLiteException ex)
                {
                    return new NTUPLE { stat = 0, rec = string.Format("{0} - {1}", ex.Message, typeof(T).Name) };
                }
                catch (Exception ex)
                {
                    return new NTUPLE { stat = 0, rec = string.Format("{0} - {1}", ex.Message, typeof(T).Name) };

                }
                finally
                {
                    command.Connection.Close();
                }


            }

            return new NTUPLE();

        }


        public static MasterResult<T> GetSqliteDefModel<T>(int Type) where T:new () {

            //-2147481665 syntax error

            T obj = new T();
            using (var connect = getConnectionSqlite())
            {
                try
                {
                    SQLiteDataAdapter adp = null;
                    if (Type==0)
                    { adp = new SQLiteDataAdapter(string.Format("select *  from {0}", typeof(T).Name), connect); }
                    else {adp = new SQLiteDataAdapter(string.Format("select *  from {0} where TRCODE={1}", typeof(T).Name,Type), connect); }
                   
                    DataTable tbl = new DataTable(typeof(T).Name);
                    adp.Fill(tbl);
                    if (tbl.Rows.Count == 0)
                        return new MasterResult<T> { Data = obj, Elapsed = 0, Message = "Kayıt Bulunamadı", Result = false };

                    obj = AppCommon.convertToEnumerable<T>(tbl).Data[0];
                    return new MasterResult<T> { Data = obj, Elapsed = 0, Message = "", Result = true };
                }
                catch (SQLiteException ex) {
                    //
                    if (ex.Message.Contains("no such table"))
                        return new MasterResult<T> { Data = obj, Elapsed = 12, Message = ex.Message, Result = false };
                    else 
                        return new MasterResult<T> { Data = obj, Elapsed = 0, Message = ex.Message, Result = false };

                }

                catch (Exception ex)
                {
                    return new MasterResult<T> { Data = obj, Elapsed = 0, Message = ex.Message, Result = false };

                }


            }


            }

        public static MasterResult<List<T>> GetSqliteData<T>() where T : new()
        {

            //-2147481665 syntax error

    
            using (var connect = getConnectionSqlite())
            {
                try
                {
                    SQLiteDataAdapter adp = null;
                     adp = new SQLiteDataAdapter(string.Format("select *  from {0}", typeof(T).Name), connect); 
                
                    DataTable tbl = new DataTable(typeof(T).Name);
                    adp.Fill(tbl);
                    if (tbl.Rows.Count == 0)
                        return new MasterResult<List<T>> { Data = new List<T>(), Elapsed = 0, Message = "Kayıt Bulunamadı", Result = false };

                  return AppCommon.convertToEnumerable<T>(tbl);
               }
                catch (SQLiteException ex)
                {
                    //
                    if (ex.Message.Contains("no such table"))
                        return new MasterResult<List<T>> { Data = new List<T>(), Elapsed = 12, Message = ex.Message, Result = false };
                    else
                        return new MasterResult<List<T>> { Data = new List<T>(), Elapsed = 0, Message = ex.Message, Result = false };

                }

                catch (Exception ex)
                {
                    return new MasterResult<List<T>> { Data = new List<T>(), Elapsed = 0, Message = ex.Message, Result = false };

                }


            }


        }



        public static NTUPLE CreateTable<T>()where T:new () {

            if (GetSqliteDefModel<T>(0).Elapsed!=12)
                return new NTUPLE { rec=string.Format("Tablo Zaten Var {0}",typeof(T).Name), stat=0 };

             using (var connect = getConnectionSqlite())
            {
                SQLiteCommand command = CreateTableQueryBuilder<T>();
                try
                {

                    command.Connection = connect;
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                    return new NTUPLE { stat=1, rec = string.Format("Tablo Oluşturuldu {0}", typeof(T).Name) };
                }

                catch (SQLiteException ex)
                {
                    return new NTUPLE { stat = 0, rec = string.Format("{0} - {1}", ex.Message, typeof(T).Name) };
                }
                catch (Exception ex)
                {

                    return new NTUPLE { stat = 0, rec = string.Format("{0} - {1}", ex.Message, typeof(T).Name) };
                }
                finally
                {
                    command.Connection.Close();
                }


            }


            }

        public static SQLiteCommand CreateTableQueryBuilder<T>() {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("Create Table {0} (", typeof(T).Name));
            int k = 0;
            foreach (var item in typeof(T).GetProperties())
            {
                k++;
                Type tp = item.PropertyType;
                string dbtype = "";
                if (tp == typeof(Int16) || tp == typeof(Int32) || tp == typeof(Int64))
                {
                    dbtype = "INTEGER";
                }
                else if (tp == typeof(Decimal) || tp == typeof(Double) || tp == typeof(float))
                {
                    dbtype = "REAL";
                }

                else
                {
                    dbtype = "TEXT";
                }

                if (k != typeof(T).GetProperties().Length)
                {
                    sb.Append(string.Format("{0} {1},", item.Name, dbtype));
                }
                else
                {
                    sb.Append(string.Format("{0} {1}", item.Name, dbtype));
                }


            }

            sb.Append(")");

            return new SQLiteCommand(sb.ToString());
        }


      


    }
}
