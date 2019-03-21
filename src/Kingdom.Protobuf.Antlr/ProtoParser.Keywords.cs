using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Combinatorics.Combinatorials;
    using static String;

    // ReSharper disable once IdentifierTypo
    /// <summary>
    /// Provides a set of Parser functionality.
    /// </summary>
    /// <inheritdoc />
    public partial class ProtoParser
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the Range of <paramref name="values"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <remarks>It is important that we do not simple return the <paramref name="values"/>
        /// instance itself, but rather a yielded return.</remarks>
        private static IEnumerable<T> GetRange<T>(params T[] values)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var x in values)
            {
                yield return x;
            }
        }

        private static IEnumerable<string> _keywords;

        /// <summary>
        /// Gets the Lexer Keywords.
        /// </summary>
        public static IEnumerable<string> Keywords
        {
            get
            {
                IEnumerable<string> GetAll()
                {
                    // In no particular order, in fact ALPHABETICAL order.
                    const string @bool = nameof(@bool);
                    const string bytes = nameof(bytes);
                    const string @double = nameof(@double);
                    const string @enum = nameof(@enum);
                    const string extend = nameof(extend);
                    const string extensions = nameof(extensions);
                    const string field = nameof(field);
                    const string @false = nameof(@false);
                    const string @float = nameof(@float);
                    const string group = nameof(group);
                    const string import = nameof(import);
                    const string inf = nameof(inf);
                    const string map = nameof(map);
                    const string max = nameof(max);
                    const string message = nameof(message);
                    const string nan = nameof(nan);
                    // ReSharper disable once IdentifierTypo
                    const string oneof = nameof(oneof);
                    const string option = nameof(option);
                    const string optional = nameof(optional);
                    const string package = nameof(package);
                    const string proto2 = nameof(proto2);
                    const string @public = nameof(@public);
                    const string repeated = nameof(repeated);
                    const string required = nameof(required);
                    const string reserved = nameof(reserved);
                    const string @string = nameof(@string);
                    const string syntax = nameof(syntax);
                    const string to = nameof(to);
                    const string @true = nameof(@true);
                    const string weak = nameof(weak);

                    yield return @bool;
                    yield return bytes;
                    yield return @double;
                    yield return @enum;
                    yield return extend;
                    yield return extensions;
                    yield return @false;
                    yield return field;
                    yield return @float;
                    yield return group;
                    yield return import;
                    yield return inf;
                    yield return map;
                    yield return max;
                    yield return message;
                    yield return nan;
                    yield return oneof;
                    yield return option;
                    yield return optional;
                    yield return package;
                    yield return proto2;
                    yield return @public;
                    yield return repeated;
                    yield return required;
                    yield return reserved;
                    yield return syntax;
                    yield return @string;
                    yield return to;
                    yield return @true;
                    yield return weak;

                    // Excepting for these which have the following Combinations.
                    const string @fixed = nameof(@fixed);
                    const string @int = nameof(@int);

                    Combiner GetIntegerCases() => GetRange<object>(Empty, "s", "u")
                        .Combine(
                            GetRange<object>(32, 64)
                            , GetRange<object>(@int)
                        );

                    Combiner GetFixedCases() => GetRange<object>(Empty, "s")
                        .Combine(
                            GetRange<object>(32, 64)
                            , GetRange<object>(@fixed)
                        );

                    // Works with the Cases in the same general form: new object[]{Signed, Bits, Base}.
                    IEnumerable<string> GetCombinedCases(Combiner cases)
                    {
                        // Ensure that we are including the full range.
                        cases.SilentOverflow = true;

                        for (var i = 0; i < cases.Count; i++, ++cases)
                        {
                            var current = cases.CurrentCombination.ToArray();
                            var signed = (string) current[0];
                            var bits = (int) current[1];
                            var @base = (string) current[2];
                            yield return $"{signed}{@base}{bits}";
                        }
                    }

                    foreach (var x in GetCombinedCases(GetIntegerCases()).Concat(GetCombinedCases(GetFixedCases())))
                    {
                        yield return x;
                    }
                }

                return _keywords ?? (_keywords = GetAll().ToArray());
            }
        }
    }
}
