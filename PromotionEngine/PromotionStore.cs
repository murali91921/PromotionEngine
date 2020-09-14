using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine
{
    //Singleton class to store and retrieve the SKU's and Promotions
    public class PromotionStore
    {
        //Singleton object
        private static PromotionStore _promotionStore;
        //List of SKU's with Unit price
        private List<SKUItem> SKUItems { get; set; }
        //List of Promotions available
        private List<Promotion> Promotions { get; set; }

        protected PromotionStore()
        {

        }

        //get singleton object
        public static PromotionStore GetStore()
        {
            if (_promotionStore == null)
            {
                _promotionStore = new PromotionStore();
                _promotionStore.SKUItems = new List<SKUItem>();
                _promotionStore.Promotions = new List<Promotion>();
            }
            return _promotionStore;
        }

        //Add or Update Promotion
        public void AddOrUpdatePromotion(Promotion promotion)
        {
            if (Promotions == null)
                Promotions = new List<Promotion>();
            else if (Promotions.Exists(obj => obj.PromotionId == promotion.PromotionId))
                Promotions.RemoveAll(obj => obj.PromotionId == promotion.PromotionId);
            Promotions.Add(promotion);
        }

        //Add or Update SKUItem
        public void AddOrUpdateSKUItem(SKUItem sKUItem)
        {
            if (SKUItems == null)
                SKUItems = new List<SKUItem>();
            else if (SKUItems.Exists(obj => obj.SKUId == sKUItem.SKUId))
                SKUItems.ForEach(obj =>
                {
                    if (obj.SKUId == sKUItem.SKUId)
                        obj.UnitPrice = sKUItem.UnitPrice;
                });
            else
                SKUItems.Add(sKUItem);
        }

        // Get all promotions
        public List<Promotion> GetPromotions() => Promotions;

        //Get all SKUs
        public List<SKUItem> GetSKUItems() => SKUItems;

        //Get a SKU by SKUId
        public SKUItem GetSKUItem(string SKUId) => SKUItems.FirstOrDefault(obj => obj.SKUId.ToLower() == SKUId.ToLower());
    }
}
