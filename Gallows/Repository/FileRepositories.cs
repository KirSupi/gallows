using System.Text.Json;
using Gallows.Models;

namespace Gallows.Repository;

public class BaseFileRepository
{
    private readonly string _root = "./assets";

    protected string GetPath(string path)
    {
        return Path.Join(_root, path);
    }

    protected BaseFileRepository()
    {
    }

    protected BaseFileRepository(string root)
    {
        _root = root;
    }
}

public class GameFileRepository : BaseFileRepository, IGameRepository
{
    public Game? LoadSavedGame()
    {
        try
        {
            var jsonString = File.ReadAllText(GetPath(PathConstants.SavedGameFileName));
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
            File.WriteAllText(GetPath(PathConstants.SavedGameFileName), jsonString);
        }
        catch (Exception e)
        {
            throw new GameSaveException($"GAME_SAVE_ERROR: {e.Message}");
        }
    }
}

public class SettingsFileRepository : BaseFileRepository, ISettingsRepository
{
    public Settings LoadSettings()
    {
        try
        {
            var jsonString = File.ReadAllText(GetPath(PathConstants.SettingsFileName));
            var s = JsonSerializer.Deserialize<Settings>(jsonString);
            return s;
        }
        catch (Exception)
        {
            return new Settings();
        }
    }

    public void SaveSettings(Settings settings)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(settings);
            File.WriteAllText(GetPath(PathConstants.SettingsFileName), jsonString);
        }
        catch (Exception e)
        {
            throw new SettingsSaveException($"SETTINGS_SAVE_ERROR: {e.Message}");
        }
    }
}

public class WordsFileRepository : BaseFileRepository, IWordsRepository
{
    public string GetRandomWord(string category)
    {
        try
        {
            var json = File.ReadAllText(GetPath(PathConstants.WordsFileName));
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
        try
        {
            var json = File.ReadAllText(GetPath(PathConstants.WordsFileName));
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

public class LeaderBoardFileRepository : BaseFileRepository, ILeaderBoardRepository
{
    public void AddItem(LeaderBoardItem item)
    {
        // Читаем
        string jsonString;
        try
        {
            jsonString = File.ReadAllText(GetPath(PathConstants.LeaderBoardFileName));
            jsonString = jsonString.Length > 2 ? jsonString : "[]";
        }
        catch (FileNotFoundException)
        {
            jsonString = "[]";
        }
        catch (Exception e)
        {
            throw new LeaderBoardRepositoryException($"ERROR: {e.Message}");
        }
        LeaderBoardItem[] leaderBoardItems;
        try
        {
            leaderBoardItems = JsonSerializer.Deserialize<LeaderBoardItem[]>(jsonString) ?? new LeaderBoardItem[] { };
        }
        catch (Exception)
        {
            leaderBoardItems = new LeaderBoardItem[] { };
        }

        // Добавляем или обновляем результат
        try
        {
            var alreadyExists = false;
            for (var i = 0; i < leaderBoardItems.Length; i++)
            {
                if (leaderBoardItems[i].Name != item.Name) continue;

                alreadyExists = true;
                leaderBoardItems[i].Scores = leaderBoardItems[i].Scores > item.Scores
                    ? leaderBoardItems[i].Scores
                    : item.Scores;
            }

            if (!alreadyExists)
            {
                leaderBoardItems = leaderBoardItems.Append(item).ToArray();
            }
        }
        catch (Exception e)
        {
            throw new LeaderBoardRepositoryException($"ERROR: {e.Message}");
        }

        // Сохраняем
        try
        {
            jsonString = JsonSerializer.Serialize(leaderBoardItems);
            File.WriteAllText(GetPath(PathConstants.LeaderBoardFileName), jsonString);
        }
        catch (Exception e)
        {
            throw new LeaderBoardRepositoryException($"ERROR: {e.Message}");
        }
    }

    public LeaderBoardItem[] GetTopResults(int count)
    {
        string jsonString;
        try
        {
            jsonString = File.ReadAllText(GetPath(PathConstants.LeaderBoardFileName));
            jsonString = jsonString.Length > 2 ? jsonString : "[]";
        }
        catch (FileNotFoundException)
        {
            jsonString = "[]";
        }
        catch (Exception e)
        {
            throw new LeaderBoardRepositoryException($"ERROR: {e.Message}");
        }

        try
        {
            var result = new LeaderBoardItem[count];

            var leaderBoardItems =
                JsonSerializer.Deserialize<LeaderBoardItem[]>(jsonString) ?? new LeaderBoardItem[count];
            leaderBoardItems = leaderBoardItems.OrderByDescending(item => item.Scores).ToArray();

            for (var i = 0; i < (leaderBoardItems.Length > count ? count : leaderBoardItems.Length); i++)
            {
                result[i] = leaderBoardItems[i];
            }

            return result;
        }
        catch (Exception e)
        {
            throw new LeaderBoardRepositoryException($"ERROR: {e.Message}");
        }
    }
}