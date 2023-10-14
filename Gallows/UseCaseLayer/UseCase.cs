namespace gallows.UseCaseLayer;
using gallows.Domain;

public class UseCase : IUseCaseLayer
{
    private IDataLayer _data;
    private Game? _game;
    
    
    public UseCase(IDataLayer data)
    {
        _data = data;
    }

    public bool LoadSavedGame()
    {
        try
        {
            _game = _data.LoadSavedGame();
            if (_game != null)
            {
                // Игра успешно загружена
            }
            else
            {
                // Обработка случая, когда игра не была загружена
                // return _game != null;
                return false;
            }
        }
        catch (GameLoadException ex)
        {
            Console.WriteLine($"Error loading game: {ex.Message}");
            // Дополнительная логика обработки ошибки загрузки игры
        }
        
        return true;
    }

    public void SaveGame(Game g)
    {
        try
        {
            _data.SaveGame(_game);
        }
        catch (GameSaveException ex)
        {
            Console.WriteLine($"Error saving game: {ex.Message}");
            // Дополнительная логика обработки ошибки сохранения игры
        }
    }
    public void StartNewGame()
    {
        _game = new Game?();
    }
}