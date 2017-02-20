using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Autofac;

using BiggWhaleDataCollector.Extensions;
using BiggWhaleDataCollector.HtmlProcessor;
using BiggWhaleDataCollector.Interfaces;
using BiggWhaleDataCollector.Services;

namespace BiggWhaleDataCollector
{
    /// <summary>
    /// 	Crawl a site and adhere to the robot rules, and also crawl 2 levels of any external
    /// 	links. Dump everything in the same instance of a IPipeline step(DumperStep)
    /// </summary>
    internal class LinkExtractor
    {
        #region Class Methods

        public static IFilter[] ExtensionsToSkip = new[]
    {
                (RegexFilter)new Regex(@"(\.jpg|\.css|\.js|\.gif|\.jpeg|\.png|\.ico)",
                    RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
            };

        public static void Run()
        {
            NCrawlerModule.Setup();

            // Register new implementation for ICrawlerRules using our custom class CustomCrawlerRules defined below
            NCrawlerModule.Register(builder =>
                builder.Register((c, p) =>
                    {
                        NCrawlerModule.Setup(); // Return to standard setup
                        return new CustomCrawlerRules(p.TypedAs<Crawler>(), c.Resolve<IRobot>(p), p.TypedAs<Uri>(),
                            p.TypedAs<ICrawlerHistory>());
                    }).
                As<ICrawlerRules>().
                InstancePerDependency());

            Console.Out.WriteLine("Advanced crawl demo");

            using (Crawler c = new Crawler(
                new Uri("http://www.cefa.com"),
                new HtmlDocumentProcessor(), // Process html
                new DumperStep())
            {
                MaximumThreadCount = 2,
                MaximumCrawlDepth = 2,
                MaximumCrawlCount = 1000,
                ExcludeFilter = LinkExtractor.ExtensionsToSkip,
            })
            {
                // Begin crawl
                c.Crawl();
            }
        }

        #endregion
    }

    public class CustomCrawlerRules : CrawlerRulesService
    {
        #region Readonly & Static Fields

        private readonly ICrawlerHistory m_CrawlerHistory;

        #endregion

        #region Constructors

        public CustomCrawlerRules(Crawler crawler, IRobot robot, Uri baseUri, ICrawlerHistory crawlerHistory)
            : base(crawler, robot, baseUri)
        {
            m_CrawlerHistory = crawlerHistory;
        }

        #endregion

        #region Instance Methods

        public override bool IsExternalUrl(Uri uri)
        {
            // Is External Url
            if (!base.IsExternalUrl(uri))
            {
                return false;
            }

            // Yes, check if we have crawled it before
            if (!m_CrawlerHistory.Register(uri.GetUrlKeyString(m_Crawler.UriSensitivity)))
            {
                return true;
            }

            // Create child crawler to traverse external site with max 2 levels
            using (Crawler externalCrawler = new Crawler(uri,
                new HtmlDocumentProcessor(), // Process html
                new DumperStep())
            {
                MaximumThreadCount = 1,
                MaximumCrawlDepth = 2,
                MaximumCrawlCount = 1000,
                ExcludeFilter = LinkExtractor.ExtensionsToSkip,
            })
            {
                // Crawl external site
                externalCrawler.Crawl();
            }

            // Do not follow link on this crawler
            return true;
        }

        #endregion
    }

}