using gallows.Domain;
using System.Text.Json;
namespace gallows.DataLayer;

public class FileData : IDataLayer
{
    private string _root = "./";
    private const string FileName = "saved_games.json";
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
            string jsonString = File.ReadAllText(FileName);
            Game g = JsonSerializer.Deserialize<Game>(jsonString)!;
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
            File.WriteAllText(FileName, jsonString);
        }
        catch (Exception e)
        {
            throw new GameSaveException($"GAME_SAVE_ERROR: {e.Message}");
        }
    }
}