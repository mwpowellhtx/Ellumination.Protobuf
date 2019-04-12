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
        INormalFieldStatement IHasParent<INormalFieldStatement>.Parent
        {
            get => Parent as INormalFieldStatement;
            set => Parent = value;
        }

        IOneOfFieldStatement IHasParent<IOneOfFieldStatement>.Parent
        {
            get => Parent as IOneOfFieldStatement;
            set => Parent = value;
        }

        IMapFieldStatement IHasParent<IMapFieldStatement>.Parent
        {
            get => Parent as IMapFieldStatement;
            set => Parent = value;
        }

        /// <inheritdoc />
        public bool Equals(FieldOption other) => Equals(this, other);
    }
}
