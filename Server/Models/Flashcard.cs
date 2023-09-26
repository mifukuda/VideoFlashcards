namespace Server.Models
{
    public class Flashcard
    {
        public int FlashcardId { get; set; }
        public int UserId { get; set; }
        public int DeckId { get; set; }
        public string FlashcardUrl { get; set; } = "";
        public string FlashcardTitle {get; set; } = "";
        public string FlashcardDescription { get; set; } = "";
        public DateTime FlashcardCreated { get; set; }
        public DateTime FlashcardUpdated { get; set; }
    }
}