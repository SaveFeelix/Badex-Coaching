namespace Server.Settings;

public class JwtSettings
{
    public JwtSettings(string secret, double duration)
    {
        Secret = secret;
        Duration = duration;
    }

    public string Secret { get; }
    public double Duration { get; }
}