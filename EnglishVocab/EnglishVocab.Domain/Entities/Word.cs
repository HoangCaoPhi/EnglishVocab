using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishVocab.Domain.Entities
{
    public class Word : BaseEntityIdInt
    {
        [MaxLength(255)]
        [Required]
        public string Vocabulary { get; set; }

        [MaxLength(255)]
        public string Pronunciation { get; set; }

        public Group? Group { get; set; }

        [ForeignKey("Group")]
        public int GroupId { get; set; }
    }
}
