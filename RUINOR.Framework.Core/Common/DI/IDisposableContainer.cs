using System;

namespace RUINOR.Framework.Core.Common.DI
{

    public interface IDisposableContainer : IDisposable
    {
        void AddDisposableObj(IDisposable disposableObj);
    }

}