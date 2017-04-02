namespace NCrawler.FundServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Funds")]
    public partial class Fund
    {
        [StringLength(64)]
        public string id { get; set; }

        [Key]
        [StringLength(100)]
        public string Name { get; set; }

        [Column("Fund Type")]
        [StringLength(50)]
        public string Fund_Type { get; set; }

        [Column("Ticker Symbol")]
        [StringLength(25)]
        public string Ticker_Symbol { get; set; }

        [Column("Asset Class")]
        [StringLength(50)]
        public string Asset_Class { get; set; }

        [Column("Inception Date")]
        public DateTime? Inception_Date { get; set; }

        [StringLength(100)]
        public string Advisor { get; set; }

        [Column("Manager And Tenure")]
        [StringLength(100)]
        public string Manager_And_Tenure { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(256)]
        public string Website { get; set; }

        [Column("Total Net Assets")]
        public decimal? Total_Net_Assets { get; set; }

        [Column("Total Net Assets Date")]
        public DateTime? Total_Net_Assets_Date { get; set; }

        [Column("Percent Leveraged Assets")]
        public decimal? Percent_Leveraged_Assets { get; set; }

        [Column("Percent Leveraged Assets Date")]
        public DateTime? Percent_Leveraged_Assets_Date { get; set; }

        [Column("Portfolio Turnover")]
        public decimal? Portfolio_Turnover { get; set; }

        [Column("Management Fees")]
        public decimal? Management_Fees { get; set; }

        [Column("Expense Ratio")]
        public decimal? Expense_Ratio { get; set; }

        [Column("Alternative Minimum Tax")]
        public decimal? Alternative_Minimum_Tax { get; set; }

        [Column("Fund Objective")]
        [StringLength(1024)]
        public string Fund_Objective { get; set; }

        public decimal? Yield { get; set; }

        public int CEF_id { get; set; }

        [Column("Create Date")]
        public DateTime Create_Date { get; set; }

        [Column("Created By")]
        [StringLength(50)]
        public string Created_By { get; set; }

        [Column("Modify Date")]
        public DateTime Modify_Date { get; set; }

        [Column("Modified By")]
        [StringLength(50)]
        public string Modified_By { get; set; }
    }
}
