using FlashCardDemo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlashCardDemo.ViewModels
{
    public class FolderAndDeckListViewModel
    {
        public Folder? Folder { get; set; }
        public IEnumerable<Deck> Decks { get; set; } = Enumerable.Empty<Deck>();
        public IEnumerable<Folder> Folders { get; set; } = Enumerable.Empty<Folder>();
    }
}
