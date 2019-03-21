using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <summary>
    /// 
    /// </summary>
    public static class LabelKindExtensionMethods
    {
        /// <summary>
        /// Renders <paramref name="value"/> as a string assuming default
        /// <see cref="IStringRenderingOptions"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescriptorString(this LabelKind value)
            => value.ToDescriptorString(new StringRenderingOptions { });

        /// <summary>
        /// Returns the Range of <see cref="LabelKind"/> Values.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<LabelKind> Values
        {
            get
            {
                foreach (LabelKind x in Enum.GetValues(typeof(LabelKind)))
                {
                    yield return x;
                }
            }
        }

        private static Dictionary<LabelKind, string> ValueMapping { get; }
            = Values.ToDictionary(x => x, x => x.ToString().ToLower());

        /// <summary>
        /// Returns the <see cref="LabelKind"/> corresponding to the <see cref="string"/>
        /// <paramref name="s"/>. It is a hard error if the match cannot be made.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <see cref="LabelKind"/>
        /// <see cref="ValueMapping"/>
        public static LabelKind ParseLabelKind(this string s) => ValueMapping.Single(x => x.Value == s).Key;

        /// <summary>
        /// Renders the <paramref name="value"/> given the <paramref name="options"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string ToDescriptorString(this LabelKind value, IStringRenderingOptions options)
            => $"{ValueMapping[value]}";
    }
}
