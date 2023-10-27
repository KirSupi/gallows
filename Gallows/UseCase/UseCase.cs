namespace Gallows.UseCase;

using Gallows.Models;
using Gallows.Repository;

public class UseCase : IUseCase
{
    private readonly IGameRepository _gameRepository;
    private readonly ISettingsRepository _settingsRepository;
    private readonly IWordsRepository _wordsRepository;
    private Game _game;
    private Settings _settings;


    public UseCase(IGameRepository gameRepo, ISettingsRepository settingsRepository,IWordsRepository wordsRepository)
    {
        _gameRepository = gameRepo;
        _settingsRepository = settingsRepository;
        _wordsRepository = wordsRepository;
    }

    public bool LoadSavedGame()
    {
        try
        {
            var savedGame = _gameRepository.LoadSavedGame();
            if (savedGame != null) _game = savedGame ?? new Game();
            return savedGame != null; // возвращаем true, если нашли созранённую игру и загрузили её
        }
        catch (GameLoadException)
        {
            return false;
            // Console.WriteLine("Ошибка при загрузке игры");
            // Дополнительная логика обработки ошибки загрузки игры
        }
    }

    public void SaveGame() => _gameRepository.SaveGame(_game);

    public void StartNewGame()
    {
        _game = new Game();
        _settings = _settingsRepository.LoadSettings();
        NextWord();
    }
    
    public bool LoadSavedSettings()
    {
        try
        {
            var savedGame = _gameRepository.LoadSavedGame();
            if (savedGame != null) _game = savedGame ?? new Game();
            return savedGame != null; // возвращаем true, если нашли созранённую игру и загрузили её
        }
        catch (SettingsLoadException)
        {
            return false;
            // Console.WriteLine("Ошибка при загрузке игры");
            // Дополнительная логика обработки ошибки загрузки игры
        }
    }

    public void SaveSettings() => _settingsRepository.SaveSettings(_settings);

    public void NextWord()
    {
        // заполняем отгаданные буквы
        var guessedLetters = new Dictionary<char, int[]>();
        for (var i = 0; i < _game.CurrentWord.Length; i++)
        {
            var letter = _game.CurrentWord[i];
            if (!_game.SelectedLetters.Contains(letter)) continue;

            if (!guessedLetters.ContainsKey(letter))
            {
                guessedLetters[letter] = new[] { i };
            }
            else
            {
                guessedLetters[letter] = guessedLetters[letter].Append(i).ToArray();
            }
        }

        // Если мы отгадали всё слово, добавляем его к уже отгаданным
        if (guessedLetters.Keys.Count == _game.CurrentWord.Length && _game.CurrentWord.Length != 0)
            _game.PreviousWords.Add(_game.CurrentWord);
        
        if (string.IsNullOrEmpty(_settings.WordsCategory))
        {
            var categories = _wordsRepository.GetWordsCategories();
            if (categories.Length != 0) _settings.WordsCategory = categories[0];
        }

        _game.CurrentWord = _wordsRepository.GetRandomWord(_settings.WordsCategory);
        _game.SelectedLetters = new List<char>();
        _game.Damage = 0;
        _game.Over = false;
    }

    public string[] GetWordsCategories() => _wordsRepository.GetWordsCategories();
    public GameState GetState()
    {
        var s = new GameState();

        s.Scores = _game.Scores;
        s.PreviousWordsCount = _game.PreviousWords.Count;
        s.CurrentWordLength = _game.CurrentWord.Length;
        s.CurrentWordCategory = _settings.WordsCategory;
        var mistakes = 0;
        _game.SelectedLetters.ForEach(
            letter =>
            {
                if (!_game.CurrentWord.Contains(letter)) mistakes++;
            }
        );
        s.Damage = (int)(
            (double)mistakes // сколько ошибок было
            /
            (((int)Const.Difficulty.Hard - _settings.Difficulty) * _game.CurrentWord.Length) // допустимое кол-во ошибок
            * 100 // получаем проценты
            // (((int)Const.Difficulty.Hard - _settings.Difficulty) * _game.CurrentWord.Length) // допустимое кол-во ошибок
           
        );

        // немного округляем урон вверх, если надо
        if (s.Damage > 91) s.Damage = 100;

        // заполняем отгаданные буквы
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

        // Если мы отгадали всё слово
        if (s.GuessedLetters.Keys.Count == _game.CurrentWord.Distinct().Count())
        {
            s.Over = true;
        }

        return s;
    }

    public void MakeMove(char letter)
    {
        if (_game.Over)
            return;

        if (_game.SelectedLetters.Contains(letter))
            return;

        _game.SelectedLetters.Add(letter);

        // Начисление очков
        var letterRepetitions = _game.CurrentWord.Count(let => let == letter);

        if (letterRepetitions == 0)
        {
            if (_game.Scores >= 10) _game.Scores -= 10;
        }
        else
        {
            _game.Scores += letterRepetitions * 100;
        }
    }
}