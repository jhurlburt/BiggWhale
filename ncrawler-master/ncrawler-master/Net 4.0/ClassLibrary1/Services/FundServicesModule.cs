using NCrawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NCrawler.FundServices;
using NCrawler.Interfaces;

namespace NCrawler.FundServices
{
    public class FundServicesModule : NCrawlerModule
    {
        private readonly bool m_Resume;

        public FundServicesModule(bool resume)
        {
            m_Resume = resume;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // Register new implementation for ICrawlerRules using our custom class CustomCrawlerRules defined below
            builder.Register((c, p) => new FundServiceCrawler(p.TypedAs<Crawler>(), c.Resolve<IRobot>(p), p.TypedAs<Uri>())).As
                <ICrawlerRules>().InstancePerDependency();
        }

        public static void Setup(bool resume)
        {
            Setup(new FundServicesModule(resume));
        }
    }
}
