namespace EnglishVocab.Domain.Options;
public class JWTOptions
{
    public static string Name = "JWTSettings";

    public static string SecretKey = "qB3YXDDTV9yaWCfl5SsqLLWl2DHVOCBg";

    public string Issuer { get; set; }
    public string Audience { get; set; }
    public double DurationInMinutes { get; set; }
    public string PrivatekeyPath { get; set; }
}
