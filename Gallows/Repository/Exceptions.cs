namespace gallows.Domain;

public class GameLoadException : Exception
{
    public GameLoadException(string message) : base(message) { }
}

public class GameSaveException : Exception
{
    public GameSaveException(string message) : base(message){ }
}

public class SettingsLoadException : Exception
{
    public SettingsLoadException(string message) : base(message) { }
}

public class SettingsSaveException : Exception
{
    public SettingsSaveException(string message) : base(message){ }
}

public class GetRandomWordException : Exception
{
    public GetRandomWordException(string message) : base(message) { }
}

public class GetWordsCategoriesException : Exception
{
    public GetWordsCategoriesException(string message) : base(message) { }
}