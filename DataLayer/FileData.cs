namespace gallows.DataLayer;
using gallows;

interface IDataLayer
{
    
}

public class FileData
{
    private string _root = "./";
    
    public FileData(string root)
    {
        _root = root;
    }
}