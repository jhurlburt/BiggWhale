namespace NCrawler.FundServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [StringLength(64)]
        public string id { get; set; }

        [StringLength(128)]
        public string username { get; set; }

        [StringLength(128)]
        public string password { get; set; }

        [StringLength(128)]
        public string firstname { get; set; }

        [StringLength(128)]
        public string lastname { get; set; }

        [StringLength(256)]
        public string email { get; set; }

        public DateTime? createdate { get; set; }

        public DateTime? lastmodified { get; set; }

        public bool? enabled { get; set; }

        public bool? passwordexpired { get; set; }

        public DateTime? passwordchangedate { get; set; }

        public short? passwordminlength { get; set; }

        public bool? passwordneverexpires { get; set; }

        [Key]
        [Column(Order = 0)]
        public Guid UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid ApplicationId { get; set; }

        public bool? IsAnonymous { get; set; }

        public DateTime? LastActivityDate { get; set; }
    }
}
