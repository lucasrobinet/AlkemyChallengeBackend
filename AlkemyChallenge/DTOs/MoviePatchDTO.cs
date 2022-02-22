using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.DTOs
{
    public class MoviePatchDTO
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        public DateTime DateOfCreation { get; set; }
        [Range(0, 10)]
        public int Rate { get; set; }
    }
}
