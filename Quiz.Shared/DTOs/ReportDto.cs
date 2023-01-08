namespace Quiz.Shared.DTOs
{
	public class ReportDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public byte[] Content { get; set; }
        public long Size { get; set; }
    }
}

