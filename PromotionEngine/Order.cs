using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine
{
    //Inherits SKUId and Quantity reusable properties
    public class OrderSKUQuantity : SKUQuantity
    {
        public bool PromoApplied { get; set; }
    }

    //Order class to store order Items and Processing.
    public partial class Order
    {
        public Guid OrderId { get; set; }

        //List of SKus with Quantity in the cart
        public List<OrderSKUQuantity> SKUs { set; get; }

        //Total Value of cart, it will be zero before process, afetr process method call, It will be set to calculated value.
        public decimal TotalValue { private set; get; }

        //List of Promotions applied on Items in cart. This was to future purpose.
        public List<Promotion> AppliedPromotions { set; get; }

        /// <summary>
        /// Processing of Promotions on cart ietms, and set the calculated Value to Total value property 
        /// </summary>
        public void Process()
        {
            AppliedPromotions = new List<Promotion>();
            PromotionStore promotionStore = PromotionStore.GetStore();
            List<Promotion> lstPromotions = promotionStore.GetPromotions();
            List<OrderSKUQuantity> tempOrderSKUs = SKUs;
            TotalValue = 0;
            //llop through all promotions
            foreach (var item in lstPromotions)
            {
                decimal min = decimal.MaxValue;
                // Checking all Promotion SKUs are available for calculation or not by campring Qunatities of Order & Promotion, Status of OrderSKU(Promo Applied, Not applied)
                if (item.promotionSKUs.All(promoSKU => tempOrderSKUs.Exists(orderSku => orderSku.SKUId.ToLower() == promoSKU.SKUId.ToLower() &&
                    orderSku.Quantity >= promoSKU.Quantity && !orderSku.PromoApplied)))
                {
                    // Counting the combinations of Promo SKU's in Order SKU's
                    foreach (var promoSKU in item.promotionSKUs)
                    {
                        min = Math.Min(min, tempOrderSKUs.Where(orderSku => orderSku.SKUId.ToLower() == promoSKU.SKUId.ToLower() && !orderSku.PromoApplied)
                            .Min(orderSku => (int)orderSku.Quantity / (int)promoSKU.Quantity));
                    }
                    //Adding applied Promotion Values to Cart Value.
                    TotalValue += item.PromoValue * min;
                    //applying the unit Price of remained Quantity after Promotion is applied, set the PromoApplied flag to not to process same SKU, and also Quantity set to 0.
                    tempOrderSKUs.ForEach(orderSku =>
                    {
                        item.promotionSKUs.ForEach(promoSku =>
                        {
                            if (orderSku.SKUId.ToLower() == promoSku.SKUId.ToLower() && !orderSku.PromoApplied)
                            {
                                TotalValue += promotionStore.GetSKUItem(orderSku.SKUId).UnitPrice * (orderSku.Quantity % (promoSku.Quantity * min));
                                //orderSku.Quantity -= promoSku.Quantity * min;
                                orderSku.PromoApplied = true;
                            }
                        });
                    });
                    AppliedPromotions.Add(item);
                }
            }
            //Applying the unit price of SKU's which are not having promotions, Quantity is not zero.
            tempOrderSKUs.ForEach(obj =>
            {
                if (obj.Quantity != 0)
                    TotalValue += promotionStore.GetSKUItem(obj.SKUId).UnitPrice * obj.Quantity;
            });
        }
    }
}