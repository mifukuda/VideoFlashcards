namespace Server.Models
{
    public class Deck
    {
        public int DeckId { get; set; }
        public int UserId { get; set; }
        public string DeckName { get; set; } = "";
        public Flashcard[] Flashcards { get; set; } = new Flashcard[0];
        public DateTime DeckCreated { get; set; }
        public DateTime DeckUpdated { get; set; }
    }
}