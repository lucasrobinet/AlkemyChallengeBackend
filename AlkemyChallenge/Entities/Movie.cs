using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.Entities
{
    public class Movie: IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        public DateTime DateOfCreation { get; set; }
        [Range(0,10)]
        public int Rate { get; set; }
        public string Image { get; set; }
        public List<MoviesCharacters> MoviesCharacters { get; set; }
        public List<MoviesGenres> MoviesGenres { get; set; }
    }
}