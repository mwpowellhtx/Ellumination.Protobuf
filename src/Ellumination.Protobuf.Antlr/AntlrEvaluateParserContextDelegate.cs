// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// <typeparamref name="TContext"/> Evaluate Callback delegate.
    /// </summary>
    /// <typeparam name="TParser"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <param name="parserInstance"></param>
    /// <returns></returns>
    public delegate TContext AntlrEvaluateParserContextDelegate<in TParser, out TContext>(TParser parserInstance)
        where TParser : Parser
        where TContext : class, IParseTree;
}
