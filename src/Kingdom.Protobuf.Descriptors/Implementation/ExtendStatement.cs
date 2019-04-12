using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Collections;
    using static Characters;
    using static String;
    using static WhiteSpaceAndCommentOption;

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc cref="DescriptorBase" />
    public class ExtendStatement
        : DescriptorBase
            , IExtendStatement
            , ITopLevelDefinition
            , IMessageBodyItem
    {
        IProto IHasParent<IProto>.Parent
        {
            get => Parent as IProto;
            set => Parent = value;
        }

        IMessageBodyParent IHasParent<IMessageBodyParent>.Parent
        {
            get => Parent as IMessageBodyParent;
            set => Parent = value;
        }

        /// <summary>
        /// Gets or sets the MessageTypeName. The Extend Statement wants a Message Type, however,
        /// we cannot differentiate Message from Enum Type from a grammatical perspective. This
        /// level of connection must wait for a second pass linkage.
        /// </summary>
        public ElementTypeIdentifierPath MessageType { get; set; } = new ElementTypeIdentifierPath();

        private IList<IExtendBodyItem> _items;

        /// <inheritdoc />
        public IList<IExtendBodyItem> Items
        {
            get => _items ?? (_items = new BidirectionalList<IExtendBodyItem>(
                       onAdded: x => x.Parent = this, onRemoved: x => x.Parent = null)
                   );
            set
            {
                _items?.Clear();
                _items = new BidirectionalList<IExtendBodyItem>(
                    value ?? GetRange<IExtendBodyItem>().ToList()
                    , onAdded: x => x.Parent = this, onRemoved: x => x.Parent = null
                );
            }
        }

        /// <inheritdoc />
        /// <see cref="!:http://developers.google.com/protocol-buffers/docs/reference/proto2-spec#extend"/>
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            // ReSharper disable once ImplicitlyCapturedClosure
            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            var lineSeparator = WithLineSeparatorCarriageReturnNewLine.RenderLineSeparator();

            string RenderItems()
                => Join(lineSeparator
                    , Items.Select(item
                        => $"{GetComments(MultiLineComment)}"
                           + $" {item.ToDescriptorString(options)} "
                           + $"{GetComments(MultiLineComment, SingleLineComment)} "
                    )
                );

            const string extend = nameof(extend);

            return $"{GetComments(MultiLineComment)}"
                   + $" {extend}"
                   + $" {GetComments(MultiLineComment)}"
                   + $" {MessageType.ToDescriptorString(options)}"
                   + $" {GetComments(MultiLineComment)}"
                   + $" {OpenCurlyBrace}{GetComments(SingleLineComment)}{lineSeparator}"
                   + $" {GetComments(MultiLineComment)}"
                   + $" {RenderItems()}{GetComments(SingleLineComment)}{lineSeparator}"
                   + $" {GetComments(MultiLineComment)}"
                   + $" {CloseCurlyBrace}"
                   + $" {GetComments(MultiLineComment, SingleLineComment)}"
                ;
        }
    }
}
