using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenBackup
{
    internal class CatchableEnumerator<T> : IEnumerator<T>
    {
        private IEnumerator<T> _enumerator;
        private ExceptionHandler _exceptionHandler;

        public T Current
        {
            get
            {
                return _enumerator.Current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return _enumerator.Current;
            }
        }

        public CatchableEnumerator(IEnumerator<T> enumerator, ExceptionHandler exceptionHandler)
        {
            _enumerator = enumerator;
            _exceptionHandler = exceptionHandler;
        }

        public void Dispose()
        {
            _enumerator.Dispose();
        }

        public bool MoveNext()
        {
            try
            {
                return _enumerator.MoveNext();
            }
            catch (Exception exception)
            {
                if (_exceptionHandler != null)
                    _exceptionHandler(exception);

                // TODO: Resume next?
                return false;
            }
        }

        public void Reset()
        {
            _enumerator.Reset();
        }
    }
}
