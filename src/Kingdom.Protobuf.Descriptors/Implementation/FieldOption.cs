using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc cref="OptionDescriptorBase"/>
    public class FieldOption
        : OptionDescriptorBase
            , INormalFieldOption
            , IOneOfFieldOption
            , IMapFieldOption
            , IEquatable<FieldOption>
    {
        private IParentItem _parent;

        INormalFieldStatement IHasParent<INormalFieldStatement>.Parent
        {
            get => _parent as INormalFieldStatement;
            set => _parent = value;
        }

        IOneOfFieldStatement IHasParent<IOneOfFieldStatement>.Parent
        {
            get => _parent as IOneOfFieldStatement;
            set => _parent = value;
        }

        IMapFieldStatement IHasParent<IMapFieldStatement>.Parent
        {
            get => _parent as IMapFieldStatement;
            set => _parent = value;
        }

        /// <inheritdoc />
        public bool Equals(FieldOption other) => Equals(this, other);
    }
}
