using FlashCardDemo.Models;

namespace FlashCardDemo.DAL;

public interface IDeckRepository
{
    Task<IEnumerable<Deck>?> GetAll();
    Task<Deck?> GetDeckById(int deckid);
    Task <bool>Create(Deck deck);
    Task <bool>Update(Deck deck);
    Task<bool> Delete(int deckid);
}
