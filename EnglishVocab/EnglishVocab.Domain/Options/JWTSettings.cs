namespace EnglishVocab.Domain.Options;
public class JWTOptions
{
    public static string Name = "JWTSettings";
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public double DurationInMinutes { get; set; }
    public string PrivatekeyPath { get; set; }
}
