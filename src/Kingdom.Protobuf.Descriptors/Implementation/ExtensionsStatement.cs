using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Collections;
    using static Characters;
    using static String;
    using static WhiteSpaceAndCommentOption;

    /// <inheritdoc cref="DescriptorBase" />
    public class ExtensionsStatement
        : DescriptorBase
            , IExtensionsStatement
            , IMessageBodyItem
    {
        // TODO: TBD: may refactor to base classes...
        private IParentItem _parent;

        /// <inheritdoc />
        IMessageBodyParent IHasParent<IMessageBodyParent>.Parent
        {
            get => _parent as IMessageBodyParent;
            set => _parent = value;
        }

        private IList<RangeDescriptor> _items;

        /// <inheritdoc />
        public IList<RangeDescriptor> Items
        {
            get => _items ?? (_items = new BidirectionalList<RangeDescriptor>(
                       onAdded: x => ((IHasParent<IExtensionsStatement>) x).Parent = this
                       , onRemoved: x => ((IHasParent<IExtensionsStatement>) x).Parent = null)
                   );
            set
            {
                _items?.Clear();
                _items = new BidirectionalList<RangeDescriptor>(
                    value ?? GetRange<RangeDescriptor>().ToList()
                    , onAdded: x => ((IHasParent<IExtensionsStatement>) x).Parent = this
                    , onRemoved: x => ((IHasParent<IExtensionsStatement>) x).Parent = null
                );
            }
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            const string extensions = nameof(extensions);

            // ReSharper disable once ImplicitlyCapturedClosure
            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            bool TryRenderRanges(out string rendered)
            {
                const string defaultRendered = null;
                return !IsNullOrEmpty(
                    rendered = Items.Any()
                        ? Join($"{Comma} ", Items.Select(x => x.ToDescriptorString(options)))
                        : defaultRendered
                );
            }

            return TryRenderRanges(out var s)
                    ? $"{GetComments(MultiLineComment)}"
                      + $" {extensions}"
                      + $" {GetComments(MultiLineComment)}"
                      + $" {s}{GetComments(MultiLineComment)}"
                      + $" {SemiColon}"
                    : throw new InvalidOperationException($"Failed to render for empty {nameof(Items)}.")
                ;
        }
    }
}
