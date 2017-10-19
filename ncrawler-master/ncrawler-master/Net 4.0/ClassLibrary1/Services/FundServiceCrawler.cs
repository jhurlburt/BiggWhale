using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCrawler.Extensions;
using NCrawler.Utils;
using NCrawler.Services;
using NCrawler.Interfaces;

namespace NCrawler.FundServices
{
    class FundServiceCrawler : CrawlerRulesService
    {
        //private readonly ICrawlerHistory m_CrawlerHistory;
        //private readonly int m_GroupId;

        public FundServiceCrawler(Crawler crawler, IRobot robot, Uri baseUri)
            : base(crawler, robot, baseUri)
        {
            //m_GroupId = baseUri.GetHashCode();
        }

        public override bool IsAllowedUrl(Uri uri, CrawlStep referrer)
        {
            if (base.IsExternalUrl(uri))
                return false;

            return base.IsAllowedUrl(uri, referrer);
        }        
    }
}
