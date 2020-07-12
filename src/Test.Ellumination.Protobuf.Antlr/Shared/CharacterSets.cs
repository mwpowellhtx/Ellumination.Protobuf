using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static Math;
    using static String;
    using static Randomizer;

    internal static class CharacterSets
    {
        private static string BuildCharacterSet(params Tuple<char, char>[] ranges)
        {
            IEnumerable<char> GetCharacters(char a, char b)
            {
                for (var x = Min(a, b); x <= Max(a, b); ++x)
                {
                    yield return (char) x;
                }
            }

            return ranges.SelectMany(x => GetCharacters(x.Item1, x.Item2))
                .Aggregate(Empty, (g, x) => g + x);
        }

        internal static string IdentLetterCharacterSet { get; }
            = BuildCharacterSet(
                  Tuple.Create('a', 'z')
                  , Tuple.Create('A', 'Z')
                  , Tuple.Create('0', '9')
              ) + "_";

        internal static string IdentFirstLetterCharacterSet { get; }
            = BuildCharacterSet(
                Tuple.Create('a', 'z')
                , Tuple.Create('A', 'Z')
            );

        internal static string GroupNameFirstLetterCharacterSet { get; }
            = BuildCharacterSet(
                Tuple.Create('A', 'Z')
            );

        internal static string GroupNameLetterCharacterSet => IdentLetterCharacterSet;

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the next Character from the <see cref="string"/> <paramref name="chSet"/>
        /// Character Set.
        /// </summary>
        /// <param name="chSet"></param>
        /// <returns></returns>
        internal static char GetCharacterSetCharacter(string chSet) => chSet[Rnd.Next(0, chSet.Length - 1)];
    }
}
