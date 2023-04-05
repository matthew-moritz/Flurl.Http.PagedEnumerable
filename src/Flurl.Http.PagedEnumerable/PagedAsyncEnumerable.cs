using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Flurl.Http.PagedEnumerable
{
    /// <summary>
    /// Exposes an enumerator that provides asynchronous iteration over a collection of paged responses.
    /// </summary>
    /// <typeparam name="T">The response type from a single paged request.</typeparam>
    public sealed class PagedAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly IFlurlRequest request;
        private readonly Func<T, Url> nextSelector;

        /// <summary>
        /// Creates a new instance of <see cref="PagedAsyncEnumerable{T}" />
        /// </summary>
        /// <param name="request">The initial HTTP request.</param>
        /// <param name="nextSelector">A function to resolve the next endpoint if there is another page to request.</param>
        public PagedAsyncEnumerable(IFlurlRequest request, Func<T, Url> nextSelector)
        {
            this.request = request;
            this.nextSelector = nextSelector;
        }

        /// <inheritdoc />
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            new PagedAsyncEnumerator(this.request, this.nextSelector, cancellationToken);

        /// <summary>
        /// Asynchronously iterates over a collection of paged requests.
        /// </summary>
        /// <remarks>
        /// The next request endpoint will be resolved from the response of the previous response.
        /// </remarks>
        private class PagedAsyncEnumerator : IAsyncEnumerator<T>
        {
            private IFlurlRequest next;
            private readonly Func<T, Url> nextSelector;
            private readonly CancellationToken cancellationToken;

            /// <summary>
            /// Creates a new instance of <see cref="PagedAsyncEnumerator" />.
            /// </summary>
            /// <param name="next">The next HTTP request.</param>
            /// <param name="nextSelector">A function to resolve the next endpoint if there is another page to request.</param>
            /// <param name="cancellationToken">A token that determines if the operation has been canceled.</param>
            public PagedAsyncEnumerator(IFlurlRequest next, Func<T, Url> nextSelector, CancellationToken cancellationToken = default)
            {
                this.next = next;
                this.nextSelector = nextSelector;
                this.cancellationToken = cancellationToken;
            }

            /// <inheritdoc />
            public T Current { get; private set; }

            /// <inheritdoc />
            public ValueTask DisposeAsync() => ValueTask.CompletedTask;

            /// <inheritdoc />
            public async ValueTask<bool> MoveNextAsync()
            {
                if (this.cancellationToken.IsCancellationRequested)
                {
                    return false;
                }

                if (this.next is null || string.IsNullOrWhiteSpace(this.next.Url))
                {
                    return false;
                }

                this.Current = await this.next.GetJsonAsync<T>();
                this.next = new FlurlRequest(this.nextSelector(Current));

                return true;
            }
        }
    }
}
