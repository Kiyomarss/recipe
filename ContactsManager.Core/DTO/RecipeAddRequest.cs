using Entities;

namespace ContactsManager.Core.DTO;

public class RecipeAddRequest
{
    public string Title { get; set; }
    
    public string publisher { get; set; }
    
    public int Servings { get; set; }
    
    public int CookingTime { get; set; }
    
    public string SourceUrl { get; set; }
    
    public string ImageUrl { get; set; }
    
    public ICollection<IngredientAddRequest> Ingredients { get; set; } = new List<IngredientAddRequest>();

    public Recipe ToRecipe()
    {
        var ingredients = Ingredients?.Select(x => x.ToIngredient()).ToList();
        
        return new Recipe
        {
            Title = Title,
            publisher = publisher,
            Servings = Servings,
            CookingTime = CookingTime,
            SourceUrl = SourceUrl,
            ImageUrl = ImageUrl,
            Ingredients = ingredients
        };
    }
}

public class RecipeAddRequest2
{
    public string Title { get; set; }

    public string publisher { get; set; }

    public int Servings { get; set; }

    public int CookingTime { get; set; }

    public string SourceUrl { get; set; }

    public string ImageUrl { get; set; }
    public ICollection<IngredientAddRequest>? Ingredients { get; set; }

}