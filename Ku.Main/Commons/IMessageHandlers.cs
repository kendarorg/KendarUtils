using Ku.Main;
using System;
using System.Collections.Generic;
namespace Ku.Commons
{
    public interface IMessageHandlers : IService
    {
        IEnumerable<Func<T, Guid>> GetHandlers<T>();
        void Initialize<THandlerBase, KMessageBase>(string methodName, Func<THandlerBase> factory);
    }
}
