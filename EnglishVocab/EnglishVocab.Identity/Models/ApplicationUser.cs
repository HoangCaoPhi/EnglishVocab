using EnglishVocab.Identity.Dtos;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishVocab.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? FullName { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }

    [NotMapped]
    public string RoleId { get; set; }
    public bool OwnsToken(string token)
    {
        return this.RefreshTokens?.Find(x => x.Token == token) != null;
    }

}