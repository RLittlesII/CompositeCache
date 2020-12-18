using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using DynamicData;
using DynamicData.Kernel;

namespace CompositeData
{
    /// <summary>
    /// Abstract base class for a mutable list of <see cref="CompositeKey{T}"/>.
    /// </summary>
    public class CompositeCacheBase<T>
        where T : Enumeration
    {
        private readonly IItemRepository _itemRepository;

        protected CompositeCacheBase(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;

            ItemChanges =
                _compositeLineItemCache
                   .Connect()
                   // QUESTION: [rlittlesii] Is this not acceptable for filtering the composite key?
                   .Filter(_aggregateKeyChanged.AsObservable())
                   // QUESTION: [rlittlesii] What does this actually do?  Am I missing something?
                   .ChangeKey(x => x.Item.Id)
                   .Transform(source => source.Item)
                   .RefCount();
        }

        public ReadOnlyObservableCollection<ItemViewModel> Items => ItemViewModels;

        public IObservable<IChangeSet<Item, int>> ItemChanges { get; protected set; }

        protected ReadOnlyObservableCollection<ItemViewModel> ItemViewModels;
        
        public void Filter(T key) => _aggregateKeyChanged.OnNext(x => x.Key.Equals(key));

        public void AddOrUpdate(int id, T key)
        {
            var item = _itemRepository
                .GetAll()
                .First(x => x.Id == id);

            _compositeLineItemCache.AddOrUpdate(new CompositeKey<T>(item, key));
        }

        private readonly ISubject<Func<CompositeKey<T>, bool>> _aggregateKeyChanged =
            new Subject<Func<CompositeKey<T>, bool>>();

        // POINT: [rlittlesii] I tried the key as a Tuple (int, T), and it didn't yield a better result.
        private readonly SourceCache<CompositeKey<T>, CompositeKey<T>> _compositeLineItemCache = new SourceCache<CompositeKey<T>, CompositeKey<T>>(x => x);
    }
}