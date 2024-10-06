using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

public class Ingredient
{
    [Key]
    public Guid IngredientID { get; set; }
    
    public string? Quantity { get; set; }
    
    public string? Unit { get; set; }
    
    public string? Description { get; set; }
    
    public Guid RecipeId { get; set; }

    
    [ForeignKey("RecipeId")]
    public virtual Recipe Recipe { get; set; }
}