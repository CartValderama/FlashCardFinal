
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace FlashCardDemo.Models;
public class Deck
{
        public int DeckId { get; set; }
        [StringLength(15)]
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,15}", ErrorMessage = "The Name must be numbers or letters and between 2 to 15 characters.")]
        [DisplayName("Name")]
        public string DeckName { get; set; } = string.Empty;
        [DisplayName("Description")]
        [StringLength(120)]
        public string? DeckDescription { get; set; }
        public DateTime CreationDate { get; set; }
        [DisplayName("Select Folder")]
        public int? FolderId { get; set; }
        //navigation property
        public virtual Folder? Folder { get; set; }
        //navigation property
        public virtual List<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
}

