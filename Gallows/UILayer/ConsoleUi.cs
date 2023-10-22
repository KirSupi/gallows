using System.Reflection.Metadata;
using gallows.Domain;

namespace gallows.UILayer;

public class ConsoleUi : IUiLayer
{
    private const string ModeMenu = "menu";
    private const string ModeGame = "game";
    private const string ModeSettings = "settings";

    private const string TextMenuNewGame = "Новая игра";
    private const string TextMenuLoadSavedGame = "Загрузить сохранённую игру";
    private const string TextMenuSettings = "Настройки";
    private const string TextMenuExit = "Выход";

    private readonly string[] _textMenu =
    {
        TextMenuNewGame,
        TextMenuLoadSavedGame,
        TextMenuSettings,
        TextMenuExit
    };

    private readonly IUseCaseLayer _uc;
    private string _mode = ModeMenu;

    public ConsoleUi(IUseCaseLayer uc)
    {
        _uc = uc;
    }

    public void Run()
    {
        var exit = false;
        while (!exit)
        {
            switch (_mode)
            {
                case ModeMenu:
                    exit = MenuHandler();
                    break;
                case ModeGame:
                    GameHandler();
                    break;
                case ModeSettings:
                    SettingsHandler();
                    break;
            }
        }
    }

    private bool MenuHandler()
    {
        for (var i = 1; i <= _textMenu.Length; i++)
        {
            Console.WriteLine($"{i}. {_textMenu[i - 1]}");
        }

        Console.WriteLine("\nВведите номер пункта меню");

        int selectedMenuItemIndex;
        while (true)
        {
            if (Int32.TryParse(Console.ReadLine(), out selectedMenuItemIndex))
            {
                if (selectedMenuItemIndex > 0 && selectedMenuItemIndex <= _textMenu.Length)
                {
                    selectedMenuItemIndex -= 1;
                    break;
                }
            }

            Console.WriteLine($"Некорректный ввод, надо ввести число от 1 до {_textMenu.Length}");
        }

        var selectedMenuItem = _textMenu[selectedMenuItemIndex];

        switch (selectedMenuItem)
        {
            case TextMenuNewGame:
                _mode = ModeGame;
                _uc.StartNewGame();
                Console.Clear();
                return false;
            case TextMenuLoadSavedGame:
                var exists = _uc.LoadSavedGame();
                if (exists)
                {
                    _mode = ModeGame;
                    Console.Clear();
                }
                else
                {
                    _mode = ModeMenu;
                    Console.WriteLine("Сохранений нет");
                }

                return false;
            case TextMenuSettings:
                _mode = ModeSettings;
                Console.Clear();
                return false;
            case TextMenuExit:
                Console.Clear();
                return true;
            default:
                Console.Clear();
                return false;
        }
    }

    private void GameHandler()
    {
        string input;
        GameState gameState;
        
        while (true)
        {
            Console.Clear();
            
            gameState = _uc.GetState();
            DrawGame(gameState);
            
            input = Console.ReadLine() ?? "";
            if (input.Length == 1 && Const.Alphabet.Contains(input.ToLower()[0]))
            {
                _uc.MakeMove(input.ToLower()[0]);
            } else if (input == "!")
            {
                _uc.SaveGame();
                _mode = ModeMenu;
                break;
            }
            else
            {
                Console.WriteLine("Невалидный ввод");
            }
        }
    }

    private void SettingsHandler()
    {
        
    }

    private void DrawGame(GameState gameState)
    {
        Console.WriteLine($"Очки: {gameState.Scores}\tОтгаданных слов: {gameState.PreviousWordsCount}");
        Console.WriteLine(GetGallowsState(gameState.Over, gameState.Damage));
        Console.WriteLine("\tСлово: " + GetWordPlaceholder(gameState.CurrentWordLength, gameState.GuessedLetters));
        Console.WriteLine();
        Console.WriteLine("Введи букву или !, чтоб сохранить и выйти в меню");
    }

    private string GetWordPlaceholder(int currentWordLength, Dictionary<char, int[]> guessedLetters)
    {
        var symbolsPlaceholders = new char[currentWordLength];

        for (var i = 0; i < currentWordLength; i++) symbolsPlaceholders[i] = '_';
        foreach (var guessedLetter in guessedLetters.Keys)
        {
            foreach (var index in guessedLetters[guessedLetter])
            {
                symbolsPlaceholders[index] = guessedLetter;
            }
        }
        
        var wordPlaceholder = string.Join(' ', symbolsPlaceholders);
        
        return wordPlaceholder;
    }
    
    // Возвращает рисунок человечка на виселице в зависимости от его "урона" (от 0 до 100, если больше 100,
    // то человечек уже повешен)
    private string GetGallowsState(bool gameIsOver, int damage)
    {
        if (gameIsOver) return Consts.Gallows100;
        switch (damage)
        {
            case >= 0 and < 10:
                return Consts.Gallows0;
            case >= 10 and < 20:
                return Consts.Gallows10;
            case >= 20 and < 30:
                return Consts.Gallows20;
            case >= 30 and < 40:
                return Consts.Gallows30;
            case >= 40 and < 50:
                return Consts.Gallows40;
            case >= 50 and < 60:
                return Consts.Gallows50;
            case >= 60 and < 70:
                return Consts.Gallows60;
            case >= 70 and < 80:
                return Consts.Gallows70;
            case >= 80 and < 90:
                return Consts.Gallows80;
            case >= 90 and < 100:
                return Consts.Gallows90;
            case > 100:
                return Consts.Gallows100;
            default:
                throw new Exception("damage must be at least 0");
        }
    }
}