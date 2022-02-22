using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.DTOs
{
    public class AssignRoleDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
