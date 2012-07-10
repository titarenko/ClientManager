using System;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    /// <summary>
    /// implements extension metods for safe chained calls
    /// for more information see http://devtalk.net/csharp/chained-null-checks-and-the-maybe-monad/
    /// </summary>
    public static class SafeExtensions
    {
        public static TResult SafeWith<TInput, TResult>(this TInput inputObject, Func<TInput, TResult> evaluator)
            where TResult : class where TInput : class
        {
            return (null == inputObject) ? null : evaluator(inputObject);
        }

        public static TResult SafeGet<TInput, TResult>(this TInput inputObject, Func<TInput, TResult> evaluator, TResult defaultValue)
            where TInput : class
        {
            return (null == inputObject) ? defaultValue : evaluator(inputObject);
        }
    }
}
