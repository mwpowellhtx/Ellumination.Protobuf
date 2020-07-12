// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TListener"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="listener"></param>
    /// <returns></returns>
    public delegate TResult ParserListenerDelegate<in TListener, out TResult>(TListener listener)
        where TListener : class, IParseTreeListener;
}
