using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <summary>
    /// Provides a set of extension methods supporting <see cref="ProtoType"/> as well as
    /// <see cref="KeyType"/>.
    /// </summary>
    public static class ProtoTypeExtensionMethods
    {
        /// <summary>
        /// Renders <paramref name="value"/> as a <see cref="string"/> assuming default
        /// <see cref="IStringRenderingOptions"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescriptorString(this ProtoType value)
            => value.ToDescriptorString(new StringRenderingOptions { });

        /// <summary>
        /// Renders <paramref name="value"/> as a <see cref="string"/> assuming default
        /// <see cref="IStringRenderingOptions"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescriptorString(this KeyType value)
            => value.ToDescriptorString(new StringRenderingOptions { });

        private static IEnumerable<T> EnumerateEnumValues<T>()
            where T : struct
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                var message = $"The generic type {nameof(T)} type '{type.FullName}' is not an enum.";
                throw new ArgumentException(message, nameof(T));
            }

            foreach (T x in Enum.GetValues(typeof(T)))
            {
                yield return x;
            }
        }

        /// <summary>
        /// Returns the Range of <see cref="ProtoType"/> Values.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ProtoType> ProtoTypeValues => EnumerateEnumValues<ProtoType>();

        /// <summary>
        /// Returns the Range of <see cref="KeyType"/> Values.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyType> KeyTypeValues => EnumerateEnumValues<KeyType>();

        private static Dictionary<ProtoType, string> ValueMapping { get; }
            = ProtoTypeValues.ToDictionary(x => x, x => x.ToString().ToLower());

        /// <summary>
        /// Returns the <see cref="ProtoType"/> corresponding to the <paramref name="s"/> value.
        /// It is a hard error if the match cannot be made.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ProtoType ParseProtoType(this string s) => ValueMapping.Single(x => x.Value == s).Key;

        /// <summary>
        /// Returns the <see cref="KeyType"/> corresponding to the <paramref name="s"/> value.
        /// It is a hard error if the match cannot be made.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <see cref="ParseProtoType"/>
        public static KeyType ParseKeyType(this string s) => (KeyType) (int) s.ParseProtoType();

        /// <summary>
        /// Renders the <paramref name="value"/> given the <paramref name="options"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string ToDescriptorString(this ProtoType value, IStringRenderingOptions options)
            => ValueMapping[value];

        /// <summary>
        /// Renders the <paramref name="value"/> given the <paramref name="options"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string ToDescriptorString(this KeyType value, IStringRenderingOptions options)
            => ToDescriptorString((ProtoType) (int) value, options);

        /// <summary>
        /// Returns the Comparison of <paramref name="value"/> with <paramref name="other"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static int CompareProtoTypeTo(this ProtoType value, ProtoType other) => (int) value - (int) other;
    }
}
