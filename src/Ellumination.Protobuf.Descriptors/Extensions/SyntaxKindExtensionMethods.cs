using System.Collections.Generic;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    internal static class SyntaxKindExtensionMethods
    {
        private static readonly IDictionary<string, SyntaxKind> Kinds
            = new Dictionary<string, SyntaxKind>
            {
                {$"{SyntaxKind.Proto2}".ToLower(), SyntaxKind.Proto2}
            };

        public static SyntaxKind ToSyntaxKind(this string s) => Kinds[s];
    }
}
