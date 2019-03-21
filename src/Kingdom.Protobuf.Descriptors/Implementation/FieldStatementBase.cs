// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc cref="DescriptorBase" />
    public abstract class FieldStatementBase
        : DescriptorBase
            , IFieldStatement
            , IHasNumber
    {
        // TODO: TBD: possible refactor to DescriptorBase...
        /// <summary>
        /// Gets or Sets the Parent.
        /// </summary>
        protected IParentItem Parent { get; set; }

        /// <inheritdoc />
        public long Number { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc cref="DescriptorBase" />
    public abstract class FieldStatementBase<T>
        : DescriptorBase<T>
            , IFieldStatement
            , IHasNumber
        where T : class, new()
    {
        /// <inheritdoc />
        protected FieldStatementBase()
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            : base(new T { })
        {
        }

        // TODO: TBD: possible refactor to DescriptorBase...
        /// <summary>
        /// Gets or Sets the Parent.
        /// </summary>
        protected IParentItem Parent { get; set; }

        /// <inheritdoc />
        public long Number { get; set; }
    }
}
