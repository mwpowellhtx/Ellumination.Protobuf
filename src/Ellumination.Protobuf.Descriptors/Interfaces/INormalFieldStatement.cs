﻿// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Collections.Variants;

    /// <inheritdoc cref="IFieldStatement"/>
    public interface INormalFieldStatement : IFieldStatement, IHasLabel, IHasOptions<FieldOption>
    {
        /// <summary>
        /// Gets or Sets the Field Type.
        /// </summary>
        IVariant FieldType { get; set; }
    }
}
