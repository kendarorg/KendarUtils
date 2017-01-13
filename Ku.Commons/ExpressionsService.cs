using Ku.Main.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ku.Commons
{
    public class ExpressionsService : IExpressionsService
    {
        /// <summary>
        /// Given an expression of a property of type T set its value
        /// </summary>
        /// <typeparam name="T">The type of a.Property.Item.Value</typeparam>
        /// <param name="destinationProperty">The expression to reach the property e.g.:  ()=>a.Property.Item.Value</param>
        /// <param name="newValue">The new value for a.Property.Item.Value</param>
        public void SetValue<T>(Expression<Func<T>> destinationProperty, T newValue)
        {
            List<MemberInfo> members = new List<MemberInfo>();

            var expression = destinationProperty.Body;
            ConstantExpression baseExpression = null;

            while (expression != null)
            {
                var memberExpression = expression as MemberExpression;

                if (memberExpression != null)
                {
                    members.Add(memberExpression.Member);
                    expression = memberExpression.Expression;
                }
                else
                {
                    baseExpression = expression as ConstantExpression;

                    if (baseExpression == null)
                    {
                        throw new NotSupportedException();
                    }

                    break;
                }
            }

            if (members.Count == 0)
            {
                return;
            }

            object target = baseExpression.Value;

            for (int i = members.Count - 1; i >= 1; i--)
            {
                var propertyInfo = members[i] as PropertyInfo;

                if (propertyInfo != null)
                {
                    target = propertyInfo.GetValue(target);
                }
                else
                {
                    var fieldInfo = (FieldInfo)members[i];
                    target = fieldInfo.GetValue(target);
                }
            }

            var finalPropertyInfo = members[0] as PropertyInfo;

            if (finalPropertyInfo != null)
            {
                finalPropertyInfo.SetValue(target, newValue);
            }
            else
            {
                var finalFieldInfo = (FieldInfo)members[0];
                finalFieldInfo.SetValue(target, newValue);
            }
        }



        /// <summary>
        /// Given an expression of a property of type T sets its value
        /// </summary>
        /// <typeparam name="T">The type of the </typeparam>
        /// <param name="destinationProperty">The expression to reach the property e.g.:  ()=>a.Property.Item.Value</param>
        /// <param name="newValue">The new value for a.Property.Item.Value</param>

        /// <summary>
        /// Given an expression of a property of type T get its value
        /// </summary>
        /// <typeparam name="T">The type of a.Property.Item.Value</typeparam>
        /// <param name="sourceProperty">The expression to reach the property e.g.:  ()=>a.Property.Item.Value</param>
        /// <returns>null if something had not been found, </returns>
        public T GetValue<T>(Expression<Func<T>> sourceProperty)
        {
            if (sourceProperty == null) return default(T);
            List<MemberInfo> members = new List<MemberInfo>();

            var expression = sourceProperty.Body;
            ConstantExpression baseExpression = null;

            while (expression != null)
            {
                var memberExpression = expression as MemberExpression;

                if (memberExpression != null)
                {
                    members.Add(memberExpression.Member);
                    expression = memberExpression.Expression;
                }
                else
                {
                    baseExpression = expression as ConstantExpression;

                    if (baseExpression == null)
                    {
                        throw new NotSupportedException();
                    }

                    break;
                }
            }

            if (members.Count == 0)
            {
                return default(T);
            }

            object target = baseExpression.Value;

            for (int i = members.Count - 1; i >= 1; i--)
            {
                var propertyInfo = members[i] as PropertyInfo;
                if (target == null) return default(T);
                if (propertyInfo != null)
                {
                    target = propertyInfo.GetValue(target);
                }
                else
                {
                    var fieldInfo = (FieldInfo)members[i];
                    target = fieldInfo.GetValue(target);
                }
            }

            var finalPropertyInfo = members[0] as PropertyInfo;

            if (target == null)
            {
                return default(T);
            }
            if (finalPropertyInfo != null)
            {
                return (T)finalPropertyInfo.GetValue(target);
            }
            else
            {
                var finalFieldInfo = (FieldInfo)members[0];
                return (T)finalFieldInfo.GetValue(target);
            }
        }
    }
}
