using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Collections;
    using static String;

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

        private IList<IProtoBodyItem> _items;

        /// <inheritdoc />
        public IList<IProtoBodyItem> Items
        {
            get => _items ?? (_items = new BidirectionalList<IProtoBodyItem>(
                       onAdded: x => x.Parent = this, onRemoved: x => x.Parent = null)
                   );
            set
            {
                _items?.Clear();
                _items = new BidirectionalList<IProtoBodyItem>(
                    value ?? GetRange<IProtoBodyItem>().ToList()
                    , onAdded: x => x.Parent = this, onRemoved: x => x.Parent = null
                );
            }
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
            => Join(Empty, GetRange<ICanRenderString>(Syntax)
                .Concat(Items.ToArray<ICanRenderString>())
                .Select(x => x.ToDescriptorString(options)));
    }
}
