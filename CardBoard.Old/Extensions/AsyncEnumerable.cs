using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardBoard
{
    public static class AsyncEnumerable
    {
        public static async Task<TResult> FirstOrDefaultAsync<TResult>(
            this IEnumerable<TResult> collection,
            Func<TResult, Task<bool>> predicate)
        {
            foreach (var item in collection)
                if (await predicate(item))
                    return item;

            return default(TResult);
        }
    }
}
