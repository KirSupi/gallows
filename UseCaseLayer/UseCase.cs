namespace gallows.UseCaseLayer;
using gallows.DataLayer;

interface IUserCaseLayer
{
    LoadSavedGame();
}

public class UseCase
{
    private IDataLayer _dl;
    
    public UseCase(IDataLayer dl)
    {
        _dl = dl;
    }
}