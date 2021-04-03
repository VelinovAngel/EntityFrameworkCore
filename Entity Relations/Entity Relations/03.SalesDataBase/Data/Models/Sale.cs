namespace P03_SalesDatabase.Data.Models
{
    using System;

    public class Sale
    {
        public int SaleId { get; set; }
        //	SaleId

        public DateTime Date { get; set; }
        //	Date

        public int ProductId { get; set; }
        //	Product
        public Product Product { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }
        //	Customer

        public int StoreId { get; set; }

        public Store Store { get; set; }
        //	Store

    }
}
