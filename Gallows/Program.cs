using Gallows.Repository;
using Gallows.UI;
using Gallows.UseCase;


namespace Gallows;

class Program
{
    private static void Main()
    {
        IGameRepository gameRepository = new GameFileRepository();
        ISettingsRepository settingsRepository = new SettingsFileRepository();
        IWordsRepository wordsRepository = new WordsFileRepository();
        ILeaderBoardRepository leaderBoardRepository = new LeaderBoardFileRepository();
        IUseCase useCaseLayer = new UseCase.UseCase(
            gameRepository,
            settingsRepository,
            wordsRepository,
            leaderBoardRepository
        );
        IUi ui = new ConsoleUi(useCaseLayer);
        ui.Run();
    }
}