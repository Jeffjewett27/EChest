using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace EChestVC.HelperFunctions
{
    sealed class GenericKeyedCollection<TKey, TVal> : KeyedCollection<TKey, TVal>
    {
        private Func<TVal, TKey> itemKey;

        public GenericKeyedCollection(Func<TVal, TKey> itemKey, IEqualityComparer<TKey> comparer = null, int threshold = 0) 
            : base(comparer, threshold)
        {
            this.itemKey = itemKey ?? throw new ArgumentNullException("itemkey");
        }

        protected override TKey GetKeyForItem(TVal item)
        {
            return itemKey(item);
        }
    }
}
