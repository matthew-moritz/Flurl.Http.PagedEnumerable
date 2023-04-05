using System;
using System.Collections.Generic;

namespace Flurl.Http.PagedEnumerable
{
    /// <summary>
    /// A collection of extension methods for iterating over a collection of paged HTTP requests.
    /// </summary>
    public static class PagedEnumerableExtensions
    {
        /// <summary>
        /// Creates an <see cref="IEnumerable{T}" /> to iterate over a collection of paged requests.
        /// </summary>
        /// <typeparam name="T">The response type from a single paged request.</typeparam>
        /// <param name="url">The URL for the initial HTTP request.</param>
        /// <param name="nextSelector">A function to resolve the next endpoint if there is another page to request.</param>
        public static IEnumerable<T> AsPagedEnumerable<T>(this Url url, Func<T, Url> nextSelector) =>
            new FlurlRequest(url).AsPagedEnumerable(nextSelector);

        /// <summary>
        /// Creates an <see cref="IEnumerable{T}" /> to iterate over a collection of paged requests.
        /// </summary>
        /// <typeparam name="T">The response type from a single paged request.</typeparam>
        /// <param name="uri">The URI for the initial HTTP request.</param>
        /// <param name="nextSelector">A function to resolve the next endpoint if there is another page to request.</param>
        public static IEnumerable<T> AsPagedEnumerable<T>(this Uri uri, Func<T, Url> nextSelector) =>
            new FlurlRequest(uri).AsPagedEnumerable(nextSelector);

        /// <summary>
        /// Creates an <see cref="IEnumerable{T}" /> to iterate over a collection of paged requests.
        /// </summary>
        /// <typeparam name="T">The response type from a single paged request.</typeparam>
        /// <param name="url">The URL for the initial HTTP request.</param>
        /// <param name="nextSelector">A function to resolve the next endpoint if there is another page to request.</param>
        public static IEnumerable<T> AsPagedEnumerable<T>(this string url, Func<T, Url> nextSelector) =>
            new FlurlRequest(url).AsPagedEnumerable(nextSelector);

        /// <summary>
        /// Creates an <see cref="IEnumerable{T}" /> to iterate over a collection of paged requests.
        /// </summary>
        /// <typeparam name="T">The response type from a single paged request.</typeparam>
        /// <param name="request">The representation of the HTTP request.</param>
        /// <param name="nextSelector">A function to resolve the next endpoint if there is another page to request.</param>
        public static IEnumerable<T> AsPagedEnumerable<T>(this IFlurlRequest request, Func<T, Url> nextSelector) =>
            new PagedEnumerable<T>(request, nextSelector);

        /// <summary>
        /// Creates an <see cref="IAsyncEnumerable{T}" /> to asynchronously iterate over a collection of paged requests.
        /// </summary>
        /// <typeparam name="T">The response type from a single paged request.</typeparam>
        /// <param name="url">The URL for the initial HTTP request.</param>
        /// <param name="nextSelector">A function to resolve the next endpoint if there is another page to request.</param>
        public static IAsyncEnumerable<T> AsPagedAsyncEnumerable<T>(this Url url, Func<T, Url> nextSelector) =>
            new FlurlRequest(url).AsPagedAsyncEnumerable(nextSelector);

        /// <summary>
        /// Creates an <see cref="IAsyncEnumerable{T}" /> to asynchronously iterate over a collection of paged requests.
        /// </summary>
        /// <typeparam name="T">The response type from a single paged request.</typeparam>
        /// <param name="uri">The URI for the initial HTTP request.</param>
        /// <param name="nextSelector">A function to resolve the next endpoint if there is another page to request.</param>
        public static IAsyncEnumerable<T> AsPagedAsyncEnumerable<T>(this Uri uri, Func<T, Url> nextSelector) =>
            new FlurlRequest(uri).AsPagedAsyncEnumerable(nextSelector);

        /// <summary>
        /// Creates an <see cref="IAsyncEnumerable{T}" /> to asynchronously iterate over a collection of paged requests.
        /// </summary>
        /// <typeparam name="T">The response type from a single paged request.</typeparam>
        /// <param name="url">The URL for the initial HTTP request.</param>
        /// <param name="nextSelector">A function to resolve the next endpoint if there is another page to request.</param>
        public static IAsyncEnumerable<T> AsPagedAsyncEnumerable<T>(this string url, Func<T, Url> nextSelector) =>
            new FlurlRequest(url).AsPagedAsyncEnumerable(nextSelector);

        /// <summary>
        /// Creates an <see cref="IAsyncEnumerable{T}" /> to asynchronously iterate over a collection of paged requests.
        /// </summary>
        /// <typeparam name="T">The response type from a single paged request.</typeparam>
        /// <param name="request">The representation of the HTTP request.</param>
        /// <param name="nextSelector">A function to resolve the next endpoint if there is another page to request.</param>
        public static IAsyncEnumerable<T> AsPagedAsyncEnumerable<T>(this IFlurlRequest request, Func<T, Url> nextSelector) =>
            new PagedAsyncEnumerable<T>(request, nextSelector);
    }
}
