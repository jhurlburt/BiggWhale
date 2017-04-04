using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BiggWhaleWebAppDemo
{
    public partial class Search : Page
    {
        public int bottom;
        public int weekMonthBottom;
        public int top;
        public string navOpt = "YTDNAVReturn";
        public string makOpt = "YTDMarketReturn";
        public string prmOpt = "YTDPremiumDiscountAvg";
        public string currentDate = DateTime.Today.ToShortDateString();
        public string oldDate = DateTime.Today.AddDays(-7).ToShortDateString();

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.IsPostBack)
            //{
            //Building an HTML string.

            //Get classes
            //String currentClass = "AMEX";
            // classes = this.getClasses(currentClass);
            /*html.Append("<select id='topFundsSelect'>");
            foreach (DataRow cl in classes.Rows)
            {
                String value = (string)cl[0];
                value = value.Split('/')[1];
                html.Append("<option value='" + value + "'>" + value + "</option>");
            }
            html.Append("</select>");*/

            if (!this.IsPostBack)
            {
                updateClassList();
                updateClasses();
            }
            //Populating a DataTable from database.
            //updateClasses();

            /*if (!this.IsPostBack)
                cachedClass = classSelect.SelectedValue;
            if (!this.IsPostBack)
                cachedFund = topFundsSelect.SelectedValue;*/
            //}
        }

        [WebMethod]
        private void updateDataTable()
        {
            StringBuilder html = new StringBuilder();
            //DataTable dt = null;

            //Table start.
            html.Append("<h2>" + /*dt.Rows[0][1]*/cboAssetClass.SelectedValue + "</h2>");
            html.Append("<div style='width:100%;max-height:600px;overflow:auto;'>");
            if (cboAssetClass.SelectedIndex == 0) //All Asset Classes
            {
                using (DataTable dt = this.GetTopView())
                {
                    html.Append("<table border='1' height='80px' width='100%' bottom='0%'>");

                    //Calculate 'total' value
                    var total = 0;
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            var filt = (int)row["FilteredFunds"];
                            total += filt;
                        }
                        //Building the Header row.
                        html.Append("<tr>");
                        html.Append("<th>");
                        html.Append("Asset Class");
                        html.Append("</th>");
                        html.Append("<th>");
                        html.Append("Funds");
                        html.Append("</th>");
                        html.Append("<th>");
                        html.Append("%");
                        html.Append("</th>");
                        html.Append("</tr>");

                        //Building the Data rows.
                        foreach (DataRow row in dt.Rows)
                        {
                            html.Append("<tr>");
                            html.Append("<td>");
                            html.Append(row["Asset Class"]);
                            html.Append("</td>");
                            html.Append("<td>");
                            html.Append(row["FilteredFunds"]);
                            html.Append("</td>");
                            //Get Percentage
                            //var total = (int)row["TotalFunds"];
                            var filt = (int)row["FilteredFunds"];
                            decimal per = Math.Round(Decimal.Divide(filt, total) * 100, 2);
                            html.Append("<td>");
                            html.Append(per);
                            html.Append("</td>");
                            html.Append("</tr>");
                        }
                        //Table end.
                        html.Append("</table>");

                    }
                    else
                    {
                        html.Append("<b>No available funds for this combination of NAV Return Range, Time Period and Asset Class");
                    }
                }
                html.Append("</div><div class='container'>");
            }
            else 
            {
                using (DataTable dt = this.GetData())
                {
                    if (dt.Rows.Count > 0)
                    {
                        html.Append("<table border = '1' height='80px' width='100%' bottom='0%'>");

                        //Building the Header row.
                        html.Append("<tr>");
                        var columnCount = 0;
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (columnCount <= 1 || columnCount >= 10)
                            {
                                columnCount++;
                                continue;
                            }

                            html.Append("<th>");
                            if (columnCount == 4)
                            {
                                if (cboTimePeriod.SelectedValue == "3")
                                {
                                    html.Append("WeeklyNAVReturn");
                                }
                                else if (cboTimePeriod.SelectedValue == "4")
                                {
                                    html.Append("MonthlyNAVReturn");
                                }
                                else
                                {
                                    html.Append(column.ColumnName);
                                }
                            }
                            else if (columnCount == 5)
                            {
                                if (cboTimePeriod.SelectedValue == "3")
                                {
                                    html.Append("WeeklyMarketReturn");
                                }
                                else if (cboTimePeriod.SelectedValue == "4")
                                {
                                    html.Append("MonthlyMarketReturn");
                                }
                                else
                                {
                                    html.Append(column.ColumnName);
                                }
                            }
                            else
                            {
                                html.Append(column.ColumnName);
                            }
                            html.Append("</th>");
                            columnCount++;
                        }
                        //html.Append("<th>");
                        //html.Append("NAV Return (YTD)");
                        //html.Append("</th>");
                        //html.Append("<th>");
                        //html.Append("Mkt Return (YTD)");
                        //html.Append("</th>");
                        html.Append("<th>");
                        html.Append("M/Q");
                        html.Append("</th>");
                        html.Append("<th>");
                        html.Append("ABS");
                        html.Append("</th>");
                        html.Append("</tr>");

                        //Building the Data rows.
                        foreach (DataRow row in dt.Rows)
                        {
                            html.Append("<tr>");
                            var count = 0;
                            foreach (DataColumn column in dt.Columns)
                            {
                                if (count <= 1 || count >= 10)
                                {
                                    count++;
                                    continue;
                                }
                                if (count == 2)
                                {
                                    using (DataTable beta = this.GetBeta())
                                    {
                                        var check = getBetaCheck(beta, row[column.ColumnName].ToString());
                                        if (check > 0)
                                            html.Append("<td bgcolor='green'>");
                                        else if (check < 0)
                                            html.Append("<td bgcolor='red'>");
                                        else
                                            html.Append("<td>");
                                    }
                                }
                                else
                                {
                                    html.Append("<td>");
                                }
                                if (count == 4)
                                {
                                    if (cboTimePeriod.SelectedValue == "3" || cboTimePeriod.SelectedValue == "4")
                                    {
                                        html.Append(((decimal)row["WeekNav"]).ToString("#.##"));
                                    }
                                    else
                                    {
                                        html.Append(row[column.ColumnName]);
                                    }
                                }
                                else if (count == 5)
                                {
                                    if (cboTimePeriod.SelectedValue == "3" || cboTimePeriod.SelectedValue == "4")
                                    {
                                        html.Append(((decimal)row["WeekMak"]).ToString("#.##"));
                                    }
                                    else
                                    {
                                        html.Append(row[column.ColumnName]);
                                    }
                                }
                                else
                                {
                                    html.Append(row[column.ColumnName]);
                                }
                                html.Append("</td>");
                                count++;
                            }
                            html.Append("<td>");
                            html.Append("");
                            html.Append("</td>");
                            html.Append("<td>");
                            //Put in code for ABS here
                            try
                            {
                                var yield = (decimal)row["Yield"];
                                var prem = (decimal)row["Discount"];
                                var abs = yield + (-1 * (prem));
                                html.Append(abs);
                            }
                            catch (Exception err)
                            {
                                html.Append(err.Message.ToString());

                            }
                            html.Append("</td>");
                            html.Append("</tr>");
                        }
                        //Table end.
                        html.Append("</table>");
                        html.Append("</div>");


                        using (DataTable topFunds = this.GetTopFunds())
                        {
                            if (topFunds.Rows.Count > 0)
                            {
                                //Top Funds Table start.
                                //html.Append("<br></br>");
                                //html.Append("<font size='4' color='blue'>Top Holdings for " + /*topFunds.Rows[0][topFunds.Columns[0].ColumnName]topFundsSelect.SelectedValue*/ + "</font><br></br>");
                                html.Append("<div class='container' style='margin-top:40px'>");
                                html.Append("<table id='topHoldingsTable' border = '1' height='80px' width='100%' bottom='0%' overflow='scroll' class='tablesorter'>");

                                //Building the Header row.
                                html.Append("<tr>");
                                /*foreach (DataColumn column in topFunds.Columns)
                                {
                                    html.Append("<th>");
                                    html.Append(column.ColumnName);
                                    html.Append("</th>");
                                }*/

                                html.Append("<th onclick='sort_table(topHoldingsTable, 0, asc1); asc1 *= -1; asc2 = 1; asc3 = 1;'>");
                                html.Append("Name");
                                html.Append("</th>");
                                html.Append("<th onclick='sort_table(topHoldingsTable, 1, asc2); asc2 *= -1; asc1 = 1; asc3 = 1;'>");
                                html.Append("Holding");
                                html.Append("</th>");
                                html.Append("</tr>");

                                var listOfTopFunds = ((string)topFunds.Rows[0]["Top Funds"]).Split('%').ToList();
                                foreach (var fund in listOfTopFunds)
                                {
                                    if (fund.Length > 4)
                                    {
                                        html.Append("<tr>");
                                        html.Append("<td>");
                                        html.Append(fund.Substring(0, (fund.Length - 4)));
                                        html.Append("</td>");
                                        html.Append("<td>");
                                        html.Append(fund.Substring((fund.Length - 4), 4) + "%");
                                        html.Append("</td>");
                                        html.Append("</tr>");
                                    }
                                }
                            }
                            //Table end.
                            html.Append("</table>");
                        }
                        //Building the Data rows.
                        /*foreach (DataRow row in topFunds.Rows)
                        {
                            html.Append("<tr>");
                            foreach (DataColumn column in topFunds.Columns)
                            {
                                html.Append("<td>");
                                html.Append(row[column.ColumnName]);
                                html.Append("</td>");
                            }
                        }*/
                    }
                    else
                    {
                        html.Append("<b>No available funds for this combination of NAV Return Range and Asset Class");
                    }
                }
            }
            html.Append("</div>");

            //Append the HTML string to Placeholder.
            phSearchResults.Controls.Add(new Literal { Text = html.ToString() });
        }

        protected void classSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            //updateClasses();
            updateDataTable();
        }

        protected void navReturnPercent_TextChanged(object sender, EventArgs e)
        {
            //updateClasses();
            updateDataTable();
        }

        protected void timePeriodSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            //updateClasses();
            updateDataTable();
        }

        private void setTimePeriod()
        {
            string timeRange = cboTimePeriod.SelectedValue;
            switch (timeRange)
            {
                case "0": //YTD
                    navOpt = "YTDNAVReturn";
                    makOpt = "YTDMarketReturn";
                    prmOpt = "YTDPremiumDiscountAvg";
                    break;
                case "1": //5 Years
                    navOpt = "FiveYearNAVReturn";
                    makOpt = "FiveYearMarketReturn";
                    prmOpt = "FiveYearPremiumDiscountAvg";
                    break;
                case "2": // 10 Years
                    navOpt = "TenYearNAVReturn";
                    makOpt = "TenYearMarketReturn";
                    prmOpt = "TenYearPremiumDiscountAvg";
                    break;
                case "5": // 1 Year
                    navOpt = "OneYearNAVReturn";
                    makOpt = "OneYearMarketReturn";
                    prmOpt = "YTDPremiumDiscountAvg";
                    break;
                default:
                    navOpt = "YTDNAVReturn";
                    makOpt = "YTDMarketReturn";
                    prmOpt = "YTDPremiumDiscountAvg";
                    break;
            }
            switch (timeRange)
            {
                case "3": //Weekly
                    oldDate = DateTime.Now.AddDays(-7).ToShortDateString();
                    break;
                case "4": //Monthly
                    oldDate = DateTime.Now.AddMonths(-1).ToShortDateString();
                    break;
                default:
                    oldDate = DateTime.Now.AddDays(-7).ToShortDateString();
                    break;
            }
        }
        private void setNavRange()
        {
            string navRange = txtNavReturnPct.Text;
            //navReturnPercent.Text = string.Empty;
            if (string.IsNullOrEmpty(navRange))
            {
                bottom = -1000;
                weekMonthBottom = -1000;
            }
            else if (cboTimePeriod.SelectedIndex == 1 || cboTimePeriod.SelectedIndex == 2)
            {
                bottom = -1000;
                weekMonthBottom = Int32.Parse(navRange);
            }
            else
            {
                bottom = Int32.Parse(navRange);
                weekMonthBottom = -1000;
            }
        }

        private int getBetaCheck(DataTable beta, String name)
        {
            foreach (DataRow row in beta.Rows)
            {
                if (row[0].ToString() == name)
                {
                    if (((decimal)row[1]) > 0)
                    {
                        return 1;
                    }
                    else if (((decimal)row[1]) < 0)
                    {
                        return -1;
                    }
                    break;
                }
            }
            return 0;
        }

        public static DataSet GetDataSet(string ConnectionString, string SQL)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = SQL;
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            conn.Open();
            DataTable table = new DataTable();
            table.Load(cmd.ExecuteReader());
            ds.Tables.Add(table);

            da.Fill(ds);
            conn.Close();

            return ds;
        }


        private DataTable GetBeta()
        {
            DataSet CalcStat = new DataSet();
            string cmd2 = "select [Ticker Symbol], [Calculated Stat], RankOrder, [Calculated Value] from SummaryData where [Summary Date] = '2016-07-05' order by [Calculated Value] desc,[Ticker Symbol]";
            //string cs = "Server=tcp:biggwhaledb.database.windows.net,1433;Initial Catalog=CEF_db;Persist Security Info=False;User ID=bw_admin;Password=BiggWhale2016!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            CalcStat = GetDataSet(ConfigurationManager.ConnectionStrings["CEF_dbConnectionString"].ConnectionString, cmd2);

            string select = cboAssetClass.SelectedValue;
            setNavRange();
            setTimePeriod();
            string constr = ConfigurationManager.ConnectionStrings["CEF_dbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select distinct [Fund Name], [Calculated Value] from SummaryData where [Calculated Stat] = '30 Day NAV Beta' and [Summary Date] >= '" + currentDate + "' order by [Fund Name] desc"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        con.Open();
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            //sda.Fill(dt);
                            dt.Load(cmd.ExecuteReader());
                            return dt;
                        }
                    }
                }
            }
        }

        private DataTable GetTopView()
        {
            string select = cboAssetClass.SelectedValue;
            setNavRange();
            setTimePeriod();
            string constr = ConfigurationManager.ConnectionStrings["CEF_dbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select a.[Asset Class], a.TotalFunds, b.FilteredFunds " +
                                                       "From ( " +
                                                       " select distinct fb.[Asset Class], count(fb.Name) as TotalFunds " +
                                                       " from (select distinct f.id, f.Name, f.[Asset Class], " + navOpt /*YTDNAVReturn*/ + " from Funds f, FundDetails fd  " +
                                                       " where f.id = fd.fund_id  " +
                                                       " and fd.[Crawl Date] = (select Max([Crawl Date]) from FundDetails where fund_id = f.id) " +
                                                       " and fd." + navOpt /*YTDNAVReturn*/ + " >= -1000 and fd." + navOpt /*YTDNAVReturn*/ + " <= 1000) fb " +
                                                       " group by fb.[Asset Class] " +
                                                       " ) a " +
                                                       " Inner Join ( " +
                                                       " select distinct fc.[Asset Class], count(fc.Name) as FilteredFunds " +
                                                       " from ( select * from (select distinct f.id, f.Name, f.[Asset Class], " + navOpt /*YTDNAVReturn*/ + " from Funds f, FundDetails fd  " +
                                                       " where f.id = fd.fund_id  " +
                                                       " and fd.[Crawl Date] = (select Max([Crawl Date]) from FundDetails where fund_id = f.id) " +
                                                       " and fd." + navOpt /*YTDNAVReturn*/ + " >= " + bottom + ") a " +
                                                       //Option if Weekly/Monthly
                                                       ((cboTimePeriod.SelectedValue == "3" || cboTimePeriod.SelectedValue == "4") ?
                                                       " inner join (select distinct a.Name as WeeklyName, ((a.NAV - b.NAV) / b.NAV * 100) as WeekNav, ((a.MarketPrice - b.MarketPrice) / b.MarketPrice * 100) as WeekMak " +
                                                       " from " +
                                                       " (select fd.NAV, fd.MarketPrice, f.id, f.Name from Funds f, FundDetails fd where f.id = fd.fund_id  " +
                                                       " and fd.[Crawl Date] = (select Min([Crawl Date]) from FundDetails where fund_id = f.id And [Crawl Date] >= '" + currentDate + "') " +
                                                       " and fd.NAV != 0 and fd.MarketPrice != 0) a, " +
                                                       " (select fd.NAV, fd.MarketPrice, f.id, f.Name from Funds f, FundDetails fd where f.id = fd.fund_id  " +
                                                       " and fd.[Crawl Date] = (select Min([Crawl Date]) from FundDetails where fund_id = f.id And [Crawl Date] >= '" + oldDate + "') " +
                                                       " and fd.Nav != 0 and fd.MarketPrice != 0) b " +
                                                       " where a.id = b.id) WeeklyNavs " +
                                                       " On a.Name = WeeklyNavs.WeeklyName " +
                                                       " where WeeklyNavs.WeekNav >= " + weekMonthBottom + " " : "") +
                                                       //End Optional
                                                       " ) fc " +
                                                       " group by fc.[Asset Class] " +
                                                       " ) b " +
                                                       " On a.[Asset Class] = b.[Asset Class] " +
                                                       " order by a.[Asset Class] desc"))
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

        private DataTable GetData()
        {
            string select = cboAssetClass.SelectedValue;
            setNavRange();
            setTimePeriod();
            string constr = ConfigurationManager.ConnectionStrings["CEF_dbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                //using (SqlCommand cmd = new SqlCommand("select distinct f.id, f.[Asset Class], f.Name, f.[Ticker Symbol], fd." + navOpt /*YTDNAVReturn*/ + ", fd." + makOpt /*YTDMarketReturn*/ + ", f.[Percent Leveraged Assets], fd.DistributionYield, f.[Total Net Assets], fd." + prmOpt /*YTDPremiumDiscountAvg*/ + ", fd.[Crawl Date]  " +
                //                                       "from Funds f,FundAssets fa, FundDetails fd  " +
                //                                       "where f.id = fa.fund_id  " +
                //                                       "and f.id = fd.fund_id  " +
                //                                       "and fa.fund_id = fd.fund_id  " +
                //                                       "and fd.[Crawl Date] = (select Max([Crawl Date]) from FundDetails where fund_id = f.id) " +
                //                                       "and f.[Asset Class] like '%" + select + "%' " +
                //                                       "and fd." + navOpt /*YTDNAVReturn*/ + " >= " + bottom + " and fd." + navOpt /*YTDNAVReturn*/ + " <= " + top + " " +
                //                                       "order by f.Name asc, f.[Total Net Assets] desc"))YTDPremiumDiscountAvg
                using (SqlCommand cmd = new SqlCommand("select * from (select distinct f.id, f.[Asset Class], f.Name, f.[Ticker Symbol], fd." + navOpt /*YTDNAVReturn*/ + " as 'NAV', fd." + makOpt /*YTDMarketReturn*/ + " as 'Market Return', f.[Percent Leveraged Assets], fd.DistributionYield as 'Yield', f.[Total Net Assets], fd." + prmOpt /*YTDPremiumDiscountAvg*/ + " as 'Discount', fd.[Crawl Date]  " +
                                                       "from Funds f, FundDetails fd  " +
                                                       "where f.id = fd.fund_id  " +
                                                       "and fd.[Crawl Date] = (select Max([Crawl Date]) from FundDetails where fund_id = f.id) " +
                                                       "and f.[Asset Class] like '%" + select + "%' " +
                                                       "and fd." + navOpt /*YTDNAVReturn*/ + " >= " + bottom + " ) a " +
                                                       //Option if Weekly/Monthly
                                                       ((cboTimePeriod.SelectedValue == "3" || cboTimePeriod.SelectedValue == "4") ?
                                                       " inner join (select distinct a.Name as WeeklyName, ((a.NAV - b.NAV) / b.NAV * 100) as WeekNav, ((a.MarketPrice - b.MarketPrice) / b.MarketPrice * 100) as WeekMak " +
                                                       " from " +
                                                       " (select fd.NAV, fd.MarketPrice, f.id, f.Name from Funds f, FundDetails fd where f.id = fd.fund_id  " +
                                                       " and fd.[Crawl Date] = (select Min([Crawl Date]) from FundDetails where fund_id = f.id And [Crawl Date] >= '" + currentDate + "') " +
                                                       " and fd.NAV != 0 and fd.MarketPrice != 0) a, " +
                                                       " (select fd.NAV, fd.MarketPrice, f.id, f.Name from Funds f, FundDetails fd where f.id = fd.fund_id  " +
                                                       " and fd.[Crawl Date] = (select Min([Crawl Date]) from FundDetails where fund_id = f.id And [Crawl Date] >= '" + oldDate + "') " +
                                                       " and fd.Nav != 0 and fd.MarketPrice != 0) b " +
                                                       " where a.id = b.id) WeeklyNavs " +
                                                       " On a.Name = WeeklyNavs.WeeklyName " +
                                                       " where WeeklyNavs.WeekNav >= " + weekMonthBottom + " " : "") +
                                                       //End Optional
                                                       " order by a.Name asc"))
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

        private DataTable GetTopFunds()
        {
            string value1 = cboAssetClass.SelectedValue;
            //string value2 = topFundsSelect.SelectedValue;
            setNavRange();
            setTimePeriod();
            string constr = ConfigurationManager.ConnectionStrings["CEF_dbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from (select distinct f.Name , f.[Ticker Symbol], fa.[Top Funds], fd." + navOpt /*YTDNAVReturn*/ + " " +
                                                       "from Funds f, FundAssets fa, FundDetails fd " +
                                                       "where f.id = fa.fund_id  " +
                                                       "and f.id = fd.fund_id  " +
                                                       "and fa.fund_id = fd.fund_id  " +
                                                       "and f.[Asset Class] like '%" + value1 + "%' " +
                                                       //"and f.[Ticker Symbol] like '%" + value2 + "%' " +
                                                       "and fd.[Crawl Date] = (select Max([Crawl Date]) from FundDetails where fund_id = f.id) " +
                                                       "and fd." + navOpt /*YTDNAVReturn*/ + " >= " + bottom + " ) a " +
                                                       //Option if Weekly/Monthly
                                                       ((cboTimePeriod.SelectedValue == "3" || cboTimePeriod.SelectedValue == "4") ?
                                                       " inner join (select distinct a.Name as WeeklyName, ((a.NAV - b.NAV) / b.NAV * 100) as WeekNav, ((a.MarketPrice - b.MarketPrice) / b.MarketPrice * 100) as WeekMak " +
                                                       " from " +
                                                       " (select fd.NAV, fd.MarketPrice, f.id, f.Name from Funds f, FundDetails fd where f.id = fd.fund_id  " +
                                                       " and fd.[Crawl Date] = (select Min([Crawl Date]) from FundDetails where fund_id = f.id And [Crawl Date] >= '" + currentDate + "') " +
                                                       " and fd.NAV != 0 and fd.MarketPrice != 0) a, " +
                                                       " (select fd.NAV, fd.MarketPrice, f.id, f.Name from Funds f, FundDetails fd where f.id = fd.fund_id  " +
                                                       " and fd.[Crawl Date] = (select Min([Crawl Date]) from FundDetails where fund_id = f.id And [Crawl Date] >= '" + oldDate + "') " +
                                                       " and fd.Nav != 0 and fd.MarketPrice != 0) b " +
                                                       " where a.id = b.id) WeeklyNavs " +
                                                       " On a.Name = WeeklyNavs.WeeklyName " +
                                                       " where WeeklyNavs.WeekNav >= " + weekMonthBottom + " " : "") +
                                                       //End Optional
                                                       " order by a.Name asc"))
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
                            cboAssetClass.Items.Clear();
                            //Add intial option
                            ListItem start = new ListItem();
                            start.Value = "All Asset Classes";
                            start.Text = "All Asset Classes";
                            cboAssetClass.Items.Add(start);
                            foreach (DataRow row in dt.Rows)
                            {
                                String value = (string)row[0];
                                ListItem item = new ListItem();
                                item.Value = value;
                                item.Text = value;
                                cboAssetClass.Items.Add(item);
                            }
                            cboAssetClass.SelectedIndex = 0;
                        }
                    }
                }
            }
            cboAssetClass.SelectedIndex = 0;

            cboAssetClass.AutoPostBack = true;
            cboAssetClass.Attributes["OnSelectedIndexChanged"] = "classSelect_SelectedIndexChanged";
            //topFundsSelect.AutoPostBack = true;
            //topFundsSelect.Attributes["OnSelectedIndexChanged"] = "topFundsSelect_SelectedIndexChanged";
            txtNavReturnPct.AutoPostBack = true;
            txtNavReturnPct.Attributes["OnTextChanged"] = "navReturnPercent_TextChanged";
            cboTimePeriod.AutoPostBack = true;
            cboTimePeriod.Attributes["OnSelectedIndexChanged"] = "timePeriodSelect_SelectedIndexChanged";
        }

        private void updateClasses()
        {
            String select = cboAssetClass.SelectedValue;
            setNavRange();
            setTimePeriod();
            string constr = ConfigurationManager.ConnectionStrings["CEF_dbConnectionString"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constr))
            {
                //using (SqlCommand cmd = new SqlCommand("select distinct f.[Ticker Symbol] from Funds f where f.[Asset Class] like '%" + select + "%'"))
                using (SqlCommand cmd = new SqlCommand("select * from (select distinct f.[Ticker Symbol] " +
                                                       "from Funds f, FundDetails fd  " +
                                                       "where f.id = fd.fund_id  " +
                                                       "and fd.[Crawl Date] = (select Max([Crawl Date]) from FundDetails where fund_id = f.id) " +
                                                       "and f.[Asset Class] like '%" + select + "%' " +
                                                       "and fd." + navOpt /*YTDNAVReturn*/ + " >= " + bottom + " ) a " +
                                                       "inner join (select distinct a.[Ticker Symbol] as WeeklyTicker, ((a.NAV - b.NAV) / b.NAV * 100) as WeekNav, ((a.MarketPrice - b.MarketPrice) / b.MarketPrice * 100) as WeekMak " +
                                                       "from " +
                                                       "(select fd.NAV, fd.MarketPrice, f.id, f.[Ticker Symbol] from Funds f, FundDetails fd where f.id = fd.fund_id  " +
                                                       "and fd.[Crawl Date] = (select Min([Crawl Date]) from FundDetails where fund_id = f.id And [Crawl Date] >= '" + currentDate + "') " +
                                                       "and fd.NAV != 0 and fd.MarketPrice != 0) a, " +
                                                       "(select fd.NAV, fd.MarketPrice, f.id, f.[Ticker Symbol] from Funds f, FundDetails fd where f.id = fd.fund_id  " +
                                                       "and fd.[Crawl Date] = (select Min([Crawl Date]) from FundDetails where fund_id = f.id And [Crawl Date] >= '" + oldDate + "') " +
                                                       "and fd.Nav != 0 and fd.MarketPrice != 0) b " +
                                                       "where a.id = b.id) WeeklyNavs " +
                                                       "On a.[Ticker Symbol] = WeeklyNavs.WeeklyTicker " +
                                                       "where WeeklyNavs.WeekNav >= " + weekMonthBottom + " " +
                                                       "order by a.[Ticker Symbol]"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (dt)
                        {
                            sda.Fill(dt);
                            //topFundsSelect.Items.Clear();
                            foreach (DataRow row in dt.Rows)
                            {
                                String value = (string)row[0];
                                ListItem item = new ListItem();
                                item.Value = value;
                                item.Text = value;
                                //topFundsSelect.Items.Add(item);
                            }
                            //topFundsSelect.SelectedIndex = 0;
                        }
                    }
                }
            }
            updateDataTable();
        }
    }
}