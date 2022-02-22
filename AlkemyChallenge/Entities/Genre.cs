using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.Entities
{
    public class Genre: IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        public List<MoviesGenres> MoviesGenres { get; set; }
    }
}
