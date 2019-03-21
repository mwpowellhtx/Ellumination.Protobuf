using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Collections;
    using static String;

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
        // TODO: TBD: may refactor to base classes...
        private IParentItem _parent;

        IProto IHasParent<IProto>.Parent
        {
            get => _parent as IProto;
            set => _parent = value;
        }

        IMessageBodyParent IHasParent<IMessageBodyParent>.Parent
        {
            get => _parent as IMessageBodyParent;
            set => _parent = value;
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
            IEnumerable<string> GetRenderedItems() => Items.Select(x => $"{x.ToDescriptorString(options)}");
            string GetRenderedItemsString() => GetRenderedItems().Aggregate(Empty, (g, x) => g + $"{x} ");

            const string message = nameof(message);
            return $"{message} {Name.ToDescriptorString(options)} {{ {GetRenderedItemsString()}}}";
        }
    }
}
