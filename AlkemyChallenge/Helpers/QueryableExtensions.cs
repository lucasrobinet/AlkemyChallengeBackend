using AlkemyChallenge.DTOs;

namespace AlkemyChallenge.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Pagination<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
        {
            return queryable
                .Skip((paginationDTO.Page - 1) * paginationDTO.NumberEntrysPerPage)
                .Take(paginationDTO.NumberEntrysPerPage);
        }
    }
}
