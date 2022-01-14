using LogoGo3Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using static LogoGo3Data.Extras.Utils;
using LogoGo3Data.Extras;
using System.Data.SqlClient;
using LogoGo3Data.Tools;
using System.Diagnostics;
using LogoGo3Data.DefineModel;
using System.Data;

namespace LogoGo3Data
{
  public  class NExec
    {
        public static MasterResult<NTUPLE> Insert<T>(dynamic document)
        {
            using (var db = new logodataDataContext(Extras.Utils.Cnn))
            {
                try { 
                    var table = db.GetTable(typeof(T));
                    table.InsertOnSubmit((T)document);
                    db.SubmitChanges();
                   return new MasterResult<NTUPLE> { Data = null, Elapsed = 0, Message = "Veri Kaydedildi", Result = true };

                    }
                    catch (Exception ex)
                    {
                      return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = ex, stat = 0 }, Elapsed = 0, Message = "Hata Oluştu Liste  Kaydedilemedi Hata Mesajı için Detayı İnceleyin", Result = false };
                    }

                     
             }
 



        }
        public static MasterResult<NTUPLE> Update<T>(dynamic document)
        {
               using (logodataDataContext ctx = new logodataDataContext(Utils.Cnn))
                {
                    try
                    {
                    ctx.CommandTimeout = int.MaxValue;
                    ITable table = ctx.GetTable(typeof(T));
                    table.Attach(document);
                    ctx.Refresh(RefreshMode.KeepCurrentValues, document);
                    ctx.SubmitChanges();
                        return new MasterResult<NTUPLE>
                        {
                            Data = null,
                            Elapsed = 0,
                            Message = "",
                            Result = true
                     
                 
                          };


                    }
                    catch (Exception ex)
                    {
                        return new MasterResult<NTUPLE>
                        {
                            Data =new NTUPLE { rec=ex, stat=0 },
                            Elapsed = 0,
                            Message = "",
                            Result = false


                        };

                    }
            
                }
             


        }

        public static MasterResult<NTUPLE> AdoInsert<T>(object document,string Table)
        {
            StackTrace stackTrace = new StackTrace();
            Stopwatch sv = new Stopwatch();
            sv.Start();
            using (var db = new SqlConnection(Extras.Utils.Cnn))
            {    /* SqlCommand cmd = AppCommon.inserCommandCreator<T>(document, Table);*/
                SqlCommand cmd = document.inserCommandCreatorT<T>(Table);
                try
                {

               
                    cmd.Connection = db;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    sv.Stop();


                    return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = "", stat = 1 }, Elapsed = sv.ElapsedMilliseconds, Message = "İşlem Başarılı", Result = true };
                }
                catch (SqlException ex)
                {
                    sv.Stop();
                  SqliteContext.addLog(AppCommon.LogModelBuild<SqlException>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = ex, stat = 0 }, Elapsed = sv.ElapsedMilliseconds, Message = "Hata Oluştu Liste  Kaydedilemedi Hata Mesajı için Detayı İnceleyin", Result = false };
                }
                catch (Exception ex)
                {
                    sv.Stop();
                    SqliteContext.addLog(AppCommon.LogModelBuild<Exception>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = ex, stat = 0 }, Elapsed = sv.ElapsedMilliseconds, Message = "Hata Oluştu Liste  Kaydedilemedi Hata Mesajı için Detayı İnceleyin", Result = false };
                }
                finally {
                    cmd.Connection.Close();
                    sv.Stop();
                }


            }




        }
        public static MasterResult<NTUPLE> AdoUpdate<T>(object document, string Table,string cond)
        {
            StackTrace stackTrace = new StackTrace();
            Stopwatch sv = new Stopwatch();
            sv.Start();
            using (var db = new SqlConnection(Extras.Utils.Cnn))
            {
                SqlCommand cmd = AppCommon.UpdateCommandCreator<T>(document, Table,cond);
                try
                {


                    cmd.Connection = db;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    sv.Stop();


                    return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = "", stat = 1 }, Elapsed = sv.ElapsedMilliseconds, Message = "İşlem Başarılı", Result = true };
                }
                catch (SqlException ex)
                {
                    sv.Stop();
                    SqliteContext.addLog(AppCommon.LogModelBuild<SqlException>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = ex, stat = 0 }, Elapsed = sv.ElapsedMilliseconds, Message = "Hata Oluştu Liste  Kaydedilemedi Hata Mesajı için Detayı İnceleyin", Result = false };
                }
                catch (Exception ex)
                {
                    sv.Stop();
                    SqliteContext.addLog(AppCommon.LogModelBuild<Exception>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = ex, stat = 0 }, Elapsed = sv.ElapsedMilliseconds, Message = "Hata Oluştu Liste  Kaydedilemedi Hata Mesajı için Detayı İnceleyin", Result = false };
                }
                finally
                {
                    cmd.Connection.Close();
                    sv.Stop();
                }


            }




        }
        public static MasterResult<NTUPLE> AdoDelete<T>(string Table, string cond)
        {
            StackTrace stackTrace = new StackTrace();
            Stopwatch sv = new Stopwatch();
            sv.Start();
            using (var db = new SqlConnection(Extras.Utils.Cnn))
            {
                SqlCommand cmd = AppCommon.DeleteCommandCreator<T>(Table, cond);
                try
                {


                    cmd.Connection = db;
                    cmd.Connection.Open();
                    int affect=  cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    sv.Stop();


                    return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = string.Format("{0} satır Silindi", affect), stat =affect }, Elapsed = sv.ElapsedMilliseconds, Message =affect!=0? "İşlem Başarılı":"Veri Silinemedi", Result = true };
                }
                catch (SqlException ex)
                {
                    sv.Stop();
                    SqliteContext.addLog(AppCommon.LogModelBuild<SqlException>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = ex, stat = 0 }, Elapsed = sv.ElapsedMilliseconds, Message = "Hata Oluştu Veri Silinemedi Hata Mesajı için Detayı İnceleyin", Result = false };
                }
                catch (Exception ex)
                {
                    sv.Stop();
                    SqliteContext.addLog(AppCommon.LogModelBuild<Exception>(ex, 0, stackTrace.GetFrame(1).GetMethod()));
                    return new MasterResult<NTUPLE> { Data = new NTUPLE { rec = ex, stat = 0 }, Elapsed = sv.ElapsedMilliseconds, Message = "Hata Oluştu Veri Silinemedi Hata Mesajı için Detayı İnceleyin", Result = false };
                }
                finally
                {
                    cmd.Connection.Close();
                    sv.Stop();
                }


            }




        }

       



    }
}
