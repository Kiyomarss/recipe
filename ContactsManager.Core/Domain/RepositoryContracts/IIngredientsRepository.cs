using Entities;

namespace RepositoryContracts;

public interface IIngredientsRepository
{
    Task<Ingredient> AddIngredient(Ingredient Ingredient);
    
    Task<bool> DeleteIngredientByIngredientID(Guid IngredientID);

    Task<Ingredient> UpdateIngredient(Ingredient Ingredient);
    
    Task<Ingredient?> GetIngredientByIngredientID(Guid IngredientID);

    Task<List<Ingredient>> GetAllIngredient();
}