using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shops;

public class ShopTests
{
    [Test]
    public void ThrowExceptionOnBuy()
    {
        Assert.Catch(
            typeof(IndexOutOfRangeException),
            () =>
            {
                (Wallet wallet, Shop shop, ShopView shopView, ShopPresenter presenter) bundle = CreateBundle();
                bundle.shop.Buy(GoodNames.ReloadSpeed);
                bundle.presenter.OnGoingBuy(new TestCard(GoodNames.ReloadSpeed));
            });
    }

    [Test]
    public void CanBuy()
    {
        (Wallet wallet, Shop shop, ShopView shopView, ShopPresenter presenter) bundle = CreateBundle();
        bundle.presenter.OnGoingBuy(new TestCard(GoodNames.ReloadSpeed));
        Assert.Pass();
    }

    private Dictionary<GoodNames, List<(object value, int price)>> CreateTestGoodsContent()
    {
        return new ()
        {
            {
                GoodNames.AmmoCount,
                new ()
                {
                    (10, 0),
                    (12, 122)
                }
            },
            {
                GoodNames.ReloadSpeed,
                new ()
                {
                    (3f, 0),
                    (2.7f, 122)
                }
            },
        };
    }

    private Purchases<int> CreateStartPurchases()
    {
        List<SerializedPair<GoodNames, int>> values = new ()
        {
            new (GoodNames.AmmoCount, 0),
            new (GoodNames.ReloadSpeed, 0),
        };
        return new Purchases<int>(values);
    }

    private (Wallet wallet, Shop shop, ShopView shopView, ShopPresenter presenter) CreateBundle()
    {
        Wallet wallet = new Wallet(new CurrencyData());
        Shop shop = new Shop(CreateTestGoodsContent(), CreateStartPurchases());
        ShopView shopView = new ShopView();
        ShopPresenter presenter = new ShopPresenter(wallet, shop, shopView);
        return (wallet, shop, shopView, presenter);
    }
}