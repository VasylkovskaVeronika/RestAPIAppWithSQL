using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DTOs;

public class AddProductWarehouse
{
    [Required]
    public int IdP { get; set; }
    
    [Required] 
    public int IdW { get; set; }
    
    
    [Required]
    [Range(1,int.MaxValue)]
    public int Amount { get; set; }

    [Required]
    public DateTime Created { get; set; }
}