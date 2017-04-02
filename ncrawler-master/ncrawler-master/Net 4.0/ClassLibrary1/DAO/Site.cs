namespace NCrawler.FundServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Site
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(64)]
        public string id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(256)]
        public string Name { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

        [Column("Created By")]
        [StringLength(50)]
        public string Created_By { get; set; }

        [Column("Last Crawl")]
        public DateTime? Last_Crawl { get; set; }

        [StringLength(256)]
        public string Url { get; set; }
    }
}
