using Ku.Main.Commons;
using Ku.Main.Ioc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ku.Commons
{
    public class MessageHandlers : IMessageHandlers
    {
        class HandlerDeclaration
        {
            public Type Handler { get; set; }
            public Func<object, Guid> Function { get; set; }
        }
        private IInspectionService _inspectionService;

        public MessageHandlers(IInspectionService inspectionService)
        {
            _inspectionService = inspectionService;
        }

        private static Dictionary<Type, object> _handlerInstances = new Dictionary<Type, object>();
        private static Dictionary<Type, List<HandlerDeclaration>> _handlers = new Dictionary<Type, List<HandlerDeclaration>>();

        public void Initialize<THandlerBase, KMessageBase>(string methodName, Func<THandlerBase> factory)
        {
            var allImplementing = _inspectionService.GetTypes<THandlerBase>(InspectedType.Concrete);
            var messageType = typeof(KMessageBase);
            foreach (var item in allImplementing)
            {
                foreach (var meth in item.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(m => string.Compare(m.Name, methodName, true) == 0))
                {
                    var pars = meth.GetParameters();
                    var parType = pars.First().ParameterType;
                    if (pars.Count() == 1 && messageType.IsAssignableFrom(parType))
                    {
                        if (!_handlerInstances.ContainsKey(item))
                        {
                            _handlerInstances[item] = factory();
                        }
                        if (!_handlers.ContainsKey(parType))
                        {
                            _handlers[parType] = new List<HandlerDeclaration>();
                        }
                        var hd = new HandlerDeclaration
                        {
                            Handler = item,
                            Function = (a) =>
                            {
                                var msg = Convert.ChangeType(a, parType);
                                var result = meth.Invoke(_handlerInstances[item], new object[] { msg });
                                return (Guid)result;
                            }
                        };
                        _handlers[parType].Add(hd);
                    }
                }
            }
        }

        public IEnumerable<Func<T, Guid>> GetHandlers<T>()
        {
            var messageType = typeof(T);
            if (_handlers.ContainsKey(messageType))
            {
                if (_handlers[messageType].Count == 1)
                {
                    yield return new Func<T, Guid>((a) => _handlers[messageType][0].Function(a));
                }
                else
                {
                    var funcs = new List<HandlerDeclaration>(_handlers[messageType]);
                    foreach (var func in funcs)
                    {
                        yield return new Func<T, Guid>((a) => func.Function(a));
                    }
                }
            }
        }
    }
}
