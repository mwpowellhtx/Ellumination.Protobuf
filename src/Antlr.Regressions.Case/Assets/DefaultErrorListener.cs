using System;

namespace Kingdom.Antlr.Regressions.Case
{
    using Antlr4.Runtime;

    /// <inheritdoc />
    public class DefaultErrorListener : BaseErrorListener
    {
        // TODO: TBD: was throwing exception in the more complex grammar use case...
        // TODO: TBD: rather, here the lexer is "working", but is skipping over the leading lowercase characters...
        // TODO: TBD: before it discovers the first UpperCase (or at least CamelCase) character...
        /// <inheritdoc />
        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol
            , int line, int charPositionInLine, string msg, RecognitionException ex)
            => throw new InvalidOperationException(
                $"line {offendingSymbol.Line}, column {offendingSymbol.Column}"
                + $", symbol '{offendingSymbol.Text}': {msg}"
            );
    }
}
