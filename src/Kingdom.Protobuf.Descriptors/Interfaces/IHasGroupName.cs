// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    // TODO: TBD: could potentially derive GroupIdentifier from Identifier for GroupFieldStatement purposes...
    /// <summary>
    /// The interface is present because <see cref="GroupFieldStatement"/> Name naming convention
    /// is slightly different. The parser knows how to handle this, and, when necessary, we can
    /// identify the object in an AST as such as well.
    /// </summary>
    /// <inheritdoc />
    public interface IHasGroupName : IHasName<Identifier>
    {
    }
}
