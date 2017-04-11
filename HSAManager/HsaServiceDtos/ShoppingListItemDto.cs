using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HsaServiceDtos
{
    public class ShoppingListItemDto : INotifyPropertyChanged
    {
        private bool? _checked;
        public int ShoppingListItemId { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }

        public bool? Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                OnPropertyChanged("Checked");
            }
        }

        public ProductDto Product { get; set; }
        public StoreDto Store { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}