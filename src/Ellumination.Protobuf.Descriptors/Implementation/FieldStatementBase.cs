// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
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

        /// <inheritdoc />
        public long Number { get; set; }
    }
}
