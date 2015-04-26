using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenBackup
{
    internal class CatchableEnumerable<T> : IEnumerable<T>
    {
        private IEnumerable<T> _enumerable;
        private ExceptionHandler _exceptionHandler;

        public CatchableEnumerable(IEnumerable<T> enumerable, ExceptionHandler exceptionHandler)
        {
            _enumerable = enumerable;
            _exceptionHandler = exceptionHandler;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new CatchableEnumerator<T>(_enumerable.GetEnumerator(), _exceptionHandler);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CatchableEnumerator<T>(_enumerable.GetEnumerator(), _exceptionHandler);
        }
    }
}
