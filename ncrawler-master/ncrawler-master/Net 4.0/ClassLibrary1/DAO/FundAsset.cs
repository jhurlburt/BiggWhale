namespace NCrawler.FundServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FundAssets")]
    public partial class FundAsset
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(64)]
        public string id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(64)]
        public string fund_id { get; set; }

        [Column("Top Funds Date")]
        public DateTime? Top_Funds_Date { get; set; }

        [Column("Top Funds")]
        [StringLength(512)]
        public string Top_Funds { get; set; }

        [Column("Total Net Assets Date")]
        public DateTime? Total_Net_Assets_Date { get; set; }

        [Column("Total Net Assets")]
        [StringLength(512)]
        public string Total_Net_Assets { get; set; }

        [Column("Quality Date")]
        public DateTime? Quality_Date { get; set; }

        [StringLength(4000)]
        public string Quality { get; set; }

        [Column("Crawl Date")]
        public DateTime? Crawl_Date { get; set; }
    }
}
