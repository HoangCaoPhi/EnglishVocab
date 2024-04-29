using System.ComponentModel.DataAnnotations;

namespace EnglishVocab.Domain.Entities
{
    public class BaseEntityIdInt : BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }

    public class BaseEntityIdGuid : BaseEntity
    {
        public Guid Id { get; set; }
    }

    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }
}
}
