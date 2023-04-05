using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Flurl.Http.PagedEnumerable
{
    /// <summary>
    /// Exposes an enumerator that provides synchronous iteration over a collection of paged responses.
    /// </summary>
    /// <typeparam name="T">The response type from a single paged request.</typeparam>
    public sealed class PagedEnumerable<T> : IEnumerable<T>
    {
        private readonly IFlurlRequest request;
        private readonly Func<T, Url> nextSelector;

        /// <summary>
        /// Creates a new instance of <see cref="PagedEnumerable{T}" />.
        /// </summary>
        /// <param name="request">The initial HTTP request.</param>
        /// <param name="nextSelector">A function to resolve the next endpoint if there is another page to request.</param>
        public PagedEnumerable(IFlurlRequest request, Func<T, Url> nextSelector)
        {
            this.request = request;
            this.nextSelector = nextSelector;
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => new PagedEnumerator(this.request, this.nextSelector);

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Synchronously iterates over a collection of paged requests.
        /// </summary>
        /// <remarks>
        /// The next request endpoint will be resolved from the response of the previous response.
        /// </remarks>
        private class PagedEnumerator : IEnumerator<T>
        {
            private IFlurlRequest next;
            private readonly Func<T, Url> nextSelector;

            private readonly TaskFactory taskFactory;

            /// <summary>
            /// Creates a new instance of <see cref="PagedEnumerator" />.
            /// </summary>
            /// <param name="next">The next HTTP request.</param>
            /// <param name="nextSelector">A function to resolve the next endpoint if there is another page to request.</param>
            public PagedEnumerator(IFlurlRequest next, Func<T, Url> nextSelector)
            {
                this.next = next;
                this.nextSelector = nextSelector;

                this.taskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);
            }

            /// <inheritdoc />
            public T Current { get; private set; }

            /// <inheritdoc />
            object IEnumerator.Current => this.Current;

            /// <inheritdoc />
            public void Dispose()
            {
                // Nothing to do here.
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                if (this.next is null)
                {
                    return false;
                }

                this.Current = this.taskFactory.StartNew(() => this.next.GetJsonAsync<T>())
                    .Unwrap()
                    .GetAwaiter()
                    .GetResult();

                this.next = new FlurlRequest(this.nextSelector(Current));

                return true;
            }

            /// <inheritdoc />
            public void Reset() =>
                throw new NotSupportedException("This enumerator cannot be reset.");
        }
    }
}
