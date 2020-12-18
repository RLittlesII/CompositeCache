using ReactiveUI;

namespace CompositeData
{
    public class Item : ReactiveObject
    {
        private int _quantity;
        public int Id { get; set; }

        public int Quantity
        {
            get => _quantity;
            set => this.RaiseAndSetIfChanged(ref _quantity, value);
        }
    }
}