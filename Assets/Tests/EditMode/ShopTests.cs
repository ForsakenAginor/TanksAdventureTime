using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shops;
using Tests.EditMode;

public class ShopTests
{
    [Test]
    public void CanNotBuyOverMax()
    {
        Assert.Catch(
            typeof(IndexOutOfRangeException),
            () => ImitateShop(
                bundle =>
                {
                    bundle.Item3.Buy(GoodNames.MachineGun);
                    ((TestCard)bundle.Item1[(int)GoodNames.MachineGun]).Click();
                }));
    }

    [Test]
    public void CanBuy()
    {
        ImitateShop(bundle => ((TestCard)bundle.Item1.Find(card => card.Good == GoodNames.MachineGun)).Click());
        Assert.Pass();
    }

    [Test]
    public void CanNotBuyEmpty()
    {
        Assert.Catch(
            () =>
            {
                ImitateShop(
                    bundle =>
                        ((TestCard)bundle.Item1.Find(card => card.Good == GoodNames.ReloadSpeed)).Click());
            });
    }

    [Test]
    public void CanNotBuyWithoutMoney()
    {
        ImitateShop(
            bundle =>
            {
                TestCard card = (TestCard)bundle.Item1.Find(card => card.Good == GoodNames.MachineGun);
                card.Click();
                Assert.IsTrue(card.DidFailed);
            });
    }

    private void ImitateShop(Action<(List<ICard>, Wallet, Shop, ShopView, ShopPresenter)> shopCallback = null)
    {
        List<ICard> cards = new ();
        (Wallet wallet, Shop shop, ShopView shopView, ShopPresenter presenter) bundle = CreateBundle(cards.Add);
        bundle.presenter.Enable();
        shopCallback?.Invoke((cards, bundle.wallet, bundle.shop, bundle.shopView, bundle.presenter));
        bundle.presenter.Disable();
    }

    private (Wallet wallet, Shop shop, ShopView shopView, ShopPresenter presenter) CreateBundle(
        Action<ICard> onCardCreated = null)
    {
        var goodsContent = CreateTestGoodsContent();
        Wallet wallet = new Wallet(new CurrencyData());
        Shop shop = new Shop(goodsContent, CreateStartPurchases());
        List<(GoodNames good, object value)> viewContent =
            goodsContent.Select(item => (item.Key, item.Value[0].value)).ToList();
        ShopView shopView = new ShopView(viewContent, new TestCardFactory(onCardCreated), null);
        ShopPresenter presenter = new ShopPresenter(wallet, shop, shopView);
        return (wallet, shop, shopView, presenter);
    }

    private Dictionary<GoodNames, List<(object value, int price)>> CreateTestGoodsContent()
    {
        return new ()
        {
            {
                GoodNames.MachineGun,
                new ()
                {
                    (false, 0),
                    (true, 122),
                }
            },
            {
                GoodNames.Grenade,
                new ()
                {
                    (false, 0),
                    (true, 122),
                }
            },
            {
                GoodNames.Health,
                new ()
                {
                    (10, 0),
                    (20, 5),
                }
            },
        };
    }

    private Purchases<int> CreateStartPurchases()
    {
        List<SerializedPair<GoodNames, int>> values = Enum.GetValues(typeof(GoodNames))
            .Cast<GoodNames>()
            .Select(type => new SerializedPair<GoodNames, int>(type, 0)).ToList();

        return new Purchases<int>(values);
    }
}