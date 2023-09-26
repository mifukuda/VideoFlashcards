namespace Server.Models
{
    public class Deck
    {
        public int DeckId { get; set; }
        public int UserId { get; set; }
        public string DeckName { get; set; } = "";
        public DateTime DeckCreated { get; set; }
        public DateTime DeckUpdated { get; set; }
    }
}