namespace gallows.UILayer;

using UseCaseLayer;

public class ConsoleUI
{
    private const string ModeMenu = "menu";
    private const string ModeGame = "game";
    private const string ModeSettings = "settings";

    private const string TextMenuNewGame = "Новая игра";
    private const string TextMenuSettings = "Настройки";
    private const string TextMenuExit = "Выход";

    private readonly string[] _textMenu =
    {
        TextMenuNewGame,
        TextMenuSettings,
        TextMenuExit
    };

    private IUserCaseLayer _uc;
    private string _mode = ModeMenu;

    public ConsoleUI(IUserCaseLayer uc)
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
                return false;
            case TextMenuSettings:
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