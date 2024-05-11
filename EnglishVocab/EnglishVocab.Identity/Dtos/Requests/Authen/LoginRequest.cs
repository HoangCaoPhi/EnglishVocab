using System.ComponentModel.DataAnnotations;

namespace EnglishVocab.Identity.Dtos.Requests.Authen;
public class LoginRequest
{
    [EmailAddress]
    public required string Email { get; set; }
    public required string Password { get; set; }
}
