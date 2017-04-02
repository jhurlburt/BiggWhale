namespace NCrawler.FundServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CrawlQueue")]
    public partial class CrawlQueue
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public byte[] SerializedData { get; set; }
    }
}
