namespace NCrawler.FundServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Profile
    {
        [Key]
        [Column(Order = 0)]
        public Guid UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4000)]
        public string PropertyNames { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(4000)]
        public string PropertyValueStrings { get; set; }

        [Key]
        [Column(Order = 3)]
        public byte[] PropertyValueBinary { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime LastUpdatedDate { get; set; }
    }
}
