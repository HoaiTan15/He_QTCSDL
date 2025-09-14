using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace QuanLyKho.Models
{
    public class ImportReceipt
    {
        [Key]
        public int ReceiptID { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;


        public int? StaffID { get; set; }
        [ForeignKey("StaffID")]
        public virtual Staff Staff { get; set; }


        public int? SupplierID { get; set; }
        [ForeignKey("SupplierID")]
        public virtual Supplier Supplier { get; set; }


        public virtual ICollection<ImportDetail> ImportDetails { get; set; }
    }
}