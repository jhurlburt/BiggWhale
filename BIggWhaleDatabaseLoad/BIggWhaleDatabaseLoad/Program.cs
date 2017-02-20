namespace BIggWhaleDatabaseLoad
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class fundDetail
    {
        //get and set properties for each 
        public string Url { get; set; }
        public string Name { get; set; }
        public string AsOfDate { get; set; }
        public string Ticker { get; set; }
        public string AssetClass { get; set; }
        public decimal OneYrLipperAvg { get; set; }
        public decimal MarketReturn10Year { get; set; }
        public decimal MarketReturn5Year { get; set; }
        public decimal MarketReturn1Year { get; set; }
        public decimal MarketReturnYTD { get; set; }
        public int MarketReturnRank10Year { get; set; }
        public int MarketReturnRank5Year { get; set; }
        public int MarketReturnRank1Year { get; set; }
        public int MarketReturnRankYTD { get; set; }
        public decimal NavReturn10Year { get; set; }
        public decimal NavReturn5Year { get; set; }
        public decimal NavReturn1Year { get; set; }
        public decimal NavReturnYTD { get; set; }
        public int NavReturnRank10Year { get; set; }
        public int NavReturnRank5Year { get; set; }
        public int NavReturnRank1Year { get; set; }
        public int NavReturnRankYTD { get; set; }
        public decimal PremiumDiscount10YearAvg { get; set; }
        public decimal PremiumDiscount5YearAvg { get; set; }
        public decimal PremiumDiscountYTDAvg { get; set; }
        public decimal PremiumDiscount { get; set; }
        public decimal Nav { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal NetChange { get; set; }
        public decimal MarketChange { get; set; }
        public decimal OneYearNavReturn { get; set; }
        public decimal OneYearNavRank { get; set; }
        public string TwelveMoYieldAsOf { get; set; }
        public decimal DefinedIncomeOnlyYield { get; set; }
        public decimal DistributionYield { get; set; }
        public decimal MostRecentIncomeDividend { get; set; }
        public string MostRecentIncomeDividendDate { get; set; }
        public decimal MostRecentCapGainDividend { get; set; }
        public string MostRecentCapGainDividendDate { get; set; }
        public decimal MonthlyYTDDivedends { get; set; }
        public decimal YTDCapGains { get; set; }
        public string InceptionDate { get; set; }
        public string FundAdvisor { get; set; }
        public string ManagerAndTenure { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public decimal TotalNetAssets { get; set; }
        public string TotalNetAssetsDate { get; set; }
        public string PercentLeveragedAssetsDate { get; set; }
        public decimal PercentLeveragedAssets { get; set; }
        public decimal PortfolioTurnover { get; set; }
        public decimal ManagementFees { get; set; }
        public decimal ExpenseRatio { get; set; }
        public decimal AlternativeMinimumTax { get; set; }
        public string FundObjective { get; set; }
        public string TopAssetsDate { get; set; }
        public string TopAssets { get; set; }
        public string NetAssetsDate { get; set; }
        public string NetAssets { get; set; }
        public string QualityDate { get; set; }
        public string Quality { get; set; }
        public decimal Yield { get; set; }
        public DateTime CrawlDate { get; set; }

        public void fundDetailConst() //constructor method
        {
            Url = string.Empty;
            Name = string.Empty;
            AsOfDate = string.Empty;
            Ticker = string.Empty;
            AssetClass = string.Empty;
            OneYrLipperAvg = 0.0M;
            FundAdvisor = string.Empty;
            Yield = 0.0M;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            string cDate = DateTime.Now.ToString("yyyy-MM-dd");
            string strFilePath = Properties.Settings.Default.DataFolder + cDate;
            int filecounter = 0;
            foreach (string f in Directory.GetFiles(strFilePath))
            {
                if (f.Contains("FundDetail_"))
                {
                    filecounter++;
                    string[] filelines = File.ReadAllLines(f);

                    List<fundDetail> fundList = new List<fundDetail>();
                    string[] m;
                    int idx1 = 0;
                    int idx2 = 0;
                    int iVal = 0;
                    decimal mVal = 0.0M;
                    bool isDecimal = false;
                    bool isInt = false;
                    //parse line by line into instance of employee class
                    fundDetail fund = new fundDetail();
                    string detailUrl = "http://www.cefa.com/FundSelector/FundDetail.fs?ID=" + Path.GetFileName(f).Replace("FundDetail_", string.Empty).Replace(".txt", string.Empty);
                    fund.Url = detailUrl;
                    DateTime cDateTime = DateTime.Parse(cDate);
                    fund.CrawlDate = cDateTime;

                    for (int a = 0; a < filelines.Length; a++)
                    {
                        string detailDump = filelines[a];
                        // do something
                        RegexOptions options = RegexOptions.None;
                        Regex regex = new Regex(@"[\s]{2,}", options);
                        detailDump = regex.Replace(detailDump, "  ");
                        if (detailDump.Contains("Initilizing list..."))
                        {
                            idx1 = detailDump.IndexOf("Initilizing list...") + 20;
                            idx2 = detailDump.Substring(idx1).IndexOf("as of") - 1;
                            string fundName = string.Empty;
                            if (idx2 == -2)
                            {
                                idx2 = detailDump.Length - 1 - idx1 - idx2 - 2;
                                fundName = detailDump.Substring(idx1, idx2).Trim();
                                fund.Name = fundName;

                                //continue;
                            }
                            else
                            {
                                fundName = detailDump.Substring(idx1, idx2).Trim();

                            }
                            if (fundName.Length > 100)
                                fundName = fundName.Substring(0, 100);
                            if (fundName.Length >= 20)
                            {
                                if (fundName.Substring(0, 20) == "Performance Criteria")
                                    fundName = "Fund ID " + Path.GetFileName(f).Replace("FundDetail_", string.Empty).Replace(".txt", string.Empty);
                            }
                            fund.Name = fundName;
                        }
                        idx1 = idx1 + idx2 + 6;
                        string asOfDate = string.Empty;
                        int pad = 13;
                        if (detailDump.Substring(idx1, 4).Trim() == "--")
                        {
                            asOfDate = detailDump.Substring(idx1, 4).Trim();
                            pad = 5;
                        }
                        else
                        {
                            asOfDate = detailDump.Substring(idx1, 12).Trim();
                        }
                        fund.AsOfDate = asOfDate;
                        // Ticker
                        idx1 = idx1 + pad;
                        idx2 = 13;
                        string ticker = string.Empty;
                        if (detailDump.Contains("Asset Class:") && detailDump.Contains("1 YR Lipper Avg:"))
                        {
                            idx2 = detailDump.Substring(idx1).IndexOf("Asset Class:");
                        }
                        ticker = detailDump.Substring(idx1, idx2).Trim().Replace(" ", string.Empty);
                        fund.Ticker = ticker;
                        if (detailDump.Contains("Asset Class:") && detailDump.Contains("1 YR Lipper Avg:"))
                        {
                            idx1 = detailDump.IndexOf("Asset Class:") + 13;
                            idx2 = detailDump.Substring(idx1).IndexOf("1 YR Lipper Avg:") - 1;
                            string assetClass = detailDump.Substring(idx1, idx2).Trim();
                            fund.AssetClass = assetClass;
                        }
                        if (detailDump.Contains("1 YR Lipper Avg:"))
                        {
                            idx1 = detailDump.IndexOf("1 YR Lipper Avg:") + 18;
                            idx2 = detailDump.Substring(idx1).IndexOf("Growth of $10K Avg Annual Total Return %  10 YR 5 YR 1 YR YTD Market Return") - 1;
                            decimal oneYearLipperAvg = 0.0M;
                            string test = detailDump.Substring(idx1, idx2).Replace("%", string.Empty);
                            isDecimal = Decimal.TryParse(detailDump.Substring(idx1, idx2).Replace("%", string.Empty), out oneYearLipperAvg);
                            if (isDecimal)
                            {
                                fund.OneYrLipperAvg = oneYearLipperAvg;
                            }
                        }
                        // Avg Annual Return Percentage
                        if (detailDump.Contains("Growth of $10K Avg Annual Total Return %  10 YR 5 YR 1 YR YTD Market Return") && detailDump.Contains("Lipper Pct. Rank"))
                        {
                            idx1 = detailDump.IndexOf("Growth of $10K Avg Annual Total Return %  10 YR 5 YR 1 YR YTD Market Return") + 76;
                            idx2 = detailDump.Substring(idx1).IndexOf("Lipper Pct. Rank");
                            string marketReturn = detailDump.Substring(idx1, idx2).Trim();
                            m = marketReturn.Split(' ');
                            for (int i = 0; i < m.Count(); i++)
                            {
                                mVal = 0.0M;
                                isDecimal = Decimal.TryParse(m[i].Replace("%", string.Empty), out mVal);
                                if (i == 0)
                                    fund.MarketReturn10Year = mVal;
                                else if (i == 1)
                                    fund.MarketReturn5Year = mVal;
                                else if (i == 2)
                                    fund.MarketReturn1Year = mVal;
                                else if (i == 3)
                                    fund.MarketReturnYTD = mVal;
                            }
                        }
                        // Avg Annual Return Percentage Lippert Pct Rank
                        if (detailDump.Contains("Lipper Pct. Rank") && detailDump.Contains("NAV Return"))
                        {
                            idx1 = detailDump.IndexOf("Lipper Pct. Rank") + 17;
                            idx2 = detailDump.Substring(idx1).IndexOf("NAV Return");
                            string marketReturnRank = detailDump.Substring(idx1, idx2).Trim();
                            m = marketReturnRank.Split(' ');
                            iVal = 0;
                            isInt = Int32.TryParse(m[0], out iVal);
                            fund.MarketReturnRank10Year = iVal;
                            isInt = Int32.TryParse(m[1], out iVal);
                            fund.MarketReturnRank5Year = iVal;
                            isInt = Int32.TryParse(m[2], out iVal);
                            fund.MarketReturnRank1Year = iVal;
                            isInt = Int32.TryParse(m[3], out iVal);
                            fund.MarketReturnRankYTD = iVal;
                        }
                        // NAV Return Pct
                        if (detailDump.Contains("NAV Return") && detailDump.Contains("Lipper Pct. Rank"))
                        {
                            idx1 = detailDump.IndexOf("NAV Return") + 11;
                            idx2 = detailDump.Substring(idx1).IndexOf("Lipper Pct. Rank") - 1;
                            string navReturn = detailDump.Substring(idx1, idx2).Trim();
                            m = navReturn.Split(' ');
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(m[0].Replace("%", string.Empty), out mVal);
                            fund.NavReturn10Year = mVal;
                            isDecimal = Decimal.TryParse(m[1].Replace("%", string.Empty), out mVal);
                            fund.NavReturn5Year = mVal;
                            isDecimal = Decimal.TryParse(m[2].Replace("%", string.Empty), out mVal);
                            fund.NavReturn1Year = mVal;
                            isDecimal = Decimal.TryParse(m[3].Replace("%", string.Empty), out mVal);
                            fund.NavReturnYTD = mVal;
                        }
                        // NAV Lipper Pct Rank
                        idx1 = idx1 + idx2 + 1;
                        string test4 = detailDump.Substring(idx1);
                        if (detailDump.Substring(idx1).Contains("Lipper Pct. Rank"))
                        {
                            pad = 1;
                            idx1 = idx1 + 16;
                            idx2 = 12;
                            string navReturnRank = detailDump.Substring(idx1, idx2).Trim();
                            m = navReturnRank.Split(' ');
                            iVal = 0;
                            isInt = Int32.TryParse(m[0], out iVal);
                            fund.NavReturnRank10Year = iVal;
                            isInt = Int32.TryParse(m[1], out iVal);
                            fund.NavReturnRank5Year = iVal;
                            isInt = Int32.TryParse(m[2], out iVal);
                            fund.NavReturnRank1Year = iVal;
                            isInt = Int32.TryParse(m[3], out iVal);
                            fund.NavReturnRankYTD = iVal;
                        }
                        else if (detailDump.Substring(idx1, 3) == " --")
                        {
                            pad = 3;
                            idx2 = detailDump.Substring(idx1).IndexOf("NAV");
                            string navReturnRank = detailDump.Substring(idx1, idx2).Trim();
                            m = navReturnRank.Split(' ');
                            iVal = 0;
                            isInt = Int32.TryParse(m[0], out iVal);
                            fund.NavReturnRank10Year = iVal;
                            isInt = Int32.TryParse(m[1], out iVal);
                            fund.NavReturnRank5Year = iVal;
                            isInt = Int32.TryParse(m[2], out iVal);
                            fund.NavReturnRank1Year = iVal;
                            isInt = Int32.TryParse(m[3], out iVal);
                            fund.NavReturnRankYTD = iVal;

                        }
                        // NAV
                        if (detailDump.Contains("NAV $"))
                        {
                            idx1 = detailDump.IndexOf("NAV $") + 5;
                        }
                        else if (detailDump.Contains("NAV --"))
                        {
                            idx1 = detailDump.IndexOf("NAV --") + 3;
                        }
                        idx2 = detailDump.Substring(idx1).IndexOf("Market Price");
                        if (idx1 > 0 && idx2 > 0)
                        {
                            string navPrice = detailDump.Substring(idx1, idx2).Trim();
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(navPrice.Replace("$", string.Empty), out mVal);
                            fund.Nav = mVal;
                        }

                        // Premium Discount History
                        if (detailDump.Contains("Premium/Discount History Premium/Discount 10 YR Avg 5 YR Avg YTD Avg"))
                        {
                            idx1 = detailDump.IndexOf("Premium/Discount History Premium/Discount 10 YR Avg 5 YR Avg YTD Avg") + 69;
                            string test3 = detailDump.Substring(idx1);

                            idx2 = detailDump.Substring(idx1).IndexOf("NAV");
                            string premiumDiscount = detailDump.Substring(idx1, idx2).Trim();
                            m = premiumDiscount.Split(' ');
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(m[0].Replace("%", string.Empty), out mVal);
                            fund.PremiumDiscount10YearAvg = mVal;
                            isDecimal = Decimal.TryParse(m[1].Replace("%", string.Empty), out mVal);
                            fund.PremiumDiscount5YearAvg = mVal;
                            isDecimal = Decimal.TryParse(m[2].Replace("%", string.Empty), out mVal);
                            fund.PremiumDiscountYTDAvg = mVal;
                        }

                        // Market Price
                        if (detailDump.Contains("Market Price"))
                        {
                            idx1 = detailDump.IndexOf("Market Price") + 13;
                            idx2 = detailDump.Substring(idx1).IndexOf("Net Change");
                            string marketPrice = detailDump.Substring(idx1, idx2).Trim();
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(marketPrice.Replace("$", string.Empty), out mVal);
                            fund.MarketPrice = mVal;
                        }

                        // Net Change
                        if (detailDump.Contains("Market Price"))
                        {
                            idx1 = detailDump.IndexOf("Net Change") + 11;
                            idx2 = detailDump.Substring(idx1).IndexOf("Market Change");
                            string netChange = detailDump.Substring(idx1, idx2).Trim();
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(netChange.Replace("%", string.Empty), out mVal);
                            fund.NetChange = mVal;
                        }

                        // Market Change
                        if (detailDump.Contains("Market Change"))
                        {
                            idx1 = detailDump.IndexOf("Market Change") + 14;
                            idx2 = detailDump.Substring(idx1).IndexOf("Premium/Discount");
                            string marketChange = detailDump.Substring(idx1, idx2).Trim();
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(marketChange.Replace("%", string.Empty), out mVal);
                            fund.MarketChange = mVal;
                        }

                        // Premium Discount
                        if (detailDump.Contains("Premium/Discount"))
                        {
                            idx1 = detailDump.LastIndexOf("Premium/Discount") + 17;
                            idx2 = detailDump.Substring(idx1).IndexOf("1 YR NAV Return");
                            fund.PremiumDiscount = 0.0M;
                            if (idx1 > 0 && idx2 > 0)
                            {
                                string premiumDiscount = detailDump.Substring(idx1, idx2).Trim();
                                mVal = 0.0M;
                                isDecimal = Decimal.TryParse(premiumDiscount.Replace("%", string.Empty), out mVal);
                                fund.PremiumDiscount = mVal;
                            }
                            //MessageBox.Show(this, marketChange, "Market Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        // 1 YR NAV return
                        if (detailDump.Contains("1 YR NAV Return"))
                        {
                            idx1 = detailDump.IndexOf("1 YR NAV Return") + 16;
                            idx2 = detailDump.Substring(idx1).IndexOf("1 YR NAV Rank");
                            string premiumDiscount2 = string.Empty;
                            if (idx2 > 0)
                            {
                                premiumDiscount2 = detailDump.Substring(idx1, idx2).Trim();
                            }
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(premiumDiscount2.Replace("%", string.Empty), out mVal);
                            fund.OneYearNavReturn = mVal;
                        }

                        // 1 YR NAV RANK
                        if (detailDump.Contains("1 YR NAV Rank"))
                        {
                            idx1 = detailDump.IndexOf("1 YR NAV Rank") + 14;
                            idx2 = detailDump.Substring(idx1).IndexOf("12-Mo Yield");
                            string oneYrNavRank = detailDump.Substring(idx1, idx2).Trim();
                            iVal = 0;
                            isInt = Int32.TryParse(oneYrNavRank, out iVal);
                            fund.OneYearNavRank = mVal;
                        }

                        // 12 MO Yield As Of
                        if (detailDump.Contains("12-Mo Yield  as of"))
                        {
                            idx1 = detailDump.IndexOf("12-Mo Yield  as of") + 19;
                            idx2 = detailDump.Substring(idx1).IndexOf("Yield") - 1;
                            string twelveMoYearAsOf = detailDump.Substring(idx1, idx2).Trim();
                            fund.TwelveMoYieldAsOf = twelveMoYearAsOf;
                        }

                        // Defined Income Only Yield
                        if (detailDump.Contains("Def Income Only Yield"))
                        {
                            idx1 = detailDump.IndexOf("Def Income Only Yield") + 22;
                            idx2 = detailDump.Substring(idx1).IndexOf("Distribution Yield  (Market)") - 1;
                            string defIncomeOnlyYield = detailDump.Substring(idx1, idx2).Trim();
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(defIncomeOnlyYield.Replace("%", string.Empty), out mVal);
                            fund.DefinedIncomeOnlyYield = mVal;
                        }

                        // Distribution Yield (Market)
                        if (detailDump.Contains("Distribution Yield  (Market)"))
                        {
                            idx1 = detailDump.IndexOf("Distribution Yield  (Market)") + 29;
                            idx2 = detailDump.Substring(idx1).IndexOf("Most Recent Income Dividend") - 1;
                            string marketDistYield = detailDump.Substring(idx1, idx2).Trim();
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(marketDistYield.Replace("%", string.Empty), out mVal);
                            fund.DistributionYield = mVal;
                        }

                        // Most Recent Income Dividend
                        if (detailDump.Contains("Most Recent Income Dividend"))
                        {
                            idx1 = detailDump.IndexOf("Most Recent Income Dividend") + 28;
                            idx2 = detailDump.Substring(idx1).IndexOf("Ex-Div Date") - 1;
                            string mostRecentIncomeDiv = detailDump.Substring(idx1, idx2).Trim();
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(mostRecentIncomeDiv.Replace("$", string.Empty), out mVal);
                            fund.MostRecentIncomeDividend = mVal;
                        }

                        // Ex-Div Date
                        if (detailDump.Contains("Ex-Div Date"))
                        {
                            idx1 = detailDump.IndexOf("Ex-Div Date") + 12;
                            idx2 = detailDump.Substring(idx1).IndexOf("Most Recent Cap Gain Dividend") - 1;
                            string mrIncomeDivExDivDate = detailDump.Substring(idx1, idx2).Trim();
                            fund.MostRecentIncomeDividendDate = mrIncomeDivExDivDate;
                        }

                        // Most Recent Cap Gain Dividend
                        if (detailDump.Contains("Most Recent Cap Gain Dividend"))
                        {
                            idx1 = detailDump.IndexOf("Most Recent Cap Gain Dividend") + 30;
                            idx2 = detailDump.Substring(idx1).IndexOf("Ex-Div Date") - 1;
                            string mostRecentCapGainDiv = detailDump.Substring(idx1, idx2).Trim();
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(mostRecentCapGainDiv.Replace("$", string.Empty), out mVal);
                            fund.MostRecentCapGainDividend = mVal;
                        }

                        // Ex-Div Date
                        if (detailDump.Contains("Ex-Div Date"))
                        {
                            idx1 = detailDump.IndexOf("Most Recent Cap Gain Dividend") + 42 + idx2;
                            idx2 = detailDump.Substring(idx1).IndexOf("Dividend Frequency Monthly YTD Dividends") - 1;
                            if (idx2 == -2)
                            {
                                idx2 = detailDump.Substring(idx1).IndexOf("Dividend Frequency Daily YTD Dividends") - 1;
                            }
                            if (idx2 == -2)
                            {
                                idx2 = detailDump.Substring(idx1).IndexOf("Dividend Frequency Quarterly YTD Dividends") - 1;
                            }
                            if (idx2 == -2)
                            {
                                idx2 = detailDump.Substring(idx1).IndexOf("Dividend Frequency Semiannual YTD Dividends") - 1;
                            }
                            if (idx2 == -2)
                            {
                                idx2 = detailDump.Substring(idx1).IndexOf("Dividend Frequency Annually YTD Dividends") - 1;
                            }
                            string mrCapGainsDivExDivDate = detailDump.Substring(idx1, idx2).Trim();
                            fund.MostRecentCapGainDividendDate = mrCapGainsDivExDivDate;
                        }

                        // Monthly YTD Dividends
                        if (detailDump.Contains("Dividend Frequency Monthly YTD Dividends"))
                        {
                            idx1 = detailDump.IndexOf("Dividend Frequency Monthly YTD Dividends") + 41;
                            idx2 = detailDump.Substring(idx1).IndexOf("YTD Capital Gains") - 1;
                            string dfMonthlyYTDDividends = detailDump.Substring(idx1, idx2).Trim();
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(dfMonthlyYTDDividends.Replace("$", string.Empty), out mVal);
                            fund.MonthlyYTDDivedends = mVal;
                        }

                        // YTD Capital Gains
                        if (detailDump.Contains("YTD Capital Gains"))
                        {
                            idx1 = detailDump.IndexOf("YTD Capital Gains") + 18;
                            idx2 = detailDump.Substring(idx1).IndexOf("Inception Date") - 1;
                            string ytdCapitalGains = detailDump.Substring(idx1, idx2).Trim();
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(ytdCapitalGains.Replace("$", string.Empty), out mVal);
                            fund.YTDCapGains = mVal;
                        }

                        // Inception Date
                        if (detailDump.Contains("Inception Date"))
                        {
                            idx1 = detailDump.IndexOf("Inception Date") + 15;
                            idx2 = detailDump.Substring(idx1).IndexOf("Fund Advisor") - 1;
                            string inceptionDate = detailDump.Substring(idx1, idx2).Trim();
                            fund.InceptionDate = inceptionDate;
                        }

                        // Fund Advisor
                        if (detailDump.Contains("Fund Advisor"))
                        {
                            idx1 = detailDump.IndexOf("Fund Advisor") + 13;
                            idx2 = detailDump.Substring(idx1).IndexOf("Manager & Tenure");
                            string fundAdvisor = string.Empty;
                            if (idx1 > 0 && idx2 > 0)
                                fundAdvisor = detailDump.Substring(idx1, idx2);
                            fund.FundAdvisor = fundAdvisor;
                        }

                        // Manager & Tenure
                        if (detailDump.Contains("Manager & Tenure"))
                        {
                            idx1 = detailDump.IndexOf("Manager & Tenure") + 17;
                            idx2 = detailDump.Substring(idx1).IndexOf("Phone");
                            string managerAndTenure = detailDump.Substring(idx1, idx2);
                            fund.ManagerAndTenure = managerAndTenure;
                        }

                        // Phone
                        if (detailDump.Contains("Phone"))
                        {
                            idx1 = detailDump.IndexOf("Phone") + 6;
                            idx2 = detailDump.Substring(idx1).IndexOf("Website");
                            string phone = detailDump.Substring(idx1, idx2);
                            fund.Phone = phone;
                        }

                        // Website
                        if (detailDump.Contains("Website"))
                        {
                            idx1 = detailDump.IndexOf("Website") + 8;
                            idx2 = detailDump.Substring(idx1).IndexOf("Total Net Assets  (mil)  as of");
                            string webSite = "http://" + detailDump.Substring(idx1, idx2);
                            fund.Website = webSite;
                        }

                        // Total Net Assets (mil)
                        if (detailDump.Contains("Total Net Assets  (mil)  as of"))
                        {
                            idx1 = detailDump.IndexOf("Total Net Assets  (mil)  as of") + 32;
                            idx2 = detailDump.Substring(idx1).IndexOf("% Leveraged Assets  as of") - 1;
                            string tna = detailDump.Substring(idx1, idx2).Trim();
                            string[] n = tna.Split(' ');
                            string tnaDate = n[0];
                            string tnaValue = n[1];
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(tnaValue.Replace("$", string.Empty), out mVal);
                            fund.TotalNetAssets = mVal;
                            fund.TotalNetAssetsDate = tnaDate;
                        }

                        // % Leveraged Assets
                        if (detailDump.Contains("% Leveraged Assets  as of"))
                        {
                            idx1 = detailDump.IndexOf("% Leveraged Assets  as of") + 26;
                            idx2 = detailDump.Substring(idx1).IndexOf("Portfolio Turnover") - 1;
                            string tla = detailDump.Substring(idx1, idx2).Trim();
                            string[] o = tla.Split(' ');
                            string tlaDate = o[0];
                            string tlaValue = o[1];
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(tlaValue.Replace("$", string.Empty), out mVal);
                            fund.PercentLeveragedAssets = mVal;
                            fund.PercentLeveragedAssetsDate = tlaDate;
                        }

                        // Portfolio Turnover
                        if (detailDump.Contains("Portfolio Turnover"))
                        {
                            idx1 = detailDump.IndexOf("Portfolio Turnover") + 19;
                            idx2 = detailDump.Substring(idx1).IndexOf("Mgmt Fees") - 1;
                            string portfolioTurnover = detailDump.Substring(idx1, idx2).Trim();
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(portfolioTurnover.Replace("%", string.Empty), out mVal);
                            fund.PortfolioTurnover = mVal;
                        }

                        // Mgmt Fees
                        if (detailDump.Contains("Mgmt Fees"))
                        {
                            idx1 = detailDump.IndexOf("Mgmt Fees") + 10;
                            idx2 = detailDump.Substring(idx1).IndexOf("Expense Ratio") - 1;
                            string mgmtFees = detailDump.Substring(idx1, idx2).Trim();
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(mgmtFees.Replace("%", string.Empty), out mVal);
                            fund.ManagementFees = mVal;
                        }

                        // Expense Ratio
                        if (detailDump.Contains("Expense Ratio"))
                        {
                            idx1 = detailDump.IndexOf("Expense Ratio") + 14;
                            idx2 = detailDump.Substring(idx1).IndexOf("Alternative Minimum Tax") - 1;
                            if (idx2 == -2)
                            {
                                idx2 = detailDump.Substring(idx1).IndexOf("Fund Objective") - 1;
                            }
                            if (idx1 > 0 && idx2 > 0)
                            {
                                string expenseRatio = detailDump.Substring(idx1, idx2).Trim();
                                mVal = 0.0M;
                                isDecimal = Decimal.TryParse(expenseRatio.Replace("%", string.Empty), out mVal);
                                fund.ExpenseRatio = mVal;
                            }
                        }

                        // AMT
                        if (detailDump.Contains("Alternative Minimum Tax"))
                        {
                            idx1 = detailDump.IndexOf("Alternative Minimum Tax") + 24;
                            idx2 = detailDump.Substring(idx1).IndexOf("Fund Objective") - 1;
                            string amt = detailDump.Substring(idx1, idx2).Trim();
                            mVal = 0.0M;
                            isDecimal = Decimal.TryParse(amt.Replace("%", string.Empty), out mVal);
                            fund.AlternativeMinimumTax = mVal;
                        }

                        // Fund Objective
                        if (detailDump.Contains("Fund Objective"))
                        {
                            string fundObjective = string.Empty;
                            idx1 = detailDump.IndexOf("Fund Objective") + 15;
                            idx2 = detailDump.Substring(idx1).IndexOf("Total Net Assets by Category  (as of") - 1;
                            if (idx2 < 0)
                            {
                                idx2 = detailDump.Substring(idx1).IndexOf("Quality  (as of") - 1;
                            }
                            if (idx2 < 0)
                            {
                                idx2 = detailDump.Substring(idx1).IndexOf("Top Holdings  (as of") - 1;
                            }
                            if (idx2 < 0)
                            {
                                idx2 = detailDump.Substring(idx1).IndexOf("1 YR Lipper Average not available for this fund.") - 2;
                            }
                            if (idx2 < 0)
                            {
                                idx2 = detailDump.Length - 1 - idx1 - idx2 - 2;
                                fundObjective = detailDump.Substring(idx1, idx2).Trim();
                                if (fundObjective.IndexOf("Resources") > 0)
                                    fund.FundObjective = fundObjective.Substring(0, fundObjective.IndexOf("Resources"));
                                else
                                    fund.FundObjective = fundObjective;
                            }
                            else
                            {
                                fundObjective = detailDump.Substring(idx1, idx2).Trim();
                                if (fundObjective.IndexOf("Resources") > 0)
                                    fund.FundObjective = fundObjective.Substring(0, fundObjective.IndexOf("Resources"));
                                else
                                    fund.FundObjective = fundObjective;
                            }
                        }

                        // Total Net Assets
                        if (detailDump.Contains("Total Net Assets by Category  (as of"))
                        {
                            string netAssets = string.Empty;
                            idx1 = detailDump.IndexOf("Total Net Assets by Category  (as of") + 37;
                            idx2 = detailDump.Substring(idx1).IndexOf(")");
                            string netAssetsAsOf = detailDump.Substring(idx1, idx2).Trim();
                            fund.TopAssetsDate = netAssetsAsOf;
                            int idx3 = detailDump.Substring(idx1 + idx2 + 2).IndexOf("Quality  (as of") - 1;
                            if (idx3 < 0)
                            {
                                idx3 = detailDump.Length - 1 - idx1 - idx2 - 2;
                                netAssets = detailDump.Substring(idx1 + idx2 + 2, idx3).Trim();
                                if (netAssets.IndexOf("Resources") > 0)
                                    netAssets.Substring(0, netAssets.IndexOf("Resources"));
                                else
                                    fund.TopAssets = netAssets;
                            }
                            netAssets = detailDump.Substring(idx1 + idx2 + 2, idx3).Trim();
                            if (netAssets.IndexOf("Resources") > 0)
                                netAssets.Substring(0, netAssets.IndexOf("Resources"));
                            else
                                fund.TopAssets = netAssets;
                        }

                        // Top Holdings
                        if (detailDump.Contains("Top Holdings  (as of"))
                        {
                            idx1 = detailDump.IndexOf("Top Holdings  (as of") + 21;
                            idx2 = detailDump.Substring(idx1).IndexOf(")");
                            string netAssetsAsOf = detailDump.Substring(idx1, idx2).Trim();
                            string netAssets = string.Empty;
                            fund.TopAssetsDate = netAssetsAsOf;
                            int idx3 = detailDump.Substring(idx1 + idx2 + 2).IndexOf("Top Sectors  (as of") - 1;
                            if (idx3 < 0)
                            {
                                idx3 = detailDump.Length - 1 - idx1 - idx2 - 2;
                                netAssets = detailDump.Substring(idx1 + idx2 + 2, idx3).Trim();
                                fund.TopAssets = netAssets;
                            }
                            else
                            {
                                netAssets = detailDump.Substring(idx1 + idx2 + 2, idx3).Trim();
                            }
                            if (netAssets.IndexOf("Resources") > 0)
                                netAssets.Substring(0, netAssets.IndexOf("Resources"));
                            else
                                fund.TopAssets = netAssets;
                        }

                        // Quality
                        if (detailDump.Contains("Quality  (as of"))
                        {
                            idx1 = detailDump.IndexOf("Quality  (as of") + 16;
                            idx2 = detailDump.Substring(idx1).IndexOf(")");
                            string qualityAsOf = detailDump.Substring(idx1, idx2).Trim();
                            fund.QualityDate = qualityAsOf;
                            string quality = detailDump.Substring(idx1 + idx2 + 2).Trim().Replace(@"\r\n", string.Empty);
                            fund.Quality = quality;
                        }
                        if (!fund.Ticker.Contains("MoYield"))
                        {
                            fundList.Add(fund);
                            // Insert into sql server
                            bool dbStatus = saveFundToDatabase(fund);
                        }

                    }
                }
            }
        }

        public static bool saveFundToDatabase(fundDetail fund)
        {
            Guid fund_id = Guid.NewGuid();
            string fundId = string.Empty;
            int rowcount = 0;
            // Check for the fund id
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.NCrawlerConn))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id from Funds WHERE Name = @name and [Ticker Symbol] = @ticker ";
                try
                {
                    SqlParameter tickerSymbolParam = new SqlParameter("@ticker", SqlDbType.VarChar);
                    tickerSymbolParam.Value = fund.Ticker;
                    command.Parameters.Add(tickerSymbolParam);
                    SqlParameter nameParam = new SqlParameter("@name", SqlDbType.VarChar);
                    nameParam.Value = fund.Name;
                    command.Parameters.Add(nameParam);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fundId = reader["id"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            if (string.IsNullOrEmpty(fundId))
            {
                fundId = fund_id.ToString().Replace("{", string.Empty).Replace("}", string.Empty).Replace("-", string.Empty);
                // Insert Funds record
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.NCrawlerConn))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Funds (id, Name, [Fund Type], [Ticker Symbol], [Asset Class], [Inception Date], Advisor, [Manager and Tenure], Phone, Website, [Total Net Assets], [Total Net Assets Date], [Percent Leveraged Assets], [Percent Leveraged Assets Date], [Portfolio Turnover], [Management Fees], [Expense Ratio], [Alternative Minimum Tax], [Fund Objective], Yield) ";
                    command.CommandText += "VALUES(@id,@name,@fundType,@tickerSymbol,@assetClass,@inceptionDate,@advisor,@managerAndTenure,@phone,@website,@totalNetAssets,@totalNetAssetsDate,@percentLeveragedAssets,@percentLeveragedAssetsDate,@portfolioTurnover,@managementFees,@expenseRatio,@alternativeMinimumTax,@fundObjective,@yield)";
                    try
                    {
                        SqlParameter idParam = new SqlParameter("@id", SqlDbType.VarChar);
                        idParam.Value = fundId;
                        command.Parameters.Add(idParam);
                        SqlParameter nameParam = new SqlParameter("@name", SqlDbType.VarChar);
                        nameParam.Value = fund.Name;
                        command.Parameters.Add(nameParam);
                        SqlParameter fundTypeParam = new SqlParameter("@fundType", SqlDbType.VarChar);
                        fundTypeParam.Value = "Closed End Bond Fund";
                        command.Parameters.Add(fundTypeParam);

                        SqlParameter tickerSymbolParam = new SqlParameter("@tickerSymbol", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(fund.Ticker))
                            tickerSymbolParam.Value = fund.Ticker;
                        else
                            tickerSymbolParam.Value = "--";
                        command.Parameters.Add(tickerSymbolParam);
                        SqlParameter assetClassParam = new SqlParameter("@assetClass", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(fund.AssetClass))
                            assetClassParam.Value = fund.AssetClass;
                        else
                            assetClassParam.Value = "--";
                        command.Parameters.Add(assetClassParam);
                        SqlParameter inceptionDateParam = new SqlParameter("@inceptionDate", SqlDbType.DateTime);
                        if (fund.InceptionDate != null)
                            inceptionDateParam.Value = Convert.ToDateTime(fund.InceptionDate);
                        else
                            inceptionDateParam.Value = SqlDateTime.MinValue;
                        command.Parameters.Add(inceptionDateParam);


                        SqlParameter advisorParam = new SqlParameter("@advisor", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(fund.FundAdvisor))
                            advisorParam.Value = fund.FundAdvisor;
                        else
                            advisorParam.Value = "--";
                        command.Parameters.Add(advisorParam);
                        SqlParameter managerAndTenureParam = new SqlParameter("@managerAndTenure", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(fund.ManagerAndTenure))
                            managerAndTenureParam.Value = fund.ManagerAndTenure;
                        else
                            managerAndTenureParam.Value = "--";
                        command.Parameters.Add(managerAndTenureParam);
                        SqlParameter phoneParam = new SqlParameter("@phone", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(fund.Phone))
                            phoneParam.Value = fund.Phone;
                        else
                            phoneParam.Value = "--";
                        command.Parameters.Add(phoneParam);
                        SqlParameter websiteParam = new SqlParameter("@website", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(fund.Website))
                            websiteParam.Value = fund.Website;
                        else
                            websiteParam.Value = "--";
                        command.Parameters.Add(websiteParam);
                        SqlParameter totalNetAssetsParam = new SqlParameter("@totalNetAssets", SqlDbType.Decimal);
                        totalNetAssetsParam.Value = fund.TotalNetAssets;
                        command.Parameters.Add(totalNetAssetsParam);
                        SqlParameter totalNetAssetsDateParam = new SqlParameter("@totalNetAssetsDate", SqlDbType.DateTime);
                        if (!string.IsNullOrEmpty(fund.TotalNetAssetsDate) && (fund.TotalNetAssetsDate != "--"))
                        {
                            totalNetAssetsDateParam.Value = Convert.ToDateTime(fund.TotalNetAssetsDate);
                        }
                        else
                        {
                            totalNetAssetsDateParam.Value = SqlDateTime.MinValue;
                        }
                        command.Parameters.Add(totalNetAssetsDateParam);
                        SqlParameter percentLeveragedAssetsParam = new SqlParameter("@percentLeveragedAssets", SqlDbType.Decimal);
                        percentLeveragedAssetsParam.Value = fund.PercentLeveragedAssets;
                        command.Parameters.Add(percentLeveragedAssetsParam);

                        SqlParameter percentLeveragedAssetsDateParam = new SqlParameter("@percentLeveragedAssetsDate", SqlDbType.DateTime);
                        if (!string.IsNullOrEmpty(fund.PercentLeveragedAssetsDate) && (fund.PercentLeveragedAssetsDate != "--"))
                        {
                            percentLeveragedAssetsDateParam.Value = Convert.ToDateTime(fund.PercentLeveragedAssetsDate);
                        }
                        else
                        {
                            percentLeveragedAssetsDateParam.Value = SqlDateTime.MinValue;
                        }
                        command.Parameters.Add(percentLeveragedAssetsDateParam);
                        SqlParameter portfolioTurnoverParam = new SqlParameter("@portfolioTurnover", SqlDbType.Decimal);
                        portfolioTurnoverParam.Value = fund.PortfolioTurnover;
                        command.Parameters.Add(portfolioTurnoverParam);
                        SqlParameter managementFeesParam = new SqlParameter("@managementFees", SqlDbType.Decimal);
                        managementFeesParam.Value = fund.ManagementFees;
                        command.Parameters.Add(managementFeesParam);
                        SqlParameter expenseRatioParam = new SqlParameter("@expenseRatio", SqlDbType.Decimal);
                        expenseRatioParam.Value = fund.ExpenseRatio;
                        command.Parameters.Add(expenseRatioParam);
                        SqlParameter alternativeMinimumTaxParam = new SqlParameter("@alternativeMinimumTax", SqlDbType.Decimal);
                        alternativeMinimumTaxParam.Value = fund.AlternativeMinimumTax;
                        command.Parameters.Add(alternativeMinimumTaxParam);
                        SqlParameter fundObjectiveParam = new SqlParameter("@fundObjective", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(fund.FundObjective))
                            fundObjectiveParam.Value = fund.FundObjective;
                        else
                            fundObjectiveParam.Value = "Unstated";
                        command.Parameters.Add(fundObjectiveParam);
                        SqlParameter yieldParam = new SqlParameter("@yield", SqlDbType.Decimal);
                        yieldParam.Value = fund.DistributionYield;
                        command.Parameters.Add(yieldParam);

                        connection.Open();
                        rowcount += command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                Console.WriteLine(rowcount.ToString() + " fund rows inserted");
            }
            else
            {
                // update the record
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.NCrawlerConn))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Funds SET Advisor = @advisor, [Manager and Tenure] = @managerAndTenure, Phone = @phone, Website = @website, [Total Net Assets] = @totalNetAssets, [Total Net Assets Date] = @totalNetAssetsDate, [Percent Leveraged Assets] = @percentLeveragedAssets, [Percent Leveraged Assets Date] = @percentLeveragedAssetsDate, [Portfolio Turnover] = @portfolioTurnover, [Management Fees] = @managementFees, [Expense Ratio] = @expenseRatio, [Alternative Minimum Tax] = @alternativeMinimumTax, [Fund Objective] = @fundObjective, Yield = @yield WHERE id = @id ";
                    try
                    {
                        SqlParameter idParam = new SqlParameter("@id", SqlDbType.VarChar);
                        idParam.Value = fundId;
                        command.Parameters.Add(idParam);
                        SqlParameter nameParam = new SqlParameter("@name", SqlDbType.VarChar);
                        nameParam.Value = fund.Name;
                        command.Parameters.Add(nameParam);
                        SqlParameter fundTypeParam = new SqlParameter("@fundType", SqlDbType.VarChar);
                        fundTypeParam.Value = "Closed End Bond Fund";
                        command.Parameters.Add(fundTypeParam);

                        SqlParameter tickerSymbolParam = new SqlParameter("@tickerSymbol", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(fund.Ticker))
                            tickerSymbolParam.Value = fund.Ticker;
                        else
                            tickerSymbolParam.Value = "--";
                        command.Parameters.Add(tickerSymbolParam);
                        SqlParameter assetClassParam = new SqlParameter("@assetClass", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(fund.AssetClass))
                            assetClassParam.Value = fund.AssetClass;
                        else
                            assetClassParam.Value = "--";
                        command.Parameters.Add(assetClassParam);
                        SqlParameter inceptionDateParam = new SqlParameter("@inceptionDate", SqlDbType.DateTime);
                        if (fund.InceptionDate != null)
                            inceptionDateParam.Value = Convert.ToDateTime(fund.InceptionDate);
                        else
                            inceptionDateParam.Value = SqlDateTime.MinValue;
                        command.Parameters.Add(inceptionDateParam);
                        SqlParameter advisorParam = new SqlParameter("@advisor", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(fund.FundAdvisor))
                            advisorParam.Value = fund.FundAdvisor;
                        else
                            advisorParam.Value = "--";
                        command.Parameters.Add(advisorParam);
                        SqlParameter managerAndTenureParam = new SqlParameter("@managerAndTenure", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(fund.ManagerAndTenure))
                            managerAndTenureParam.Value = fund.ManagerAndTenure;
                        else
                            managerAndTenureParam.Value = "--";
                        command.Parameters.Add(managerAndTenureParam);
                        SqlParameter phoneParam = new SqlParameter("@phone", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(fund.Phone))
                            phoneParam.Value = fund.Phone;
                        else
                            phoneParam.Value = "--";
                        command.Parameters.Add(phoneParam);
                        SqlParameter websiteParam = new SqlParameter("@website", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(fund.Website))
                            websiteParam.Value = fund.Website;
                        else
                            websiteParam.Value = "--";
                        command.Parameters.Add(websiteParam);
                        SqlParameter totalNetAssetsParam = new SqlParameter("@totalNetAssets", SqlDbType.Decimal);
                        totalNetAssetsParam.Value = fund.TotalNetAssets;
                        command.Parameters.Add(totalNetAssetsParam);
                        SqlParameter totalNetAssetsDateParam = new SqlParameter("@totalNetAssetsDate", SqlDbType.DateTime);
                        if (!string.IsNullOrEmpty(fund.TotalNetAssetsDate) && (fund.TotalNetAssetsDate != "--"))
                        {
                            totalNetAssetsDateParam.Value = Convert.ToDateTime(fund.TotalNetAssetsDate);
                        }
                        else
                        {
                            totalNetAssetsDateParam.Value = SqlDateTime.MinValue;
                        }
                        command.Parameters.Add(totalNetAssetsDateParam);
                        SqlParameter percentLeveragedAssetsParam = new SqlParameter("@percentLeveragedAssets", SqlDbType.Decimal);
                        percentLeveragedAssetsParam.Value = fund.PercentLeveragedAssets;
                        command.Parameters.Add(percentLeveragedAssetsParam);

                        SqlParameter percentLeveragedAssetsDateParam = new SqlParameter("@percentLeveragedAssetsDate", SqlDbType.DateTime);
                        if (!string.IsNullOrEmpty(fund.PercentLeveragedAssetsDate) && (fund.PercentLeveragedAssetsDate != "--"))
                        {
                            percentLeveragedAssetsDateParam.Value = Convert.ToDateTime(fund.PercentLeveragedAssetsDate);
                        }
                        else
                        {
                            percentLeveragedAssetsDateParam.Value = SqlDateTime.MinValue;
                        }
                        command.Parameters.Add(percentLeveragedAssetsDateParam);
                        SqlParameter portfolioTurnoverParam = new SqlParameter("@portfolioTurnover", SqlDbType.Decimal);
                        portfolioTurnoverParam.Value = fund.PortfolioTurnover;
                        command.Parameters.Add(portfolioTurnoverParam);
                        SqlParameter managementFeesParam = new SqlParameter("@managementFees", SqlDbType.Decimal);
                        managementFeesParam.Value = fund.ManagementFees;
                        command.Parameters.Add(managementFeesParam);
                        SqlParameter expenseRatioParam = new SqlParameter("@expenseRatio", SqlDbType.Decimal);
                        expenseRatioParam.Value = fund.ExpenseRatio;
                        command.Parameters.Add(expenseRatioParam);
                        SqlParameter alternativeMinimumTaxParam = new SqlParameter("@alternativeMinimumTax", SqlDbType.Decimal);
                        alternativeMinimumTaxParam.Value = fund.AlternativeMinimumTax;
                        command.Parameters.Add(alternativeMinimumTaxParam);
                        SqlParameter fundObjectiveParam = new SqlParameter("@fundObjective", SqlDbType.VarChar);
                        if (!string.IsNullOrEmpty(fund.FundObjective))
                            fundObjectiveParam.Value = fund.FundObjective;
                        else
                            fundObjectiveParam.Value = "Unstated";
                        command.Parameters.Add(fundObjectiveParam);
                        SqlParameter yieldParam = new SqlParameter("@yield", SqlDbType.Decimal);
                        yieldParam.Value = fund.Yield;
                        command.Parameters.Add(yieldParam);

                        connection.Open();
                        rowcount += command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                Console.WriteLine(rowcount.ToString() + " fund rows updated");
            }

            // Insert FundDetails record
            Guid fund_details_id = Guid.NewGuid();
            string fundDetailsId = fund_details_id.ToString().Replace("{", string.Empty).Replace("}", string.Empty).Replace("-", string.Empty);
            DateTime crawlDate = fund.CrawlDate;


            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.NCrawlerConn))
            using (SqlCommand command = connection.CreateCommand())
            {
                //command.CommandText = "INSERT INTO FundDetails (id, fund_id, [Create Date], [Created By], [Crawl Date], OneYearLipperAvg, TenYearMarketReturn, FiveYearMarketReturn, OneYearMarketReturn, YTDMarketReturn, TenYearMarketReturnRank, FiveYearMarketReturnRank, OneYearMarketReturnRank, YTDMarketReturnRank, TenYearNAVReturn, FiveYearNAVReturn, OneYearNAVReturn, YTDNAVReturn, TenYearNAVReturnRank, FiveYearNAVReturnRank, OneYearNAVReturnRank, YTDNAVReturnRank, TenYearPremiumDiscountAvg, FiveYearPremiumDiscountAvg, YTDPremiumDiscountAvg, NAV, MarketPrice, NetChange, MarketChange, OneYearNAVReturnAct, TwelveMonthYieldDate, DefinedIncomeOnlyYield, DistributionYield, MostRecentIncimeDividend, MostRecentIncomeDividendDate, MostRecentCapGainDiviednd, MostRecentCapGainDividendDate, MonthlyYTDDividends, YTDCapGains) ";
                //command.CommandText += "VALUES(@id,@fundId,@createDate, @createdBy, @crawlDate, @oneYearLipperAvg, @tenYearMarketReturn, @fiveYearMarketReturn, @oneYearMarketReturn, @ytdMarketReturn, @tenYearMarketReturnRank, @fiveYearMarketReturnRank, @oneYearMarketReturnRank, @ytdMarketReturnRank, @tenYearNAVReturn, @fiveYearNAVReturn, @oneYearNAVReturn, @ytdNAVReturn, @tenYearNAVReturnRank, @fiveYearNAVReturnRank, @oneYearNAVReturnRank, @ytdNAVReturnRank, @tenYearPremiumDiscountAvg, @fiveYearPremiumDiscountAvg, @ytdPremiumDiscountAvg, @nav, @marketPrice, @netChange, @marketChange, @oneYearNAVReturnAct, @twelveMonthYieldDate, @definedIncomeOnlyYield, @distributionYield, @mostRecentIncomeDividend, @mostRecentIncomeDividendDate, @mostRecentCapGainDividend, @mostRecentCapGainDividendDate, @monthlyYTDDiviends, @ytdCapGains)";
                command.CommandText = "INSERT INTO FundDetails (id, fund_id, [Create Date], [Created By], [Crawl Date], OneYearLipperAvg,  TenYearMarketReturn,  FiveYearMarketReturn,  OneYearMarketReturn,  YTDMarketReturn,  TenYearMarketReturnRank,  FiveYearMarketReturnRank,  OneYearMarketReturnRank,  YTDMarketReturnRank,  TenYearNAVReturn,  FiveYearNAVReturn,  OneYearNAVReturn,  YTDNAVReturn,  TenYearNAVReturnRank,  FiveYearNAVReturnRank,  OneYearNAVReturnRank,  YTDNAVReturnRank,  TenYearPremiumDiscountAvg,  FiveYearPremiumDiscountAvg,  YTDPremiumDiscountAvg,  NAV,  MarketPrice,  NetChange,  MarketChange, OneYearNAVReturnAct, TwelveMonthYieldDate, DefinedIncomeOnlyYield, DistributionYield, MostRecentIncimeDividend, MostRecentCapGainDiviednd, MonthlyYTDDividends, YTDCapGains, PremiumDiscount) ";
                command.CommandText += "VALUES(                @id, @fundId, @createDate,   @createdBy,   @crawlDate,   @oneYearLipperAvg, @tenYearMarketReturn, @fiveYearMarketReturn, @oneYearMarketReturn, @ytdMarketReturn, @tenYearMarketReturnRank, @fiveYearMarketReturnRank, @oneYearMarketReturnRank, @ytdMarketReturnRank, @tenYearNAVReturn, @fiveYearNAVReturn, @oneYearNAVReturn, @ytdNAVReturn, @tenYearNAVReturnRank, @fiveYearNAVReturnRank, @oneYearNAVReturnRank, @ytdNAVReturnRank, @tenYearPremiumDiscountAvg, @fiveYearPremiumDiscountAvg, @ytdPremiumDiscountAvg, @nav, @marketPrice, @netChange, @marketChange, @oneYearNAVReturnAct, @twelveMonthYieldDate, @definedIncomeOnlyYield, @distributionYield, @mostRecentIncomeDividend, @mostRecentCapGainDividend, @monthlyYTDDiviends, @ytdCapGains, @premiumDiscount)";
                try
                {
                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.VarChar);
                    idParam.Value = fundDetailsId;
                    command.Parameters.Add(idParam);
                    SqlParameter fundIdParam = new SqlParameter("@fundId", SqlDbType.VarChar);
                    fundIdParam.Value = fundId;
                    command.Parameters.Add(fundIdParam);
                    SqlParameter createDateParam = new SqlParameter("@createDate", SqlDbType.DateTime);
                    createDateParam.Value = crawlDate;
                    command.Parameters.Add(createDateParam);
                    SqlParameter createdByParam = new SqlParameter("@createdBy", SqlDbType.VarChar);
                    createdByParam.Value = Environment.UserName;
                    command.Parameters.Add(createdByParam);
                    SqlParameter crawlDateParam = new SqlParameter("@crawlDate", SqlDbType.DateTime);
                    crawlDateParam.Value = crawlDate;
                    command.Parameters.Add(crawlDateParam);
                    SqlParameter oneYearLipperAvgParam = new SqlParameter("@oneYearLipperAvg", SqlDbType.Decimal);
                    oneYearLipperAvgParam.Value = fund.OneYrLipperAvg;
                    command.Parameters.Add(oneYearLipperAvgParam);
                    SqlParameter tenYearMarketReturnParam = new SqlParameter("@tenYearMarketReturn", SqlDbType.Decimal);
                    tenYearMarketReturnParam.Value = fund.MarketReturn10Year;
                    command.Parameters.Add(tenYearMarketReturnParam);
                    SqlParameter fiveYearMarketReturnParam = new SqlParameter("@fiveYearMarketReturn", SqlDbType.Decimal);
                    fiveYearMarketReturnParam.Value = fund.MarketReturn5Year;
                    command.Parameters.Add(fiveYearMarketReturnParam);
                    SqlParameter oneYearMarketReturnParam = new SqlParameter("@oneYearMarketReturn", SqlDbType.Decimal);
                    oneYearMarketReturnParam.Value = fund.MarketReturn1Year;
                    command.Parameters.Add(oneYearMarketReturnParam);
                    SqlParameter ytdMarketReturnParam = new SqlParameter("@ytdMarketReturn", SqlDbType.Decimal);
                    ytdMarketReturnParam.Value = fund.MarketReturnYTD;
                    command.Parameters.Add(ytdMarketReturnParam);
                    SqlParameter tenYearMarketReturnRankParam = new SqlParameter("@tenYearMarketReturnRank", SqlDbType.Int);
                    tenYearMarketReturnRankParam.Value = fund.MarketReturnRank10Year;
                    command.Parameters.Add(tenYearMarketReturnRankParam);
                    SqlParameter fiveYearMarketReturnRankParam = new SqlParameter("@fiveYearMarketReturnRank", SqlDbType.Int);
                    fiveYearMarketReturnRankParam.Value = fund.MarketReturnRank5Year;
                    command.Parameters.Add(fiveYearMarketReturnRankParam);
                    SqlParameter oneYearMarketReturnRankParam = new SqlParameter("@oneYearMarketReturnRank", SqlDbType.Int);
                    oneYearMarketReturnRankParam.Value = fund.MarketReturnRank1Year;
                    command.Parameters.Add(oneYearMarketReturnRankParam);
                    SqlParameter ytdMarketReturnRankParam = new SqlParameter("@ytdMarketReturnRank", SqlDbType.Int);
                    ytdMarketReturnRankParam.Value = fund.MarketReturnRankYTD;
                    command.Parameters.Add(ytdMarketReturnRankParam);
                    SqlParameter tenYearNAVReturnParam = new SqlParameter("@tenYearNAVReturn", SqlDbType.Decimal);
                    tenYearNAVReturnParam.Value = fund.NavReturn10Year;
                    command.Parameters.Add(tenYearNAVReturnParam);
                    SqlParameter fiveYearNAVReturnParam = new SqlParameter("@fiveYearNAVReturn", SqlDbType.Decimal);
                    fiveYearNAVReturnParam.Value = fund.NavReturn5Year;
                    command.Parameters.Add(fiveYearNAVReturnParam);
                    SqlParameter oneYearNAVReturnParam = new SqlParameter("@oneYearNAVReturn", SqlDbType.Decimal);
                    oneYearNAVReturnParam.Value = fund.NavReturn1Year;
                    command.Parameters.Add(oneYearNAVReturnParam);
                    SqlParameter ytdNAVReturnParam = new SqlParameter("@ytdNAVReturn", SqlDbType.Decimal);
                    ytdNAVReturnParam.Value = fund.NavReturnYTD;
                    command.Parameters.Add(ytdNAVReturnParam);
                    SqlParameter tenYearNAVReturnRankParam = new SqlParameter("@tenYearNAVReturnRank", SqlDbType.Int);
                    tenYearNAVReturnRankParam.Value = fund.NavReturnRank10Year;
                    command.Parameters.Add(tenYearNAVReturnRankParam);
                    SqlParameter fiveYearNAVReturnRankParam = new SqlParameter("@fiveYearNAVReturnRank", SqlDbType.Int);
                    fiveYearNAVReturnRankParam.Value = fund.NavReturnRank5Year;
                    command.Parameters.Add(fiveYearNAVReturnRankParam);
                    SqlParameter oneYearNAVReturnRankParam = new SqlParameter("@oneYearNAVReturnRank", SqlDbType.Int);
                    oneYearNAVReturnRankParam.Value = fund.NavReturnRank1Year;
                    command.Parameters.Add(oneYearNAVReturnRankParam);
                    SqlParameter ytdNAVReturnRankParam = new SqlParameter("@ytdNAVReturnRank", SqlDbType.Int);
                    ytdNAVReturnRankParam.Value = fund.NavReturnRankYTD;
                    command.Parameters.Add(ytdNAVReturnRankParam);
                    SqlParameter tenYearPremiumDiscountAvgParam = new SqlParameter("@tenYearPremiumDiscountAvg", SqlDbType.Decimal);
                    tenYearPremiumDiscountAvgParam.Value = fund.PremiumDiscount10YearAvg;
                    command.Parameters.Add(tenYearPremiumDiscountAvgParam);
                    SqlParameter fiveYearPremiumDiscountAvgParam = new SqlParameter("@fiveYearPremiumDiscountAvg", SqlDbType.Decimal);
                    fiveYearPremiumDiscountAvgParam.Value = fund.PremiumDiscount5YearAvg;
                    command.Parameters.Add(fiveYearPremiumDiscountAvgParam);
                    SqlParameter ytdPremiumDiscountAvgParam = new SqlParameter("@ytdPremiumDiscountAvg", SqlDbType.Decimal);
                    ytdPremiumDiscountAvgParam.Value = fund.PremiumDiscountYTDAvg;
                    command.Parameters.Add(ytdPremiumDiscountAvgParam);
                    SqlParameter navParam = new SqlParameter("@nav", SqlDbType.Decimal);
                    navParam.Value = fund.Nav;
                    command.Parameters.Add(navParam);
                    SqlParameter marketPriceParam = new SqlParameter("@marketPrice", SqlDbType.Decimal);
                    marketPriceParam.Value = fund.MarketPrice;
                    command.Parameters.Add(marketPriceParam);
                    SqlParameter netChangeParam = new SqlParameter("@netChange", SqlDbType.Decimal);
                    netChangeParam.Value = fund.NetChange;
                    command.Parameters.Add(netChangeParam);
                    SqlParameter marketChangeParam = new SqlParameter("@marketChange", SqlDbType.Decimal);
                    marketChangeParam.Value = fund.MarketChange;
                    command.Parameters.Add(marketChangeParam);
                    SqlParameter oneYearNavReturnActParam = new SqlParameter("@oneYearNavReturnAct", SqlDbType.Decimal);
                    oneYearNavReturnActParam.Value = fund.OneYearNavReturn;
                    command.Parameters.Add(oneYearNavReturnActParam);
                    SqlParameter twelveMonthYieldDateParam = new SqlParameter("@twelveMonthYieldDate", SqlDbType.DateTime);
                    if (!string.IsNullOrEmpty(fund.TwelveMoYieldAsOf) && (fund.TwelveMoYieldAsOf != "--"))
                    {
                        twelveMonthYieldDateParam.Value = System.Convert.ToDateTime(fund.TwelveMoYieldAsOf);
                    }
                    else
                    {
                        twelveMonthYieldDateParam.Value = SqlDateTime.MinValue;
                    }
                    command.Parameters.Add(twelveMonthYieldDateParam);
                    SqlParameter definedIncomeOnlyYieldParam = new SqlParameter("@definedIncomeOnlyYield", SqlDbType.Decimal);
                    definedIncomeOnlyYieldParam.Value = fund.DefinedIncomeOnlyYield;
                    command.Parameters.Add(definedIncomeOnlyYieldParam);
                    SqlParameter distributionYieldParam = new SqlParameter("@distributionYield", SqlDbType.Decimal);
                    distributionYieldParam.Value = fund.DistributionYield;
                    command.Parameters.Add(distributionYieldParam);
                    SqlParameter mostRecentIncomeDividendParam = new SqlParameter("@mostRecentIncomeDividend", SqlDbType.Decimal);
                    mostRecentIncomeDividendParam.Value = fund.MostRecentIncomeDividend;
                    command.Parameters.Add(mostRecentIncomeDividendParam);
                    SqlParameter mostRecentIncomeDividendDateParam = new SqlParameter("@mostRecentIncomeDividendDate", SqlDbType.DateTime);
                    if (!string.IsNullOrEmpty(fund.MostRecentIncomeDividendDate) && (fund.MostRecentIncomeDividendDate != "--"))
                    {
                        mostRecentIncomeDividendDateParam.Value = System.Convert.ToDateTime(fund.MostRecentIncomeDividendDate);
                    }
                    else
                    {
                        mostRecentIncomeDividendDateParam.Value = SqlDateTime.MinValue;
                    }
                    command.Parameters.Add(mostRecentIncomeDividendDateParam);
                    SqlParameter mostRecentCapGainDividendParam = new SqlParameter("@mostRecentCapGainDividend", SqlDbType.Decimal);
                    mostRecentCapGainDividendParam.Value = fund.MostRecentCapGainDividend;
                    mostRecentCapGainDividendParam.Value = fund.MostRecentCapGainDividend;
                    command.Parameters.Add(mostRecentCapGainDividendParam);
                    SqlParameter mostRecentCapGainDividendDateParam = new SqlParameter("@mostRecentCapGainDividendDate", SqlDbType.DateTime);
                    if (!string.IsNullOrEmpty(fund.MostRecentCapGainDividendDate) && (fund.MostRecentCapGainDividendDate != "--"))
                    {
                        mostRecentCapGainDividendDateParam.Value = System.Convert.ToDateTime(fund.MostRecentCapGainDividendDate);
                    }
                    else
                    {
                        mostRecentCapGainDividendDateParam.Value = SqlDateTime.MinValue;
                    }
                    command.Parameters.Add(mostRecentCapGainDividendDateParam);
                    SqlParameter monthlyYtdDividendsParam = new SqlParameter("@monthlyYTDDiviends", SqlDbType.Decimal);
                    monthlyYtdDividendsParam.Value = fund.MonthlyYTDDivedends;
                    command.Parameters.Add(monthlyYtdDividendsParam);
                    SqlParameter ytdCapGainsParam = new SqlParameter("@ytdCapGains", SqlDbType.Decimal);
                    ytdCapGainsParam.Value = fund.YTDCapGains;
                    command.Parameters.Add(ytdCapGainsParam);
                    SqlParameter premiumDiscountParam = new SqlParameter("@premiumDiscount", SqlDbType.Decimal);
                    premiumDiscountParam.Value = fund.PremiumDiscount;
                    command.Parameters.Add(premiumDiscountParam);
                    connection.Open();
                    rowcount += command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            // Insert FundAssets record
            Guid fund_asset_id = Guid.NewGuid();
            string fundAssetId = fund_asset_id.ToString().Replace("{", string.Empty).Replace("}", string.Empty).Replace("-", string.Empty);

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.NCrawlerConn))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO FundAssets (id, fund_id, [Top Funds Date], [Top Funds], [Total Net Assets Date], [Total Net Assets], [Quality Date], [Quality], [Crawl Date]) ";
                command.CommandText += "VALUES(@id,@fundId,@topFundsDate,@topFunds,@totalNetAssetsDate,@totalNetAssets,@qualityDate,@quality,@crawlDate)";
                try
                {
                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.VarChar);
                    idParam.Value = fundAssetId;
                    command.Parameters.Add(idParam);
                    SqlParameter fundIdParam = new SqlParameter("@fundId", SqlDbType.VarChar);
                    fundIdParam.Value = fundId;
                    command.Parameters.Add(fundIdParam);
                    SqlParameter topFundsParam = new SqlParameter("@topFunds", SqlDbType.VarChar);
                    if (!string.IsNullOrEmpty(fund.TopAssets))
                    {
                        topFundsParam.Value = fund.TopAssets;
                    }
                    else
                    {
                        topFundsParam.Value = "--";
                    }
                    command.Parameters.Add(topFundsParam);
                    SqlParameter topFundsDateParam = new SqlParameter("@topFundsDate", SqlDbType.DateTime);
                    if (!string.IsNullOrEmpty(fund.TopAssetsDate) && (fund.TopAssetsDate != "--"))
                    {
                        topFundsDateParam.Value = System.Convert.ToDateTime(fund.TopAssetsDate);
                    }
                    else
                    {
                        topFundsDateParam.Value = SqlDateTime.MinValue;
                    }
                    command.Parameters.Add(topFundsDateParam);

                    SqlParameter totalNetAssetsParam = new SqlParameter("@totalNetAssets", SqlDbType.VarChar);
                    if (!string.IsNullOrEmpty(fund.NetAssets))
                    {
                        totalNetAssetsParam.Value = fund.NetAssets;
                    }
                    else
                    {
                        totalNetAssetsParam.Value = "--";
                    }
                    command.Parameters.Add(totalNetAssetsParam);
                    SqlParameter totalNetAssetsDateParam = new SqlParameter("@totalNetAssetsDate", SqlDbType.DateTime);
                    if (!string.IsNullOrEmpty(fund.NetAssetsDate) && (fund.NetAssetsDate != "--"))
                    {
                        totalNetAssetsDateParam.Value = System.Convert.ToDateTime(fund.NetAssetsDate);
                    }
                    else
                    {
                        totalNetAssetsDateParam.Value = SqlDateTime.MinValue;
                    }
                    command.Parameters.Add(totalNetAssetsDateParam);

                    SqlParameter qualityParam = new SqlParameter("@quality", SqlDbType.VarChar);
                    if (!string.IsNullOrEmpty(fund.Quality))
                    {
                        if (fund.Quality.Length >= 512)
                            qualityParam.Value = fund.Quality.Substring(0, 256);
                        else
                            qualityParam.Value = fund.Quality;
                    }
                    else
                    {
                        qualityParam.Value = "--";
                    }
                    command.Parameters.Add(qualityParam);
                    SqlParameter qualityDateParam = new SqlParameter("@qualityDate", SqlDbType.DateTime);
                    if (!string.IsNullOrEmpty(fund.QualityDate) && (fund.QualityDate != "--"))
                    {
                        qualityDateParam.Value = System.Convert.ToDateTime(fund.QualityDate);
                    }
                    else
                    {
                        qualityDateParam.Value = SqlDateTime.MinValue;
                    }
                    command.Parameters.Add(qualityDateParam);
                    SqlParameter crawlDateParam = new SqlParameter("@crawlDate", SqlDbType.DateTime);
                    crawlDateParam.Value = crawlDate;
                    command.Parameters.Add(crawlDateParam);
                    connection.Open();
                    rowcount += command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine(fund.TopAssets);
                    if (!string.IsNullOrEmpty(fund.Quality))
                        Console.WriteLine(fund.Quality.Substring(0, 256));
                }
            }

            return true;
        }

    }
}
