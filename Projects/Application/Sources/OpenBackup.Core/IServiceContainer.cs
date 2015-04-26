using System;

namespace OpenBackup
{
    public interface IServiceContainer
    {
        T Get<T>();

        T[] GetAll<T>();
    }
}
