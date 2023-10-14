namespace gallows.Domain;

public interface IDataLayer
{
    void SaveGame(Game? g);
    Game? LoadSavedGame();
}

public interface IUseCaseLayer
{
    void SaveGame();
    bool LoadSavedGame();
    void StartNewGame();
}

public interface IUiLayer
{
    void Run();
}
