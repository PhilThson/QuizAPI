using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Quiz.Data.Models.Base;

namespace Quiz.Data.Models
{
	public class KartaPracy : BaseDictionaryEntity<int>
	{
		public byte[] Zawartosc { get; set; }

        [MaxLength(64)]
        public string? RodzajZawartosci { get; set; }

        public long Rozmiar { get; set; }

        public int ZestawPytanId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(ZestawPytanId))]
        [InverseProperty("ZestawPytanKartyPracy")]
        public virtual ZestawPytan ZestawPytan { get; set; }
    }
}