using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCrawler.FundServices;
using NCrawler.HtmlProcessor;
using System.Data.SqlClient;

namespace NCrawler.Demo
{
    class CrawlUsingFundServices
    {
        public static void Run()
        {
            FundServicesModule.Setup(false);
            Console.Out.WriteLine("Simple crawl demo using FundSvc");

            // Setup crawler to crawl http://ncrawler.codeplex.com
            // with 1 thread adhering to robot rules, and maximum depth
            // of 2 with 4 pipeline steps:
            //	* Step 1 - The Html Processor, parses and extracts links, text and more from html
            //  * Step 2 - Processes PDF files, extracting text
            //  * Step 3 - Try to determine language based on page, based on text extraction, using google language detection
            //  * Step 4 - Dump the information to the console, this is a custom step, see the DumperStep class
            using (Crawler c = new Crawler(new Uri("http://www.cefa.com"),
                new HtmlDocumentProcessor(),
                new FundServicesWriter())
            {
                // Custom step to visualize crawl
                MaximumThreadCount = 2,
                MaximumCrawlDepth = 10,
                ExcludeFilter = Program.ExtensionsToSkip,
            })
            {
                // Begin crawl
                c.Crawl();
            }
        }

    }
}
