using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ku.Main.Commons
{
    public interface IExpressionsService : IService
    {
        void SetValue<T>(Expression<Func<T>> destinationProperty, T newValue);
        T GetValue<T>(Expression<Func<T>> sourceProperty);
    }
}
