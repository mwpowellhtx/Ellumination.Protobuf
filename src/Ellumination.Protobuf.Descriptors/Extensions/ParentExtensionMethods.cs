using System;
using System.Collections.Generic;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <summary>
    /// 
    /// </summary>
    public static class ParentExtensionMethods
    {
        /// <summary>
        /// Returns the <paramref name="descriptor"/> Parentage starting with the parent-most
        /// <see cref="IDescriptor"/> in the ownership chain.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public static IEnumerable<IDescriptor> GetParentage<T>(this T descriptor)
            where T : class, IDescriptor
        {
            if (!(descriptor.Parent is null) || descriptor.Parent is IDescriptor)
            {
                foreach (var parent in descriptor.GetParentage())
                {
                    yield return parent;
                }
            }

            yield return descriptor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="item"></param>
        /// <param name="value"></param>
        /// <param name="replaceParent"></param>
        internal static void ReplaceItemParent<TItem, T>(this T parent, ref TItem item
            , TItem value, Action<TItem, T> replaceParent)
            where T : IParentItem
        {
            if (item != null)
            {
                replaceParent.Invoke(item, default(T));
            }

            if (value != null)
            {
                replaceParent.Invoke(value, parent);
            }

            item = value;
        }
    }
}
