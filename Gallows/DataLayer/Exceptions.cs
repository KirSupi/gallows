namespace gallows.Domain;

public class GameLoadException : Exception
{
    public GameLoadException(string message) : base(message)
    {
    }
}

public class JsonParseException : Exception
{
    public JsonParseException(string message) : base(message) { }
}

public class GameSaveException : Exception
{
    public GameSaveException(string message) : base(message)
    {
    }
}