using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using static LogoGo3Data.Extras.Utils;

namespace LogoGo3Data
{
   public static class Jms
    {

        public static MasterResult<List<T>> DataTableToEnumerable<T>(this DataTable table) where T : new()
        {

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

        public static T deserializeJson<T>(this string data)
        {
            JavaScriptSerializer jsoncreator = new JavaScriptSerializer();

            return jsoncreator.Deserialize<T>(data);
        }

    }
}
