using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Collections;
    using static String;

    /// <inheritdoc cref="DescriptorBase{T}" />
    public class OneOfStatement
        : DescriptorBase<Identifier>
            , IOneOfStatement
            , IMessageBodyItem
    {
        private IParentItem _parent;

        IMessageBodyParent IHasParent<IMessageBodyParent>.Parent
        {
            get => _parent as IMessageBodyParent;
            set => _parent = value;
        }

        /// <inheritdoc />
        public OneOfStatement()
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            Name = new Identifier { };
        }

        private IList<IOneOfBodyItem> _items;

        /// <inheritdoc />
        public IList<IOneOfBodyItem> Items
        {
            get => _items ?? (_items = new BidirectionalList<IOneOfBodyItem>(
                       onAdded: x => x.Parent = this, onRemoved: x => x.Parent = null)
                   );
            set
            {
                _items?.Clear();
                _items = new BidirectionalList<IOneOfBodyItem>(
                    value ?? GetRange<IOneOfBodyItem>().ToList()
                    , onAdded: x => x.Parent = this, onRemoved: x => x.Parent = null
                );
            }
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            string RenderItems() => Join(" ", Items.Select(x => x.ToDescriptorString(options)));
            // ReSharper disable once IdentifierTypo
            const string oneof = nameof(oneof);
            return $"{oneof} {Name.ToDescriptorString(options)} {{ {RenderItems()}}}";
        }
    }
}
