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

    [Test]
    public void DidReachMaximum()
    {
        ImitateShop(
            bundle =>
            {
                int startMoney = 1000;
                bundle.Item2.AddCurrency(startMoney);
                TestCard card = (TestCard)bundle.Item1.Find(card => card.Good == GoodNames.MachineGun);
                card.Click();
                Assert.IsTrue(card.IsMaximum);
            });
    }

    private void ImitateShop(Action<(List<ICard>, Wallet, Shop, ShopView, ShopPresenter)> shopCallback = null)
    {
        List<ICard> cards = new ();
        (Wallet, Shop, ShopView, ShopPresenter) bundle = CreateBundle(cards.Add);
        bundle.Item4.Enable();
        shopCallback?.Invoke((cards, bundle.Item1, bundle.Item2, bundle.Item3, bundle.Item4));
        bundle.Item4.Disable();
    }

    private (Wallet, Shop, ShopView, ShopPresenter) CreateBundle(
        Action<ICard> onCardCreated = null)
    {
        var goodsContent = CreateTestGoodsContent();
        Wallet wallet = new Wallet(new CurrencyData());
        Shop shop = new Shop(goodsContent, CreateStartPurchases(), _ => { });
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

    private Purchases CreateStartPurchases()
    {
        List<SerializedPair<GoodNames, int>> values = Enum.GetValues(typeof(GoodNames))
            .Cast<GoodNames>()
            .Select(type => new SerializedPair<GoodNames, int>(type, 0)).ToList();

        return new Purchases(values);
    }
}