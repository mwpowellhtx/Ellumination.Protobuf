﻿// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <summary>
    /// General purpose String Rendering Callback delegate.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <remarks>Introduced this at same the moment in which we refactored
    /// <see cref="Ellumination.Collections.Variants.IVariant{T}"/> dependencies.</remarks>
    public delegate string ObjectStringRenderingCallback(object value, IStringRenderingOptions options);
}
