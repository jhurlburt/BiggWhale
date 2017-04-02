namespace NCrawler.FundServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Membership
    {
        [Key]
        [Column(Order = 0)]
        public Guid ApplicationId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid UserId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string Password { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PasswordFormat { get; set; }

        [Key]
        [Column(Order = 4)]
        public string PasswordSalt { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(256)]
        public string PasswordQuestion { get; set; }

        [StringLength(128)]
        public string PasswordAnswer { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool IsApproved { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool IsLockedOut { get; set; }

        [Key]
        [Column(Order = 7)]
        public DateTime CreateDate { get; set; }

        [Key]
        [Column(Order = 8)]
        public DateTime LastLoginDate { get; set; }

        [Key]
        [Column(Order = 9)]
        public DateTime LastPasswordChangedDate { get; set; }

        [Key]
        [Column(Order = 10)]
        public DateTime LastLockoutDate { get; set; }

        [Key]
        [Column(Order = 11)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FailedPasswordAttemptCount { get; set; }

        [Key]
        [Column(Order = 12)]
        public DateTime FailedPasswordAttemptWindowStart { get; set; }

        [Key]
        [Column(Order = 13)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FailedPasswordAnswerAttemptCount { get; set; }

        [Key]
        [Column(Order = 14)]
        public DateTime FailedPasswordAnswerAttemptWindowsStart { get; set; }

        [StringLength(256)]
        public string Comment { get; set; }
    }
}
