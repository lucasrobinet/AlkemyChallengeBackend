using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.DTOs
{
    public class GenreDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
    }
}
