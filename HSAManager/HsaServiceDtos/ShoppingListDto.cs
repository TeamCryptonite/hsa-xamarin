using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HsaServiceDtos
{
    public class ShoppingListDto
    {
        public ShoppingListDto()
        {
            ShoppingListItems = new ObservableCollection<ShoppingListItemDto>();
        }

        public int ShoppingListId { get; set; }
        public Guid UserObjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DateTime { get; set; }
        public ObservableCollection<ShoppingListItemDto> ShoppingListItems { get; set; }
    }
}