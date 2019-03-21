// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit;
    using static SyntaxKind;

    internal static class Statements
    {
        /// <summary>
        /// Provides a convenience <see cref="SyntaxStatement"/> instance factory.
        /// </summary>
        /// <see cref="Proto2"/>
        internal static SyntaxStatement NewSyntaxStatement
        {
            get
            {
                var result = new SyntaxStatement {Syntax = Proto2};
                Assert.Equal(Proto2, result.Syntax);
                return result;
            }
        }

        /// <summary>
        /// Provides a convenience <see cref="EmptyStatement"/> instance factory.
        /// </summary>
        internal static EmptyStatement NewEmptyStatement => new EmptyStatement { };
    }
}
