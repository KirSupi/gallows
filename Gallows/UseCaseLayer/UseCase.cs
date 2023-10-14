namespace gallows.UseCaseLayer;
using gallows.Domain;

public class UseCase : IUseCaseLayer
{
    private IDataLayer _data;
    private Game _game;
    
    
    public UseCase(IDataLayer data)
    {
        _data = data;
    }

    public bool LoadSavedGame()
    {
        bool exists;
        
        (_game, exists) = _data.LoadSavedGame();
        
        return exists;
    }

    public void StartNewGame()
    {
        _game = new Game();
    }
}