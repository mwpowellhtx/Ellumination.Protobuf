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
    /// <inheritdoc cref="DescriptorBase" />
    public class ExtendStatement
        : DescriptorBase
            , IExtendStatement
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

        // TODO: TBD: MessageTypeName would go better as a proper ElementType, including instructions re: Leading Dot.
        /// <inheritdoc />
        /// <see cref="!:http://developers.google.com/protocol-buffers/docs/reference/proto2-spec#extend"/>
        public override string ToDescriptorString(IStringRenderingOptions options)
            => $"extend {MessageType.ToDescriptorString(options)}"
               + $" {{ {Join(Empty, Items.Select(x => $"{x.ToDescriptorString(options)} "))}}}";
    }
}
