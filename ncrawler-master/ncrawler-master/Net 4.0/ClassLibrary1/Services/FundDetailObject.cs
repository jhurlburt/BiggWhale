using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCrawler.FundServices
{
    class FundDetailObject
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
        public int CEF_id { get; set; }

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
}
