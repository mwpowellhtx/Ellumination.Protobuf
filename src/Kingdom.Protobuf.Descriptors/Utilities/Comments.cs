using System;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using TOption = WhiteSpaceAndCommentOption;
    using static Characters;
    using static Guid;
    using static WhiteSpaceAndCommentOption;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>We could go [string.]Empty here, but we already invoke Guid[.Empty], so there
    /// is a potential collision.</remarks>
    internal static class Comments
    {
        private static Guid SampleGuid => NewGuid();

        public static string RenderLineSeparator(this TOption options)
            => options.Intersects(WithLineSeparatorNewLine)
                ? $"{NewLine}"
                : options.Intersects(WithLineSeparatorCarriageReturnNewLine)
                    ? $"{CarriageReturn}{NewLine}"
                    : "";

        private static string RenderSingleLineComment(this TOption options, bool silent = true)
        {
            if (options.Intersects(SingleLineComment)
                && options.Intersects(EmbeddedComments)
                && options.Intersects(CommentSameLine))
            {
                if (silent)
                {
                    return "";
                }

                throw new InvalidOperationException("Cannot support Single Same Line Embedded Comments.");
            }

            return options.Intersects(SingleLineComment)
                ? $"// {SampleGuid:D}"
                : "";
        }

        private static string RenderMultiLineComment(this TOption options)
            => options.Intersects(MultiLineComment)
                ? options.Intersects(EmbeddedComments)
                    ? $"/* {SampleGuid:D} */"
                    : $"/* {SampleGuid:D}{options.RenderLineSeparator()} * {SampleGuid:D} */"
                : "";

        public static string RenderComments(this TOption options, TOption commentMask)
        {
            if (options == WithLineSeparatorMask)
            {
                throw new InvalidOperationException(
                    "Cannot specify both New Line and Carriage Return New Line rendering options."
                );
            }

            return options.Intersects(commentMask)
                ? $"{options.RenderMultiLineComment()} {options.RenderSingleLineComment()}"
                : "";
        }

        /// <summary>
        /// Renders Comments based on the <paramref name="option"/> and <paramref name="masks"/>.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="masks"></param>
        /// <returns></returns>
        public static string RenderMaskedComments(this TOption option
            , params TOption[] masks)
            => string.Join($"{Space}", masks.Select(x => option.RenderComments(x)));
    }
}
