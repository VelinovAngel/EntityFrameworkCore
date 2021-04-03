namespace P03_SalesDatabase.Data.Models
{

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Customer
    {
        public Customer()
        {
            this.Sales = new HashSet<Sale>();
        }

        public int CustomerId { get; set; }
        //	CustomerId

        [Column(TypeName = "NVARCHAR(100)")]
        public string Name { get; set; }
        //	Name(up to 100 characters, unicode)

        [Column(TypeName = "VARCHAR(80)")]
        public string Email { get; set; }
        //	Email(up to 80 characters, not unicode)

        public string CreditCardNumber { get; set; }
        //	CreditCardNumber(string)

        public ICollection<Sale> Sales { get; set; }
        //	Sales

    }
}
