namespace DataProvider.Base.Error;
public struct ErrorModel
{
    public ErrorModel(int code, string title, List<string> messages)
    {
        Code = code;
        Title = title;

        Messages = [];

        foreach (var message in messages)
            Messages.Add(message);
    }

    public readonly int Code;
    public readonly string Title;
    public List<string> Messages;

    public ErrorModel Ready(params string[] values)
    {
        var messages = new List<string>();
        foreach (var message in Messages)
            messages.Add(message);

        Messages = messages;
        return this;
    }
}
