using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HsaServiceDtos
{
    public class ReceiptDto : INotifyPropertyChanged
    {
        public ReceiptDto()
        {
            LineItems = new ObservableCollection<LineItemDto>();
        }

        public int ReceiptId { get; set; }
        private StoreDto _store;
        public StoreDto Store
        {
            get
            {
                return _store;
            } 
            set
            {
                _store = value;
                OnPropertyChanged();
            } 
        }

        private DateTime? _dateTime;

        public DateTime? DateTime
        {
            get
            {
                return _dateTime;
            }
            set
            {
                _dateTime = value;
                OnPropertyChanged();
            }
        }

        public bool? IsScanned { get; set; }
        public string ImageUrl { get; set; }
        public string OcrUrl { get; set; }
        private bool _waitingForOcr;

        public bool WaitingForOcr
        {
            get
            {
                return _waitingForOcr;
            }
            set
            {
                _waitingForOcr = value;
                OnPropertyChanged();
            }
        }

        private bool _provisional;

        public bool Provisional
        {
            get
            {
                return _provisional;
            }
            set
            {
                _provisional = value;
                OnPropertyChanged();
            }
        }

        public ICollection<LineItemDto> LineItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}