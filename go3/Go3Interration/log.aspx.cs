using LogoGo3Data;
using LogoGo3Data.Context;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Go3Interration
{
    public partial class log : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var Data = SqliteContext.GetSqliteData<ReqLog>();
                List<XReqLog> XRL = new List<XReqLog>();
                foreach (var item in Data.Data)
                {
                    XRL.Add(new XReqLog
                    {
                        Date = item.Date,
                        dateH = DateTime.Parse(item.Date),
                         MetodName=item.MetodName,
                          ReqModel=item.ReqModel
                    }); 
                    
                }

                reper.DataSource = XRL.OrderByDescending(x=>x.dateH).ToList();
                reper.DataBind();
            }

        }
    }
}