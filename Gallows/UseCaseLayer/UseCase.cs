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
        _settings = _data.LoadSettings();
        if (string.IsNullOrEmpty(_settings.WordsCategory))
        {
            var categories = _data.GetWordsCategories();
            if (categories.Length != 0) _settings.WordsCategory = categories[0];
        }

        _game.CurrentWord = _data.GetRandomWord(_settings.WordsCategory);
    }

    public GameState GetState()
    {
        var s = new GameState();

        s.Scores = _game.Scores;
        s.PreviousWordsCount = _game.PreviousWords.Count;
        s.CurrentWordLength = _game.CurrentWord.Length;

        var mistakes = 0;
        _game.SelectedLetters.ForEach(
            (letter) =>
            {
                if (!_game.CurrentWord.Contains(letter)) mistakes++;
            }
        );
        s.Damage = (int)(
            (double)mistakes // сколько ошибок было
            /
            ((int)Const.Difficulty.Hard - _settings.Difficulty) * _game.CurrentWord.Length // допустимое кол-во ошибок
                                                                * 100 // получаем проценты
        );

        // немного округляем урон вверх, если надо
        if (s.Damage > 91) s.Damage = 100;

        s.GuessedLetters = new Dictionary<char, int[]>();
        for (var i = 0; i < _game.CurrentWord.Length; i++)
        {
            var letter = _game.CurrentWord[i];
            if (!_game.SelectedLetters.Contains(letter)) continue;

            if (!s.GuessedLetters.ContainsKey(letter))
            {
                s.GuessedLetters[letter] = new[] { i };
            }
            else
            {
                s.GuessedLetters[letter] = s.GuessedLetters[letter].Append(i).ToArray();
            }
        }

        return s;
    }

    public void MakeMove(char letter)
    {
        if (_game.Over) return;

        if (_game.SelectedLetters.Contains(letter)) return;
        
        _game.SelectedLetters.Add(letter);
    }
}