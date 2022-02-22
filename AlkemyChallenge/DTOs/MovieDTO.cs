using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateOfCreation { get; set; }
        [Range(0, 10)]
        public int Rate { get; set; }
        public string Image { get; set; }
    }
}
