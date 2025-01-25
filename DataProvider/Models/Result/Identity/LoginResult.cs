namespace DataProvider.Models.Result.Identity
{
    public class LoginResultDto
    {
        public bool IsAuthSuccessful { get; set; }
        public string? Error { get; set; }
        public string? Token { get; set; }

    }
}
