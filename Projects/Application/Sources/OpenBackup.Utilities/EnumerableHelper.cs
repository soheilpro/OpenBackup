using System;
using System.Collections.Generic;

namespace OpenBackup
{
    public static class EnumerableHelper
    {
        public static IEnumerable<T> Catch<T>(this IEnumerable<T> enumerable, ExceptionHandler exceptionHandler)
        {
            return new CatchableEnumerable<T>(enumerable, exceptionHandler);
        }
    }
}
