using UnityEngine;

namespace Shops
{
    public class BoolGoodCard : GoodCard, ISelectable
    {
        private const string Selected = nameof(Selected);
        private const string Choose = nameof(Choose);

        [SerializeField] private Color _fade;

        private bool _didSelect;

        public ICard Init(GoodNames good, Sprite icon, bool didSelect = false)
        {
            _didSelect = didSelect;
            return base.Init(good, icon);
        }

        public override void ShowMaximum()
        {
            ChangeButtonTranslation(GetLabel());
            HidePrice(HighLightImage);
        }

        public override void ShowNext((object currentValue, object nextValue, int price) purchase)
        {
            SetPrice(purchase.price);
            Icon.color = _fade;
        }

        public void Select()
        {
            if (_didSelect == true)
                return;

            _didSelect = true;
            ShowMaximum();
        }

        public void Deselect()
        {
            if (_didSelect == false)
                return;

            _didSelect = false;
            ShowMaximum();
        }

        private string GetLabel()
        {
            if (_didSelect == false)
                return Choose;

            SetButtonInteractable(false);
            return Selected;
        }

        private void HighLightImage()
        {
            Icon.color = Color.white;
        }
    }
}