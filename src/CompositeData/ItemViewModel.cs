namespace CompositeData
{
    public class ItemViewModel
    {
        public ItemViewModel(Item item)
        {
            Item = item;
        }

        public Item Item { get; }
    }
}