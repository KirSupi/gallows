namespace gallows.UseCaseLayer;

using Domain;

public class UseCase : IUseCaseLayer
{
    private readonly IDataLayer _data;
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
            return _game != null; // возвращаем true, если нашли созранённую игру и загрузили её
        }
        catch (GameLoadException ex)
        {
            Console.WriteLine($"Error loading game: {ex.Message}");
            // Дополнительная логика обработки ошибки загрузки игры
        }

        return true;
    }

    public void SaveGame() => _data.SaveGame(_game);

    public void StartNewGame() => _game = new Game?();

    // public State GetState()
    // {
    //     
    // }
}