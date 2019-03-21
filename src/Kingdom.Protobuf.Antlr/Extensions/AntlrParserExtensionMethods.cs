using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using static Objects;
    using static String;

    /// <summary>
    /// Provides the core methods required to facilitate Antlr Parser evaluation.
    /// </summary>
    /// <remarks>Note, there is not enough here to justify something like
    /// a <see cref="IDisposable"/> context. </remarks>
    public static class AntlrParserExtensionMethods
    {
        /// <summary>
        /// Evaluates the <paramref name="inputText"/> given a <paramref name="contextFactory"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TStream"></typeparam>
        /// <typeparam name="TParser"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="inputText"></param>
        /// <param name="contextFactory"></param>
        /// <param name="errorListeners"></param>
        /// <returns></returns>
        public static TContext Evaluate<TSource, TStream, TParser, TContext>(this string inputText
            , AntlrEvaluateParserContextDelegate<TParser, TContext> contextFactory
            , params IParserErrorListener[] errorListeners)
            where TSource : class, ITokenSource
            where TStream : class, ITokenStream
            where TParser : Parser
            where TContext : class, IParseTree
        {
            contextFactory = contextFactory.VerifyNotNull();

            var inputStream = Construct<AntlrInputStream>((inputText ?? Empty).ToArgument()).VerifyNotNull();

            var inputSource = Construct<TSource>(inputStream.ToArgument<ICharStream>()).VerifyNotNull();

            var tokenStream = Construct<TStream>(inputSource.ToArgument()).VerifyNotNull();

            var parser = Construct<TParser>(tokenStream.ToArgument()).VerifyNotNull();

            // TODO: TBD: do we need to add tree listeners?
            foreach (var errorListener in errorListeners)
            {
                parser.AddErrorListener(errorListener);
            }

            return contextFactory(parser).VerifyNotNull();
        }
    }
}
