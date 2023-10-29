using FlashCardDemo.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace FlashCardDemo.DAL;

public class FolderRepository : IFolderRepository
{
    private readonly FlashcardDbContext _db;
    private readonly ILogger<FolderRepository> _logger;

    public FolderRepository(FlashcardDbContext db, ILogger<FolderRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Folder>?> GetAll()
    {
        try
        {
            return await _db.Folders.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[FolderRepository] Failed to get all folders, error message: {Message}", e.Message);
            return null;
        }
    }
    public async Task<Folder?> GetFolderById(int folderid)
    {
        try
        {
            return await _db.Folders.FindAsync(folderid);
        }
        catch(Exception e)
        {
            _logger.LogError("[FolderRepository] Failed to get folder with ID {Folder}, error message: {ErrorMessage}", folderid, e.Message);
            return null;
        }
        
    }
    public async Task<bool> Create(Folder folder)
    {
        try
        {
            folder.CreationDate = DateTime.Now;
            _db.Folders.Add(folder);
            await _db.SaveChangesAsync();
            return true;
        }

        catch (Exception e)
        {
            _logger.LogError("[FolderRepository] Folder creation failed for folder {FolderName}, error message: {ErrorMessage}", folder.FolderId, e.Message);
            return false;
        }
    }
    public async Task<bool> Update(Folder folder)
    {
        try
        {
            _db.Folders.Update(folder);
            await _db.SaveChangesAsync();
            return true;
        }catch(Exception e)
        {
            _logger.LogError("[FolderRepository] Folder update failed for folder {FolderName}, error message: {ErrorMessage}", folder.FolderId, e.Message);
            return false;
        }
    }
    public async Task<bool> Delete(int folderid)
    {
        try
        {
            var folder = await _db.Folders.FindAsync(folderid);
            if (folder == null)
                return false;

            List<Deck> decks = await _db.Decks.ToListAsync(); // list all decks
            foreach (var deck in decks)
            {
                if (deck.FolderId == folderid)
                    _db.Decks.Remove(deck);
            }
            _db.Folders.Remove(folder);
            await _db.SaveChangesAsync();
            return true;
        }catch (Exception e)
        {
            _logger.LogError("[FolderRepository] Folder deletion failed for folder ID {FolderId}, error message: {ErrorMessage}", folderid, e.Message);
            return false;
        }
    }
}
