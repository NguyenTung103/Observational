using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ES_CapDien.Helpers
{
    public class CMSHelper
    {               
        public static int[] pageSizes = { 50, 80, 100 };
        public static double Total(Models.Entity.Data data, string code, double input)
        {
            if (code == "RP0")
            {
                input += data.BTI;
            }
            else if (code == "RP1")
            {
                input += data.BHU;
            }
            else if (code == "RP2")
            {
                input += data.BAV;
            }
            else if (code == "RP3")
            {
                input += data.BAC;
            }
            else if (code == "RP4")
            {
                input += data.BAP;
            }
            else if (code == "RP5")
            {
                input += data.BAF;
            }
            else if (code == "RP6")
            {
                input += data.BWS;
            }
            return input;
        }
    }

    internal static class GenericHelpers
    {
        /// <summary>
        /// Generates tree of items from item list
        /// </summary>
        /// 
        /// <typeparam name="T">Type of item in collection</typeparam>
        /// <typeparam name="K">Type of parent_id</typeparam>
        /// 
        /// <param name="collection">Collection of items</param>
        /// <param name="id_selector">Function extracting item's id</param>
        /// <param name="parent_id_selector">Function extracting item's parent_id</param>
        /// <param name="root_id">Root element id</param>
        /// 
        /// <returns>Tree of items</returns>
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(
            this IEnumerable<T> collection,
            Func<T, K> id_selector,
            Func<T, K> parent_id_selector,
            K root_id = default(K))
        {
            foreach (T c in collection.Where(c => parent_id_selector(c).Equals(root_id)))
            {
                yield return new TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(id_selector, parent_id_selector, id_selector(c))
                };
            }
        }
    }

    public class TreeItem<T>
    {
        public T Item { get; set; }
        public IEnumerable<TreeItem<T>> Children { get; set; }
    }
}

