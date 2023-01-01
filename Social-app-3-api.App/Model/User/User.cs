using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Social_app_3_api.Model.User
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(2)]
        [Column(TypeName = "varchar(100)")]
        public string? FirstName { get; set; }

        [Required, MinLength(2)]
        [Column(TypeName = "varchar(100)")]
        public string? LastName { get; set; }

        [Required, MinLength(2), Unique]
        [Column(TypeName = "varchar(100)")]
        [Index]
        public string? UserName { get; set; }

        // !!!!! UNIQUE NÃO ESTÁ FUNCIONANDO !!!!!
        [Required, Unique]
        [Index]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public UserRoleEnum Role { get; set; } = UserRoleEnum.RegularUser;

        [Required, MinLength(8)]
        [Column(TypeName = "varchar(100)")]
        public string? Password { get; set; }

        public ICollection<Post.Post> Posts { get; set; } = default!;
    }
}
