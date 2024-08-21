using System;

namespace RUINORERP.Common.DI
{

    public interface IDisposableContainer : IDisposable
    {
        void AddDisposableObj(IDisposable disposableObj);
    }

}