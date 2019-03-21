using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Collections;

    /// <inheritdoc cref="FieldStatementBase" />
    public class OneOfFieldStatement
        : FieldStatementBase<Identifier>
            , IOneOfFieldStatement
            , IOneOfBodyItem
    {
        IOneOfStatement IHasParent<IOneOfStatement>.Parent
        {
            get => Parent as IOneOfStatement;
            set => Parent = value;
        }

        /// <inheritdoc />
        public IVariant FieldType { get; set; }

        private IList<FieldOption> _options;

        /// <inheritdoc />
        public IList<FieldOption> Options
        {
            get => _options ?? (_options = new BidirectionalList<FieldOption>(
                       onAdded: x => ((IHasParent<IOneOfFieldStatement>) x).Parent = this
                       , onRemoved: x => ((IHasParent<IOneOfFieldStatement>) x).Parent = null)
                   );
            set
            {
                _options?.Clear();
                _options = new BidirectionalList<FieldOption>(
                    value ?? GetRange<FieldOption>().ToList()
                    , onAdded: x => ((IHasParent<IOneOfFieldStatement>) x).Parent = this
                    , onRemoved: x => ((IHasParent<IOneOfFieldStatement>) x).Parent = null
                );
            }
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
            => $"{FieldType.ToDescriptorString(options)} {Name.ToDescriptorString(options)}"
               + $" = {Number.RenderLong(options.IntegerRendering)}{Options.RenderOptions(options)};";
    }
}
