using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class RecipesRepository : IRecipesRepository
    {
        private readonly ApplicationDbContext _db;

        public RecipesRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Recipe> AddRecipe(Recipe recipe)
        {
            _db.Recipes.Add(recipe);
            await _db.SaveChangesAsync();

            return recipe;
        }

        public async Task AddRecipeWithIngredients(Recipe recipe, List<Ingredient> ingredients)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    _db.Recipes.Add(recipe);
                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<List<Recipe>> GetAllRecipes()
        {
            return await _db.Recipes.Include(r => r.Ingredients).ToListAsync();
        }

        public async Task<Recipe?> GetRecipeByRecipeId(Guid recipeId)
        {
            return await _db.Recipes.Include(r => r.Ingredients).FirstOrDefaultAsync(temp => temp.RecipeID == recipeId);
        }

        public async Task<List<Recipe>> GetRecipeByRecipeName(string title)
        {
            return await _db.Recipes
                .Include(r => r.Ingredients)
                .Where(temp => string.IsNullOrEmpty(title) || temp.Title.Contains(title))
                .ToListAsync();
        }
    }
}