namespace gallows.Domain;

public interface IDataLayer
{
    SavedGame LoadSavedGame();
}

public interface IUseCaseLayer
{
    void LoadSavedGame();
    void StartNewGame();
}

public interface IUiLayer
{
    void Run();
}
