namespace NCrawler.FundServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UsersOpenAuthData")]
    public partial class UsersOpenAuthData
    {
        [Key]
        [Column(Order = 0)]
        public string ApplicationName { get; set; }

        [Key]
        [Column(Order = 1)]
        public string MembershipUserName { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool HasLocalPassword { get; set; }
    }
}
