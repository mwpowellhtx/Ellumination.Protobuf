// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    internal static class Characters
    {
        /// <summary>
        /// &apos;.&apos;
        /// </summary>
        public const char Dot = '.';

        /// <summary>
        /// &apos;\n&apos;
        /// </summary>
        public const char NewLine = '\n';

        /// <summary>
        /// &apos;\r&apos;
        /// </summary>
        public const char CarriageReturn = '\r';

        /// <summary>
        /// ,
        /// </summary>
        public const char Comma = ',';

        /// <summary>
        /// &apos; &apos;
        /// </summary>
        public const char Space = ' ';

        /// <summary>
        /// &apos;\&apos;&apos;
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
        /// Gets the Open Curly Brace, &apos;{&apos;.
        /// </summary>
        /// <see cref="CurlyBraces"/>
        public static char OpenCurlyBrace => CurlyBraces[0];

        /// <summary>
        /// Gets the Close Curly Brace, &apos;}&apos;.
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

        private const string AngleBrackets = "<>";

        /// <summary>
        /// Gets the Open Angle Bracket.
        /// </summary>
        /// <see cref="AngleBrackets"/>
        public static char OpenAngleBracket => AngleBrackets[0];

        /// <summary>
        /// Gets the Close Angle Bracket.
        /// </summary>
        /// <see cref="AngleBrackets"/>
        public static char CloseAngleBracket => AngleBrackets[1];
    }
}
