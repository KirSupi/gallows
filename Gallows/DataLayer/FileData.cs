using gallows.Domain;
using System.Text.Json;
namespace gallows.DataLayer;

public class FileData : IDataLayer
{
    private readonly string _root = "./";
    private const string SavedGameFileName = "assets/saved_game.json";
    private const string WordsFileName = "assets/words.json";
    private const string SettingsFileName = "assets/settings.json";
    public FileData() { }

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
            File.WriteAllText(Path.Join(_root, SavedGameFileName), jsonString);
        }
        catch (Exception e)
        {
            throw new GameSaveException($"GAME_SAVE_ERROR: {e.Message}");
        }
    }
    public Settings LoadSettings()
    {
        try
        {
            var jsonString = File.ReadAllText(Path.Join(_root, SettingsFileName));
            var s = JsonSerializer.Deserialize<Settings>(jsonString);
            return s;
        }
        catch (FileNotFoundException)
        {
            return new Settings();
        }
        catch (Exception e)
        {
            throw new SettingsLoadException($"SETTINGS_LOAD_ERROR: {e.Message}");
        }
    }
    
    public void SaveSettings(Settings s)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(s);
            File.WriteAllText(Path.Join(_root, SettingsFileName), jsonString);
        }
        catch (Exception e)
        {
            throw new SettingsSaveException($"SETTINGS_SAVE_ERROR: {e.Message}");
        }
    }
    public string GetRandomWord(string category)
    {
        try
        {
            var json = File.ReadAllText(Path.Join(_root, WordsFileName));
            var data = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
            var collection = data[category];
            
            var random = new Random();
            var randomIndex = random.Next(collection.Count);
            
            return collection[randomIndex];
        }
        catch (Exception e)
        {
            throw new GetRandomWordException($"GET_RANDOM_WORD_ERROR: {e.Message}");
        }
        
    }

    public string[] GetWordsCategories()
    {
        string json = File.ReadAllText(Path.Join(_root, WordsFileName));
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
            throw new GetWordsCategoriesException($"GET_WORDS_CATEGORIES_ERROR: {e.Message}");
            // Console.WriteLine("Ошибка при чтении JSON: " + e.Message);
            // return new string[0]; // В случае ошибки возвращаем пустой массив строк.
        }
    }
}
