namespace PunckCodersMvc.Configs;

public class CacheOptions
{
    public TimeSpan AbsoluteExpiration { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan SlidingExpiration { get; set; } = TimeSpan.FromMinutes(2);
    public TimeSpan LockExpiration { get; set; } = TimeSpan.FromMicroseconds(30);
}
