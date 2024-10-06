using Entities;

namespace RepositoryContracts;

public interface IRecipesRepository
{
    Task<Recipe> AddRecipe(Recipe recipe);

    Task AddRecipeWithIngredients(Recipe recipe, List<Ingredient> ingredients);

    Task<List<Recipe>> GetAllRecipes();

    Task<Recipe?> GetRecipeByRecipeId(Guid recipeId);
    
    Task<List<Recipe>> GetRecipeByRecipeName(string title);

}