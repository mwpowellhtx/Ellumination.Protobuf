using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Collections;
    using static String;
    using static LabelKind;

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc cref="FieldStatementBase{T}" />
    public class GroupFieldStatement
        : FieldStatementBase<Identifier>
            , IGroupFieldStatement
            , IMessageBodyItem
            , IExtendBodyItem
    {
        IMessageBodyParent IHasParent<IMessageBodyParent>.Parent
        {
            get => Parent as IMessageBodyParent;
            set => Parent = value;
        }

        IExtendStatement IHasParent<IExtendStatement>.Parent
        {
            get => Parent as IExtendStatement;
            set => Parent = value;
        }

        /// <inheritdoc />
        public LabelKind Label { get; set; } = Optional;

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
            IEnumerable<string> GetRenderedItems() => Items.Select(x => x.ToDescriptorString(options));
            string GetRenderedItemString() => GetRenderedItems().Aggregate(Empty, (g, x) => g + $"{x} ");

            const string group = nameof(group);

            return $"{Label.ToDescriptorString(options)} {group} {Name.ToDescriptorString(options)}"
                   + $" = {Number.RenderLong(options.IntegerRendering)} {{ {GetRenderedItemString()}}}";
        }
    }
}
