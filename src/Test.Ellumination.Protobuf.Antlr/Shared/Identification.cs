using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Combinatorics.Combinatorials;
    using static CharacterSets;
    using static CollectionHelpers;
    using static String;

    internal static class Identification
    {
        internal static Guid EmptyId => Guid.Empty;

        internal static Guid NewId => Guid.NewGuid();

        private delegate string NameCharacterSetSelector(int index);

        private static string BuildName(int length, NameCharacterSetSelector characterSetSelector)
        {
            IEnumerable<char> GetCharacters()
            {
                for (var i = 0; i < length; ++i)
                {
                    var chSet = characterSetSelector(i);
                    yield return GetCharacterSetCharacter(chSet);
                }
            }

            return GetCharacters().Aggregate(Empty, (g, x) => g + x);
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static string GetIdent(int length)
        {
            string s;
            do
            {
                // This is a little more expensive, but helps to avoid Keyword Collisions.
                s = BuildName(length, i => i == 0 ? IdentFirstLetterCharacterSet : IdentLetterCharacterSet);
            } while (ProtoParser.Keywords.Contains(s.ToLower()));

            return s;
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the Identifiers corresponding with the <paramref name="lengths"/>.
        /// </summary>
        /// <param name="lengths"></param>
        /// <returns></returns>
        internal static IEnumerable<string> GetIdents(IEnumerable<int> lengths)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var length in lengths)
            {
                yield return GetIdent(length);
            }
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns a Full Identifier given <paramref name="length"/> and
        /// <paramref name="parts"/>.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="parts"></param>
        /// <returns></returns>
        internal static IdentifierPath GetFullIdent(int length, int parts)
        {
            IEnumerable<string> GetFullIdentParts()
            {
                while (parts-- > 0)
                {
                    yield return GetIdent(length);
                }
            }

            return new IdentifierPath(GetFullIdentParts().Select(x => (Identifier) x));
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns a set of Full Identifiers given <paramref name="lengths"/> and
        /// <see cref="allParts"/>.
        /// </summary>
        /// <param name="lengths"></param>
        /// <param name="allParts"></param>
        /// <returns></returns>
        internal static IEnumerable<IdentifierPath> GetFullIdents(IEnumerable<int> lengths, IEnumerable<int> allParts)
        {
            var inputs = lengths.Select(x => (object) x) // Length
                .Combine(
                    allParts.Select(x => (object) x) // Parts
                );

            inputs.SilentOverflow = true;

            for (var i = 0; i < inputs.Count; i++, ++inputs)
            {
                var current = inputs.CurrentCombination.ToArray();
                var length = (int) current[0];
                var parts = (int) current[1];
                yield return GetFullIdent(length, parts);
            }
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <param name="parts"></param>
        /// <param name="globalScope"></param>
        /// <returns></returns>
        internal static ElementTypeIdentifierPath GetElementTypeIdentifierPath(int length, int parts, bool globalScope)
        {
            var path = new ElementTypeIdentifierPath {IsGlobalScope = globalScope};
            while (parts-- > 0)
            {
                path.Add(GetIdent(length));
            }

            return path;
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lengths"></param>
        /// <param name="parts"></param>
        /// <param name="globalScopes"></param>
        /// <returns></returns>
        internal static IEnumerable<ElementTypeIdentifierPath> GetElementTypeIdentifierPaths(IEnumerable<int> lengths
            , IEnumerable<int> parts, IEnumerable<bool> globalScopes)
        {
            var inputs = lengths.Select(x => (object) x).ToArray().Combine(
                parts.Select(x => (object) x).ToArray()
                , globalScopes.Select(x => (object) x).ToArray()
            );

            inputs.SilentOverflow = true;

            for (var i = 0; i < inputs.Count; i++, ++inputs)
            {
                var current = inputs.CurrentCombination.ToArray();
                var currentLength = (int) current[0];
                var currentParts = (int) current[1];
                var globalScope = (bool) current[2];
                yield return GetElementTypeIdentifierPath(currentLength, currentParts, globalScope);
            }
        }

        /// <summary>
        /// Gets a set of <see cref="ElementTypeIdentifierPath"/> instances for use within Test Cases.
        /// </summary>
        internal static IEnumerable<ElementTypeIdentifierPath> ElementTypes
        {
            get
            {
                var inputs = GetRange<object>(1, 3) // Length
                    .Combine(
                        GetRange<object>(1, 3) // Parts
                        , GetRange<object>(true, false) // GlobalScope
                    );

                inputs.SilentOverflow = true;

                for (var i = 0; i < inputs.Count; i++, ++inputs)
                {
                    var current = inputs.CurrentCombination.ToArray();
                    var length = (int) current[0];
                    var parts = (int) current[1];
                    var globalScope = (bool) current[2];
                    yield return GetElementTypeIdentifierPath(length, parts, globalScope);
                }
            }
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the Group Name corresponding with <paramref name="length"/>.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static string GetGroupName(int length)
        {
            string s;
            do
            {
                // This is a little more expensive, but helps to avoid Keyword Collisions.
                s = BuildName(length, i => i == 0 ? GroupNameFirstLetterCharacterSet : GroupNameLetterCharacterSet);
            } while (ProtoParser.Keywords.Contains(s.ToLower()));

            return s;
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the Group Names corresponding with <paramref name="lengths"/>.
        /// </summary>
        /// <param name="lengths"></param>
        /// <returns></returns>
        internal static IEnumerable<string> GetGroupNames(IEnumerable<int> lengths)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var length in lengths)
            {
                yield return GetGroupName(length);
            }
        }
    }
}
