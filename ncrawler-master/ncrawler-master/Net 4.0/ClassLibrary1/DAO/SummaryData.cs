namespace NCrawler.FundServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SummaryData")]
    public partial class SummaryData
    {
        [StringLength(64)]
        public string id { get; set; }

        [Column("Fund Name")]
        [StringLength(100)]
        public string Fund_Name { get; set; }

        [Column("Ticker Symbol")]
        [StringLength(25)]
        public string Ticker_Symbol { get; set; }

        [Column("Summary Date")]
        public DateTime? Summary_Date { get; set; }

        [Column("Calculated Stat")]
        [StringLength(100)]
        public string Calculated_Stat { get; set; }

        [Column("Calculated Value")]
        public decimal? Calculated_Value { get; set; }

        public int? RankOrder { get; set; }

        [Column("Data Source")]
        [StringLength(512)]
        public string Data_Source { get; set; }
    }
}
