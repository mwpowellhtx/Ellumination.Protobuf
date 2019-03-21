using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Collections;

    /// <inheritdoc cref="FieldStatementBase{T}" />
    public class MapFieldStatement
        : FieldStatementBase<Identifier>
            , IMapFieldStatement
            , IMessageBodyItem
    {
        IMessageBodyParent IHasParent<IMessageBodyParent>.Parent
        {
            get => Parent as IMessageBodyParent;
            set => Parent = value;
        }

        /// <inheritdoc />
        public KeyType KeyType { get; set; }

        /// <inheritdoc />
        public IVariant ValueType { get; set; }

        private IList<FieldOption> _options;

        /// <inheritdoc />
        public IList<FieldOption> Options
        {
            get => _options ?? (_options = new BidirectionalList<FieldOption>(
                       onAdded: x => ((IHasParent<IMapFieldStatement>) x).Parent = this
                       , onRemoved: x => ((IHasParent<IMapFieldStatement>) x).Parent = null)
                   );
            set
            {
                _options?.Clear();
                _options = new BidirectionalList<FieldOption>(
                    value ?? GetRange<FieldOption>().ToList()
                    , onAdded: x => ((IHasParent<IMapFieldStatement>) x).Parent = this
                    , onRemoved: x => ((IHasParent<IMapFieldStatement>) x).Parent = null
                );
            }
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            return $"map<{KeyType.ToDescriptorString(options)}, {ValueType.ToDescriptorString(options)}> "
                   + $"{Name.ToDescriptorString(options)} = {Number.RenderLong(options.IntegerRendering)}"
                   + $"{Options.RenderOptions(options)};"
                ;
        }
    }
}
