using FlashCardDemo.DAL;
using FlashCardDemo.Models;
using FlashCardDemo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FlashCardDemo.Controllers;

public class DeckController : Controller
{
    private readonly IDeckRepository _deckRepository;
    private readonly IFolderRepository _folderRepository;
    private readonly ILogger<DeckController> _logger;

    public DeckController(IDeckRepository deckRepository, IFolderRepository folderRepository, ILogger<DeckController> logger)
    {
        _deckRepository = deckRepository;
        _folderRepository = folderRepository;
        _logger = logger;
    }

    /* ------------------ Display ------------------- */
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> DeckDashboard(int deckid)
    {
        try
        {
            var findDeck = await _deckRepository.GetDeckById(deckid); // find the deck
            if (findDeck == null)
                return NotFound();
            return View(findDeck);
        }
        catch(Exception e)
        {
            _logger.LogError("Error retrieving deck dashboard for deck ID {DeckId}: {ErrorMessage}", deckid, e.Message);
            return BadRequest("An error occurred while retrieving the deck dashboard.");

        }
    }
    /* ------------------------------------- CREATE UPDATE DELETE (CRUD) --------------------------------------- */

    /* ---------------------- CREATE ------------------------- */
    /* Create A Deck With A Folder  */
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> CreateDeckInFolder(int folderid)
    {
        try
        {
            var findFolder = await _folderRepository.GetFolderById(folderid);
            if (findFolder == null)
                return NotFound();
            var deck = new Deck { FolderId = findFolder.FolderId };
            return View(deck);
        }
        catch (Exception e)
        {
            _logger.LogError("Error creating deck in folder {FolderID} and error: {ErrorMessage}", folderid, e.Message);
            return BadRequest("Deck creation failed");
        }
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateDeckInFolder(Deck deck)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _deckRepository.Create(deck);
                if (deck.FolderId == null) // checks if the folder belongs to a folder, if not then return to the library view ListFolderAndDeck
                    return RedirectToAction("ListFolderAndDeck", "Library", new { area = "" });
                return RedirectToAction("FolderDashboard", "Folder", new { folderid = deck.FolderId }); // return to folder dashboard if the deck is inside a folder
            }
            return View(deck); // stay on the current page if the inputs are invalid
        }
        catch (Exception e)
        {
            _logger.LogError("Error creating deck in folder: {ErrorMessage}", e.Message);
            return BadRequest("Deck creation failed");
        }
    }
    /* Create A Deck Without A Folder  */
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> CreateDeck()
    {
        try
        {
            var folders = await _folderRepository.GetAll();

            if (folders == null)
            {
                _logger.LogError("Folders retrieved returned as null.");
                return BadRequest("Unable to retrieve folders.");
            }
            var createDeckViewModel = new CreateDeckViewModel
            {
                Deck = new Deck(),
                // does not have folder
                FolderSelectList = folders.Select(folder => new SelectListItem
                {
                    Value = folder.FolderId.ToString(),
                    Text = folder.FolderId.ToString() + ": " + folder.FolderName
                }).ToList(),
            };
            return View(createDeckViewModel);
        }
        catch (Exception e)
        {
            _logger.LogError("Error initializing deck creation view: {ErrorMessage}", e.Message);
            return BadRequest("An error occurred while initializing the deck creation view.");

        }
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateDeck(CreateDeckViewModel createDeckViewModel)
    {

        try
        {
            if (ModelState.IsValid)
            {
                await _deckRepository.Create(createDeckViewModel.Deck);
                if (createDeckViewModel.Deck.FolderId == null) // checks if the folder belongs to a folder, if not then return to the library view ListFolderAndDeck
                    return RedirectToAction("ListFolderAndDeck", "Library", new { area = "" });
                return RedirectToAction("FolderDashboard", "Folder", new { folderid = createDeckViewModel.Deck.FolderId }); // return to ListDeck if the deck is inside a folder

            }
            return View(createDeckViewModel); // stay on the current page if the inputs are invalid
        }
        catch (Exception e)
        {
            _logger.LogError("Error creating deck: {ErrorMessage}", e.Message);
            return BadRequest("Deck creation failed");
        }
    }
    /* ------------- UPDATE -------------- */
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> UpdateDeck(int deckid)
    {
        try
        {
            var findDeck = await _deckRepository.GetDeckById(deckid);
            var folders = await _folderRepository.GetAll();

            if (findDeck == null)
            {
                return NotFound();
            }

            if (folders == null)
            {
                _logger.LogError("Folders retrieved returned as null.");
                return BadRequest("Unable to retrieve folders.");
            }
            var createDeckViewModel = new CreateDeckViewModel
            {
                Deck = findDeck,
                FolderSelectList = folders.Select(folder => new SelectListItem
                {
                    Value = folder.FolderId.ToString(),
                    Text = folder.FolderId.ToString() + ": " + folder.FolderName
                }).ToList()
            };
            return View(createDeckViewModel);
        }
        catch (Exception e)
        {
            _logger.LogError("Error initializing update view for deck ID {DEckId}: {ErrorMessage}", deckid, e.Message);
            return BadRequest("An error occurred while initializing the deck update view.");
        }
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateDeck(CreateDeckViewModel createDeckViewModel)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _deckRepository.Update(createDeckViewModel.Deck);
                return RedirectToAction("DeckDashboard", "Deck", new { deckid = createDeckViewModel.Deck.DeckId }); // return to ListDeck if the deck is inside a folder
            }
            return View(createDeckViewModel);
        }
        catch (Exception e)
        {
            _logger.LogError("Error updating deck with ID {DeckId}: {ErrorMessage}", createDeckViewModel.Deck.DeckId, e.Message);
            return BadRequest("Deck update failed");
        }
    }

    /* ------------- DELETE -------------- */
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> DeleteDeck(int deckid)
    {
        try
        {
            var deck = await _deckRepository.GetDeckById(deckid);
            if (deck == null)
                return NotFound();
            return View(deck);
        }
        catch (Exception e)
        {
            _logger.LogError("Error delering deck with deckid {DEckId}, error: {ErrorMessage}", deckid, e.Message);
            return BadRequest("Deck deletion failed");
        }
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmedDeck(int deckid)
    {
        try
        {
            var deck = await _deckRepository.GetDeckById(deckid);
            if (deck == null)
                return NotFound();
            await _deckRepository.Delete(deckid);
            if (deck.FolderId == null) // checks if the folder belongs to a folder, if not then return to the library view ListFolderAndDeck
                return RedirectToAction("ListFolderAndDeck", "Library", new { area = "" });
            return RedirectToAction("FolderDashboard", "Folder", new { folderid = deck.FolderId }); // return to deck dashboard if the deck is inside a folder
        }
        catch (Exception e)
        {
            _logger.LogError("Error delering deck with deckid {DEckId}, error: {ErrorMessage}", deckid, e.Message);
            return BadRequest("Deck deletion failed");
        }
    }
}
