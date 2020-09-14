using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
