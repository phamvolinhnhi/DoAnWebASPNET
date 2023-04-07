namespace WebSach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Books = new HashSet<Books>();
            ReadHistory = new HashSet<ReadHistory>();
            User_Login = new HashSet<User_Login>();
        }

        [Key]
        [StringLength(50)]
        public string User_Name { get; set; }

        [StringLength(50)]
        public string Full_Name { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Avatar { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Last_Login { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public bool? Permission_Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Books> Books { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReadHistory> ReadHistory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_Login> User_Login { get; set; }
    }
}
