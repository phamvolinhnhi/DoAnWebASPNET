namespace WebSach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User_Login
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        public DateTime LoginTime { get; set; }

        public int Id { get; set; }

        public virtual User User { get; set; }
    }
}
