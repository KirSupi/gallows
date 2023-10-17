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
    public int CreatedAt;
    public int Scores;
    public string[] PreviousWords;
    public string CurrentWord;
    public char[] SelectedLetters;
    public int Damage;
    public bool Over;
}

public struct GameState
{
    public int Scores;
    public int PreviousWordsCount;
    public int CurrentWordLength;
    public Dictionary<char,int[]> GuessedLetters; // "a"=>[0, 3] - отгаданная буква и её позиции
    public int Damage;
    public bool Over;
}

public struct Settings
{
    public int Difficulty;
    public string WordsCategory;
}
