
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FlashCardDemo.Models
{
    public class Folder
    {
        public int FolderId { get; set; }
        [StringLength(15)]
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,15}", ErrorMessage = "The Name must be numbers or letters and between 2 to 15 characters.")]
        [DisplayName("Folder Name")]
        public string FolderName { get; set; } = string.Empty;
        [DisplayName("Description")]
        [StringLength(150)]
        public string? FolderDescription { get; set; } 

        public DateTime CreationDate { get; set; }
        // navigation property
        public virtual List<Deck> Decks { get; set; } = new List<Deck>();
    }
}
