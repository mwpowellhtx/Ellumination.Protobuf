using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Collections.Generic;
    using static Characters;
    using static String;
    using static WhiteSpaceAndCommentOption;

    /// <inheritdoc cref="DescriptorBase" />
    public class ExtensionsStatement
        : DescriptorBase
            , IExtensionsStatement
            , IMessageBodyItem
    {
        /// <inheritdoc />
        IMessageBodyParent IHasParent<IMessageBodyParent>.Parent
        {
            get => Parent as IMessageBodyParent;
            set => Parent = value;
        }

        private IList<RangeDescriptor> _items = GetRange<RangeDescriptor>().ToList();

        private IList<RangeDescriptor> _bidiItems;

        /// <inheritdoc />
        public IList<RangeDescriptor> Items
        {

#pragma warning disable IDE0074 // Use compound assignment
            get => _bidiItems ?? (_bidiItems = _items.ToBidirectionalList(
                onAdded: x => ((IHasParent<IExtensionsStatement>) x).Parent = this
                , onRemoved: x => ((IHasParent<IExtensionsStatement>) x).Parent = null)
            );
#pragma warning restore IDE0074 // Use compound assignment

            set
            {
                Items?.Clear();
                _items = (value ?? GetRange<RangeDescriptor>()).ToList();
                _bidiItems = null;
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
                      + $" {extensions} {GetComments(MultiLineComment)}"
                      + $" {s} {GetComments(MultiLineComment)} {SemiColon}"
                    : throw new InvalidOperationException($"Failed to render for empty {nameof(Items)}.")
                ;
        }
    }
}
