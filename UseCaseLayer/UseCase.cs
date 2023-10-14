namespace gallows.UseCaseLayer;
using gallows.DataLayer;

interface IUserCaseLayer
{
    
}

public class UseCase
{
    private IDataLayer _dl;
    
    public UseCase(IDataLayer dl)
    {
        _dl = dl;
    }
}