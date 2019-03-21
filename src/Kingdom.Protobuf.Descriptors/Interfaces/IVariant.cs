using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <summary>
    /// This is not quite like <see cref="IConstant"/> although the motivation is similar.
    /// We need to be able to support a single bucket being able to handle potentially at
    /// least a couple different types of values.
    /// </summary>
    /// <inheritdoc cref="ICanRenderString" />
    public interface IVariant : IEquatable<IVariant>, ICanRenderString
    {
        /// <summary>
        /// Gets the Type of the Variant.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the Value of the Variant in terms of <see cref="object"/>.
        /// </summary>
        object Value { get; }
    }

    /// <summary>
    /// Represents the Strongly Typed <typeparamref name="T"/> <see cref="IVariant"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc />
    public interface IVariant<T> : IVariant
    {
        /// <summary>
        /// Gets or sets the Strongly Typed <typeparamref name="T"/> Value.
        /// </summary>
        new T Value { get; set; }
    }
}
