using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HsaServiceDtos
{
    public class ReceiptDto
    {
        public ReceiptDto()
        {
            LineItems = new ObservableCollection<LineItemDto>();
        }

        public int ReceiptId { get; set; }
        public StoreDto Store { get; set; }
        public DateTime? DateTime { get; set; }
        public bool? IsScanned { get; set; }
        public string ImageUrl { get; set; }
        public string OcrUrl { get; set; }
        public bool WaitingForOcr { get; set; }
        public bool Provisional { get; set; }
        public ICollection<LineItemDto> LineItems { get; set; }
    }
}