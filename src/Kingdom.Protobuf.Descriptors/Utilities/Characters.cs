// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    internal static class Characters
    {
        /// <summary>
        /// &apos;
        /// </summary>
        private const char Tick = '\'';

        /// <summary>
        /// <see cref="Tick"/>
        /// </summary>
        public static char OpenTick => Tick;

        /// <summary>
        /// <see cref="Tick"/>
        /// </summary>
        public static char CloseTick => Tick;

        /// <summary>
        /// &quot;
        /// </summary>
        private const char Quote = '"';

        /// <summary>
        /// <see cref="Quote"/>
        /// </summary>
        public static char OpenQuote => Quote;

        /// <summary>
        /// <see cref="Quote"/>
        /// </summary>
        public static char CloseQuote => Quote;

        /// <summary>
        /// ;
        /// </summary>
        public const char SemiColon = ';';

        /// <summary>
        /// =
        /// </summary>
        public const char EqualSign = '=';

        private const string CurlyBraces = "{}";

        /// <summary>
        /// Gets the Open Curly Brace.
        /// </summary>
        /// <see cref="CurlyBraces"/>
        public static char OpenCurlyBrace => CurlyBraces[0];

        /// <summary>
        /// Gets the Close Curly Brace.
        /// </summary>
        /// <see cref="CurlyBraces"/>
        public static char CloseCurlyBrace => CurlyBraces[1];

        private const string SquareBrackets = "[]";

        /// <summary>
        /// Gets the Open Square Bracket.
        /// </summary>
        /// <see cref="SquareBrackets"/>
        public static char OpenSquareBracket => SquareBrackets[0];

        /// <summary>
        /// Gets the Close Square Bracket.
        /// </summary>
        /// <see cref="SquareBrackets"/>
        public static char CloseSquareBracket => SquareBrackets[1];
    }
}
