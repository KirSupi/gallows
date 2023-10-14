namespace gallows.Domain;

public struct SavedGame
{
    int CreatedAt;
    int Scores;
    string[] PreviousWords;
    string CurrentWord;
    char[] SelectedLetters;
}