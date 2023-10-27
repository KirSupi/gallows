using Gallows.Repository;
using Gallows.UI;
using Gallows.UseCase;


namespace Gallows;
class Program
{
    private static void Main()
    {
        IGameRepository gameRepository = new GameRepository();
        ISettingsRepository settingsRepository = new SettingsRepository();
        IWordsRepository wordsRepository = new WordsRepository();
        IUseCase useCaseLayer = new UseCase.UseCase(gameRepository, settingsRepository, wordsRepository);
        IUi ui = new ConsoleUi(useCaseLayer);
        ui.Run();
    }
}