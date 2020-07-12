using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Collections.Generic;
    using static Characters;
    using static String;
    using static WhiteSpaceAndCommentOption;

    /// <inheritdoc cref="DescriptorBase{T}" />
    public class OneOfStatement
        : DescriptorBase<Identifier>
            , IOneOfStatement
            , IMessageBodyItem
    {
        IMessageBodyParent IHasParent<IMessageBodyParent>.Parent
        {
            get => Parent as IMessageBodyParent;
            set => Parent = value;
        }

        /// <inheritdoc />
        public OneOfStatement()
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            Name = new Identifier { };
        }

        private IList<IOneOfBodyItem> _items = GetRange<IOneOfBodyItem>().ToList();

        private IList<IOneOfBodyItem> _bidiItems;

        /// <inheritdoc />
        public IList<IOneOfBodyItem> Items
        {

#pragma warning disable IDE0074 // Use compound assignment
            get => _bidiItems ?? (_bidiItems = _items.ToBidirectionalList(
                onAdded: x => x.Parent = this, onRemoved: x => x.Parent = null)
            );
#pragma warning restore IDE0074 // Use compound assignment

            set
            {
                Items?.Clear();
                _items = (_items ?? GetRange<IOneOfBodyItem>()).ToList();
            }
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            // ReSharper disable once IdentifierTypo
            const string oneof = nameof(oneof);

            var lineSeparator = WithLineSeparatorCarriageReturnNewLine.RenderLineSeparator();

            // ReSharper disable once ImplicitlyCapturedClosure
            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            string GetRenderedItems() => Join(
                $"{GetComments(MultiLineComment, SingleLineComment)}{lineSeparator} "
                , Items.Select(x => x.ToDescriptorString(options))
            );

            return $" {GetComments(MultiLineComment)}"
                   + $" {oneof}"
                   + $" {GetComments(MultiLineComment)}"
                   + $" {Name.ToDescriptorString(options)}"
                   + $" {GetComments(MultiLineComment)}"
                   + $" {OpenCurlyBrace}{GetComments(MultiLineComment, SingleLineComment)}{lineSeparator}"
                   + $" {GetRenderedItems()}{GetComments(MultiLineComment, SingleLineComment)}{lineSeparator}"
                   + $" {CloseCurlyBrace}"
                ;
        }
    }
}
