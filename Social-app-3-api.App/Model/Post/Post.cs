using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Social_app_3_api.Model.Post
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required, MinLength(1)]
        [Column(TypeName = "varchar(500)")]
        public string? Content { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public User.User? User{ get; set; }
    }
}
