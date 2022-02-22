using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.DTOs
{
    public class CharacterPatchDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; }
        public string Lore { get; set; }
    }
}
