using System;
using System.Reactive.Linq;
using DynamicData;

namespace CompositeData
{
    public class ItemCache : CompositeCacheBase<ItemType>
    {
        public ItemCache(IItemRepository itemRepository)
            : base(itemRepository)
        {
            ItemChanges
                .AutoRefresh(x => x.Quantity)
                .Transform(x => new ItemViewModel(x))
                .Do(_ => { }, _ => { })
                .Bind(out ItemViewModels)
                .Do(_ => { }, _ => { })
                .Subscribe();
        }
    }
}