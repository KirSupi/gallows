namespace gallows.Domain;

public interface IDataLayer
{
    void SaveGame(Game? g);
    Game? LoadSavedGame();
}

public interface IUseCaseLayer
{
    bool LoadSavedGame();
    void StartNewGame();
}

public interface IUiLayer
{
    void Run();
}
