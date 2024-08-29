using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shops
{
    public class CardFactory : ICardFactory
    {
        private readonly Dictionary<Type, GoodCard> _cardTemplates;
        private readonly Dictionary<GoodNames, Sprite> _icons;
        private readonly RectTransform _holder;
        private readonly GoodNames _selectedGood;
        private readonly bool _haveSelectedGood;

        public CardFactory(
            Dictionary<Type, GoodCard> goodCards,
            Dictionary<GoodNames, Sprite> icons,
            RectTransform holder,
            GoodNames selectedGood = GoodNames.MachineGun,
            bool haveSelectedGood = false)
        {
            _cardTemplates = goodCards;
            _icons = icons;
            _holder = holder;
            _selectedGood = selectedGood;
            _haveSelectedGood = haveSelectedGood;
        }

        public ICard CreateNumber(GoodNames good)
        {
            return Create<NumberGoodCard>().Init(good, _icons[good]);
        }

        public ICard CreateBool(GoodNames good)
        {
            return Create<BoolGoodCard>().Init(good, _icons[good], _haveSelectedGood == true && good == _selectedGood);
        }

        private T Create<T>()
            where T : GoodCard
        {
            return Object.Instantiate((T)_cardTemplates[typeof(T)], _holder);
        }
    }
}