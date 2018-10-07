using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        
        [Required, MaxLength(128)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        public int? SupplierId { get; set; }
        
        public int? CategoryId { get; set; }
        
        [Display(Name = "Quantity Per Unit")]
        public string QuantityPerUnit { get; set; }
        
        [Display(Name = "Unit Price")]
        [Range(0, 999.99)]
        public decimal? UnitPrice { get; set; }
        
        [Display(Name = "Units In Stock")]
        public short? UnitsInStock { get; set; }
        
        [Display(Name = "Units On Order")]
        public short? UnitsOnOrder { get; set; }
        
        [Display(Name = "Reorder Level")]
        public short? ReorderLevel { get; set; }
        
        [Display(Name = "Discontinued")]
        public bool Discontinued { get; set; }

        public Category Category { get; set; }
        public Supplier Supplier { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
