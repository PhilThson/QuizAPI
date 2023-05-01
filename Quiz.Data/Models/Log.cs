using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Data.Models
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [StringLength(32)]
        public string? Level { get; set; }

        public string? Message { get; set; }

        [StringLength(1024)]
        public string? Logger { get; set; }

        [StringLength(255)]
        public string? RequestUrl { get; set; }

        [StringLength(32)]
        public string? RequestType { get; set; }

        public string? Exception { get; set; }

        public string? AddInfo { get; set; }
    }
}
