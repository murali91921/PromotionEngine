using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace PromotionEngine.Tests
{
    [TestClass]
    public class PromotionTest
    {
        [TestMethod]
        public void TestPromotionStore()
        {
            PromotionStore promotionStore = PromotionStore.GetStore();
            PromotionStore promotionStore2 = PromotionStore.GetStore();
            Assert.AreSame(promotionStore, promotionStore2);
        }

        public static void AddSKUItems()
        {
            PromotionStore promotionStore = PromotionStore.GetStore();
            promotionStore.AddOrUpdateSKUItem(new SKUItem() { SKUId = "A", UnitPrice = 50 });
            promotionStore.AddOrUpdateSKUItem(new SKUItem() { SKUId = "B", UnitPrice = 30 });
            promotionStore.AddOrUpdateSKUItem(new SKUItem() { SKUId = "C", UnitPrice = 20 });
            promotionStore.AddOrUpdateSKUItem(new SKUItem() { SKUId = "D", UnitPrice = 15 });
        }

        public static void AddPromotions()
        {
            PromotionStore promotionStore = PromotionStore.GetStore();
            promotionStore.AddOrUpdatePromotion(new Promotion
            {
                PromotionId = Guid.NewGuid(),
                promotionSKUs = new List<PromotionSKU> {
                    new PromotionSKU
                    {
                        Quantity=3,
                        SKUId="A"
                    }
                },
                Name = "3 X A = 130",
                PromoValue = 130
            });
            promotionStore.AddOrUpdatePromotion(new Promotion
            {
                PromotionId = Guid.NewGuid(),
                promotionSKUs = new List<PromotionSKU> {
                new PromotionSKU{
                    Quantity=2,SKUId="B" }
                },
                Name = "2 X B = 45",
                PromoValue = 45
            });
            promotionStore.AddOrUpdatePromotion(new Promotion
            {
                PromotionId = Guid.NewGuid(),
                promotionSKUs = new List<PromotionSKU> {
                    new PromotionSKU
                    {
                        Quantity=1,
                        SKUId="C"
                    },
                    new PromotionSKU
                    {
                        Quantity=1,
                        SKUId="D"
                    }
                },
                Name = "C + D = 30",
                PromoValue = 30
            });
        }


        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            //Setting up SKU's and Promotions
            PromotionStore.GetStore();
            AddSKUItems();
            AddPromotions();
        }

        [TestMethod]
        public void TestOrder1()
        {
            Order order = new Order()
            {
                OrderId = Guid.NewGuid(),
                SKUs = new List<OrderSKUQuantity>
                {
                    new OrderSKUQuantity{SKUId="A",Quantity=1 },
                    new OrderSKUQuantity{SKUId="B",Quantity=1 },
                    new OrderSKUQuantity{SKUId="C",Quantity=1 },
                }
            };
            order.Process();
            Assert.AreEqual(order.TotalValue, 100);
        }

        [TestMethod]
        public void TestOrder2()
        {
            Order order = new Order()
            {
                OrderId = Guid.NewGuid(),
                SKUs = new List<OrderSKUQuantity>
                {
                    new OrderSKUQuantity{SKUId="A",Quantity=5 },
                    new OrderSKUQuantity{SKUId="B",Quantity=5 },
                    new OrderSKUQuantity{SKUId="C",Quantity=1 },
                }
            };
            order.Process();
            Assert.AreEqual(order.TotalValue, 370);
        }

        [TestMethod]
        public void TestOrder3()
        {
            Order order = new Order()
            {
                OrderId = Guid.NewGuid(),
                SKUs = new List<OrderSKUQuantity>
                {
                    new OrderSKUQuantity{SKUId="A",Quantity=3 },
                    new OrderSKUQuantity{SKUId="B",Quantity=5 },
                    new OrderSKUQuantity{SKUId="C",Quantity=1 },
                    new OrderSKUQuantity{SKUId="D",Quantity=1 },
                }
            };
            order.Process();
            Assert.AreEqual(order.TotalValue, 280);
        }

        [TestMethod]
        public void TestOrder4()
        {
            Order order = new Order()
            {
                OrderId = Guid.NewGuid(),
                SKUs = new List<OrderSKUQuantity>
                {
                    new OrderSKUQuantity{SKUId="A",Quantity=10 },
                    new OrderSKUQuantity{SKUId="B",Quantity=5 },
                    new OrderSKUQuantity{SKUId="C",Quantity=3 },
                    new OrderSKUQuantity{SKUId="D",Quantity=1 },
                }
            };
            order.Process();
            //130+130+130+50
            //45+45+30
            //30+ 20*20
            //--
            Assert.AreEqual(order.TotalValue, 590);
        }
    }
}