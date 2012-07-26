using System;
using System.Linq.Expressions;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    /// <summary>
    /// implements extension metods for safe chained calls
    /// for more information see http://devtalk.net/csharp/chained-null-checks-and-the-maybe-monad/
    /// </summary>
    public static class SafeExtensions
    {
        /// <summary>
        /// Return property or default result of property type if input object is null and do not throw exception.
        /// <example>
        /// Inquiry inquiry=null;
        /// var id=inquiry.SafeGet(x=>x.Id); //returns null and do not throw exception
        /// </example>
        /// </summary>
        /// <param name="evaluator">lamda expression for getting property</param>
        public static TResult SafeGet<TInput, TResult>(this TInput inputObject, Expression<Func<TInput, TResult>> evaluator)
            where TInput : class
        {
            try
            {
                return evaluator.Compile().Invoke(inputObject);
            }
            catch(NullReferenceException)
            {
                return default(TResult);
            }
        }
    }
}