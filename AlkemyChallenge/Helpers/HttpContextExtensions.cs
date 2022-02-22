using Microsoft.EntityFrameworkCore;

namespace AlkemyChallenge.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParameters<T>(this HttpContext httpContext, IQueryable<T> queryable, int numberOfEntrysPerPage)
        {
            double cant = await queryable.CountAsync();
            double numbersOfPages = Math.Ceiling(cant / numberOfEntrysPerPage);
            httpContext.Response.Headers.Add("numberOfPages", numbersOfPages.ToString());
        }
    }
}
