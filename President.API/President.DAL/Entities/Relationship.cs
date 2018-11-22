namespace President.DAL.Entities
{
    public class Relationship
    {
        public int ID { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public string Status { get; set; }
    }
}
