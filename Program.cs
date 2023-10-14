using gallows.DataLayer;
using gallows.UILayer;
using gallows.UseCaseLayer;

namespace gallows;

class Program
{
    static void Main()
    {
        var dataLayer = new FileData("");
        var useCaseLayer = new UseCase(dataLayer);
        var uiLayer = new ConsoleUI(useCaseLayer);

        uiLayer.Run();
    }
}