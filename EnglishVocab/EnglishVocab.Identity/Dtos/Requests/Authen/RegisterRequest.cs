using System.ComponentModel.DataAnnotations;

namespace EnglishVocab.Identity.Dtos.Requests.Authen;
public class RegisterRequest
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    [EmailAddress]
    public required string Email { get; set; }

    [MinLength(6)]
    public required string UserName { get; set; }

    [MinLength(6)]
    public required string  Password { get; set; }

    [Compare("Password")]
    public required string ConfirmPassword { get; set; }
}
