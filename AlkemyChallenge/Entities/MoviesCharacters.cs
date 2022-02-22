namespace AlkemyChallenge.Entities
{
    public class MoviesCharacters
    {
        public int CharacterId { get; set; }
        public int MovieId { get; set; }
        public Character Character { get; set; }
        public Movie Movie { get; set; }
        public int Order { get; set; }
    }
}
