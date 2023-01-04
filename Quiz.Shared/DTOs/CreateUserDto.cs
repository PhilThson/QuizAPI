using System;
using System.ComponentModel.DataAnnotations;

namespace Quiz.Shared.DTOs
{
	public class CreateUserDto
	{
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(200)]
        public string PasswordHash { get; set; }
        [Required]
        public byte? RoleId { get; set; }
    }
}

