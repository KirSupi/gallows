using System.Text.Json;
using Gallows.Models;

namespace Gallows.Repository;

public class BaseRepository
{
    protected const string Root = "./";
}

public class GameRepository : BaseRepository, IGameRepository 
{
    public Game? LoadSavedGame()
    {
        try
        {
            var jsonString = File.ReadAllText(Path.Join(Root, PathConstants.SavedGameFileName));
            var g = JsonSerializer.Deserialize<Game?>(jsonString)!;
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
            var jsonString = JsonSerializer.Serialize(g);
            File.WriteAllText(Path.Join(Root, PathConstants.SavedGameFileName), jsonString);
        }
        catch (Exception e)
        {
            throw new GameSaveException($"GAME_SAVE_ERROR: {e.Message}");
        }
    }
}

public class SettingsRepository : BaseRepository, ISettingsRepository
{
    public Settings LoadSettings()
    {
        try
        {
            var jsonString = File.ReadAllText(Path.Join(Root, PathConstants.SettingsFileName));
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

    public void SaveSettings(Settings settings)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(settings);
            File.WriteAllText(Path.Join(Root, PathConstants.SettingsFileName), jsonString);
        }
        catch (Exception e)
        {
            throw new SettingsSaveException($"SETTINGS_SAVE_ERROR: {e.Message}");
        }
    }
}
public class WordsRepository : BaseRepository, IWordsRepository
{
    public string GetRandomWord(string category)
    {
        try
        {
            var json = File.ReadAllText(Path.Join(Root, PathConstants.WordsFileName));
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
        var json = File.ReadAllText(Path.Join(Root, PathConstants.WordsFileName));
        try
        {
            // Распарсим JSON в объект.
            var data = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
            
            if (data is null) return Array.Empty<string>();
            
            // Инициализируем массив с размером количества категорий в JSON-файле.
            var categories = new string[data.Keys.Count];

            var index = 0;
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
        }
    }
}

