using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BiggWhaleWebAppDemo
{
    public partial class TopHoldings : System.Web.UI.Page
    {
        private String FundID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            FundID = Request.QueryString["field1"];
        }

        private DataTable GetTopFunds()
        {
            string constr = ConfigurationManager.ConnectionStrings["CEF_dbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select distinct f.Name , f.[Ticker Symbol], fa.[Top Funds] " +
                                                       "from Funds f,FundAssets fa, FundDetails fd " +
                                                       "where f.id = fa.fund_id " +
                                                       "and f.id = fd.fund_id " +
                                                       "and fa.fund_id = fd.fund_id " +
                                                       "and f.id = '" + FundID + "' " +
                                                       "order by f.Name desc"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }
    }
}