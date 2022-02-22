using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.DTOs
{
    public class GenreCreationDTO
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
    }
}
