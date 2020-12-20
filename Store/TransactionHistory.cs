namespace Store
{
    public class TransactionHistory<T> : IIdentifiable
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public T Value { get; set; }
    }
}