namespace EnglishVocab.Identity.Models;
class RolePermission
{
    public string Resource { get; set; }
    public string[] Action { get; set; }
}