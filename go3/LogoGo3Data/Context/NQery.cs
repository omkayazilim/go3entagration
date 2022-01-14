using LogoGo3Data.Context;
using LogoGo3Data.DefineModel;
using LogoGo3Data.Extras;
using LogoGo3Data.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using static LogoGo3Data.Extras.Utils;

namespace LogoGo3Data
{
    public static class NQery
    {

        public static MasterResult<List<T>> Find<T>(string SQL)
        {



            try
            {
                var db = new logodataDataContext(Utils.Cnn);
                string tblname = typeof(T).Name;
                var table = db.GetTable(typeof(T));

                var cmd = db.GetCommand(table.AsQueryable());
                var result = db.ExecuteQuery<T>(string.Format("{0} where {1}", cmd.CommandText, SQL)).ToList();
                // SysLogsData.Info("Data.DbActions/Query/findlambda", "İşlem Başarılı");
            
                return new MasterResult<List<T>> {
                    Data = result,
                    Elapsed = 0,
                    Message = result.Count > 0 ? string.Format("{0} satır Listelendi", result.Count) : string.Format("{0} satır Listelendi", result.Count),
                    Result = result.Count > 0 ? true : false

                };



            }
            catch (Exception ex)
            {

                return new MasterResult<List<T>>
                {
                    Data = null,
                    Elapsed = 0,
                    Message = ex.Message,
                    Result = false

                };
            }

        }


        public static MasterResult<List<T>> Find<T>()
        {



            try
            {
                using (var db = new logodataDataContext(Utils.Cnn)) {



                    var table = db.GetTable(typeof(T));
                    var tablename = table.Expression;
                    var cmd = db.GetCommand(table.AsQueryable());

                    var result = db.ExecuteQuery<T>(string.Format("{0}", cmd.CommandText)).ToList();
                    // SysLogsData.Info("Data.DbActions/Query/findlambda", "İşlem Başarılı");
                    return new MasterResult<List<T>>
                    {
                        Data = result,
                        Elapsed = 0,
                        Message = result.Count > 0 ? string.Format("{0} satır Listelendi", result.Count) : string.Format("{0} satır Listelendi", result.Count),
                        Result = result.Count > 0 ? true : false

                    };

                }

            }
            catch (Exception ex)
            {

                return new MasterResult<List<T>>
                {
                    Data = null,
                    Elapsed = 0,
                    Message = ex.Message,
                    Result = false

                };
            }

        }

        public static MasterResult<List<T>> Find<T>(Func<T, bool> f)
        {



            try
            {
                using (var db = new logodataDataContext(Utils.Cnn))
                {



                    var table = db.GetTable(typeof(T));
                    var tablename = table.Expression;
                    var cmd = db.GetCommand(table.AsQueryable());

                    var result = db.ExecuteQuery<T>(string.Format("{0}", cmd.CommandText)).Where(f).ToList();
                    // SysLogsData.Info("Data.DbActions/Query/findlambda", "İşlem Başarılı");
                    return new MasterResult<List<T>>
                    {
                        Data = result,
                        Elapsed = 0,
                        Message = result.Count > 0 ? string.Format("{0} satır Listelendi", result.Count) : string.Format("{0} satır Listelendi", result.Count),
                        Result = result.Count > 0 ? true : false

                    };

                }

            }
            catch (Exception ex)
            {

                return new MasterResult<List<T>>
                {
                    Data = null,
                    Elapsed = 0,
                    Message = ex.Message,
                    Result = false

                };
            }

        }

        public static MasterResult<List<T>> AdoFind<T>(string Table) where T : new()
        {
                  StackTrace stackTrace = new StackTrace();
                  

            using (SqlConnection cnn = new SqlConnection(Utils.Cnn)) {

                try
                {
                    SqlDataAdapter ad = new SqlDataAdapter(string.Format("select * from  {0} ", Table), cnn);
                    DataTable tbl = new DataTable(Table);
                    ad.Fill(tbl);
                    if (tbl.Rows.Count == 0)
                        return new MasterResult<List<T>> { Data = null, Elapsed = 0, Message = "Kayıt Bulunamadı", Result = false };


                   return AppCommon.convertToEnumerable<T>(tbl);
                }
                catch (SqlException ex)
                {
                    SqliteContext.addLog(AppCommon.LogModelBuild<SqlException>(ex,0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<List<T>> { Result = false, Message = ex.Message, Elapsed = 0, Data = null };

                }
                catch (Exception ex) {
                    SqliteContext.addLog(AppCommon.LogModelBuild<Exception>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<List<T>> { Result = false, Message = ex.Message, Elapsed = 0, Data = null };
                }
              
               

            }

        }


        public static MasterResult<List<T>> AdoFind<T>(string Table,string Filter) where T : new()
        {
            StackTrace stackTrace = new StackTrace();
         

            Stopwatch sv = new Stopwatch();
            sv.Start();
      
            using (SqlConnection cnn = new SqlConnection(Utils.Cnn))
            {

                try
                {
                    SqlDataAdapter ad = new SqlDataAdapter(string.Format("select * from {0} where {1}", Table, Filter), cnn);
                    DataTable tbl = new DataTable(Table);
                    ad.Fill(tbl);
                    sv.Stop();
                    if (tbl.Rows.Count == 0)
                        return new MasterResult<List<T>> { Data = null, Elapsed = sv.ElapsedMilliseconds, Message = "Kayıt Bulunamadı", Result = false };

                    MasterResult<List<T>> M = AppCommon.convertToEnumerable<T>(tbl);
                    M.Elapsed = sv.ElapsedMilliseconds;
               
                    return M;
                }
                catch (SqlException ex)
                {
                    SqliteContext.addLog(AppCommon.LogModelBuild<SqlException>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<List<T>> { Result = false, Message = ex.Message, Elapsed = sv.ElapsedMilliseconds, Data = null };

                }
                catch (Exception ex)
                {
                    sv.Stop();
                    SqliteContext.addLog(AppCommon.LogModelBuild<Exception>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<List<T>> { Result = false, Message = ex.Message, Elapsed = sv.ElapsedMilliseconds, Data = null };
                }
                finally {
                    cnn.Close();
                    sv.Stop();
                }



            }

        }

        public static MasterResult<List<T>> AdoFind<T>(string Table, string Filter, int limit) where T : new()
        {
            StackTrace stackTrace = new StackTrace();


            Stopwatch sv = new Stopwatch();
            sv.Start();

            using (SqlConnection cnn = new SqlConnection(Utils.Cnn))
            {

                try
                {
                    SqlDataAdapter ad = new SqlDataAdapter(string.Format("select top {2} * from {0} where {1}", Table, Filter,limit), cnn);
                    DataTable tbl = new DataTable(Table);
                    ad.Fill(tbl);
                    sv.Stop();
                    if (tbl.Rows.Count == 0)
                        return new MasterResult<List<T>> { Data = null, Elapsed = sv.ElapsedMilliseconds, Message = "Kayıt Bulunamadı", Result = false };

                    MasterResult<List<T>> M = AppCommon.convertToEnumerable<T>(tbl);
                    M.Elapsed = sv.ElapsedMilliseconds;

                    return M;
                }
                catch (SqlException ex)
                {
                    SqliteContext.addLog(AppCommon.LogModelBuild<SqlException>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<List<T>> { Result = false, Message = ex.Message, Elapsed = sv.ElapsedMilliseconds, Data = null };

                }
                catch (Exception ex)
                {
                    sv.Stop();
                    SqliteContext.addLog(AppCommon.LogModelBuild<Exception>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<List<T>> { Result = false, Message = ex.Message, Elapsed = sv.ElapsedMilliseconds, Data = null };
                }
                finally
                {
                    cnn.Close();
                    sv.Stop();
                }



            }

        }

        public static MasterResult<List<T>> AdoGetDefModel<T>(string Table,int Trcode) where T : new()
        {
            Stopwatch sv = new Stopwatch();
            StackTrace stackTrace = new StackTrace();
            sv.Start();
            using (SqlConnection cnn = new SqlConnection(Utils.Cnn))
            {

                try
                {
                    SqlDataAdapter ad = null;
                    switch (Trcode)
                    {
                        case 0: {ad= new SqlDataAdapter(string.Format("select top 1 * from {0}  order by LOGICALREF desc", Table), cnn); break; }
            
                        default: {ad= new SqlDataAdapter(string.Format("select top 1 * from {0} where TRCODE={1} order by LOGICALREF desc", Table,Trcode), cnn); break; }
                     
                    }

                  
                    DataTable tbl = new DataTable(Table);
                    ad.Fill(tbl);
                    sv.Stop();
                    if (tbl.Rows.Count == 0)
                        return new MasterResult<List<T>> { Data = null, Elapsed = sv.ElapsedMilliseconds, Message = "Kayıt Bulunamadı", Result = false };

                    MasterResult<List<T>> M = AppCommon.convertToEnumerable<T>(tbl);
                    M.Elapsed = sv.ElapsedMilliseconds;
                    return M;
                
                }
                catch (SqlException ex)
                {
                    SqliteContext.addLog(AppCommon.LogModelBuild<SqlException>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<List<T>> { Result = false, Message = ex.Message, Elapsed = sv.ElapsedMilliseconds, Data = null };

                }
                catch (Exception ex)
                {
                    sv.Stop();
                    SqliteContext.addLog(AppCommon.LogModelBuild<Exception>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<List<T>> { Result = false, Message = ex.Message, Elapsed = sv.ElapsedMilliseconds, Data = null };
                }
                finally
                {
                    cnn.Close();
                    sv.Stop();
                }



            }

        }

        public static MasterResult<List<T>> RawQuery<T>(string query)where T:new() {
            Stopwatch sv = new Stopwatch();
            StackTrace stackTrace = new StackTrace();
            sv.Start();
            using (SqlConnection cnn = new SqlConnection(Utils.Cnn))
            {

                try
                {
                    SqlDataAdapter ad = null;
                    ad = new SqlDataAdapter(query, cnn); 

                    

                    DataTable tbl = new DataTable("MDL");
                    ad.Fill(tbl);
                    sv.Stop();
                    if (tbl.Rows.Count == 0)
                        return new MasterResult<List<T>> { Data =null, Elapsed = sv.ElapsedMilliseconds, Message = "Kayıt Bulunamadı", Result = false };

                    MasterResult<List<T>> M = tbl.DataTableToEnumerable<T>();
                    M.Elapsed = sv.ElapsedMilliseconds;
                    return M;

                }
                catch (SqlException ex)
                {
                    SqliteContext.addLog(AppCommon.LogModelBuild<SqlException>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<List<T>> { Result = false, Message = ex.Message, Elapsed = sv.ElapsedMilliseconds, Data = null };

                }
                catch (Exception ex)
                {
                    sv.Stop();
                    SqliteContext.addLog(AppCommon.LogModelBuild<Exception>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<List<T>> { Result = false, Message = ex.Message, Elapsed = sv.ElapsedMilliseconds, Data = null };
                }
                finally
                {
                    cnn.Close();
                    sv.Stop();
                }



            }
        }


    }
}
