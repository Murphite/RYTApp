using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RYT.Commons
{
    public static class Pagination
    {
        public static void UsePagination<T>(IQueryable<T> items, int page, int pageSize, out List<T> itemsOnPage, out int totalItems, out int totalPages)
        {
            totalItems = items.Count();
            totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            itemsOnPage = items
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}

