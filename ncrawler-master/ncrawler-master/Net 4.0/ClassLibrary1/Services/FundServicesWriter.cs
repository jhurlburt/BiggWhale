using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;


using NCrawler.FundServices.Extensions;
using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler;
using NCrawler.Services;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Linq;
using log4net;

namespace NCrawler.FundServices
{
    internal static class MissingDllHack
    {
        // Must reference a type in EntityFramework.SqlServer.dll so that this dll will be
        // included in the output folder of referencing projects without requiring a direct 
        // dependency on Entity Framework. See http://stackoverflow.com/a/22315164/1141360.
        private static SqlProviderServices instance = SqlProviderServices.SingletonInstance;
    }

    public class FundServicesWriter : IPipelineStep
    {
        private static readonly log4net.ILog log = LogManager.GetLogger(typeof(FundServicesWriter));

        //private string dataFolder = @"C:\crawler\daily\";
        public void Process(Crawler crawler, PropertyBag propertyBag)
        {
            //using (var context = new CEF())
            //{
            //    context.CrawlQueues.Add(new CrawlQueue());
            //    context.SaveChanges();
            //}


            CultureInfo contentCulture = (CultureInfo)propertyBag["LanguageCulture"].Value;
            string cultureDisplayValue = "N/A";
            if (!contentCulture.IsNull())
            {
                cultureDisplayValue = contentCulture.DisplayName;
            }

            lock (this)
            {
                Console.Out.WriteLine(ConsoleColor.Gray, "Url: {0}", propertyBag.Step.Uri);
                Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tContent type: {0}", propertyBag.ContentType);
                Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tContent length: {0}", propertyBag.Text.IsNull() ? 0 : propertyBag.Text.Length);
                Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tDepth: {0}", propertyBag.Step.Depth);
                Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tCulture: {0}", cultureDisplayValue);
                Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
                Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tThread Count: {0}", crawler.ThreadsInUse);

                //string outputFolder = dataFolder + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                //try
                //{
                //    Directory.CreateDirectory(outputFolder);
                //}
                //catch (Exception ex)
                //{
                //    Console.Out.WriteLine(ex.ToString());
                //}

                try
                {
                    #region FundDetail
                    if (propertyBag.Step.Uri.ToString().Contains("FundDetail") && propertyBag.Text != null)
                    {
                        log.Debug("create fund object");
                        FundDetailObject obj = createFundObject(propertyBag);

                        if (!obj.Ticker.Contains("MoYield"))
                        {
                            log.Debug("create new CEF db");
                            using (CEF db = new CEF())
                            {
                                //Task<Fund> task = db.Funds.SqlQuery("SELECT [id]"+
                                //    ",[Name]" +
                                //    ",[Fund Type]" +
                                //    ",[Ticker Symbol]" +
                                //    ",[Asset Class]" +
                                //    ",[Inception Date]" +
                                //    ",[Advisor]" +
                                //    ",[Manager And Tenure]" +
                                //    ",[Phone]" +
                                //    ",[Website]" +
                                //    ",[Total Net Assets]" +
                                //    ",[Total Net Assets Date]" +
                                //    ",[Percent Leveraged Assets]" +
                                //    ",[Percent Leveraged Assets Date]" +
                                //    ",[Portfolio Turnover]" +
                                //    ",[Management Fees]" +
                                //    ",[Expense Ratio]" +
                                //    ",[Alternative Minimum Tax]" +
                                //    ",[Fund Objective]" +
                                //    ",[Yield]" +
                                //    "FROM [dbo].[Funds]" +
                                //    "WHERE [Name] = @p0", obj.Name).FirstAsync();
                                //task.Wait();

                                Fund fund = db.Funds.Find(obj.Name);
                                if (fund == null)
                                {
                                    log.Debug("create fund");
                                    fund = db.Funds.Create();
                                    fund.id = Guid.NewGuid().ToString().Replace("{", string.Empty).Replace("}", string.Empty).Replace("-", string.Empty);
                                    fund.Created_By = Environment.UserName;
                                    fund.Create_Date = obj.CrawlDate;

                                    log.Debug("transform fund");
                                    transformFund(fund, obj);
                                    //db.Funds.Attach(fund);
                                    db.Funds.Add(fund);
                                }
                                else
                                {
                                    log.Info("found fund: " + fund.id);
                                    log.Debug("transform fund");
                                    transformFund(fund, obj);
                                }
                                log.Debug("begin transform fund detail");

                                FundDetail fundDetail = db.FundDetails.
                                    Where(b => b.fund_id == fund.id && b.Crawl_Date == DateTime.Today).
                                    FirstOrDefault();                                    

                                if (fundDetail == null)
                                {
                                    log.Debug("create fund detail");
                                    fundDetail = db.FundDetails.Create();
                                    fundDetail.id = Guid.NewGuid().ToString().Replace("{", string.Empty).Replace("}", string.Empty).Replace("-", string.Empty);
                                    fundDetail.fund_id = fund.id;
                                    fundDetail.Created_By = Environment.UserName;
                                    fundDetail.Create_Date = obj.CrawlDate;

                                    log.Debug("transform fund detail");
                                    transformFundDetail(fundDetail, obj);

                                    log.Debug("add fund detail");
                                    db.FundDetails.Add(fundDetail);
                                } else
                                {
                                    log.Info("found fund detail: " + fundDetail.id);
                                    log.Debug("transform fund detail");
                                    transformFundDetail(fundDetail, obj);
                                }

                                FundAsset fundAsset = db.FundAssets.
                                    Where(a => a.fund_id == fund.id && a.Crawl_Date == DateTime.Today).
                                    FirstOrDefault();

                                if (fundAsset == null)
                                {
                                    log.Debug("create fund asset");
                                    fundAsset = db.FundAssets.Create();
                                    fundAsset.id = Guid.NewGuid().ToString().Replace("{", string.Empty).Replace("}", string.Empty).Replace("-", string.Empty);
                                    fundAsset.fund_id = fund.id;

                                    log.Debug("transform fund detail");
                                    transformFundAsset(fundAsset, obj);

                                    log.Debug("add fund asset");
                                    db.FundAssets.Add(fundAsset);
                                } else
                                {
                                    log.Info("found fund asset: " + fundAsset.id);
                                    log.Debug("transform fund asset");
                                    transformFundAsset(fundAsset, obj);
                                }

                                log.Debug("db save changes");
                                int result = db.SaveChanges();

                                Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tRecords Saved: {0}", result);
                            }
                        }

                    }
                    #endregion

                }
                catch(DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);
                    Console.Out.WriteLine(fullErrorMessage);
                    Console.Out.WriteLine(ex.StackTrace.ToString());
                    log.Error(errorMessages);
                    log.Fatal(ex.Message, ex);
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine(ex.Message.ToString());
                    Console.Out.WriteLine(ex.StackTrace.ToString());
                    log.Fatal(ex.Message, ex);
                }
                //#region AssetClass
                //if (propertyBag.Step.Uri.ToString().Contains("AssetClass"))
                //{
                //    writeAssetClass(propertyBag);
                //}
                //#endregion

                //#region Classifications
                //if (propertyBag.Step.Uri.ToString().Contains("Classifications"))
                //{
                //    writeClassifications(propertyBag);
                //}
                //#endregion
            }
        }

        //private void writeClassifications(PropertyBag propertyBag)
        //{
        //    // write the asset class content to a file
        //    try
        //    {
        //        //string fileName = dataFolder + DateTime.Now.ToString("yyyy-MM-dd") + "\\Classifications.txt";
        //        //using (StreamWriter writetext = new StreamWriter(fileName))
        //        //{
        //        //    writetext.WriteLine(propertyBag.Text.Replace(Environment.NewLine, string.Empty));
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Out.WriteLine(ex.ToString());
        //    }
        //}

        //private void writeAssetClass(PropertyBag propertyBag)
        //{
        //    // write the asset class content to a file
        //    try
        //    {
        //        //string fileName = outputFolder + "AssetClass_" + propertyBag.Step.Uri.ToString().Substring(propertyBag.Step.Uri.ToString().IndexOf("AssetClass=") + 11) + ".txt";
        //        //using (StreamWriter writetext = new StreamWriter(fileName))
        //        //{
        //        //    writetext.WriteLine(propertyBag.Text.Replace(Environment.NewLine, string.Empty));
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Out.WriteLine(ex.ToString());
        //    }
        //}

        private FundDetailObject createFundObject(PropertyBag propertyBag)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[\s]{2,}", options);
            propertyBag.Text = regex.Replace(propertyBag.Text, "  ");
            string cDate = DateTime.Now.ToString("yyyy-MM-dd");
            //string strFilePath = Properties.Settings.Default.DataFolder + cDate;

            //List<FundDetailObject> fundList = new List<FundDetailObject>();
            string[] m;
            int idx1 = 0;
            int idx2 = 0;
            int iVal = 0;
            decimal mVal = 0.0M;
            bool isDecimal = false;
            bool isInt = false;
            //parse line by line into instance of employee class
            //Fund fund = new FundServices.Fund();
            //FundDetail fundDetail = new FundDetail();
            //FundAsset fundAsset = new FundAsset();
            FundDetailObject fundObj = new FundDetailObject();

            try
            {
                fundObj.CEF_id = int.Parse(propertyBag.OriginalUrl.Replace("/FundSelector/FundDetail.fs?ID=", ""));
            } catch
            {
                fundObj.CEF_id = 0;
            }           
            //string detailUrl = "http://www.cefa.com/FundSelector/FundDetail.fs?ID=" + Path.GetFileName(f).Replace("FundDetail_", string.Empty).Replace(".txt", string.Empty);
            //fund.Url = detailUrl;

            DateTime cDateTime = DateTime.Parse(cDate);
            fundObj.CrawlDate = cDateTime;

            if (propertyBag.Text.Contains("Initilizing list..."))
            {
                idx1 = propertyBag.Text.IndexOf("Initilizing list...") + 20;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("as of") - 1;
                string fundName = string.Empty;
                if (idx2 == -2)
                {
                    idx2 = propertyBag.Text.Length - 1 - idx1 - idx2 - 2;
                    fundName = propertyBag.Text.Substring(idx1, idx2).Trim();
                    fundObj.Name = fundName;
                    //continue;
                }
                else
                {
                    fundName = propertyBag.Text.Substring(idx1, idx2).Trim();

                }
                if (fundName.Length > 100)
                    fundName = fundName.Substring(0, 100);
                if (fundName.Length >= 20)
                {
                    if (fundName.Substring(0, 20) == "Performance Criteria")
                        fundName = "Fund ID " + propertyBag.Step.Uri.ToString().Substring(propertyBag.Step.Uri.ToString().IndexOf("ID=") + 3);
                }
                fundObj.Name = fundName;
            }
            idx1 = idx1 + idx2 + 6;
            string asOfDate = string.Empty;
            int pad = 13;
            if (propertyBag.Text.Substring(idx1, 4).Trim() == "--")
            {
                asOfDate = propertyBag.Text.Substring(idx1, 4).Trim();
                pad = 5;
            }
            else
            {
                asOfDate = propertyBag.Text.Substring(idx1, 12).Trim();
            }
            //fund.AsOfDate = asOfDate;
            // Ticker
            idx1 = idx1 + pad;
            idx2 = 13;
            string ticker = string.Empty;
            if (propertyBag.Text.Contains("Asset Class:") && propertyBag.Text.Contains("1 YR Lipper Avg:"))
            {
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Asset Class:");
            }
            ticker = propertyBag.Text.Substring(idx1, idx2).Trim().Replace(" ", string.Empty);
            fundObj.Ticker = ticker;
            if (propertyBag.Text.Contains("Asset Class:") && propertyBag.Text.Contains("1 YR Lipper Avg:"))
            {
                idx1 = propertyBag.Text.IndexOf("Asset Class:") + 13;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("1 YR Lipper Avg:") - 1;
                string assetClass = propertyBag.Text.Substring(idx1, idx2).Trim();
                fundObj.AssetClass = assetClass;
            }
            if (propertyBag.Text.Contains("1 YR Lipper Avg:"))
            {
                idx1 = propertyBag.Text.IndexOf("1 YR Lipper Avg:") + 18;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Growth of $10K Avg Annual Total Return %  10 YR 5 YR 1 YR YTD Market Return") - 1;
                decimal oneYearLipperAvg = 0.0M;
                string test = propertyBag.Text.Substring(idx1, idx2).Replace("%", string.Empty);
                isDecimal = Decimal.TryParse(propertyBag.Text.Substring(idx1, idx2).Replace("%", string.Empty), out oneYearLipperAvg);
                if (isDecimal)
                {
                    fundObj.OneYrLipperAvg = oneYearLipperAvg;
                }
            }
            // Avg Annual Return Percentage
            if (propertyBag.Text.Contains("Growth of $10K Avg Annual Total Return %  10 YR 5 YR 1 YR YTD Market Return") && propertyBag.Text.Contains("Lipper Pct. Rank"))
            {
                idx1 = propertyBag.Text.IndexOf("Growth of $10K Avg Annual Total Return %  10 YR 5 YR 1 YR YTD Market Return") + 76;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Lipper Pct. Rank");
                string marketReturn = propertyBag.Text.Substring(idx1, idx2).Trim();
                m = marketReturn.Split(' ');
                for (int i = 0; i < m.Length; i++)
                {
                    mVal = 0.0M;
                    isDecimal = Decimal.TryParse(m[i].Replace("%", string.Empty), out mVal);
                    if (i == 0)
                        fundObj.MarketReturn10Year = mVal;
                    else if (i == 1)
                        fundObj.MarketReturn5Year = mVal;
                    else if (i == 2)
                        fundObj.MarketReturn1Year = mVal;
                    else if (i == 3)
                        fundObj.MarketReturnYTD = mVal;
                }
            }
            // Avg Annual Return Percentage Lippert Pct Rank
            if (propertyBag.Text.Contains("Lipper Pct. Rank") && propertyBag.Text.Contains("NAV Return"))
            {
                idx1 = propertyBag.Text.IndexOf("Lipper Pct. Rank") + 17;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("NAV Return");
                string marketReturnRank = propertyBag.Text.Substring(idx1, idx2).Trim();
                m = marketReturnRank.Split(' ');
                iVal = 0;
                isInt = Int32.TryParse(m[0], out iVal);
                fundObj.MarketReturnRank10Year = iVal;
                isInt = Int32.TryParse(m[1], out iVal);
                fundObj.MarketReturnRank5Year = iVal;
                isInt = Int32.TryParse(m[2], out iVal);
                fundObj.MarketReturnRank1Year = iVal;
                isInt = Int32.TryParse(m[3], out iVal);
                fundObj.MarketReturnRankYTD = iVal;
            }
            // NAV Return Pct
            if (propertyBag.Text.Contains("NAV Return") && propertyBag.Text.Contains("Lipper Pct. Rank"))
            {
                idx1 = propertyBag.Text.IndexOf("NAV Return") + 11;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Lipper Pct. Rank") - 1;
                string navReturn = propertyBag.Text.Substring(idx1, idx2).Trim();
                m = navReturn.Split(' ');
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(m[0].Replace("%", string.Empty), out mVal);
                fundObj.NavReturn10Year = mVal;
                isDecimal = Decimal.TryParse(m[1].Replace("%", string.Empty), out mVal);
                fundObj.NavReturn5Year = mVal;
                isDecimal = Decimal.TryParse(m[2].Replace("%", string.Empty), out mVal);
                fundObj.NavReturn1Year = mVal;
                isDecimal = Decimal.TryParse(m[3].Replace("%", string.Empty), out mVal);
                fundObj.NavReturnYTD = mVal;
            }
            // NAV Lipper Pct Rank
            idx1 = idx1 + idx2 + 1;
            string test4 = propertyBag.Text.Substring(idx1);
            if (propertyBag.Text.Substring(idx1).Contains("Lipper Pct. Rank"))
            {
                pad = 1;
                idx1 = idx1 + 16;
                idx2 = 12;
                string navReturnRank = propertyBag.Text.Substring(idx1, idx2).Trim();
                m = navReturnRank.Split(' ');
                iVal = 0;
                isInt = Int32.TryParse(m[0], out iVal);
                fundObj.NavReturnRank10Year = iVal;
                isInt = Int32.TryParse(m[1], out iVal);
                fundObj.NavReturnRank5Year = iVal;
                isInt = Int32.TryParse(m[2], out iVal);
                fundObj.NavReturnRank1Year = iVal;
                isInt = Int32.TryParse(m[3], out iVal);
                fundObj.NavReturnRankYTD = iVal;
            }
            else if (propertyBag.Text.Substring(idx1, 3) == " --")
            {
                pad = 3;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("NAV");
                string navReturnRank = propertyBag.Text.Substring(idx1, idx2).Trim();
                m = navReturnRank.Split(' ');
                iVal = 0;
                isInt = Int32.TryParse(m[0], out iVal);
                fundObj.NavReturnRank10Year = iVal;
                isInt = Int32.TryParse(m[1], out iVal);
                fundObj.NavReturnRank5Year = iVal;
                isInt = Int32.TryParse(m[2], out iVal);
                fundObj.NavReturnRank1Year = iVal;
                isInt = Int32.TryParse(m[3], out iVal);
                fundObj.NavReturnRankYTD = iVal;

            }
            // NAV
            if (propertyBag.Text.Contains("NAV $"))
            {
                idx1 = propertyBag.Text.IndexOf("NAV $") + 5;
            }
            else if (propertyBag.Text.Contains("NAV --"))
            {
                idx1 = propertyBag.Text.IndexOf("NAV --") + 3;
            }
            idx2 = propertyBag.Text.Substring(idx1).IndexOf("Market Price");
            if (idx1 > 0 && idx2 > 0)
            {
                string navPrice = propertyBag.Text.Substring(idx1, idx2).Trim();
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(navPrice.Replace("$", string.Empty), out mVal);
                fundObj.Nav = mVal;
            }

            // Premium Discount History
            if (propertyBag.Text.Contains("Premium/Discount History Premium/Discount 10 YR Avg 5 YR Avg YTD Avg"))
            {
                idx1 = propertyBag.Text.IndexOf("Premium/Discount History Premium/Discount 10 YR Avg 5 YR Avg YTD Avg") + 69;
                string test3 = propertyBag.Text.Substring(idx1);

                idx2 = propertyBag.Text.Substring(idx1).IndexOf("NAV");
                string premiumDiscount = propertyBag.Text.Substring(idx1, idx2).Trim();
                m = premiumDiscount.Split(' ');
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(m[0].Replace("%", string.Empty), out mVal);
                fundObj.PremiumDiscount10YearAvg = mVal;
                isDecimal = Decimal.TryParse(m[1].Replace("%", string.Empty), out mVal);
                fundObj.PremiumDiscount5YearAvg = mVal;
                isDecimal = Decimal.TryParse(m[2].Replace("%", string.Empty), out mVal);
                fundObj.PremiumDiscountYTDAvg = mVal;
            }

            // Market Price
            if (propertyBag.Text.Contains("Market Price"))
            {
                idx1 = propertyBag.Text.IndexOf("Market Price") + 13;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Net Change");
                string marketPrice = propertyBag.Text.Substring(idx1, idx2).Trim();
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(marketPrice.Replace("$", string.Empty), out mVal);
                fundObj.MarketPrice = mVal;
            }

            // Net Change
            if (propertyBag.Text.Contains("Market Price"))
            {
                idx1 = propertyBag.Text.IndexOf("Net Change") + 11;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Market Change");
                string netChange = propertyBag.Text.Substring(idx1, idx2).Trim();
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(netChange.Replace("%", string.Empty), out mVal);
                fundObj.NetChange = mVal;
            }

            // Market Change
            if (propertyBag.Text.Contains("Market Change"))
            {
                idx1 = propertyBag.Text.IndexOf("Market Change") + 14;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Premium/Discount");
                string marketChange = propertyBag.Text.Substring(idx1, idx2).Trim();
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(marketChange.Replace("%", string.Empty), out mVal);
                fundObj.MarketChange = mVal;
            }

            // Premium Discount
            if (propertyBag.Text.Contains("Premium/Discount"))
            {
                idx1 = propertyBag.Text.LastIndexOf("Premium/Discount") + 17;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("1 YR NAV Return");
                fundObj.PremiumDiscount = 0.0M;
                if (idx1 > 0 && idx2 > 0)
                {
                    string premiumDiscount = propertyBag.Text.Substring(idx1, idx2).Trim();
                    mVal = 0.0M;
                    isDecimal = Decimal.TryParse(premiumDiscount.Replace("%", string.Empty), out mVal);
                    fundObj.PremiumDiscount = mVal;
                }
                //MessageBox.Show(this, marketChange, "Market Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // 1 YR NAV return
            if (propertyBag.Text.Contains("1 YR NAV Return"))
            {
                idx1 = propertyBag.Text.IndexOf("1 YR NAV Return") + 16;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("1 YR NAV Rank");
                string premiumDiscount2 = string.Empty;
                if (idx2 > 0)
                {
                    premiumDiscount2 = propertyBag.Text.Substring(idx1, idx2).Trim();
                }
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(premiumDiscount2.Replace("%", string.Empty), out mVal);
                fundObj.OneYearNavReturn = mVal;
            }

            // 1 YR NAV RANK
            if (propertyBag.Text.Contains("1 YR NAV Rank"))
            {
                idx1 = propertyBag.Text.IndexOf("1 YR NAV Rank") + 14;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("12-Mo Yield");
                string oneYrNavRank = propertyBag.Text.Substring(idx1, idx2).Trim();
                iVal = 0;
                isInt = Int32.TryParse(oneYrNavRank, out iVal);
                fundObj.OneYearNavRank = mVal;
            }

            // 12 MO Yield As Of
            if (propertyBag.Text.Contains("12-Mo Yield  as of"))
            {
                idx1 = propertyBag.Text.IndexOf("12-Mo Yield  as of") + 19;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Yield") - 1;
                string twelveMoYearAsOf = propertyBag.Text.Substring(idx1, idx2).Trim();
                fundObj.TwelveMoYieldAsOf = twelveMoYearAsOf;
            }

            // Defined Income Only Yield
            if (propertyBag.Text.Contains("Def Income Only Yield"))
            {
                idx1 = propertyBag.Text.IndexOf("Def Income Only Yield") + 22;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Distribution Yield  (Market)") - 1;
                string defIncomeOnlyYield = propertyBag.Text.Substring(idx1, idx2).Trim();
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(defIncomeOnlyYield.Replace("%", string.Empty), out mVal);
                fundObj.DefinedIncomeOnlyYield = mVal;
            }

            // Distribution Yield (Market)
            if (propertyBag.Text.Contains("Distribution Yield  (Market)"))
            {
                idx1 = propertyBag.Text.IndexOf("Distribution Yield  (Market)") + 29;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Most Recent Income Dividend") - 1;
                string marketDistYield = propertyBag.Text.Substring(idx1, idx2).Trim();
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(marketDistYield.Replace("%", string.Empty), out mVal);
                fundObj.DistributionYield = mVal;
            }

            // Most Recent Income Dividend
            if (propertyBag.Text.Contains("Most Recent Income Dividend"))
            {
                idx1 = propertyBag.Text.IndexOf("Most Recent Income Dividend") + 28;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Ex-Div Date") - 1;
                string mostRecentIncomeDiv = propertyBag.Text.Substring(idx1, idx2).Trim();
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(mostRecentIncomeDiv.Replace("$", string.Empty), out mVal);
                fundObj.MostRecentIncomeDividend = mVal;
            }

            // Ex-Div Date
            if (propertyBag.Text.Contains("Ex-Div Date"))
            {
                idx1 = propertyBag.Text.IndexOf("Ex-Div Date") + 12;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Most Recent Cap Gain Dividend") - 1;
                string mrIncomeDivExDivDate = propertyBag.Text.Substring(idx1, idx2).Trim();
                fundObj.MostRecentIncomeDividendDate = mrIncomeDivExDivDate;
            }

            // Most Recent Cap Gain Dividend
            if (propertyBag.Text.Contains("Most Recent Cap Gain Dividend"))
            {
                idx1 = propertyBag.Text.IndexOf("Most Recent Cap Gain Dividend") + 30;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Ex-Div Date") - 1;
                string mostRecentCapGainDiv = propertyBag.Text.Substring(idx1, idx2).Trim();
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(mostRecentCapGainDiv.Replace("$", string.Empty), out mVal);
                fundObj.MostRecentCapGainDividend = mVal;
            }

            // Ex-Div Date
            if (propertyBag.Text.Contains("Ex-Div Date"))
            {
                idx1 = propertyBag.Text.IndexOf("Most Recent Cap Gain Dividend") + 42 + idx2;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Dividend Frequency Monthly YTD Dividends") - 1;
                if (idx2 == -2)
                {
                    idx2 = propertyBag.Text.Substring(idx1).IndexOf("Dividend Frequency Daily YTD Dividends") - 1;
                }
                if (idx2 == -2)
                {
                    idx2 = propertyBag.Text.Substring(idx1).IndexOf("Dividend Frequency Quarterly YTD Dividends") - 1;
                }
                if (idx2 == -2)
                {
                    idx2 = propertyBag.Text.Substring(idx1).IndexOf("Dividend Frequency Semiannual YTD Dividends") - 1;
                }
                if (idx2 == -2)
                {
                    idx2 = propertyBag.Text.Substring(idx1).IndexOf("Dividend Frequency Annually YTD Dividends") - 1;
                }
                string mrCapGainsDivExDivDate = propertyBag.Text.Substring(idx1, idx2).Trim();
                fundObj.MostRecentCapGainDividendDate = mrCapGainsDivExDivDate;
            }

            // Monthly YTD Dividends
            if (propertyBag.Text.Contains("Dividend Frequency Monthly YTD Dividends"))
            {
                idx1 = propertyBag.Text.IndexOf("Dividend Frequency Monthly YTD Dividends") + 41;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("YTD Capital Gains") - 1;
                string dfMonthlyYTDDividends = propertyBag.Text.Substring(idx1, idx2).Trim();
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(dfMonthlyYTDDividends.Replace("$", string.Empty), out mVal);
                fundObj.MonthlyYTDDivedends = mVal;
            }

            // YTD Capital Gains
            if (propertyBag.Text.Contains("YTD Capital Gains"))
            {
                idx1 = propertyBag.Text.IndexOf("YTD Capital Gains") + 18;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Inception Date") - 1;
                string ytdCapitalGains = propertyBag.Text.Substring(idx1, idx2).Trim();
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(ytdCapitalGains.Replace("$", string.Empty), out mVal);
                fundObj.YTDCapGains = mVal;
            }

            // Inception Date
            if (propertyBag.Text.Contains("Inception Date"))
            {
                idx1 = propertyBag.Text.IndexOf("Inception Date") + 15;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Fund Advisor") - 1;
                string inceptionDate = propertyBag.Text.Substring(idx1, idx2).Trim();
                fundObj.InceptionDate = inceptionDate;
            }

            // Fund Advisor
            if (propertyBag.Text.Contains("Fund Advisor"))
            {
                idx1 = propertyBag.Text.IndexOf("Fund Advisor") + 13;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Manager & Tenure");
                string fundAdvisor = string.Empty;
                if (idx1 > 0 && idx2 > 0)
                    fundAdvisor = propertyBag.Text.Substring(idx1, idx2);
                fundObj.FundAdvisor = fundAdvisor;
            }

            // Manager & Tenure
            if (propertyBag.Text.Contains("Manager & Tenure"))
            {
                idx1 = propertyBag.Text.IndexOf("Manager & Tenure") + 17;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Phone");
                string managerAndTenure = propertyBag.Text.Substring(idx1, idx2);
                fundObj.ManagerAndTenure = managerAndTenure;
            }

            // Phone
            if (propertyBag.Text.Contains("Phone"))
            {
                idx1 = propertyBag.Text.IndexOf("Phone") + 6;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Website");
                string phone = propertyBag.Text.Substring(idx1, idx2);
                fundObj.Phone = phone;
            }

            // Website
            if (propertyBag.Text.Contains("Website"))
            {
                idx1 = propertyBag.Text.IndexOf("Website") + 8;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Total Net Assets  (mil)  as of");
                string webSite = "http://" + propertyBag.Text.Substring(idx1, idx2);
                fundObj.Website = webSite;
            }

            // Total Net Assets (mil)
            if (propertyBag.Text.Contains("Total Net Assets  (mil)  as of"))
            {
                idx1 = propertyBag.Text.IndexOf("Total Net Assets  (mil)  as of") + 32;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("% Leveraged Assets  as of") - 1;
                string tna = propertyBag.Text.Substring(idx1, idx2).Trim();
                string[] n = tna.Split(' ');
                string tnaDate = n[0];
                string tnaValue = n[1];
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(tnaValue.Replace("$", string.Empty), out mVal);
                fundObj.TotalNetAssets = mVal;
                fundObj.TotalNetAssetsDate = tnaDate;
            }

            // % Leveraged Assets
            if (propertyBag.Text.Contains("% Leveraged Assets  as of"))
            {
                idx1 = propertyBag.Text.IndexOf("% Leveraged Assets  as of") + 26;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Portfolio Turnover") - 1;
                string tla = propertyBag.Text.Substring(idx1, idx2).Trim();
                string[] o = tla.Split(' ');
                string tlaDate = o[0];
                string tlaValue = o[1];
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(tlaValue.Replace("$", string.Empty), out mVal);
                fundObj.PercentLeveragedAssets = mVal;
                fundObj.PercentLeveragedAssetsDate = tlaDate;
            }

            // Portfolio Turnover
            if (propertyBag.Text.Contains("Portfolio Turnover"))
            {
                idx1 = propertyBag.Text.IndexOf("Portfolio Turnover") + 19;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Mgmt Fees") - 1;
                string portfolioTurnover = propertyBag.Text.Substring(idx1, idx2).Trim();
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(portfolioTurnover.Replace("%", string.Empty), out mVal);
                fundObj.PortfolioTurnover = mVal;
            }

            // Mgmt Fees
            if (propertyBag.Text.Contains("Mgmt Fees"))
            {
                idx1 = propertyBag.Text.IndexOf("Mgmt Fees") + 10;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Expense Ratio") - 1;
                string mgmtFees = propertyBag.Text.Substring(idx1, idx2).Trim();
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(mgmtFees.Replace("%", string.Empty), out mVal);
                fundObj.ManagementFees = mVal;
            }

            // Expense Ratio
            if (propertyBag.Text.Contains("Expense Ratio"))
            {
                idx1 = propertyBag.Text.IndexOf("Expense Ratio") + 14;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Alternative Minimum Tax") - 1;
                if (idx2 == -2)
                {
                    idx2 = propertyBag.Text.Substring(idx1).IndexOf("Fund Objective") - 1;
                }
                if (idx1 > 0 && idx2 > 0)
                {
                    string expenseRatio = propertyBag.Text.Substring(idx1, idx2).Trim();
                    mVal = 0.0M;
                    isDecimal = Decimal.TryParse(expenseRatio.Replace("%", string.Empty), out mVal);
                    fundObj.ExpenseRatio = mVal;
                }
            }

            // AMT
            if (propertyBag.Text.Contains("Alternative Minimum Tax"))
            {
                idx1 = propertyBag.Text.IndexOf("Alternative Minimum Tax") + 24;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Fund Objective") - 1;
                string amt = propertyBag.Text.Substring(idx1, idx2).Trim();
                mVal = 0.0M;
                isDecimal = Decimal.TryParse(amt.Replace("%", string.Empty), out mVal);
                fundObj.AlternativeMinimumTax = mVal;
            }

            // Fund Objective
            if (propertyBag.Text.Contains("Fund Objective"))
            {
                string fundObjective = string.Empty;
                idx1 = propertyBag.Text.IndexOf("Fund Objective") + 15;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf("Total Net Assets by Category  (as of") - 1;
                if (idx2 < 0)
                {
                    idx2 = propertyBag.Text.Substring(idx1).IndexOf("Quality  (as of") - 1;
                }
                if (idx2 < 0)
                {
                    idx2 = propertyBag.Text.Substring(idx1).IndexOf("Top Holdings  (as of") - 1;
                }
                if (idx2 < 0)
                {
                    idx2 = propertyBag.Text.Substring(idx1).IndexOf("1 YR Lipper Average not available for this fund.") - 2;
                }
                if (idx2 < 0)
                {
                    idx2 = propertyBag.Text.Length - 1 - idx1 - idx2 - 2;
                    fundObjective = propertyBag.Text.Substring(idx1, idx2).Trim();
                    if (fundObjective.IndexOf("Resources") > 0)
                        fundObj.FundObjective = fundObjective.Substring(0, fundObjective.IndexOf("Resources"));
                    else
                        fundObj.FundObjective = fundObjective;
                }
                else
                {
                    fundObjective = propertyBag.Text.Substring(idx1, idx2).Trim();
                    if (fundObjective.IndexOf("Resources") > 0)
                        fundObj.FundObjective = fundObjective.Substring(0, fundObjective.IndexOf("Resources"));
                    else
                        fundObj.FundObjective = fundObjective;
                }
            }

            // Total Net Assets
            if (propertyBag.Text.Contains("Total Net Assets by Category  (as of"))
            {
                string netAssets = string.Empty;
                idx1 = propertyBag.Text.IndexOf("Total Net Assets by Category  (as of") + 37;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf(")");
                string netAssetsAsOf = propertyBag.Text.Substring(idx1, idx2).Trim();
                fundObj.TopAssetsDate = netAssetsAsOf;
                int idx3 = propertyBag.Text.Substring(idx1 + idx2 + 2).IndexOf("Quality  (as of") - 1;
                if (idx3 < 0)
                {
                    idx3 = propertyBag.Text.Length - 1 - idx1 - idx2 - 2;
                    netAssets = propertyBag.Text.Substring(idx1 + idx2 + 2, idx3).Trim();
                    if (netAssets.IndexOf("Resources") > 0)
                        netAssets.Substring(0, netAssets.IndexOf("Resources"));
                    else
                        fundObj.TopAssets = netAssets;
                }
                netAssets = propertyBag.Text.Substring(idx1 + idx2 + 2, idx3).Trim();
                if (netAssets.IndexOf("Resources") > 0)
                    netAssets.Substring(0, netAssets.IndexOf("Resources"));
                else
                    fundObj.TopAssets = netAssets;
            }

            // Top Holdings
            if (propertyBag.Text.Contains("Top Holdings  (as of"))
            {
                idx1 = propertyBag.Text.IndexOf("Top Holdings  (as of") + 21;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf(")");
                string netAssetsAsOf = propertyBag.Text.Substring(idx1, idx2).Trim();
                string netAssets = string.Empty;
                fundObj.TopAssetsDate = netAssetsAsOf;
                int idx3 = propertyBag.Text.Substring(idx1 + idx2 + 2).IndexOf("Top Sectors  (as of") - 1;
                if (idx3 < 0)
                {
                    idx3 = propertyBag.Text.Length - 1 - idx1 - idx2 - 2;
                    netAssets = propertyBag.Text.Substring(idx1 + idx2 + 2, idx3).Trim();
                    fundObj.TopAssets = netAssets;
                }
                else
                {
                    netAssets = propertyBag.Text.Substring(idx1 + idx2 + 2, idx3).Trim();
                }
                if (netAssets.IndexOf("Resources") > 0)
                    netAssets.Substring(0, netAssets.IndexOf("Resources"));
                else
                    fundObj.TopAssets = netAssets;
            }

            // Quality
            if (propertyBag.Text.Contains("Quality  (as of"))
            {
                idx1 = propertyBag.Text.IndexOf("Quality  (as of") + 16;
                idx2 = propertyBag.Text.Substring(idx1).IndexOf(")");
                string qualityAsOf = propertyBag.Text.Substring(idx1, idx2).Trim();
                fundObj.QualityDate = qualityAsOf;
                string quality = propertyBag.Text.Substring(idx1 + idx2 + 2).Trim().Replace(@"\r\n", string.Empty);
                fundObj.Quality = quality;
            }
            return fundObj;
        }

        private void transformFundAsset(FundAsset asset, FundDetailObject fundObj)
        {
            //FundAsset asset = new FundAsset();
            asset.Crawl_Date = fundObj.CrawlDate;
            asset.Quality = fundObj.Quality;
            if (!string.IsNullOrEmpty(fundObj.QualityDate) && (fundObj.QualityDate != "--"))
            {
                asset.Quality_Date = System.Convert.ToDateTime(fundObj.QualityDate);
            }
            else
            {
                asset.Quality_Date = DateTime.MinValue;
            }
            if (!string.IsNullOrEmpty(fundObj.TopAssets))
            {
                asset.Top_Funds = fundObj.TopAssets;
            }
            else
            {
                asset.Top_Funds = "--";
            }
            if (!string.IsNullOrEmpty(fundObj.TopAssetsDate) && (fundObj.TopAssetsDate != "--"))
            {
                asset.Top_Funds_Date = System.Convert.ToDateTime(fundObj.TopAssetsDate);
            }
            else
            {
                asset.Top_Funds_Date = DateTime.MinValue;
            }

            if (!string.IsNullOrEmpty(fundObj.NetAssets))
            {
                asset.Total_Net_Assets = fundObj.NetAssets;
            }
            else
            {
                asset.Total_Net_Assets = "--";
            }
            if (!string.IsNullOrEmpty(fundObj.NetAssetsDate) && (fundObj.NetAssetsDate != "--"))
            {
                asset.Total_Net_Assets_Date = System.Convert.ToDateTime(fundObj.NetAssetsDate);
            }
            else
            {
                asset.Total_Net_Assets_Date = DateTime.MinValue;
            }
            //return asset;
        }

        private void transformFundDetail(FundDetail detail, FundDetailObject fundObj)
        {
            //FundDetail detail = new FundDetail();
            detail.Crawl_Date = fundObj.CrawlDate;
            //detail.Created_By = Environment.UserName;
            //detail.Create_Date = fundObj.CrawlDate;
            detail.DefinedIncomeOnlyYield = fundObj.DefinedIncomeOnlyYield;
            detail.DistributionYield = fundObj.DistributionYield;
            detail.FiveYearMarketReturn = fundObj.MarketReturn5Year;
            detail.FiveYearMarketReturnRank = fundObj.MarketReturnRank5Year;
            detail.FiveYearNAVReturn = fundObj.NavReturn5Year;
            detail.FiveYearNAVReturnRank = fundObj.NavReturnRank5Year;
            detail.FiveYearPremiumDiscountAvg = fundObj.PremiumDiscount5YearAvg;
            detail.MarketChange = fundObj.MarketChange;
            detail.MarketPrice = fundObj.MarketPrice;
            detail.MonthlyYTDDividends = fundObj.MonthlyYTDDivedends;
            if (!string.IsNullOrEmpty(fundObj.MostRecentCapGainDividendDate) && (fundObj.MostRecentCapGainDividendDate != "--"))
            {
                detail.MostRecentCapGainDividendDate = System.Convert.ToDateTime(fundObj.MostRecentCapGainDividendDate);
            }
            else
            {
                detail.MostRecentCapGainDividendDate = DateTime.MinValue;
            }
            detail.MostRecentCapGainDiviednd = fundObj.MostRecentCapGainDividend;
            detail.MostRecentIncimeDividend = fundObj.MostRecentIncomeDividend;
            if (!string.IsNullOrEmpty(fundObj.MostRecentIncomeDividendDate) && (fundObj.MostRecentIncomeDividendDate != "--"))
            {
                detail.MostRecentIncomeDividendDate = System.Convert.ToDateTime(fundObj.MostRecentIncomeDividendDate);
            }
            else
            {
                detail.MostRecentIncomeDividendDate = DateTime.MinValue;
            }
            detail.NAV = fundObj.Nav;
            detail.NetChange = fundObj.NetChange;
            detail.OneYearLipperAvg = fundObj.OneYrLipperAvg;
            detail.OneYearMarketReturn = fundObj.MarketReturn1Year;
            detail.OneYearMarketReturnRank = fundObj.MarketReturnRank1Year;
            detail.OneYearNAVReturn = fundObj.NavReturn1Year;
            detail.OneYearNAVReturnRank = fundObj.NavReturnRank1Year;
            detail.PremiumDiscount = fundObj.PremiumDiscount;
            detail.TenYearMarketReturn = fundObj.MarketReturn10Year;
            detail.TenYearMarketReturnRank = fundObj.MarketReturnRank10Year;
            detail.TenYearNAVReturn = fundObj.NavReturn10Year;
            detail.TenYearNAVReturnRank = fundObj.NavReturnRank10Year;
            detail.TenYearPremiumDiscountAvg = fundObj.PremiumDiscount10YearAvg;
            if (!string.IsNullOrEmpty(fundObj.TwelveMoYieldAsOf) && (fundObj.TwelveMoYieldAsOf != "--"))
            {
                detail.TwelveMonthYieldDate = System.Convert.ToDateTime(fundObj.TwelveMoYieldAsOf);
            }
            else
            {
                detail.TwelveMonthYieldDate = DateTime.MinValue;
            }
            detail.YTDCapGains = fundObj.YTDCapGains;
            detail.YTDMarketReturn = fundObj.MarketReturnYTD;
            detail.YTDMarketReturnRank = fundObj.MarketReturnRankYTD;
            detail.YTDNAVReturn = fundObj.NavReturnYTD;
            detail.YTDNAVReturnRank = fundObj.NavReturnRankYTD;
            detail.YTDPremiumDiscountAvg = fundObj.PremiumDiscountYTDAvg;

            //return detail;
        }

        private void transformFund(Fund fund, FundDetailObject fundObj)
        {
            //fundList.Add(fund);
            // Insert into sql server
            //bool dbStatus = FundDAO.SaveToDatabase();
            //Fund fund = new Fund();
            fund.Modified_By = Environment.UserName;
            fund.Modify_Date = fundObj.CrawlDate;
            fund.CEF_id = fundObj.CEF_id;
            fund.Advisor = fundObj.FundAdvisor;
            fund.Alternative_Minimum_Tax = fundObj.AlternativeMinimumTax;
            fund.Asset_Class = fundObj.AssetClass;
            fund.Expense_Ratio = fundObj.ExpenseRatio;
            fund.Fund_Objective = fundObj.FundObjective;
            fund.Fund_Type = "Closed End Bond Fund";
            if (!string.IsNullOrEmpty(fundObj.InceptionDate) && (fundObj.InceptionDate != "--"))
            {
                fund.Inception_Date = System.Convert.ToDateTime(fundObj.InceptionDate);
            }
            else
            {
                fund.Inception_Date = DateTime.MinValue;
            }

            fund.Management_Fees = fundObj.ManagementFees;
            fund.Manager_And_Tenure = fundObj.ManagerAndTenure;
            fund.Name = fundObj.Name;
            fund.Percent_Leveraged_Assets = fundObj.PercentLeveragedAssets;
            if (!string.IsNullOrEmpty(fundObj.PercentLeveragedAssetsDate) && (fundObj.PercentLeveragedAssetsDate != "--"))
            {
                fund.Percent_Leveraged_Assets_Date = System.Convert.ToDateTime(fundObj.PercentLeveragedAssetsDate);
            }
            else
            {
                fund.Percent_Leveraged_Assets_Date = DateTime.MinValue;
            }
            fund.Phone = fundObj.Phone;
            fund.Portfolio_Turnover = fundObj.PortfolioTurnover;
            fund.Ticker_Symbol = fundObj.Ticker;
            fund.Total_Net_Assets = fundObj.TotalNetAssets;
            if (!string.IsNullOrEmpty(fundObj.TotalNetAssetsDate) && (fundObj.TotalNetAssetsDate != "--"))
            {
                fund.Total_Net_Assets_Date = System.Convert.ToDateTime(fundObj.TotalNetAssetsDate);
            }
            else
            {
                fund.Total_Net_Assets_Date = DateTime.MinValue;
            }
            fund.Website = fundObj.Website;
            fund.Yield = fundObj.Yield;

            //return fund;
        }
    }
}
