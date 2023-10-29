using FlashCardDemo.Models;

namespace FlashCardDemo.DAL
{
    public interface IFolderRepository
    {
        Task<IEnumerable<Folder>?> GetAll();
        Task<Folder?> GetFolderById(int folderid);
        Task <bool>Create(Folder folder);
        Task<bool> Update(Folder folder);
        Task<bool> Delete(int folderid);
    }
}
