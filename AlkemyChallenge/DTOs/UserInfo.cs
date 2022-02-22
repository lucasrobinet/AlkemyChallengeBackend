using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.DTOs
{
    public class UserInfo
    {
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
