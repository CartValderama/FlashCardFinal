using FlashCardDemo.Models;

namespace FlashCardDemo.DAL;

public interface IFlashcardRepository
{
    Task<IEnumerable<Flashcard>?> GetAll();
    Task<Flashcard?> GetFlashcardById(int flashcardid);
    Task <bool>Create(Flashcard flashcard);
    Task <bool> Update(Flashcard flashcard);
    Task<bool> Delete(int flashcardid);
}
