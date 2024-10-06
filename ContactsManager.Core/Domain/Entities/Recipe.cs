using System.ComponentModel.DataAnnotations;

namespace Entities;

public class Recipe
{
    [Key]
    public Guid RecipeID { get; set; }
    
    public string Title { get; set; }
    
    public string publisher { get; set; }
    
    public int Servings { get; set; }
    
    public int CookingTime { get; set; }
    
    public string SourceUrl { get; set; }
    
    public string ImageUrl { get; set; }
    
    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}