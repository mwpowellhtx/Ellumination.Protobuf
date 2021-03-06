﻿// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Collections.Variants;

    /// <inheritdoc cref="IParentItem"/>
    public interface IOneOfFieldStatement
        : ICanRenderString
            , IParentItem
            , IHasParent<IOneOfStatement>
            , IHasOptions<FieldOption>
    {
        /// <summary>
        /// Gets or Sets the Field Type.
        /// </summary>
        IVariant FieldType { get; set; }
    }
}
