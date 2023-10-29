using System.ComponentModel.DataAnnotations;

namespace FlashCardDemo.Models
{
    public class Flashcard
    {
        public int FlashcardId { get; set; }
        [StringLength(90)]
        public string Question { get; set; } = string.Empty;
        [StringLength(90)]
        public string Answer { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public int DeckId { get; set; }
        // navigation property
        public virtual Deck? Deck { get; set; }
    }
}
