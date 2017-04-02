namespace NCrawler.FundServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FundDetails")]
    public partial class FundDetail
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(64)]
        public string id { get; set; }

        [Column(Order = 1)]
        [StringLength(64)]
        public string fund_id { get; set; }

        [Column("Create Date", Order = 2)]
        public DateTime? Create_Date { get; set; }

        [Column("Created By")]
        [StringLength(50)]
        public string Created_By { get; set; }

        [Column("Crawl Date")]
        public DateTime? Crawl_Date { get; set; }

        public decimal? OneYearLipperAvg { get; set; }

        public decimal? TenYearMarketReturn { get; set; }

        public decimal? FiveYearMarketReturn { get; set; }

        public decimal? OneYearMarketReturn { get; set; }

        public decimal? YTDMarketReturn { get; set; }

        public int? TenYearMarketReturnRank { get; set; }

        public int? FiveYearMarketReturnRank { get; set; }

        public int? OneYearMarketReturnRank { get; set; }

        public int? YTDMarketReturnRank { get; set; }

        public decimal? TenYearNAVReturn { get; set; }

        public decimal? FiveYearNAVReturn { get; set; }

        public decimal? OneYearNAVReturn { get; set; }

        public decimal? YTDNAVReturn { get; set; }

        public int? TenYearNAVReturnRank { get; set; }

        public int? FiveYearNAVReturnRank { get; set; }

        public int? OneYearNAVReturnRank { get; set; }

        public int? YTDNAVReturnRank { get; set; }

        public decimal? TenYearPremiumDiscountAvg { get; set; }

        public decimal? FiveYearPremiumDiscountAvg { get; set; }

        public decimal? YTDPremiumDiscountAvg { get; set; }

        public decimal? NAV { get; set; }

        public decimal? MarketPrice { get; set; }

        public decimal? NetChange { get; set; }

        public decimal? MarketChange { get; set; }

        public decimal? OneYearNAVReturnAct { get; set; }

        public DateTime? TwelveMonthYieldDate { get; set; }

        public decimal? DefinedIncomeOnlyYield { get; set; }

        public decimal? DistributionYield { get; set; }

        public decimal? MostRecentIncimeDividend { get; set; }

        public DateTime? MostRecentIncomeDividendDate { get; set; }

        public decimal? MostRecentCapGainDiviednd { get; set; }

        public DateTime? MostRecentCapGainDividendDate { get; set; }

        public decimal? MonthlyYTDDividends { get; set; }

        public decimal? YTDCapGains { get; set; }

        public decimal? PremiumDiscount { get; set; }
    }
}
