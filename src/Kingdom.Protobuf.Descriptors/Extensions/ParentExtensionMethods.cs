using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    internal static class ParentExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="item"></param>
        /// <param name="value"></param>
        /// <param name="replaceParent"></param>
        public static void ReplaceItemParent<TItem, T>(this T parent, ref TItem item
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
