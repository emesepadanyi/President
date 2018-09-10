namespace President.DAL.Entities
{
    public class Relationship
    {
        public int ID { get; set; }
        public User Sender { get; set; }
        public User Reciever { get; set; }
        public string Status { get; set; }
    }
}
