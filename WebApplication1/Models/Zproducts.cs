namespace WebApplication1.Models
{
    public class Zproducts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }


        public string? Status { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }  
        
        public string? EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }


        public string? VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string? UnverifiedBy { get; set; }
        public DateTime? UnverifiedDate { get; set; }

      

    }
}
