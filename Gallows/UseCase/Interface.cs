using Gallows.Models;

namespace Gallows.UseCase;

public interface IUseCase
{
    void SaveGame();
    bool LoadSavedGame();
    void SaveSettings();
    bool LoadSavedSettings();
    void StartNewGame();
    void NextWord();
    GameState GetState();
    string[] GetWordsCategories();
    void MakeMove(char letter);
}