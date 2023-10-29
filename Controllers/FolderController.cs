using FlashCardDemo.DAL;
using FlashCardDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlashCardDemo.Controllers;

public class FolderController : Controller
{
    private readonly IFolderRepository _folderRepository;
    private readonly ILogger<FolderController> _logger;
    public FolderController(IFolderRepository folderRepository, ILogger<FolderController> logger)
    {
        _folderRepository = folderRepository;
        _logger = logger;
    }

    /* ------------------ Display ------------------------ */
    public async Task<IActionResult> ListFolder()
    {
        try
        {
            var folders = await _folderRepository.GetAll();
            return View(folders);
        }
        catch (Exception e)
        {
            _logger.LogError("Error fetching folder list: {ErrorMessage}", e.Message);
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> FolderDashboard(int folderid) // this will list all deck inside a specific folder
    {
        try
        {
            var findFolder = await _folderRepository.GetFolderById(folderid);
            if (findFolder == null)
                return RedirectToAction("ListFolderAndDeck", "Library", new { area = "" });
            return View(findFolder);
        }
        catch (Exception e)
        {
            _logger.LogError("Error fetching folder dashboard for folder ID {folderid}: {ErrorMessage}", folderid, e.Message);
            return NotFound();
        }  
    }

    /* ------------------------------------- CREATE UPDATE DELETE (CRUD) --------------------------------------- */

    /* ------------- CREATE -------------- */
    [Authorize]
    [HttpGet]
    public IActionResult CreateFolder()
    {
        return View();
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateFolder(Folder folder)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _folderRepository.Create(folder);
                return RedirectToAction("ListFolderAndDeck", "Library", new { area = "" }); // return to the library view ListFolderAndDeck
            }
            return RedirectToAction("Folder", "CreateFolder", new { area = "" });
        }
        catch (Exception e)
        {
            _logger.LogError("Error when creating folder: {ErrorMessage}", e.Message);
        }
        return NotFound();
    }

    /* ------------- UPDATE -------------- */
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> UpdateFolder(int folderid)
    {
        try
        {
            var folder = await _folderRepository.GetFolderById(folderid);
            if (folder == null)
            {
                return NotFound();
            }
            return View(folder);
        }
        catch (Exception e)
        {
            _logger.LogError("Error updating folder with ID {FolderId}: {ErrorMessage}", folderid, e.Message);

            return NotFound();  
        }
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateFolder(Folder folder)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _folderRepository.Update(folder);
                return RedirectToAction("FolderDashboard", "Folder", new { folderid = folder.FolderId });
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error updating folder with ID {FolderId}: {ErrorMessage}", folder.FolderId, e.Message);

        }
        return View(folder);
    }

    /* ------------- DELETE -------------- */
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> DeleteFolder(int folderid)
    {
        try
        {
            var folder = await _folderRepository.GetFolderById(folderid);
            if (folder == null)
                return NotFound();
            return View(folder);
        }
        catch (Exception e)
        {
            _logger.LogError("Error fetching folder for deletion with ID {FodlerId}: {ErrorMessage}", folderid, e.Message);
            return NotFound();
        }
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmedFolder(int folderid)
    {
        try
        {
            await _folderRepository.Delete(folderid);
            return RedirectToAction("ListFolderAndDeck", "Library", new { area = "" }); // return to the library view ListFolderAndDeck
        }
        catch (Exception e)
        {
            _logger.LogError("Error confirming folder deletion with ID {FolderId}: {ERrorMessage}", folderid, e.Message);
            return NotFound();
        }
    }
}

