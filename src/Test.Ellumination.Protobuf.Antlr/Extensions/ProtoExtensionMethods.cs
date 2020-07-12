using System.Collections.Generic;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Ellumination.Collections.Generic;
    using Xunit;

    internal static class ProtoExtensionMethods
    {
        /// <summary>
        /// Verifies the Parentage of the <paramref name="children"/> given
        /// <paramref name="expectedParent"/>.
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="children"></param>
        /// <param name="expectedParent"></param>
        public static void VerifyBidirectionalParentage<TParent, TChild>(this IList<TChild> children, TParent expectedParent)
            where TParent : class, IParentItem
            where TChild : class, IHasParent<TParent>
        {
            // Verify Children has an Instance to begin with.
            Assert.NotNull(children);

            // Although Children is seen as IList, this should be true.
            Assert.IsAssignableFrom<IBidirectionalList<TChild>>(children);

            // Which should also tell us whether Is of the same type.
            Assert.True(children is IBidirectionalList<TChild>);

            // Followed by the remainder of the Parentage verification.
            children.VerifyParentage(expectedParent);
        }

        /// <summary>
        /// Verifies the Parentage of the <paramref name="children"/> given
        /// <paramref name="expectedParent"/>.
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="children"></param>
        /// <param name="expectedParent"></param>
        public static void VerifyParentage<TParent, TChild>(this IEnumerable<TChild> children, TParent expectedParent)
            where TParent : class, IParentItem
            where TChild : class, IHasParent<TParent>
        {
            new object[] {children, expectedParent}.AllNotNull();
            // ReSharper disable once PossibleMultipleEnumeration
            children.AllNotNull();
            // ReSharper disable once PossibleMultipleEnumeration
            foreach (var x in children)
            {
                Assert.NotNull(x.Parent);
                Assert.Same(expectedParent, x.Parent);
            }
        }
    }
}
