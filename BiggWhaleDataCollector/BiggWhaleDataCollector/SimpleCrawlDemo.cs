// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="SimpleCrawlDemo.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the SimpleCrawlDemo type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;

using NCrawler.Demo.Extensions;
using NCrawler.Extensions;
using NCrawler.HtmlProcessor;
using NCrawler.Interfaces;
using NCrawler.LanguageDetection.Google;
using NCrawler;
using NCrawler.Services;
using BiggWhaleDataCollector.Properties;
using BiggWhaleDataCollector;

namespace BiggWhaleDataCollector.NCrawler.Demo
{
    public class SimpleCrawlDemo
    {
        #region Class Methods

        public static void Run()
        {
            NCrawlerModule.Setup();
            Console.Out.WriteLine("http://www.cefa.com/FundSelector/");

            // Setup crawler to crawl http://www.cefa.com/FundSelector/
            // with 1 thread adhering to robot rules, and maximum depth
            // of 2 with 4 pipeline steps:
            //	* Step 1 - The Html Processor, parses and extracts links, text and more from html
            //  * Step 2 - Processes PDF files, extracting text
            //  * Step 3 - Try to determine language based on page, based on text extraction, using google language detection
            //  * Step 4 - Dump the information to the console, this is a custom step, see the DumperStep class
            using (Crawler c = new Crawler(new Uri("http://www.cefa.com/"),
                new HtmlDocumentProcessor(), // Process html
                                             //new iTextSharpPdfProcessor.iTextSharpPdfProcessor(),
                                             //new GoogleLanguageDetection(),
                new DumperStep())
            {
                // Custom step to visualize crawl
                MaximumThreadCount = 4,
                MaximumCrawlDepth = 10000,
                ExcludeFilter = Program.ExtensionsToSkip,
            })
            {
                // Begin crawl
                c.Crawl();
            }
        }

        #endregion
    }

    #region Nested type: DumperStep

    /// <summary>
    /// 	Custom pipeline step, to dump url to console
    /// </summary>
    internal class DumperStep : IPipelineStep
    {
        #region IPipelineStep Members

        /// <summary>
        /// </summary>
        /// <param name = "crawler">
        /// 	The crawler.
        /// </param>
        /// <param name = "propertyBag">
        /// 	The property bag.
        /// </param>
        public void Process(Crawler crawler, PropertyBag propertyBag)
        {
            CultureInfo contentCulture = (CultureInfo)propertyBag["LanguageCulture"].Value;
            string cultureDisplayValue = "N/A";
            if (!contentCulture.IsNull())
            {
                cultureDisplayValue = contentCulture.DisplayName;
            }

            lock (this)
            {
                Console.Out.WriteLine(ConsoleColor.Gray, "Url: {0}", propertyBag.Step.Uri);
                string outputFolder = BiggWhaleDataCollector.Properties.Settings.Default.DataFolder + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine(ex.ToString());
                }
                //Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tContent type: {0}", propertyBag.ContentType);
                //Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tContent length: {0}", propertyBag.Text.IsNull() ? 0 : propertyBag.Text.Length);
                //Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tDepth: {0}", propertyBag.Step.Depth);
                //Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tCulture: {0}", cultureDisplayValue);
                //Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
                //Console.Out.WriteLine(ConsoleColor.DarkGreen, "\tThread Count: {0}", crawler.ThreadsInUse);
                if (propertyBag.Step.Uri.ToString().Contains("FundDetail"))
                {
                    // Parse the detail string
                    string detailDump = propertyBag.Text.Replace(Environment.NewLine, string.Empty);
                    //detailDump = detailDump.Substring(0, detailDump.IndexOf("Resources"));
                    // replace multiple spaces with exactly 2 spaces
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex(@"[\s]{2,}", options);
                    detailDump = regex.Replace(detailDump, @"  ");
                    //detailDump = Regex.Replace(detailDump, @"  +","  ");
                    // write the fund detail content to a file
                    try
                    {
                        string fileName = outputFolder + "FundDetail_" + propertyBag.Step.Uri.ToString().Substring(propertyBag.Step.Uri.ToString().IndexOf("ID=") + 3) + ".txt";
                        int contentLength = detailDump.IsNull() ? 0 : detailDump.Length;
                        if (contentLength > 0)
                        {
                            using (StreamWriter writetext = new StreamWriter(fileName))
                            {
                                writetext.WriteLine(detailDump);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLine(ex.ToString());
                    }

                    int rowcount = 0;
                    // Query the database for the for the detail url and insert it if it is not already in the list
                    //using (SqlConnection connection = new SqlConnection("Server=nabccrmdev.cloudapp.net;Database=CEF_db;User Id=cef_admin; Password = BW2016!; "))
                    using (SqlConnection connection = new SqlConnection(BiggWhaleDataCollector.Properties.Settings.Default.NCrawlerConn))
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "SELECT [Crawl Url] from [Crawl Urls] where [Crawl Url] = @crawlUrl ";
                            try
                            {
                                SqlParameter crawlUrlParam = new SqlParameter("@crawlUrl", SqlDbType.VarChar);
                                crawlUrlParam.Value = propertyBag.Step.Uri.AbsoluteUri;
                                command.Parameters.Add(crawlUrlParam);
                                string row = "";
                                connection.Open();
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        rowcount++;
                                        row = reader["Crawl Url"].ToString();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }

                        if (rowcount == 0)
                        {
                            // insert the row
                            //using (SqlConnection insertCon = new SqlConnection("Server=nabccrmdev.cloudapp.net;Database=CEF_db;User Id=cef_admin; Password = BW2016!; "))
                            //using (SqlConnection insertCon = new SqlConnection("Server=tcp:biggwhaledb.database.windows.net,1433;Initial Catalog=CEF_db;Persist Security Info=False;User ID=cef_admin;Password=BiggWhale2016!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                            using (SqlConnection insertCon = new SqlConnection(BiggWhaleDataCollector.Properties.Settings.Default.NCrawlerConn))
                            {
                                string saveUrl = "INSERT into [Crawl Urls] ([Site Name],[Crawl Url]) VALUES (@siteName,@newCrawlUrl)";

                                using (SqlCommand querySaveUrl = new SqlCommand(saveUrl))
                                {
                                    SqlParameter siteNameParam = new SqlParameter("@siteName", SqlDbType.VarChar);
                                    siteNameParam.Value = "www.cefa.com";
                                    querySaveUrl.Parameters.Add(siteNameParam);
                                    SqlParameter newCrawlUrlParam = new SqlParameter("@newCrawlUrl", SqlDbType.VarChar);
                                    newCrawlUrlParam.Value = propertyBag.Step.Uri.AbsoluteUri;
                                    querySaveUrl.Parameters.Add(newCrawlUrlParam);

                                    insertCon.Open();
                                    querySaveUrl.Connection = insertCon;
                                    try
                                    {
                                        querySaveUrl.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.ToString());
                                    }
                                }
                            }
                        }
                    }
                }

                if (propertyBag.Step.Uri.ToString().Contains("AssetClass"))
                {
                    // write the asset class content to a file
                    try
                    {
                        string fileName = outputFolder + "AssetClass_" + propertyBag.Step.Uri.ToString().Substring(propertyBag.Step.Uri.ToString().IndexOf("AssetClass=") + 11) + ".txt";
                        using (StreamWriter writetext = new StreamWriter(fileName))
                        {
                            writetext.WriteLine(propertyBag.Text.Replace(Environment.NewLine, string.Empty));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLine(ex.ToString());
                    }
                }

                if (propertyBag.Step.Uri.ToString().Contains("Classifications"))
                {
                    // write the asset class content to a file
                    try
                    {
                        string fileName = BiggWhaleDataCollector.Properties.Settings.Default.DataFolder + DateTime.Now.ToString("yyyy-MM-dd") + "\\Classifications.txt";
                        using (StreamWriter writetext = new StreamWriter(fileName))
                        {
                            writetext.WriteLine(propertyBag.Text.Replace(Environment.NewLine, string.Empty));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLine(ex.ToString());
                    }
                }

            }
        }

        #endregion
    }

    #endregion
}