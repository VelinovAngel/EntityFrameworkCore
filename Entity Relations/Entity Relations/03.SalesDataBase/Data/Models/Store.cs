namespace P03_SalesDatabase.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Store
    {
        public Store()
        {
            this.Sales = new HashSet<Sale>();
        }
        public int StoreId { get; set; }
        //	StoreId

        [Column(TypeName = "NVARCHAR(80)")]
        public string Name { get; set; }
        //	Name(up to 80 characters, unicode)

        public ICollection<Sale> Sales { get; set; }
        //	Sales

    }
}
