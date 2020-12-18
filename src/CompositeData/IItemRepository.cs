using System.Collections.Generic;

namespace CompositeData
{
    public interface IItemRepository
    {
        IEnumerable<Item> GetAll();
    }
}