using System.Reflection.Metadata;
using Gallows.Models;
using Gallows.Repository;
using Gallows.UseCase;

namespace Gallows.UI;

public class ConsoleUi : IUi
{
    private readonly string[] _textMenu =
    {
        MenuButtons.TextMenuNewGame,
        MenuButtons.TextMenuLoadSavedGame,
        MenuButtons.TextMenuSettings,
        MenuButtons.TextMenuUsersTable,
        MenuButtons.TextMenuExit
    };
    
    private readonly string[] _textSettings =
    {
        SettingsButtons.SettingsDifficulty,
        SettingsButtons.SettingsCategory
    };

    private readonly IUseCase _uc;
    private string _mode = Modes.ModeMenu;

    public ConsoleUi(IUseCase uc)
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
                case Modes.ModeMenu:
                    exit = MenuHandler();
                    break;
                case Modes.ModeGame:
                    GameHandler();
                    break;
                case Modes.ModeSettings:
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
            case MenuButtons.TextMenuNewGame:
                _mode = Modes.ModeGame;
                _uc.StartNewGame();
                Console.Clear();
                return false;
            case MenuButtons.TextMenuLoadSavedGame:
                var exists = _uc.LoadSavedGame();
                if (exists)
                {
                    _mode = Modes.ModeGame;
                    Console.Clear();
                }
                else
                {
                    _mode = Modes.ModeMenu;
                    Console.WriteLine("Сохранений нет");
                }

                return false;
            case MenuButtons.TextMenuSettings:
                _mode = Modes.ModeSettings;
                Console.Clear();
                return false;
            case MenuButtons.TextMenuExit:
                Console.Clear();
                return true;
            default:
                Console.Clear();
                return false;
        }
    }

    private void GameHandler()
    {
        while (true)
        {
            Console.Clear();

            var gameState = _uc.GetState();
            DrawGame(gameState);

            // Если отгадали слово
            if (gameState.Over && gameState.Damage < 100)
            {
                _uc.NextWord();
                Thread.Sleep(3000);
                continue;
            }

            var input = Console.ReadLine() ?? "";
            if (input.Length == 1 && Const.Alphabet.Contains(input.ToLower()[0]))
            {
                _uc.MakeMove(input.ToLower()[0]);
            }
            else if (input == "!")
            {
                _uc.SaveGame();
                _mode = Modes.ModeMenu;
                break;
            }
            else
            {
                Console.WriteLine("Невалидный ввод");
                Thread.Sleep(500);
            }
        }
    }

    private void SettingsHandler()
    {
        for (var i = 1; i <= _textSettings.Length; i++)
        {
            Console.WriteLine($"{i}. {_textSettings[i - 1]}");
        }
        
        Console.WriteLine("\nВведите номер настройки");
        
        int selectedSettingsItemIndex;
        while (true)
        {
            if (Int32.TryParse(Console.ReadLine(), out selectedSettingsItemIndex))
            {
                if (selectedSettingsItemIndex > 0 && selectedSettingsItemIndex <= _textSettings.Length)
                {
                    selectedSettingsItemIndex -= 1;
                    break;
                }
            }

            Console.WriteLine($"Некорректный ввод, надо ввести число от 1 до {_textSettings.Length}");
        }

        var selectedSettingsItem = _textSettings[selectedSettingsItemIndex];
        
        switch (selectedSettingsItem)
        {
            case SettingsButtons.SettingsDifficulty:
                Console.Clear();
                break;
            case SettingsButtons.SettingsCategory:
                var categories = _uc.GetWordsCategories();
                for (var i = 1; i <= categories.Length; i++)
                {
                    Console.WriteLine($"{i}. {categories[i - 1]}");
                }
                Console.WriteLine("\nВведите номер категории");
                break;
            default:
                Console.Clear();
                break;
        }
    }

    private void DrawGame(GameState gameState)
    {
        Console.WriteLine($"Очки: {gameState.Scores}\tОтгаданных слов: {gameState.PreviousWordsCount}");
        Console.WriteLine($"Категория слова: {gameState.CurrentWordCategory}");
        Console.WriteLine(GetGallowsState(gameState.Damage));
        Console.WriteLine("\tСлово: " + GetWordPlaceholder(gameState.CurrentWordLength, gameState.GuessedLetters));
        Console.WriteLine();
        Console.WriteLine(
            gameState.Over ? "Ты отгадал слово! Давай следующее.." : "Введи букву или !, чтоб сохранить и выйти в меню"
        );
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
    private string GetGallowsState(int damage)
    {
        switch (damage)
        {
            case >= 0 and < 10:
                return DrawConstants.Gallows0;
            case >= 10 and < 20:
                return DrawConstants.Gallows10;
            case >= 20 and < 30:
                return DrawConstants.Gallows20;
            case >= 30 and < 40:
                return DrawConstants.Gallows30;
            case >= 40 and < 50:
                return DrawConstants.Gallows40;
            case >= 50 and < 60:
                return DrawConstants.Gallows50;
            case >= 60 and < 70:
                return DrawConstants.Gallows60;
            case >= 70 and < 80:
                return DrawConstants.Gallows70;
            case >= 80 and < 90:
                return DrawConstants.Gallows80;
            case >= 90 and < 100:
                return DrawConstants.Gallows90;
            case > 100:
                return DrawConstants.Gallows100;
            default:
                throw new Exception("damage must be at least 0");
        }
    }
}