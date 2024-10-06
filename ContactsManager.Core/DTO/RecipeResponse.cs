using Entities;

namespace ContactsManager.Core.DTO;

public class RecipeResponse
{
    public Guid RecipeID { get; set; }

    public string Title { get; set; }
    
    public string publisher { get; set; }
    
    public int Servings { get; set; }
    
    public int CookingTime { get; set; }
    
    public string SourceUrl { get; set; }
    
    public string ImageUrl { get; set; }
    
    public IEnumerable<IngredientResponse>? Ingredients { get; set; }
}


public static class RecipeExtensions
{
    public static RecipeResponse ToRecipeResponse(this Recipe recipe)
    {
        return new RecipeResponse()
        {
            RecipeID = recipe.RecipeID, 
            Title = recipe.publisher, 
            Servings = recipe.Servings, 
            CookingTime = recipe.CookingTime, 
            ImageUrl = recipe.ImageUrl, 
            SourceUrl = recipe.SourceUrl, 
            Ingredients = recipe.Ingredients?.Select(x => x.ToIngredientResponse())
        };
    }
}