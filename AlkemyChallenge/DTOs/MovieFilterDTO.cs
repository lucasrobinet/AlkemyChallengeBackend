namespace AlkemyChallenge.DTOs
{
    public class MovieFilterDTO
    {
        public int Page { get; set; } = 1;
        public int NumberOfEntrysPerPage { get; set; } = 10;
        public PaginationDTO Pagination
        {
            get { return new PaginationDTO() { Page = Page, NumberEntrysPerPage = NumberOfEntrysPerPage }; }
        }

        public string Title { get; set; }
        public int GenreId { get; set; }
        public string Order { get; set; }
    }
}
