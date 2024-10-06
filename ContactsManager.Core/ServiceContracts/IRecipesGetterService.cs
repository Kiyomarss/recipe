using ContactsManager.Core.DTO;

namespace ServiceContracts;

public interface IRecipesGetterService
{
    Task<RecipeResponse?> GetRecipeByRecipeID(Guid? RecipeID);

    Task<List<RecipeResponse>> GetAllRecipes();

    Task<List<RecipeResponse>> GetRecipeByRecipeName(string title);
}