using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static String;

    /// <inheritdoc />
    public abstract class DescriptorBase : IDescriptor
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the Range corresponding to <paramref name="values"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        protected static IEnumerable<T> GetRange<T>(params T[] values)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var x in values)
            {
                yield return x;
            }
        }

        /// <summary>
        /// Renders Comments based on the <paramref name="option"/> and <paramref name="masks"/>.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="masks"></param>
        /// <returns></returns>
        protected static string RenderMaskedComments(WhiteSpaceAndCommentOption option
            , params WhiteSpaceAndCommentOption[] masks)
            => Join(" ", masks.Select(x => option.RenderComments(x)));

        // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
        private static IStringRenderingOptions DefaultStringRenderingOptions => new StringRenderingOptions { };

        /// <inheritdoc />
        /// <see cref="ToDescriptorString(IStringRenderingOptions)"/>
        public string ToDescriptorString() => ToDescriptorString(DefaultStringRenderingOptions);

        /// <inheritdoc />
        public abstract string ToDescriptorString(IStringRenderingOptions options);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc cref="DescriptorBase" />
    public abstract class DescriptorBase<T>
        : DescriptorBase
            , IDescriptor<T>
        where T : class
    {
        /// <inheritdoc cref="DescriptorBase"/>
        public T Name { get; set; }

        /// <inheritdoc />
        protected DescriptorBase()
            : this(default(T))
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="name"></param>
        protected DescriptorBase(T name)
        {
            Name = name;
        }
    }
}
