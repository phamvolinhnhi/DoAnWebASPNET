namespace WebSach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Books
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Books()
        {
            ReadHistory = new HashSet<ReadHistory>();
        }

        [Key]
        public int Book_Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public int Category_Id { get; set; }

        [StringLength(50)]
        public string Author { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set; }

        [StringLength(50)]
        public string Avatar { get; set; }

        public int? View { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }

        [StringLength(50)]
        public string User_Name { get; set; }

        public virtual Categories Categories { get; set; }

        public virtual User User { get; set; }
        public bool isFollowing = false;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReadHistory> ReadHistory { get; set; }
    }
}
