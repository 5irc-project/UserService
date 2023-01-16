using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models.EntityFramework
{
    [Table("t_e_user_usr")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("usr_id")]
        public int UserId { get; set; }

        [MaxLength(50)]
        [Column("usr_name")]
        public string? Nom { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        [Column("usr_email")]
        public string? Email { get; set; }

        [MaxLength(500)]
        [Column("usr_profile_picture_url")]
        public string? ProfilePictureUrl { get; set; }
    }
}
