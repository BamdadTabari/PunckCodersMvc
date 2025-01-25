namespace DataProvider.Certain.Configs;

public class LockoutConfig
{
    public const string Key = "Lockout";

    public readonly int FailedLoginLimit = 4;
    public readonly TimeSpan Duration = TimeSpan.FromMinutes(4);
}