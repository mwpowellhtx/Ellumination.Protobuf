using System;
using Kingdom.Collections.Variants;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Collections.Variants;
    using static String;
    using static VariantConfiguration;
    using static StringComparison;

    // TODO: TBD: what to do with Constants/Constant-Configuration...
    /// <summary>
    /// Provides a Domain level facade interfacing with <see cref="Variant{T}"/> for
    /// Variant purposes.
    /// </summary>
    /// <see cref="Collections.Variants.Variant"/>
    /// <see cref="Collections.Variants.Variant{T}"/>
    /// <see cref="Collections.Variants.Variant.Create"/>
    public static class Variant
    {
        private static int CompareTo(IElementTypeIdentifierPath x, IElementTypeIdentifierPath y)
        {
            const int eq = 0;

            if (ReferenceEquals(x, y))
            {
                return eq;
            }

            for (int i = 0, j = 0; i < x.Count && j < y.Count; ++i, ++j)
            {
                /* We are not expecting any of the contributing elements to be Null,
                 * but allow for that to be an exceptional case. */
                if (x[i] == null || x[i].Name == null)
                {
                    throw new InvalidOperationException("One or more left hand side elements were null.");
                }

                if (y[j] == null || y[j].Name == null)
                {
                    throw new InvalidOperationException("One or more right hand side elements were null.");
                }

                int delta;
                switch (delta = Compare(x[i].Name, y[j].Name, InvariantCulture))
                {
                    case 0: break;
                    default: return delta;
                }
            }

            const int gt = 1, lt = -1;

            // If we have anything remaining, the counts determine the outcome.
            return x.Count > y.Count ? gt : x.Count < y.Count ? lt : eq;
        }

        /// <summary>
        /// Gets the <see cref="Variant{T}"/> Configuration.
        /// </summary>
        /// <remarks>String Rendering assets are refactored to
        /// <see cref="VariantExtensionMethods"/>. Additionally, we cannot support the
        /// <see cref="IElementTypeIdentifierPath"/> interface here. Rather, we must support
        /// the concrete <see cref="ElementTypeIdentifierPath"/> type instead.</remarks>
        private static IVariantConfigurationCollection VariantConfiguration
            => VariantConfigurationCollection.Create(
                Configure<ProtoType>(
                    (x, y) => (ProtoType) x == (ProtoType) y
                    , (x, y) => ((ProtoType) x).CompareProtoTypeTo((ProtoType) y))
                , Configure<ElementTypeIdentifierPath>(
                    (x, y) => ReferenceEquals(x, y)
                              || ((ElementTypeIdentifierPath) x).Equals((ElementTypeIdentifierPath) y)
                    , (x, y) => CompareTo((ElementTypeIdentifierPath) x, (ElementTypeIdentifierPath) y))
            );


#if DEBUG
        // ReSharper disable once UnusedMember.Global
        internal static IVariantConfigurationCollection InternalConfiguration => VariantConfiguration;
#endif // DEBUG

        /// <summary>
        /// Returns a new <see cref="Variant{T}"/> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Variant<T> Create<T>() => Collections.Variants.Variant.Create<T>(VariantConfiguration);

        /// <summary>
        /// Returns a new <see cref="Variant{T}"/> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Variant<T> Create<T>(T value) => Collections.Variants.Variant.Create(value, VariantConfiguration);
    }
}
