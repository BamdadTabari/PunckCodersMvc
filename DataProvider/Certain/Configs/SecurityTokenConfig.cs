namespace DataProvider.Certain.Configs;

public class SecurityTokenConfig
{
    public string Key = "SecuritysecaeasdDdw23294913129-123-1wqndlqndwoeoqiy931q429y19eiojwoo13Token";

    public string Issuer { get; set; } = "https://localhost";
    public string Audience { get; set; } = "https://localhost";
    public string AccessTokenSecretKey { get; set; } = "WQ7+dPhLEHdhdaKNzu!ck-fg86TPhUfd#E&&Qq+=vUtfxJ!@sDfe#u^prXW2&Qhmy33u!@e?5-xb*";
    public TimeSpan AccessTokenLifetime { get; set; } = TimeSpan.FromDays(15);
    public string RefreshTokenSecretKey { get; set; } = "WQ7+dPsdjifdsklhFBNzudsifpifsiPhUAPOSIDPIOUFDIOshF#u^prXW2&Qhmy33u!@e5455sdfd";
    public TimeSpan RefreshTokenLifetime { get; set; } = TimeSpan.FromDays(15);
}