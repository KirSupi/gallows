using gallows.DataLayer;
using gallows.Domain;
using gallows.UILayer;
using gallows.UseCaseLayer;

namespace gallows;

class Program
{
    static void Main()
    {
        IDataLayer dataLayer = new FileData("./assets");
        IUseCaseLayer useCaseLayer = new UseCase(dataLayer);
        IUiLayer uiLayer = new ConsoleUi(useCaseLayer);
        uiLayer.Run();
    }
}