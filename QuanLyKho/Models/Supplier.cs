using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace QuanLyKho.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }


        public virtual ICollection<ImportReceipt> ImportReceipts { get; set; }
    }
}