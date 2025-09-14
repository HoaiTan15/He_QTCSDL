using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace QuanLyKho.Models
{
    public class ImportDetail
    {
        [Key, Column(Order = 0)]
        public int ReceiptID { get; set; }


        [Key, Column(Order = 1)]
        public int ProductID { get; set; }


        public int Quantity { get; set; }
        public decimal Price { get; set; }


        [ForeignKey("ReceiptID")]
        public virtual ImportReceipt ImportReceipt { get; set; }


        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
}