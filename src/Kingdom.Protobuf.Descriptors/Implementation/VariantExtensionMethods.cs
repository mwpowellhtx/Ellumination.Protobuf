using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Collections.Variants;
    using StringRenderingTuple = Tuple<Type, ObjectStringRenderingCallback>;

    internal static class VariantExtensionMethods
    {
        private static IList<StringRenderingTuple> _renderingCallbacks;

        private static readonly ObjectStringRenderingCallback RenderProtoType
            = (x, o) => ((ProtoType) x).ToDescriptorString(o);

        private static readonly ObjectStringRenderingCallback RenderElementTypeIdentifierPath
            = (x, o) => ((IElementTypeIdentifierPath) x).ToDescriptorString(o);

        private static IEnumerable<StringRenderingTuple> RenderingCallbacks
            => _renderingCallbacks ?? (_renderingCallbacks = new List<StringRenderingTuple>
            {
                Tuple.Create(typeof(ProtoType), RenderProtoType),
                Tuple.Create(typeof(IElementTypeIdentifierPath), RenderElementTypeIdentifierPath)
            });

        /// <summary>
        /// Renders the <paramref name="value"/> as a <see cref="string"/> assuming default
        /// <see cref="IStringRenderingOptions"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescriptorString(this IVariant value)
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            => ToDescriptorString(value, new StringRenderingOptions { });

        /// <summary>
        /// Renders the <paramref name="value"/> as a <see cref="string"/> given the
        /// <paramref name="options"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string ToDescriptorString(this IVariant value, IStringRenderingOptions options)
        {
            // We want first, best possible pairing given the Variant Value.
            var pair = RenderingCallbacks.FirstOrDefault(x => x.Item1 == value.VariantType || x.Item1.IsAssignableFrom(value.VariantType));
            var renderingCallback = pair == null ? (x, o) => $"{x}" : pair.Item2;

            // TODO: TBD: potential Nullness going on here?
            return renderingCallback.Invoke(value?.Value, options);
        }
    }
}
