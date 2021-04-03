namespace P03_SalesDatabase.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Product
    {
        public Product()
        {
            this.Sales = new HashSet<Sale>();
        }

        public int ProductId { get; set; }
        //	ProductId

        [Column(TypeName = "NVARCHAR(50)")]
        public string Name { get; set; }
        //	Name(up to 50 characters, unicode)

        public double Quantity { get; set; }
        //	Quantity(real number)

        public decimal Price { get; set; }
        //	Price

        [Column(TypeName = "NVARCHAR(250)")]
        [DefaultValue("No description")]
        public string Description { get; set; }

        public ICollection<Sale> Sales { get; set; }
        //	Sales

    }
}
