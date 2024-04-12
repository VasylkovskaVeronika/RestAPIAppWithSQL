using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DTOs;

public class OrderAnimals
{
    [Required]
    [MaxLength(80)]
    public string? Name { get; set; }
    [MaxLength(200)]
    public string? Desc { get; set; }
    public string? Category { get; set; }
    public string? Area { get; set; }

    public bool IsEmpty()
    {
        return Name == null && Desc == null && Category == null && Area == null;
    }

    //public string columnToOrder()
    //{
        
    //}
}