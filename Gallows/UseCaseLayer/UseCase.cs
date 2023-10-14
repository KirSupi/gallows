namespace gallows.UseCaseLayer;
using gallows.Domain;

public class UseCase : IUseCaseLayer
{
    private IDataLayer _dl;
    
    public UseCase(IDataLayer dl)
    {
        _dl = dl;
    }

    public void LoadSavedGame()
    {
    }

    public void StartNewGame()
    {
        
    }
}