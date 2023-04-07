namespace WebSach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Chapter")]
    public partial class Chapter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Chapter()
        {
            ReadHistory = new HashSet<ReadHistory>();
        }

        [Key]
        public int Chapter_Id { get; set; }

        public int Book_Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Chapter_Name { get; set; }

        public string Content { get; set; }

        public DateTime? UpdateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReadHistory> ReadHistory { get; set; }
    }
}
