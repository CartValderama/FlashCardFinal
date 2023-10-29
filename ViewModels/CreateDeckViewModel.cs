using FlashCardDemo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlashCardDemo.ViewModels;

public class CreateDeckViewModel
{
    public Deck Deck { get; set; } = default!;
    public Folder? Folder { get; set; }
    public List<SelectListItem>? FolderSelectList { get; set; }
}
