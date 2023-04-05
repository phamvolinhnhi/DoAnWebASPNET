namespace WebSach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        [Key]
        public int Comment_Id { get; set; }

        [StringLength(250)]
        public string Comment_content { get; set; }

        public DateTime Update_at { get; set; }

        [Required]
        [StringLength(50)]
        public string User_Name { get; set; }

        public int? Book_Id { get; set; }

        public virtual Books Books { get; set; }

        public virtual User User { get; set; }
    }
}
