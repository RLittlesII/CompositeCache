using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using DynamicData;
using DynamicData.Binding;
using DynamicData.Kernel;

namespace CompositeData
{
    /// <summary>
    /// Abstract base class for a mutable list of <see cref="CompositeKey{T}"/>.
    /// </summary>
    public abstract class CompositeCacheBase<T> : AbstractNotifyPropertyChanged, INotifyPropertyChanged
        where T : Enumeration
    {
        private readonly IItemRepository _itemRepository;
        private T _filter;

        protected CompositeCacheBase(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;

            var dynamicFilter = this.WhenValueChanged(x => x.Filter)
               .Select(x => IsMatch(x));

            ItemChanges =
                _compositeLineItemCache
                   .Connect()
                   .AutoRefreshOnObservable(key => dynamicFilter)
                   // QUESTION: [rlittlesii] Is this not acceptable for filtering the composite key?
                   .Filter(dynamicFilter)
                   // QUESTION: [rlittlesii] What does this actually do?  Am I missing something?
                   .ChangeKey(x => x.Item.Id)
                   .Transform(source => source.Item)
                   .RefCount();
        }

        public ReadOnlyObservableCollection<ItemViewModel> Items => ItemViewModels;

        public IObservable<IChangeSet<Item, int>> ItemChanges { get; protected set; }

        protected ReadOnlyObservableCollection<ItemViewModel> ItemViewModels;

        public T Filter
        {
            get => _filter;
            set => this.SetAndRaise(ref _filter, value);
        }


        public void AddOrUpdate(int id, T key)
        {
            var item = _itemRepository
                .GetAll()
                .First(x => x.Id == id);

            _compositeLineItemCache.AddOrUpdate((item, key));
        }

        private Func<(Item Item, T Key), bool> IsMatch(T result) => x => x.Key.Equals(result);

        // POINT: [rlittlesii] I tried the key as a Tuple (int, T), and it didn't yield a better result.
        private readonly SourceCache<(Item Item, T Key), (Item Item, T Key)> _compositeLineItemCache = new(x => x);
    }
}