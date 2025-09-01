using System.ComponentModel.DataAnnotations;

namespace VehicleAPI.Models;

public class Vehicle
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, StringLength(50)] 
    public string Make { get; set; } = null!;
    
    [Required, StringLength(50)]
    public string Model { get; set; } = null!;
    
    [Range(1900, 2100)]
    public int Year { get; set; }
}