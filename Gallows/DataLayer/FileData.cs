using gallows.Domain;
using System.Text.Json;
namespace gallows.DataLayer;

public class FileData : IDataLayer
{
    private readonly string _root = "./";
    private const string SavedGameFileName = "saved_game.json";
    private const string WordsFileName = "words.json";
    public FileData()
    {
    }

    public FileData(string root)
    {
        _root = root;
    }

    public Game? LoadSavedGame()
    {
        try
        {
            var jsonString = File.ReadAllText(Path.Join(_root, SavedGameFileName));
            var g = JsonSerializer.Deserialize<Game>(jsonString)!;
            return g;
        }
        catch (FileNotFoundException)
        {
            return null;
        }
        catch (Exception e)
        {
            throw new GameLoadException($"GAME_LOAD_ERROR: {e.Message}");
        }
    }

    public void SaveGame(Game? g)
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(g);
            File.WriteAllText(SavedGameFileName, jsonString);
        }
        catch (Exception e)
        {
            throw new GameSaveException($"GAME_SAVE_ERROR: {e.Message}");
        }
    }
}