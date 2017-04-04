using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RDotNet;

namespace BiggWhaleWebAppDemo
{
    public partial class Dashboard : System.Web.UI.Page
    {
        public static string selectedAssetClass = "%";
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Add("CurrentDate", DateTime.Now.ToString("MM-dd-yyyy"));
            //txtNavRetPctFilter.Text = "0";
            Session.Add("ReturnPercent", txtNavReturnPct.Text);
            Session.Add("NAVReturnType", "YTDNAVReturn");
            Session.Add("MarketReturnType", "YTDMarketReturn");
            Session.Add("PremiumReturnType", "YTDPremiumDiscountAvg");
            Session.Add("PeriodStartDate", DateTime.Now.AddDays(-7).ToShortDateString());
            Session.Add("MinAssetCount", 1);

            lblAssetKey.Text = "Asset Class Counts";
            if (!IsPostBack)
            {
                updateClassList();
                Session["Chart1AssetClass"] = "%";
                //ListView1.DataBind();
                //ListView3.DataBind();

            }
            chrAssetClass.Series[0]["PieLabelStyle"] = "Disabled";
            refreshChart();
            //REngine.SetEnvironmentVariables();
            //REngine engine = REngine.GetInstance();
            //engine.Initialize();
            //engine.Evaluate("x <- 40 + 2");
            //engine.Evaluate("s <- paste('hello', letters[5])");
            //var x = engine.GetSymbol("x").AsNumeric().First();
            //var s = engine.GetSymbol("s").AsCharacter().First();
            //Console.WriteLine(x);
            //Console.WriteLine(s);
        }

        private void refreshChart()
        {
            try
            {
                String selectCommand = "SELECT [Asset Class] AS Asset_Class, Count([Asset Class]) AS Count " +
        "FROM[Funds] a, [FundDetails] b " +
        "WHERE a.Id = b.Fund_Id " +
        "AND b.[Crawl Date] = (select Max([Crawl Date]) from FundDetails where fund_id = a.id) " +
        "AND b.YTDNAVReturn >= @Chart1ReturnPercent " +
        "GROUP BY[Asset Class] " +
        "HAVING Count([Asset Class]) >= @MinAssetCount " +
        "ORDER BY Count";

                int assetCount = getAssetCount();
                if (assetCount > 1)
                {
                    pnlChart.Visible = true;
                    lblError.Visible = false;
                    Session["MinAssetCount"] = 1;
                    if (assetCount >= 20)
                    {
                        Session["MinAssetCount"] = 2;
                    }
                    using (SqlDataSource src = new SqlDataSource())
                    {
                        src.ConnectionString = ConfigurationManager.ConnectionStrings["CEF_dbConnectionString"].ConnectionString;
                        src.SelectCommand = selectCommand;
                        src.SelectParameters.Add("Chart1ReturnPercent", Session["ReturnPercent"].ToString());
                        src.SelectParameters.Add("MinAssetCount", Session["MinAssetCount"].ToString());
                        src.SelectCommandType = SqlDataSourceCommandType.Text;

                        chrAssetClass.DataSource = src;
                        chrAssetClass.DataBind();
                    }
                }
                else
                {
                    pnlChart.Visible = false;
                    lblError.Visible = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void Chart1_Click(object sender, ImageMapEventArgs e)
        {
        }

        private int getAssetCount()
        {
            if (String.IsNullOrEmpty(Session["ReturnPercent"].ToString()))
                return 0;

            try
            {
                String selectCommand = "SELECT COUNT(DISTINCT [Asset Class]) " +
        "FROM[Funds] a, [FundDetails] b " +
        "WHERE a.Id = b.Fund_Id " +
        "AND b.[Crawl Date] = (select Max([Crawl Date]) from FundDetails where fund_id = a.id) " +
        "AND b.YTDNAVReturn >= @Chart1ReturnPercent ";

                SqlDataSource src = new SqlDataSource();
                src.ConnectionString = ConfigurationManager.ConnectionStrings["CEF_dbConnectionString"].ConnectionString;
                src.SelectCommand = selectCommand;
                //src.SelectParameters.Add("SessionChart1AssetClass", Session["Chart1AssetClass"].ToString());
                src.SelectParameters.Add("Chart1ReturnPercent", Session["ReturnPercent"].ToString());
                src.SelectCommandType = SqlDataSourceCommandType.Text;

                DataView dv = (DataView)src.Select(DataSourceSelectArguments.Empty);
                if (dv != null)
                {
                    return (int)dv.Table.Rows[0][0];
                }
            }
            catch (Exception)
            {
                throw;
            }
            return 0;
        }

        private void updateClassList()
        {
            string constr = ConfigurationManager.ConnectionStrings["CEF_dbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select distinct f.[Asset Class] from Funds f where f.[Asset Class] not like '%Muni%'"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            cboClassSelect.Items.Clear();
                            //Add intial option
                            ListItem start = new ListItem();
                            start.Value = "All Asset Classes";
                            start.Text = "All Asset Classes";
                            cboClassSelect.Items.Add(start);
                            foreach (DataRow row in dt.Rows)
                            {
                                String value = (string)row[0];
                                ListItem item = new ListItem();
                                item.Value = value;
                                item.Text = value;
                                cboClassSelect.Items.Add(item);
                            }
                            cboClassSelect.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        protected void txtNavReturnPct_TextChanged(object sender, EventArgs e)
        {
            Session["ReturnPercent"] = txtNavReturnPct.Text;
            refreshChart();
        }

        protected void cboTimePeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            string timeRange = cboTimePeriod.SelectedValue;
            switch (timeRange)
            {
                case "0": //YTD
                    Session["NAVReturnType"] = "YTDNAVReturn";
                    Session["MarketReturnType"] = "YTDMarketReturn";
                    Session["PremiumReturnType"] = "YTDPremiumDiscountAvg";
                    break;
                case "1": //5 Years
                    Session["NAVReturnType"] = "FiveYearNAVReturn";
                    Session["MarketReturnType"] = "FiveYearMarketReturn";
                    Session["PremiumReturnType"] = "FiveYearPremiumDiscountAvg";
                    break;
                case "2": // 10 Years
                    Session["NAVReturnType"] = "TenYearNAVReturn";
                    Session["MarketReturnType"] = "TenYearMarketReturn";
                    Session["PremiumReturnType"] = "TenYearPremiumDiscountAvg";
                    break;
                case "5": // 1 Year
                    Session["NAVReturnType"] = "OneYearNAVReturn";
                    Session["MarketReturnType"] = "OneYearMarketReturn";
                    Session["PremiumReturnType"] = "YTDPremiumDiscountAvg";
                    break;
                default:
                    Session["NAVReturnType"] = "YTDNAVReturn";
                    Session["MarketReturnType"] = "YTDMarketReturn";
                    Session["PremiumReturnType"] = "YTDPremiumDiscountAvg";
                    break;
            }
            switch (timeRange)
            {
                case "3": //Weekly
                    Session["PeriodStartDate"] = DateTime.Now.AddDays(-7).ToShortDateString();
                    break;
                case "4": //Monthly
                    Session["PeriodStartDate"] = DateTime.Now.AddMonths(-1).ToShortDateString();
                    break;
                default:
                    Session["PeriodStartDate"] = DateTime.Now.AddDays(-7).ToShortDateString();
                    break;
            }
        }

        protected void cboClassSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedAssetClass = cboClassSelect.Items[cboClassSelect.SelectedIndex].Text;
            if (!string.IsNullOrEmpty(selectedAssetClass))
            {
                if (selectedAssetClass == "All Asset Classes")
                {
                    Session["Chart1AssetClass"] = "%";
                }
                else
                {
                    Session["Chart1AssetClass"] = selectedAssetClass;
                }
            }
            refreshChart();

        }

        protected void btnAllClasses_Click(object sender, EventArgs e)
        {
            Session["Chart1AssetClass"] = "%";
            lblAssetClassFundReturns.Text = "Asset Class Fund Returns for All Classes";
        }

        protected void chrAssetClass_Click(object sender, ImageMapEventArgs e)
        {
            string assetClass = e.PostBackValue;
            selectedAssetClass = assetClass;
            Session["Chart1AssetClass"] = assetClass;
            lblAssetClassFundReturns.Text = "Asset Class Fund Returns for " + assetClass;

        }

        protected void chrAssetClass_Load(object sender, EventArgs e)
        {

        }

        protected void grdFundSummaryDetail_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdFundSummaryDetail.Rows)
            {
                if (row.Cells[9].Text == "Rising")
                {
                    row.Cells[9].Text = "<img src='Images/Rising2.png' />";
                }
                else if (row.Cells[9].Text == "Falling")
                {
                    row.Cells[9].Text = "<img src='Images/Falling2.png' />";
                }
                else
                {
                    row.Cells[9].Text = "<img src='Images/Steady2.png' />";
                }

                if (row.Cells[10].Text == "Rising")
                {
                    row.Cells[10].Text = "<img src='Images/Rising2.png' />";
                }
                else if (row.Cells[10].Text == "Falling")
                {
                    row.Cells[10].Text = "<img src='Images/Falling2.png' />";
                }
                else
                {
                    row.Cells[10].Text = "<img src='Images/Steady2.png' />";
                }

                if (row.Cells[11].Text == "Rising")
                {
                    row.Cells[11].Text = "<img src='Images/Rising2.png' />";
                }
                else if (row.Cells[11].Text == "Falling")
                {
                    row.Cells[11].Text = "<img src='Images/Falling2.png' />";
                }
                else
                {
                    row.Cells[11].Text = "<img src='Images/Steady2.png' />";
                }
            }
        }
    }
}