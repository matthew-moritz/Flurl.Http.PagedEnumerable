using System;
using System.Collections;
using System.Collections.Generic;

namespace Flurl.Http.PagedEnumerable
{
    public sealed class PagedEnumerable<T> : IEnumerable<T>
    {
        private readonly IFlurlRequest request;
        private readonly Func<T, Url> nextSelector;

        public PagedEnumerable(IFlurlRequest request, Func<T, Url> nextSelector)
        {
            this.request = request;
            this.nextSelector = nextSelector;
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() =>
            new PagedEnumerator(this.request, this.nextSelector);

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class PagedEnumerator : IEnumerator<T>
        {
            private IFlurlRequest next;
            private readonly Func<T, Url> nextSelector;

            public PagedEnumerator(IFlurlRequest next, Func<T, Url> nextSelector)
            {
                this.next = next;
                this.nextSelector = nextSelector;
            }

            public T Current { get; private set; }

            object IEnumerator.Current => this.Current;

            public void Dispose()
            {
                // Nothing to do here.
            }

            public bool MoveNext()
            {
                if(this.next is null)
                {
                    return false;
                }

                this.Current = this.next.GetJsonAsync<T>().Result;
                this.next = new FlurlRequest(this.nextSelector(Current));

                return true;
            }

            public void Reset()
            {
                throw new InvalidOperationException("This enumerator cannot be reset.");
            }
        }
    }
}
