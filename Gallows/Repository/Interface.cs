using Gallows.Models;

namespace Gallows.Repository;

public interface IGameRepository
{
    void SaveGame(Game? game);
    Game? LoadSavedGame();
}

public interface ISettingsRepository
{
    void SaveSettings(Settings settings);
    Settings LoadSettings();
}

public interface IWordsRepository
{
    string[] GetWordsCategories();
    string GetRandomWord(string category);
}

public interface ILeaderBoardRepository
{
    void AddItem(LeaderBoardItem item);
    LeaderBoardItem[] GetTopResults(int count);
}
