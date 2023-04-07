namespace WebSach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReadHistory")]
    public partial class ReadHistory
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string UserName { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BookId { get; set; }

        public int ChapId { get; set; }

        public DateTime Time { get; set; }

        public virtual Books Books { get; set; }

        public virtual Chapter Chapter { get; set; }

        public virtual User User { get; set; }
    }
}
