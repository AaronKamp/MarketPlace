namespace TCCMarketPlace.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int TransactionId { get; set; }
        public bool Status { get; set; }
    }
}
