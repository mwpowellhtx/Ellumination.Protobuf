// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using static Objects;

    /// <summary>
    /// 
    /// </summary>
    public static class WalkableAntlrParserExtensionMethods
    {
        /// <summary>
        /// Returns the <typeparamref name="TListener"/> after Walking the
        /// <typeparamref name="TContext"/> during Parsing.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TStream"></typeparam>
        /// <typeparam name="TParser"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TListener"></typeparam>
        /// <param name="inputText"></param>
        /// <param name="evaluateCallback"></param>
        /// <param name="errorListeners"></param>
        /// <returns></returns>
        public static TListener WalkEvaluatedContext<TSource, TStream, TParser, TContext, TListener>(this string inputText
            , AntlrEvaluateParserContextDelegate<TParser, TContext> evaluateCallback, params IParserErrorListener[] errorListeners)
            where TSource : class, ITokenSource
            where TStream : class, ITokenStream
            where TParser : Parser
            where TContext : class, IParseTree
            where TListener : class, IParseTreeListener
        {
            evaluateCallback = evaluateCallback.VerifyNotNull();

            var context = inputText.Evaluate<TSource, TStream, TParser, TContext>(evaluateCallback, errorListeners);

            var listener = Construct<TListener>().VerifyNotNull();

            Construct<ParseTreeWalker>().VerifyNotNull().Walk(listener, context);

            return listener;
        }
    }
}
