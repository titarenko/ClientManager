using System;
using System.Linq.Expressions;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    public static class Extension
    {
        public static string GetPath <T, TProperty> (this Expression<Func<T, TProperty>> expression)
        {
            var e = (MemberExpression)expression.Body;
            var stringified = e.ToString().Replace(".get_Item(0)", string.Empty);
            return stringified.Substring(stringified.IndexOf(".") + 1);
        }
    }
}
