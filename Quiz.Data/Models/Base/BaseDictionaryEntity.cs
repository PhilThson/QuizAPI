using System.ComponentModel.DataAnnotations;

namespace Quiz.Data.Models.Base
{
    public class BaseDictionaryEntity<T> : BaseEntity<T>
    {
        [Required]
        [MaxLength(512)]
        public string Nazwa { get; set; }

        [MaxLength(1024)]
        public string? Opis { get; set; }
    }
}
