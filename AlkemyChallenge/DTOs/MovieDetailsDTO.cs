namespace AlkemyChallenge.DTOs
{
    public class MovieDetailsDTO: MovieDTO
    {
        public List<CharacterMovieDetailsDTO> Characters { get; set; }
    }
}
