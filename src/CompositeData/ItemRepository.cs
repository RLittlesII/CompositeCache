using System.Collections.Generic;

namespace CompositeData
{
    public class ItemRepository : IItemRepository
    {
        private List<Item> _list = new List<Item>
        {
            new Item {Id = 1},
            new Item {Id = 2},
            new Item {Id = 3},
        };
        public IEnumerable<Item> GetAll() => _list;
    }
}