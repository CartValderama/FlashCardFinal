using FlashCardDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashCardDemo.DAL;

public class FlashcardRepository : IFlashcardRepository
{
    private readonly FlashcardDbContext _db;
    private readonly ILogger<FlashcardRepository> _logger;

    public FlashcardRepository(FlashcardDbContext db, ILogger<FlashcardRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Flashcard>?> GetAll()
    {
        try
        {
            return await _db.Flashcards.ToListAsync();
        }
        catch(Exception e)
        {
            _logger.LogError("Error fetching all flashcards: {Errormessage}", e.Message);
            // ide-suggestion
            return Enumerable.Empty<Flashcard>();
        }
        
    }
    public async Task<Flashcard?> GetFlashcardById(int flashcardid)
    {
        try
        {
            return await _db.Flashcards.FindAsync(flashcardid);
        }
        catch (Exception e)
        {
            _logger.LogError("Error fetching flashcard with ID {FlashcardId}: {ErrorMessage}", flashcardid, e.Message);
            return null;
        }

    }
    public async Task <bool>Create(Flashcard flashcard)
    {
        try
        {
            flashcard.CreationDate = DateTime.Now;
            _db.Flashcards.Add(flashcard);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("ERror creating flashcard: {ErrorMessage}", e.Message);
            return false;
        }
    }
    public async Task <bool>Update(Flashcard flashcard)
    {
        try
        {
            _db.Flashcards.Update(flashcard);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error updating flashcard with ID {FlashcardId}: {ErrorMessage}", flashcard.FlashcardId, e.Message);
            return false;
        }
    }
    public async Task<bool> Delete(int flashcardid)
    {
        try
        {
            var flashcard = await _db.Flashcards.FindAsync(flashcardid);
            if (flashcard == null)
                return false;

            _db.Flashcards.Remove(flashcard);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error deleting flashcard with ID {FlashcardId}: {ErrorMessage}", flashcardid, e.Message);
            return false;
        }
    }
}
