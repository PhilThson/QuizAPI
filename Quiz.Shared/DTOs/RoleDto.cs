using System;
namespace Quiz.Shared.DTOs
{
	public class RoleDto
	{
        public byte Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}