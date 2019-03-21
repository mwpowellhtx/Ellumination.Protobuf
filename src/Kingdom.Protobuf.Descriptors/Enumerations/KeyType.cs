// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <summary>
    /// Derived values based on <see cref="ProtoType"/>.
    /// </summary>
    public enum KeyType
    {
        /// <summary>
        /// <see cref="ProtoType.Int32"/>
        /// </summary>
        Int32 = ProtoType.Int32,

        /// <summary>
        /// <see cref="ProtoType.Int64"/>
        /// </summary>
        Int64 = ProtoType.Int64,

        /// <summary>
        /// <see cref="ProtoType.UInt32"/>
        /// </summary>
        UInt32 = ProtoType.UInt32,

        /// <summary>
        /// <see cref="ProtoType.UInt64"/>
        /// </summary>
        UInt64 = ProtoType.UInt64,

        /// <summary>
        /// <see cref="ProtoType.SInt32"/>
        /// </summary>
        SInt32 = ProtoType.SInt32,

        /// <summary>
        /// <see cref="ProtoType.SInt64"/>
        /// </summary>
        SInt64 = ProtoType.SInt64,

        /// <summary>
        /// <see cref="ProtoType.Fixed32"/>
        /// </summary>
        Fixed32 = ProtoType.Fixed32,

        /// <summary>
        /// <see cref="ProtoType.Fixed64"/>
        /// </summary>
        Fixed64 = ProtoType.Fixed64,

        /// <summary>
        /// <see cref="ProtoType.SFixed32"/>
        /// </summary>
        SFixed32 = ProtoType.SFixed32,

        /// <summary>
        /// <see cref="ProtoType.SFixed64"/>
        /// </summary>
        SFixed64 = ProtoType.SFixed64,

        /// <summary>
        /// <see cref="ProtoType.Bool"/>
        /// </summary>
        Bool = ProtoType.Bool,

        /// <summary>
        /// <see cref="ProtoType.String"/>
        /// </summary>
        String = ProtoType.String
    }
}
