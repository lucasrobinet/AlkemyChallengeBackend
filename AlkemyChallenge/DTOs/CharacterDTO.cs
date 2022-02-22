using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.DTOs
{
    public class CharacterDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; }
        public string Lore { get; set; }
        public string Image { get; set; }
    }
}
