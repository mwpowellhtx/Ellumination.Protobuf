using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Collections.Generic;
    using static String;
    using static WhiteSpaceAndCommentOption;

    // TODO: TBD: remember also Dependencies...
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc cref="DescriptorBase" />
    public class ProtoDescriptor
        : DescriptorBase
            , IProto
    {
        private SyntaxStatement _syntax;

        /// <summary>
        /// 
        /// </summary>
        public SyntaxStatement Syntax
        {
            get => _syntax;
            set => this.ReplaceItemParent(ref _syntax, value
                , (item, parent) => item.Parent = parent);
        }

        private IList<IProtoBodyItem> _items = GetRange<IProtoBodyItem>().ToList();

        private IList<IProtoBodyItem> _bidiItems;

        /// <inheritdoc />
        public IList<IProtoBodyItem> Items
        {

#pragma warning disable IDE0074 // Use compound assignment
            get => _bidiItems ?? (_bidiItems = _items.ToBidirectionalList(
                onAdded: x => x.Parent = this, onRemoved: x => x.Parent = null)
            );
#pragma warning restore IDE0074 // Use compound assignment

            set
            {
                Items?.Clear();
                _items = (value ?? GetRange<IProtoBodyItem>()).ToList();
                _bidiItems = null;
            }
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            var itemSeparator = WithLineSeparatorCarriageReturnNewLine.RenderLineSeparator();

            return Join(itemSeparator
                , GetRange<ICanRenderString>(Syntax)
                    .Concat(Items.ToArray<ICanRenderString>())
                    .Select(x => x.ToDescriptorString(options))
            );
        }
    }
}
