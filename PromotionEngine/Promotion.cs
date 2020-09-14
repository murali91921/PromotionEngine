using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionEngine
{
    public partial class Promotion
    {
        //Name of Promotion
        public string Name { get; set; }
        //Id of Promotion
        public Guid PromotionId { get; set; }
        //List of SKUs with Quantity
        public List<PromotionSKU> promotionSKUs { get; set; }
        //Promotional Value
        public decimal PromoValue { get; set; }
    }

    public partial class PromotionSKU : SKUQuantity
    {
        //Additional Properties
    }

    public class SKUQuantity
    {
        public string SKUId { get; set; }

        public virtual decimal Quantity { get; set; }
    }

}
