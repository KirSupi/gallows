using gallows.Domain;

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

    public SavedGame LoadSavedGame()
    {
        return new();
    }
}