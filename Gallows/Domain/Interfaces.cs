namespace gallows.Domain;

public interface IDataLayer
{
    void SaveGame(Game? g);
    Game? LoadSavedGame();

    string[] GetWordsCategories();
    string GetRandomWord(string category);
}

public interface IUseCaseLayer
{
    void SaveGame();
    bool LoadSavedGame();
    void StartNewGame();

    GameState GetState();
    void MakeMove(char letter);
}

public interface IUiLayer
{
    void Run();
}
