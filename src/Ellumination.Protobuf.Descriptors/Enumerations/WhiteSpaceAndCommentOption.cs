using System;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <summary>
    /// Several Comment rendering Options. Represents basically two intersecting dynamics.
    /// First, comments. Second, white space. Not necessarily in that order, but both playing
    /// a complementary role alongside one another. A best effort is made in order to accept
    /// the furnished actual options at the time of string rendering in order to accommodate
    /// the options.
    /// </summary>
    [Flags]
    public enum WhiteSpaceAndCommentOption
    {
        /// <summary>
        /// 0
        /// </summary>
        NoWhiteSpaceOrCommentOption = 0,

        /// <summary>
        /// 1
        /// </summary>
        CommentBefore = 1,

        /// <summary>
        /// 1 &lt;&lt; 1
        /// </summary>
        CommentAfter = 1 << 1,

        /// <summary>
        /// 1 &lt;&lt; 2
        /// </summary>
        CommentSameLine = 1 << 2,

        /// <summary>
        /// 1 &lt;&lt; 3, Embeds Comments between Elements such as Keywords. Comments involving
        /// <see cref="MultiLineComment"/> are no problem. However, comments involving
        /// <see cref="SingleLineComment"/> may be Embedded, which should yield Line Separators.
        /// </summary>
        EmbeddedComments = 1 << 3,

        /// <summary>
        /// 1 &lt;&lt; 4, implies Line Separator normalization.
        /// </summary>
        WithLineSeparatorNewLine = 1 << 4,

        /// <summary>
        /// 1 &lt;&lt; 5, implies Line Separator normalization.
        /// </summary>
        WithLineSeparatorCarriageReturnNewLine = 1 << 5,

        /// <summary>
        /// 1 &lt;&lt; 6
        /// </summary>
        SingleLineComment = 1 << 6,

        /// <summary>
        /// 1 &lt;&lt; 7
        /// </summary>
        MultiLineComment = 1 << 7,

        /// <summary>
        /// Single or Multi Line Comment Mask.
        /// </summary>
        /// <see cref="SingleLineComment"/>
        /// <see cref="MultiLineComment"/>
        CommentLinesMask = SingleLineComment | MultiLineComment,

        /// <summary>
        /// New Line or Carriage Return New Line Normalize Mask.
        /// </summary>
        /// <see cref="WithLineSeparatorNewLine"/>
        /// <see cref="WithLineSeparatorCarriageReturnNewLine"/>
        WithLineSeparatorMask = WithLineSeparatorNewLine | WithLineSeparatorCarriageReturnNewLine,

        /// <summary>
        /// Comment Position Mask.
        /// </summary>
        /// <see cref="CommentBefore"/>
        /// <see cref="CommentAfter"/>
        /// <see cref="EmbeddedComments"/>
        /// <see cref="CommentSameLine"/>
        CommentPositionMask = CommentBefore | CommentAfter | EmbeddedComments | CommentSameLine
    }
}
