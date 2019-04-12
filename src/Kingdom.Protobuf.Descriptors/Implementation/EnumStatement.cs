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
    public class EnumStatement
        : DescriptorBase<Identifier>
            , IEnumStatement
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
        public EnumStatement()
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            : base(new Identifier { })
        {
        }

        private IList<IEnumBodyItem> _items;

        /// <inheritdoc />
        public IList<IEnumBodyItem> Items
        {
            get => _items ?? (_items = new BidirectionalList<IEnumBodyItem>(
                       onAdded: x => x.Parent = this, onRemoved: x => x.Parent = null)
                   );
            set
            {
                _items?.Clear();
                _items = new BidirectionalList<IEnumBodyItem>(
                    value ?? new List<IEnumBodyItem>().ToList()
                    , onAdded: x => x.Parent = this, onRemoved: x => x.Parent = null);
            }
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            const string @enum = nameof(@enum);

            // ReSharper disable once ImplicitlyCapturedClosure
            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            var lineSeparator = WithLineSeparatorCarriageReturnNewLine.RenderLineSeparator();

            string RenderItems()
                => Items.Any()
                    ? $"{Join($"{lineSeparator}", Items.Select(x => x.ToDescriptorString(options)))}"
                    : Empty;

            return $" {GetComments(MultiLineComment)}"
                   + $" {@enum}"
                   + $" {GetComments(MultiLineComment)}"
                   + $" {Name.ToDescriptorString(options)}"
                   + $" {GetComments(MultiLineComment)}"
                   + $" {OpenCurlyBrace} {GetComments(MultiLineComment, SingleLineComment)}{lineSeparator}"
                   + $" {GetComments(MultiLineComment)}"
                   + $" {RenderItems()}"
                   + $" {GetComments(MultiLineComment)}{lineSeparator}"
                   + $" {CloseCurlyBrace}{GetComments(MultiLineComment, SingleLineComment)}"
                ;
        }
    }
}
