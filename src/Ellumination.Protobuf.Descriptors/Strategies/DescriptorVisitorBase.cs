// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <inheritdoc cref="DescriptorStrategyBase{T}"/>
    public abstract class DescriptorVisitorBase<T> : DescriptorStrategyBase<T>, IDescriptorVisitor<T>
        where T : IDescriptor
    {
    }
}
