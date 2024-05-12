using EnglishVocab.Application.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishVocab.Domain.Entities
{
    public class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }

        public bool?  IsDeleted { get; set; }

        [NotMapped]
        public ModelState ModelState { get; set; }

        private Type _type;
        public Type GetEntityType()
        {
            if (_type == null)
            {
                _type = this.GetType();
            }
            return _type;
        }
    }
}
