namespace gallows.Domain;

public struct Const
{
    public static readonly char[] Alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToCharArray();
}

public struct Game
{
    public int CreatedAt;
    public int Scores;
    public string[] PreviousWords;
    public string CurrentWord;
    public char[] SelectedLetters;
}

public struct Settings
{
    public enum Difficulty {
        Easy,
        Medium,
        Hard,
    };
}
