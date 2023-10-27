using Gallows.Models;

namespace Gallows.UseCase;

public interface IUseCase
{
    void SaveGame();
    bool LoadSavedGame();
    void SaveSettings(string category, int difficulty);
    bool LoadSavedSettings();
    void StartNewGame();
    void NextWord();
    GameState GetState();
    string[] GetWordsCategories();
    void MakeMove(char letter);
}