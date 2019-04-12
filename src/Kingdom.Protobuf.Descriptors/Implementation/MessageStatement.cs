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
    /// <inheritdoc cref="DescriptorBase{T}" />
    public class MessageStatement
        : DescriptorBase<Identifier>
            , IMessageStatement
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

        /// <inheritdoc />
        public MessageStatement()
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            Name = new Identifier { };
        }

        private IList<IMessageBodyItem> _items;

        /// <inheritdoc />
        public IList<IMessageBodyItem> Items
        {
            get => _items ?? (_items = new BidirectionalList<IMessageBodyItem>(
                       onAdded: x => x.Parent = this, onRemoved: x => x.Parent = null)
                   );
            set
            {
                _items?.Clear();
                _items = new BidirectionalList<IMessageBodyItem>(
                    value ?? GetRange<IMessageBodyItem>().ToList()
                    , onAdded: x => x.Parent = this, onRemoved: x => x.Parent = null
                );
            }
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            const string message = nameof(message);

            var lineSeparator = WithLineSeparatorCarriageReturnNewLine.RenderLineSeparator();

            // ReSharper disable once ImplicitlyCapturedClosure
            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            string GetRenderedItems() => Join(
                $"{GetComments(MultiLineComment, SingleLineComment)}{lineSeparator}"
                , Items.Select(x => $"{x.ToDescriptorString(options)}")
            );

            return $" {GetComments(MultiLineComment)}"
                   + $" {message}"
                   + $" {GetComments(MultiLineComment)}"
                   + $" {Name.ToDescriptorString(options)}"
                   + $" {GetComments(MultiLineComment)}"
                   + $" {OpenCurlyBrace}{GetComments(MultiLineComment, SingleLineComment)}{lineSeparator}"
                   + $" {GetRenderedItems()}{GetComments(MultiLineComment, SingleLineComment)}{lineSeparator}"
                   + $" {CloseCurlyBrace}{GetComments(MultiLineComment, SingleLineComment)}"
                ;
        }
    }
}
