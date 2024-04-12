using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DTOs;

public class AddAnimal
{
    [Required]
    [MaxLength(80)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string? Desc { get; set; }
    public string Category { get; set; }
    public string Area { get; set; }
}