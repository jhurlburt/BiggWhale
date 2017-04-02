namespace NCrawler.FundServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CrawlHistory")]
    public partial class CrawlHistory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1024)]
        public string Key { get; set; }

        public int GroupId { get; set; }
    }
}
