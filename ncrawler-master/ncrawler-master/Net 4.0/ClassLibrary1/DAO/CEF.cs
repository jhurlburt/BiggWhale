namespace NCrawler.FundServices
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CEF : DbContext
    {
        public CEF()
            : base("name=CEF")
        {
        }

        public virtual DbSet<CrawlHistory> CrawlHistories { get; set; }
        public virtual DbSet<CrawlQueue> CrawlQueues { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<FundAsset> FundAssets { get; set; }
        public virtual DbSet<FundDetail> FundDetails { get; set; }
        public virtual DbSet<Fund> Funds { get; set; }
        public virtual DbSet<Membership> Memberships { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<SummaryData> SummaryDatas { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersOpenAuthAccount> UsersOpenAuthAccounts { get; set; }
        public virtual DbSet<UsersOpenAuthData> UsersOpenAuthDatas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FundDetail>()
                .Property(e => e.OneYearLipperAvg)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.TenYearMarketReturn)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.FiveYearMarketReturn)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.OneYearMarketReturn)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.YTDMarketReturn)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.TenYearNAVReturn)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.FiveYearNAVReturn)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.OneYearNAVReturn)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.YTDNAVReturn)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.TenYearPremiumDiscountAvg)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.FiveYearPremiumDiscountAvg)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.YTDPremiumDiscountAvg)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.NAV)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.MarketPrice)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.NetChange)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.MarketChange)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.OneYearNAVReturnAct)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.DefinedIncomeOnlyYield)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.DistributionYield)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.MostRecentIncimeDividend)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.MostRecentCapGainDiviednd)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.MonthlyYTDDividends)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.YTDCapGains)
                .HasPrecision(18, 3);

            modelBuilder.Entity<FundDetail>()
                .Property(e => e.PremiumDiscount)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Fund>()
                .Property(e => e.Total_Net_Assets)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Fund>()
                .Property(e => e.Percent_Leveraged_Assets)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Fund>()
                .Property(e => e.Portfolio_Turnover)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Fund>()
                .Property(e => e.Management_Fees)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Fund>()
                .Property(e => e.Expense_Ratio)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Fund>()
                .Property(e => e.Alternative_Minimum_Tax)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Fund>()
                .Property(e => e.Yield)
                .HasPrecision(18, 3);

            modelBuilder.Entity<SummaryData>()
                .Property(e => e.Calculated_Value)
                .HasPrecision(18, 3);
        }
    }
}
