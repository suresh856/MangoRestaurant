using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Sevices.OrderAPI.Models
{
    public class OrderDetails
    {
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public virtual OrderHeader OrderHeader { get; set; }


        public int ProductId { get; set; }

        // We can have product table if needed
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }


    }
}
