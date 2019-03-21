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
    public class EnumFieldDescriptor
        : DescriptorBase<Identifier>
            , IEnumFieldDescriptor
    {
        /// <inheritdoc />
        public EnumFieldDescriptor()
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            : base(new Identifier { })
        {
        }

        // TODO: TBD: may refactor to base classes...
        private IParentItem _parent;

        IEnumStatement IHasParent<IEnumStatement>.Parent
        {
            get => _parent as IEnumStatement;
            set => _parent = value;
        }

        /// <summary>
        /// Gets or sets the Ordinal.
        /// </summary>
        public long Ordinal { get; set; }

        private IList<EnumValueOption> _options;

        /// <inheritdoc />
        public IList<EnumValueOption> Options
        {
            get => _options ?? (_options = new BidirectionalList<EnumValueOption>(
                       onAdded: x => ((IEnumValueOption) x).Parent = this
                       , onRemoved: x => ((IEnumValueOption) x).Parent = null)
                   );
            set
            {
                _options?.Clear();
                _options = new BidirectionalList<EnumValueOption>(
                    value ?? GetRange<EnumValueOption>().ToList()
                    , onAdded: x => ((IEnumValueOption) x).Parent = this
                    , onRemoved: x => ((IEnumValueOption) x).Parent = null);
            }
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            string RenderOptions()
            {
                // This one makes sense to leverage String Join.
                return Options.Any()
                    ? $" [{Join(", ", Options.Select(x => x.ToDescriptorString(options)))}]"
                    : Empty;
            }

            /* Do not jump through any String Join hoops unless we absolutely have to.
             * And, plus, we can leverage Integer Literal Rendering just the same. */

            return $"{Name.ToDescriptorString(options)} = {Ordinal.RenderLong(options.IntegerRendering)}{RenderOptions()};";
        }
    }
}
