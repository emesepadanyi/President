namespace President.DAL.Entities
{
    public class Game
    {
        public int ID { get; set; }
        public bool Available { get; set; }
        public bool Finished { get; set; }
        public string Name { get; set; }
    }
}
