using System;
using System.Collections.Generic;

namespace HsaServiceDtos
{
    public class ShoppingListDto
    {
        public ShoppingListDto()
        {
            ShoppingListItems = new List<ShoppingListItemDto>();
        }

        public int ShoppingListId { get; set; }
        public Guid UserObjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DateTime { get; set; }
        public ICollection<ShoppingListItemDto> ShoppingListItems { get; set; }
    }
}