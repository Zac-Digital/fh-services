using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.Idam.Core.Models
{

    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }

        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public PaginatedList()
        {
            Items = new List<T>();
        }

        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling((double)count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            return new PaginatedList<T>(count: await source.CountAsync(), items: await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(), pageNumber: pageNumber, pageSize: pageSize);
        }
    }
}
