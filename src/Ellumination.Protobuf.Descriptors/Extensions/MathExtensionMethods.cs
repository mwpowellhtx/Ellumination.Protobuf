using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static MethodImplOptions;

    /// <summary>
    /// 
    /// </summary>
    /// <see cref="!:http://stackoverflow.com/questions/2411392/double-epsilon-for-equality-greater-than-less-than-less-than-or-equal-to-gre"/>
    internal static class MathExtensionMethods
    {
        private static IDictionary<Type, object> Epsilons { get; }
            = new Dictionary<Type, object>
            {
                {typeof(float), float.Epsilon},
                {typeof(double), double.Epsilon},
            };

        private static T GetEpsilon<T>() => (T) Epsilons[typeof(T)];

        [MethodImpl(AggressiveInlining)]
        public static bool IsZero(this float value)
        {
            var epsilon = GetEpsilon<float>();
            return value < epsilon && value > -epsilon;
        }

        [MethodImpl(AggressiveInlining)]
        public static bool IsZero(this double value)
        {
            var epsilon = GetEpsilon<double>();
            return value < epsilon && value > -epsilon;
        }

        [MethodImpl(AggressiveInlining)]
        public static int Sign(this float value)
        {
            var epsilon = GetEpsilon<float>();
            return value < -epsilon ? -1 : (value > epsilon ? 1 : 0);
        }

        [MethodImpl(AggressiveInlining)]
        public static int Sign(this double value)
        {
            var epsilon = GetEpsilon<double>();
            return value < -epsilon ? -1 : (value > epsilon ? 1 : 0);
        }
    }
}
