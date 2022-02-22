namespace AlkemyChallenge.DTOs
{
    public class CharacterFilterDTO
    {
        public int Page { get; set; } = 1;
        public int NumberOfEntrysPerPage { get; set; } = 10;
        public PaginationDTO Pagination
        {
            get { return new PaginationDTO() { Page = Page, NumberEntrysPerPage = NumberOfEntrysPerPage }; }
        }

        public string Name { get; set; }
        public int Age { get; set; }
        public int Movies { get; set; }
    }
}
