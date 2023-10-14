namespace gallows.Domain;

public interface IDataLayer
{
    (Game, bool) LoadSavedGame();
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
