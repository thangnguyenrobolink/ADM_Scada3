using System;

namespace ADM_Scada.Core.Models
{
    // Product class representing the product table
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProdCode { get; set; }
        public string ProdFullName { get; set; }
        public string HashCode { get; set; }
        public string Ingredient { get; set; }
        public decimal? Exp { get; set; }
        public string Market { get; set; }
        public string ProdName { get; set; }
        public string LabelPath { get; set; }
        public string Barcode { get; set; }
        public string DelayM4 { get; set; }
        public string DelayM5 { get; set; }
        public decimal PackSize { get; set; }
        public string LooseUom { get; set; }
        public string WholeUom { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
