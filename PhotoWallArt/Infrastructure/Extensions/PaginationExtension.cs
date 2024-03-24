using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class PaginationExtension
    {
        private static readonly int MAX_PAGE_SIZE = 100;
        private static readonly int MIN_PAGE_SIZE = 20;

        public static PagedResult<T> Paginate<T>(this IQueryable<T> source, PagedRequest paginationParams)
        {
            var (pageSize, page) = paginationParams;
            return ExecutePaging(source, page, pageSize);
        }

        public static Task<PagedResult<T>> PaginateAsync<T>(this IEnumerable<T> source, PagedRequest paginationParams)
        {
            var (pageSize, page) = paginationParams;
            return Task.FromResult(ExecutePaging(source, page, pageSize));
        }

        public static PagedResult<T> Paginate<T>(this IEnumerable<T> source, int page, int pageSize = 20)
        {
            return ExecutePaging(source, page, pageSize);
        }

        public static Task<PagedResult<T>> PaginateAsync<T>(this IEnumerable<T> source, int page, int pageSize = 20)
        {
            return Task.FromResult(ExecutePaging(source, page, pageSize));
        }

        private static PagedResult<T> ExecutePaging<T>(IEnumerable<T> source, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? MIN_PAGE_SIZE : pageSize;
            pageSize = pageSize > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : pageSize;

            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize)
                .Take(pageSize);
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PagedResult<T>
            {
                CurrentPage = page,
                Data = items,
                CurrentPageRecord = items.Count(),
                PageSize = pageSize,
                NextPage = page < totalPages ? page + 1 : null,
                PreviousPage = page > 1 && totalPages > 1 ? page - 1 : null,
                TotalPages = totalPages,
                TotalRecord = count
            };
        }
    }

    #region Paged Request & Result
    public interface IPagedRequest
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }

    public class PagedRequest : IPagedRequest
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public PagedRequest()
        {
        }
        /// <summary>
        /// Parameter Constructor
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        public PagedRequest(int page, int pageSize)
        {
            PageSize = pageSize;
            Page = page;
        }

        /// <summary>
        /// Deconstruct
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        public void Deconstruct(out int page, out int pageSize)
        {
            pageSize = PageSize;
            page = Page;
        }

        /// <summary>
        /// Page
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// PageSize
        /// </summary>
        public int PageSize { get; set; }
    };

    public class PagedResult<T>
    {
        /// <summary>
        /// CurrentPage
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        public IEnumerable<T> Data { get; set; } = null!;
        /// <summary>
        /// CurrentPageRecord
        /// </summary>
        public int CurrentPageRecord { get; set; }
        /// <summary>
        /// PageSize
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// NextPage
        /// </summary>
        public int? NextPage { get; set; }
        /// <summary>
        /// PreviousPage
        /// </summary>
        public int? PreviousPage { get; set; }
        /// <summary>
        /// TotalPages
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// TotalRecord
        /// </summary>
        public int TotalRecord { get; set; }
    }
    #endregion
}

