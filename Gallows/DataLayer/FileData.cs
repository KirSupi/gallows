using gallows.Domain;
using System.Text.Json;
namespace gallows.DataLayer;

public class FileData : IDataLayer
{
    private string _root = "./";

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
            string fileName = "user_games.json";
            string jsonString = File.ReadAllText(fileName);
            Game g = JsonSerializer.Deserialize<Game>(jsonString)!;
            return g;
        }
        catch (FileNotFoundException)
        {
            throw new GameLoadException("GAME_FILE_NOT_FOUND: The game file was not found.");
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
            string fileName = "user_games.json";
            string jsonString = JsonSerializer.Serialize(g);
            File.WriteAllText(fileName, jsonString);
        }
        catch (Exception e)
        {
            throw new GameSaveException($"GAME_SAVE_ERROR: {e.Message}");
        }
    }
}