using FlashCardDemo.DAL;
using FlashCardDemo.Models;
using FlashCardDemo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FlashCardDemo.Controllers;

public class FlashcardController : Controller
{
    private readonly IFlashcardRepository _flashcardRepository;
    private readonly IDeckRepository _deckRepository;
    private readonly ILogger<FlashcardController> _logger;

    public FlashcardController(IFlashcardRepository flashcardRepository, IDeckRepository deckRepository, ILogger<FlashcardController> logger)
    {
        _flashcardRepository = flashcardRepository;
        _deckRepository = deckRepository;
        _logger = logger;
    }

    /* ------------------ Display ------------------- */
    [HttpGet]
    public async Task<IActionResult> FlashcardTypeOne(int deckid)
    {
        try
        {
            var findDeck = await _deckRepository.GetDeckById(deckid); // find the deck
            if (findDeck == null)
                return NotFound();
            return View(findDeck);
        }
        catch (Exception e){
            _logger.LogError("Error fetching FlashcardTypeOne with Deck ID {DeckId}: {ErrorMessage}", deckid, e.Message);
            return BadRequest("Error fetching FlashcardTypeOne.");
        }
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> FlashcardTypeTwo(int deckid)
    {
        try
        {
            var findDeck = await _deckRepository.GetDeckById(deckid); // find the deck
            if (findDeck == null)
                return NotFound();
            return View(findDeck);
        }
        catch(Exception e){
            _logger.LogError("Error fetching FlashcardTypeTwo with Deck ID {DeckId}: {ErrorMessage}", deckid, e.Message);
            return BadRequest("Error fetching FlashcardTypeTwo.");
        }
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> FlashcardTypeThree(int deckid)
    {
        try
        {
            var findDeck = await _deckRepository.GetDeckById(deckid); // find the deck
            if (findDeck == null)
                return NotFound();
            return View(findDeck);
        }
        catch (Exception e)
        {
            _logger.LogError("Error fetching FlashcardTypeTwo with Deck ID {DeckId}: {ErrorMessage}", deckid, e.Message);
            return BadRequest("Error fetching FlashcardTypeTwo.");
        }
    }


    /* ------------------------------------- CREATE UPDATE DELETE (CRUD) --------------------------------------- */

    /* ------------- CREATE -------------- */
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> CreateFlashcard(int deckid)
    {
        try
        {
            var findDeck = await _deckRepository.GetDeckById(deckid);

            if (findDeck == null)
                return NotFound();

            var flashcard = new Flashcard
            {
                Deck = findDeck,
                DeckId = findDeck.DeckId
            };
            return View(flashcard);
        }
        catch (Exception e)
        {
            _logger.LogError("Error initializing flashcard creation view for Deck ID {DeckId}: {ErrorMessage}", deckid, e.Message);
            return BadRequest("Error initializing flashcard creation view.");

        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateFlashcard(Flashcard flashcard)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _flashcardRepository.Create(flashcard);
                return RedirectToAction("DeckDashboard", "Deck", new { deckid = flashcard.DeckId }); // return to ListFlashcard if the deck is inside a folder
            }
            return View(flashcard);

        }
        catch (Exception e)
        {
            _logger.LogError("Error creating flashcard: {ErrorMessage}", e.Message);
            return BadRequest("Flashcard creation failed.");
        }
    }

    /* ------------- UPDATE -------------- */
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> UpdateFlashcard(int flashcardid)
    {
        try
        {
            var flashcard = await _flashcardRepository.GetFlashcardById(flashcardid);
            if (flashcard == null)
                return NotFound();
            return View(flashcard);
        }
        catch (Exception e)
        {
            _logger.LogError("Error updating flashcard with ID {FlashcardId} for update: {ErrorMessage}", flashcardid, e.Message);
            return BadRequest("Error updating flashcard.");
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateFlashcard(Flashcard flashcard)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _flashcardRepository.Update(flashcard);
                return RedirectToAction("DeckDashboard", "Deck", new { deckid = flashcard.DeckId }); // return to ListFlashcard if the deck is inside a folder
            }
            return View(flashcard);
        }
        catch (Exception e) {
            _logger.LogError("Error updating flashcard with ID {FlashcardId}: {ErrorMessage}", flashcard.FlashcardId, e.Message);
            return BadRequest("Flashcard update failed.");
        }
    }

    /* ------------- DELETE -------------- */
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> DeleteFlashcard(int flashcardid)
    {
        try
        {
            var flashcard = await _flashcardRepository.GetFlashcardById(flashcardid);
            if (flashcard == null)
                return NotFound();
            return View(flashcard);
        }
        catch (Exception e)
        {
            _logger.LogError("Error fetching flashcard with ID {FlashcardId} for deletion: {ErrorMessage}", flashcardid, e.Message);
            return BadRequest("Error fetching flashcard for deletion.");
        }
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmedFlashcard(int flashcardid, int deletedeckid)
    {
        try
        {
            await _flashcardRepository.Delete(flashcardid);
            return RedirectToAction("DeckDashboard", "Deck", new { deckid = deletedeckid }); // return to ListFlashcard 
        }
        catch (Exception e)
        {
            _logger.LogError("Error deleting flashcard with ID {FlashcardId}: {ErrorMessage}", flashcardid, e.Message);
            return BadRequest("Flashcard deletion failed.");
        }
    }
}
