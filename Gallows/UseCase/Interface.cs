using Gallows.Models;

namespace Gallows.UseCase;

public interface IUseCase
{
    void SaveGame();
    bool LoadSavedGame();
    void SaveSettings(string category, int difficulty);
    void StartNewGame();
    void NextWord();
    GameState GetState();
    string[] GetWordsCategories();
    void MakeMove(char letter);
    void SaveResult(string name);
    LeaderBoardItem[] GetLeaderBoard();
}