using System;

namespace Shops
{
    public class ShopView
    {
        public ShopView()
        {
            
        }
        
        public event Action<ICard> GoingBuy;
        
        
    }
}