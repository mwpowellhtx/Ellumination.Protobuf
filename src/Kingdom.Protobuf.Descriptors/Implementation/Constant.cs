using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Collections.Variants;
    using static String;
    using static StringComparison;
    using IBytesEnumerable = IEnumerable<byte>;

    // TODO: TBD: see notes on IConstant for purposes of evaluating potential refactoring to Kingdom.Collections.Variants.Variant usage...
    /// <summary>
    /// Provides a Domain level facade interfacing with <see cref="Variant{T}"/> for
    /// Constant purposes.
    /// </summary>
    /// <see cref="Collections.Variants.Variant"/>
    /// <see cref="Collections.Variants.Variant{T}"/>
    /// <see cref="Collections.Variants.Variant.Create"/>
    public static class Constant
    {
        private static IVariantConfigurationCollection _constantConfiguration;

        private static bool FloatEquals(float x, float y)
        {
            if (float.IsNaN(x)
                || float.IsNaN(y)
                || float.IsInfinity(x)
                || float.IsInfinity(y))
            {
                return false;
            }

            return (x - y).IsZero();
        }

        private static bool DoubleEquals(double x, double y)
        {
            if (double.IsNaN(x)
                || double.IsNaN(y)
                || double.IsInfinity(x)
                || double.IsInfinity(y))
            {
                return false;
            }

            return (x - y).IsZero();
        }

        private static int FloatCompareTo(float x, float y) => x.CompareTo(y);

        private static int DoubleCompareTo(double x, double y) => x.CompareTo(y);

        private static int ElementCompareTo<T, TElement>(IEnumerable<T> x, IEnumerable<T> y, Func<T, TElement> getter)
            where TElement : IComparable<TElement>
        {
            // ReSharper disable PossibleMultipleEnumeration
            for (int i = 0, j = 0; i < x.Count() && j < y.Count(); ++i, ++j)
            {
                int delta;
                switch (delta = getter(x.ElementAt(i)).CompareTo(getter(y.ElementAt(j))))
                {
                    case 0: break;
                    default: return delta;
                }
            }

            const int lt = -1, gt = 1, eq = 0;
            return x.Count() < y.Count() ? lt : y.Count() < x.Count() ? gt : eq;
            // ReSharper restore PossibleMultipleEnumeration
        }

        internal static IVariantConfigurationCollection ConstantConfiguration
            => _constantConfiguration ?? (_constantConfiguration = VariantConfigurationCollection.Create(
                       VariantConfiguration.Configure<bool>((x, y) => (bool) x == (bool) y
                           , (x, y) => ((bool) x).CompareTo((bool) y))
                       , VariantConfiguration.Configure<long>((x, y) => (long) x == (long) y
                           , (x, y) => ((long) x).CompareTo((long) y))
                       , VariantConfiguration.Configure<ulong>((x, y) => (ulong) x == (ulong) y
                           , (x, y) => ((ulong) x).CompareTo((ulong) y))
                       , VariantConfiguration.Configure<float>((x, y) => FloatEquals((float) x, (float) y)
                           , (x, y) => FloatCompareTo((float) x, (float) y))
                       , VariantConfiguration.Configure<double>((x, y) => DoubleEquals((double) x, (double) y)
                           , (x, y) => DoubleCompareTo((double) x, (double) y))
                       , VariantConfiguration.Configure<string>((x, y) => (string) x == (string) y
                           , (x, y) => Compare((string) x, (string) y, InvariantCulture))
                       , VariantConfiguration.Configure<IBytesEnumerable>(
                           (x, y) => ReferenceEquals(x, y)
                                     || ((IBytesEnumerable) x).SequenceEqual((IBytesEnumerable) y)
                           , (x, y) => ElementCompareTo((IBytesEnumerable) x, (IBytesEnumerable) y, item => item))
                       , VariantConfiguration.Configure<IdentifierPath>(
                           (x, y) => IdentifierPath.Equals((IdentifierPath) x, (IdentifierPath) y)
                           , (x, y) => ElementCompareTo((IdentifierPath) x, (IdentifierPath) y, item => item.Name))
                   )
               );

        /// <summary>
        /// Returns a new <see cref="Variant{T}"/> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <see cref="Variant{T}"/>
        /// <see cref="Collections.Variants.Variant.Create{T}(IVariantConfigurationCollection)"/>
        public static Variant<T> Create<T>() => Collections.Variants.Variant.Create<T>(ConstantConfiguration);

        /// <summary>
        /// Returns a new <see cref="Variant{T}"/> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <see cref="Variant{T}"/>
        /// <see cref="Collections.Variants.Variant.Create{T}(T,IVariantConfigurationCollection)"/>
        public static Variant<T> Create<T>(T value) => Collections.Variants.Variant.Create(value, ConstantConfiguration);
    }
}
