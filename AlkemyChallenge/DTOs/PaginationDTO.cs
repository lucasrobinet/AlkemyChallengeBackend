namespace AlkemyChallenge.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;

        private int numberOfEntrysPerPage = 10;
        private readonly int maxNumberEntrysPerPage = 30;

        public int NumberEntrysPerPage
        {
            get => numberOfEntrysPerPage;

            set
            {
                numberOfEntrysPerPage = (value > maxNumberEntrysPerPage) ? maxNumberEntrysPerPage : value;
            }
        }
    }
}
