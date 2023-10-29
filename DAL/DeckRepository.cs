using FlashCardDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashCardDemo.DAL;

public class DeckRepository : IDeckRepository
{
    private readonly FlashcardDbContext _db;
    private readonly ILogger<DeckRepository> _logger;

    public DeckRepository(FlashcardDbContext db, ILogger<DeckRepository> logger)
    {
        _db = db;
        _logger = logger;
    }
    public async Task<IEnumerable<Deck>?> GetAll()
    {
        try
        {
            return await _db.Decks.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("Error retrieving all decks: {ErrorMessage}", e.Message);
            return null;
        }
    }
    public async Task<Deck?> GetDeckById(int deckid)
    {
        try
        {
            return await _db.Decks.FindAsync(deckid);
        }
        catch (Exception e)
        {
            _logger.LogError("Error retrieving deck with ID {DeckId}: {ErrorMessage}", deckid, e.Message);
            return null;

        }
    }
    public async Task <bool> Create(Deck deck)
    {
        try
        {
            deck.CreationDate = DateTime.Now;
            _db.Decks.Add(deck);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error creating deck: {ErrorMessage}", e.Message);
            return false;
        }
    }
    public async Task <bool>Update(Deck deck)
    {
        try
        {
            _db.Decks.Update(deck);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("ERror updating deck with ID {DeckId}: {ErrorMessage}", deck.DeckId, e.Message);
            return false;
        }
    }
    public async Task<bool> Delete(int deckid)
    {
        try
        {
            var deck = await _db.Decks.FindAsync(deckid);
            if (deck == null)
                return false;

            _db.Decks.Remove(deck);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error deleting deck with ID {DeckId}: {ErrorMessage}", deckid, e.Message);
            return false;
        }
    }
}
