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

    public string GetRandomWord(string category)
    {
        try
        {
            string json = File.ReadAllText(WordsFileName);
            var data = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
            var collection = data[category];
            Random random = new Random();
            int randomIndex = random.Next(collection.Count);
            return collection[randomIndex];
        }
        catch (JsonException e)
        {
            throw new JsonParseException($"JSON_PARSE_ERROR: {e.Message}");
        }
        
    }

    public string[] GetWordsCategories()
    {
        string json = File.ReadAllText(WordsFileName);
        try
        {
            // Распарсим JSON в объект.
            var data = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);

            // Инициализируем массив с размером количества категорий в JSON-файле.
            string[] categories = new string[data.Keys.Count];

            int index = 0;
            // Добавляем все ключи (категории) в массив.
            foreach (var key in data.Keys)
            {
                categories[index] = key;
                index++;
            }

            return categories;
        }
        catch (JsonException e)
        {
            throw new JsonParseException($"JSON_PARSE_ERROR: {e.Message}");
            // Console.WriteLine("Ошибка при чтении JSON: " + e.Message);
            // return new string[0]; // В случае ошибки возвращаем пустой массив строк.
        }
    }
}
