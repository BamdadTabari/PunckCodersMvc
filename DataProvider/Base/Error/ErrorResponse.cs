namespace DataProvider.Base.Error;
public class ErrorResponse
{
    public ErrorResponse(ErrorModel error)
    {
        Code = error.Code;
        Title = error.Title;
        Messages = error.Messages;
    }

    public int Code { get; }
    public string Title { get; }
    public List<string> Messages { get; }

}
