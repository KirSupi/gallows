namespace gallows.UseCaseLayer;

using Domain;

public class UseCase : IUseCaseLayer
{
    private readonly IDataLayer _data;
    private Game _game;
    private Settings _settings;


    public UseCase(IDataLayer data)
    {
        _data = data;
    }

    public bool LoadSavedGame()
    {
        try
        {
            var savedGame = _data.LoadSavedGame();
            
            if (savedGame != null) _game = savedGame ?? new Game();
            
            return savedGame != null; // возвращаем true, если нашли созранённую игру и загрузили её
        }
        catch (GameLoadException ex)
        {
            Console.WriteLine($"Error loading game: {ex.Message}");
            // Дополнительная логика обработки ошибки загрузки игры
        }

        return true;
    }

    public void SaveGame() => _data.SaveGame(_game);

    public void StartNewGame()
    {
        _game = new Game();
        _game.CurrentWord = _data.GetRandomWord(_settings.WordsCategory);
    }

    public GameState GetState()
    {
        var s = new GameState();

        s.Scores = _game.Scores;
        s.PreviousWordsCount = _game.PreviousWords.Length;
        s.CurrentWordLength = _game.CurrentWord.Length;
        // todo fill state
        
        return s;
    }

    public void MakeMove(char letter)
    {
        if (_game.Over) return;
    }
}