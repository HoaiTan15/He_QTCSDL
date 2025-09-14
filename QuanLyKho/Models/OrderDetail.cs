using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace QuanLyKho.Models
{
    public class OrderDetail
    {
        [Key, Column(Order = 0)]
        public int OrderID { get; set; }
        [Key, Column(Order = 1)]
        public int ProductID { get; set; }


        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }


        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }


        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
}