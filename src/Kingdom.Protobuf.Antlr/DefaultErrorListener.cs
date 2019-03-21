using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Antlr4.Runtime;

    /// <inheritdoc />
    public class DefaultErrorListener : BaseErrorListener
    {
        /// <inheritdoc />
        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol
            , int line, int column, string msg, RecognitionException rex)
        {
            var ex = new InvalidOperationException(
                $"line {offendingSymbol.Line}, column {offendingSymbol.Column}"
                + $", symbol '{offendingSymbol.Text}': {msg}"
            );

            ex.Data[nameof(line)] = line;
            ex.Data[nameof(column)] = column;
            ex.Data[nameof(msg)] = msg;
            ex.Data[nameof(RecognitionException)] = rex;

            throw ex;
        }
    }
}
