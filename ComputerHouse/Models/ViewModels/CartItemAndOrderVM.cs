using System.Collections.Generic;

namespace ComputerHouse.Models.ViewModels
{
    public class CartItemAndOrderVM
    {
        public IList<CartItem> CartItemList { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
