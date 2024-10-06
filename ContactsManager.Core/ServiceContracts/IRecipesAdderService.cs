using ContactsManager.Core.DTO;

namespace ServiceContracts;

public interface IRecipesAdderService
{
    Task<RecipeResponse> AddRecipe(RecipeAddRequest? recipeAddRequest);
}