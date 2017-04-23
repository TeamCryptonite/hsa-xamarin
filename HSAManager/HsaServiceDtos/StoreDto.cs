using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HsaServiceDtos
{
    public class StoreDto : INotifyPropertyChanged
    {
        public int StoreId { get; set; }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public LocationDto Location { get; set; }
        public double? DistanceToUser { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}