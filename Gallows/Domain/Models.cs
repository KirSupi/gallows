namespace gallows.Domain;

public struct Game
{
    int CreatedAt;
    int Scores;
    string[] PreviousWords;
    string CurrentWord;
    char[] SelectedLetters;
}