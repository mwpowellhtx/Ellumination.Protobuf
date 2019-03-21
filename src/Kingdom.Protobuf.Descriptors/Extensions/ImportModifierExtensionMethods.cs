using System.Collections.Generic;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    internal static class ImportModifierExtensionMethods
    {
        private static readonly IDictionary<string, ImportModifierKind?> Kinds
            = new Dictionary<string, ImportModifierKind?>
            {
                {$"{ImportModifierKind.Weak}".ToLower(), ImportModifierKind.Weak},
                {$"{ImportModifierKind.Public}".ToLower(), ImportModifierKind.Public}
            };

        public static ImportModifierKind? ToImportModifier(this string s)
            => Kinds.ContainsKey(s) ? Kinds[s] : null;
    }
}
