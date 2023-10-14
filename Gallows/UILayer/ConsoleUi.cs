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

    private IUseCaseLayer _uc;
    private string _mode = ModeMenu;

    public ConsoleUi(IUseCaseLayer uc)
    {
        _uc = uc;
    }

    public void Run()
    {
        var exit = false;
        while (true)
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

            if (exit) break;
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
                return false;
            case TextMenuLoadSavedGame:
                var exists = _uc.LoadSavedGame();
                if (exists)
                {
                    _mode = ModeGame;
                }
                else
                {
                    _mode = ModeMenu;
                    Console.WriteLine("Сохранений нет");
                }

                return false;
            case TextMenuSettings:
                _mode = ModeSettings;
                return false;
            case TextMenuExit:
                return true;
            default:
                return false;
        }
    }

    private void GameHandler()
    {
    }

    private void SettingsHandler()
    {
    }
}