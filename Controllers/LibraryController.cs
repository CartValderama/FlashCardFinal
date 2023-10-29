using FlashCardDemo.DAL;
using FlashCardDemo.Models;
using FlashCardDemo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlashCardDemo.Controllers;

public class LibraryController: Controller
{
    private readonly FlashcardDbContext _flashcardDbContext;
    private readonly ILogger<LibraryController> _logger;

    public LibraryController(FlashcardDbContext flashcardDbContext, ILogger<LibraryController> logger)
    {
        _flashcardDbContext = flashcardDbContext;
        _logger = logger;
    }
    /* -------------- Display ---------------- */
    public async Task<IActionResult> ListFolderAndDeck()
    {
        try
        {
            List<Folder> folders = await _flashcardDbContext.Folders.ToListAsync();
            List<Deck> decks = await _flashcardDbContext.Decks.ToListAsync(); // list all decks
            List<Deck> validDecks = new(); // temporary storage
            foreach (var deck in decks)
            {
                if (deck.FolderId == null)
                    validDecks.Add(deck); // store every deck with folder id inside the temporary storage
            }
            var folderAndDeckListViewModel = new FolderAndDeckListViewModel
            {
                Decks = validDecks,
                Folders = folders
            };
            return View(folderAndDeckListViewModel);

        }catch(Exception e)
        {
            _logger.LogError("Error fetching ListFolderAndDeck: {ErrorMessage}", e.Message);
            return BadRequest("Error fetching ListFolderAndDeck.");
        }
    }

}
