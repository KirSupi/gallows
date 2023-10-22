namespace gallows.Domain;

public struct Const
{
    public static readonly char[] Alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToCharArray();
    public enum Difficulty {
        Easy = 1,
        Medium = 2,
        Hard = 5,
    };
}

public struct Game
{
    public long CreatedAt = ((DateTimeOffset)DateTime.UtcNow.ToUniversalTime()).ToUnixTimeSeconds();
    public int Scores = 0;
    public List<string> PreviousWords;
    public string CurrentWord = "";
    public List<char> SelectedLetters;
    public int Damage = 0;
    public bool Over = false;

    public Game()
    {
        PreviousWords = new();
        SelectedLetters = new();
    }
}

public struct GameState
{
    public int Scores = 0;
    public int PreviousWordsCount = 0;
    public int CurrentWordLength = 0;
    public Dictionary<char,int[]> GuessedLetters; // "a"=>[0, 3] - отгаданная буква и её позиции
    public int Damage = 0;
    public bool Over = false;

    public GameState()
    {
        GuessedLetters = new();
    }
}

public struct Settings
{
    public int Difficulty;
    public string WordsCategory;
}
