using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Collections;
    using static LabelKind;

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc cref="FieldStatementBase" />
    public class NormalFieldStatement
        : FieldStatementBase<Identifier>
            , INormalFieldStatement
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

        /// <inheritdoc />
        public IVariant FieldType { get; set; }

        private IList<FieldOption> _options;

        /// <inheritdoc />
        public IList<FieldOption> Options
        {
            get => _options ?? (_options = new BidirectionalList<FieldOption>(
                       onAdded: x => ((IHasParent<INormalFieldStatement>) x).Parent = this
                       , onRemoved: x => ((IHasParent<INormalFieldStatement>) x).Parent = null)
                   );
            set
            {
                _options?.Clear();
                _options = new BidirectionalList<FieldOption>(
                    value ?? GetRange<FieldOption>().ToList()
                    , onAdded: x => ((IHasParent<INormalFieldStatement>) x).Parent = this
                    , onRemoved: x => ((IHasParent<INormalFieldStatement>) x).Parent = null
                );
            }
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
            => $"{Label.ToDescriptorString(options)} {FieldType.ToDescriptorString(options)}"
               + $" {Name.ToDescriptorString(options)} = {Number}{Options.RenderOptions(options)};";
    }
}
