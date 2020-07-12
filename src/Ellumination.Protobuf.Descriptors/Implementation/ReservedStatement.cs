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

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc cref="DescriptorBase" />
    public abstract class ReservedStatement
        : DescriptorBase
            , IReservedStatement
            , IMessageBodyItem
    {
        IMessageBodyParent IHasParent<IMessageBodyParent>.Parent
        {
            get => Parent as IMessageBodyParent;
            set => Parent = value;
        }
    }

    /// <inheritdoc cref="ReservedStatement" />
    public abstract class ReservedStatement<T>
        : ReservedStatement
            , IReservedStatement<T>
        where T : IHasParent<IReservedStatement>
    {
        private IList<T> _items = GetRange<T>().ToList();

        private IList<T> _bidiItems;

        /// <inheritdoc />
        public IList<T> Items
        {

#pragma warning disable IDE0074 // Use compound assignment
            get => _bidiItems ?? (_bidiItems = _items.ToBidirectionalList(
                onAdded: x => x.Parent = this, onRemoved: x => x.Parent = null)
            );
#pragma warning restore IDE0074 // Use compound assignment

            set
            {
                Items?.Clear();
                _items = (value ?? GetRange<T>()).ToList();
                _bidiItems = null;
            }
        }

        /// <summary>
        /// Renders the <paramref name="item"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected abstract string RenderItem(T item, IStringRenderingOptions options);

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            const string reserved = nameof(reserved);

            // ReSharper disable once ImplicitlyCapturedClosure
            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            string GetRenderedItems() => Join($"{Comma} ", Items.Select(x => RenderItem(x, options)));

            return $"{GetComments(MultiLineComment)}"
                   + $" {reserved} {GetComments(MultiLineComment)}"
                   + $" {GetRenderedItems()} {GetComments(MultiLineComment)} {SemiColon}"
                ;
        }
    }
}
