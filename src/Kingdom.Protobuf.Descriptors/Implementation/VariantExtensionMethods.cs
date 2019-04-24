using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Collections.Variants;
    using static Characters;
    using IBytesEnumerable = IEnumerable<byte>;
    using StringRenderingTuple = Tuple<Type, ObjectStringRenderingCallback>;

    internal static class VariantExtensionMethods
    {
        private static IList<StringRenderingTuple> _renderingCallbacks;

        private static readonly ObjectStringRenderingCallback RenderBoolean = (x, o) => $"{x}".ToLower();
        private static readonly ObjectStringRenderingCallback RenderLong = (x, o) => ((long) x).RenderLong(o.IntegerRendering);
        private static readonly ObjectStringRenderingCallback RenderUnsignedLong = (x, o) => RenderLong((long) x, o);

        private static readonly ObjectStringRenderingCallback RenderDouble = (x, o) =>
        {
            var y = (double) x;

            const string inf = nameof(inf);
            const string nan = nameof(nan);

            const int isPosInf = 1, isNegInf = 2, isNaN = 3, isDef = 0;

            switch (double.IsPositiveInfinity(y)
                ? isPosInf
                : double.IsNegativeInfinity(y)
                    ? isNegInf
                    : double.IsNaN(y)
                        ? isNaN
                        : isDef)
            {
                case isPosInf: return inf;
                case isNegInf: return $"-{inf}";
                case isNaN: return nan;
                default: return y.RenderDouble(o.FloatingPointRendering);
            }
        };

        // TODO: TBD: escape the string...
        private static readonly ObjectStringRenderingCallback RenderString = (x, o) => $"{OpenQuote}{(string) x}{CloseQuote}";
        private static readonly ObjectStringRenderingCallback RenderBytes = (_, __) => throw new NotImplementedException();
        private static readonly ObjectStringRenderingCallback RenderIdentifierPath = (x, o) => ((IIdentifierPath) x).ToDescriptorString(o);

        private static readonly ObjectStringRenderingCallback RenderProtoType = (x, o) => ((ProtoType) x).ToDescriptorString(o);
        private static readonly ObjectStringRenderingCallback RenderElementTypeIdentifierPath = (x, o) => ((IElementTypeIdentifierPath) x).ToDescriptorString(o);

        private static IEnumerable<StringRenderingTuple> RenderingCallbacks
            => _renderingCallbacks ?? (_renderingCallbacks = new List<StringRenderingTuple>
            {
                // Domain-level 'Variant' Rendering support.
                Tuple.Create(typeof(ProtoType), RenderProtoType)
                , Tuple.Create(typeof(IElementTypeIdentifierPath), RenderElementTypeIdentifierPath)
                // Constant-level Rendering support.
                , Tuple.Create(typeof(bool), RenderBoolean)
                , Tuple.Create(typeof(long), RenderLong)
                , Tuple.Create(typeof(ulong), RenderUnsignedLong)
                , Tuple.Create(typeof(double), RenderDouble)
                , Tuple.Create(typeof(string), RenderString)
                , Tuple.Create(typeof(IBytesEnumerable), RenderBytes)
                // In this instance, we need to have ruled out the more specific Element-based Identifier Path.
                , Tuple.Create(typeof(IIdentifierPath), RenderIdentifierPath)
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
