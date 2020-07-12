using System;

namespace Ellumination.Antlr.Regressions.Case
{
    using Xunit;

    internal static class ExceptionExtensionMethods
    {
        /// <summary>
        /// Verifies the <typeparamref name="T"/> <paramref name="exception"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exception"></param>
        /// <param name="verify"></param>
        /// <returns></returns>
        public static T VerifyException<T>(this T exception, Action<T> verify = null)
            where T : Exception
        {
            Assert.NotNull(exception);
            verify?.Invoke(exception);
            return exception;
        }
    }
}
