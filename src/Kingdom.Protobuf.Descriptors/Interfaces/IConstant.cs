using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <summary>
    /// Provides a general use <see cref="Constant"/> interface.
    /// </summary>
    /// <inheritdoc cref="ICanRenderString" />
    public interface IConstant : IEquatable<IConstant>, ICanRenderString
    {
        /// <summary>
        /// Gets the Type.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the Value in terms of <see cref="object"/>.
        /// </summary>
        object Value { get; }
    }

    /// <summary>
    /// Provides a <typeparamref name="T"/> specific implementation of <see cref="IConstant"/>.
    /// </summary>
    /// <inheritdoc />
    /// <typeparam name="T"></typeparam>
    public interface IConstant<T> : IConstant
    {
        /// <summary>
        /// Gets or sets the Value in terms of <typeparamref name="T"/>.
        /// </summary>
        new T Value { get; set; }
    }
}
