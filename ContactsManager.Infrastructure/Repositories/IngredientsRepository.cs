using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Entities;
using RepositoryContracts;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace Repositories
{
 public class IngredientsRepository : IIngredientsRepository
 {
  private readonly ApplicationDbContext _db;
  private readonly ILogger<IngredientsRepository> _logger;

  public IngredientsRepository(ApplicationDbContext db, ILogger<IngredientsRepository> logger)
  {
   _db = db;
   _logger = logger;
  }

  public async Task<Ingredient> AddIngredient(Ingredient ingredient)
  {
   var existingIngredient = await _db.Ingredients
    .FirstOrDefaultAsync(i => i.IngredientID == ingredient.IngredientID);

   if (existingIngredient == null)
   {
    // اگر موجودیت جدید است، آن را به دیتابیس اضافه کنید
    _db.Entry(ingredient).State = EntityState.Added;
   }
   else
   {
    // اگر موجودیت وجود دارد، فقط فیلدهای قابل تغییر را به روزرسانی کنید
    _db.Entry(existingIngredient).CurrentValues.SetValues(ingredient);
    _db.Entry(existingIngredient).State = EntityState.Modified;
   }

   await _db.SaveChangesAsync();
   return ingredient;
  }


  public async Task<bool> DeleteIngredientByIngredientID(Guid ingredientId)
  {
   _db.Ingredients.RemoveRange(_db.Ingredients.Where(temp => temp.IngredientID == ingredientId));
   int rowsDeleted = await _db.SaveChangesAsync();

   return rowsDeleted > 0;
  }

  public async Task<List<Ingredient>> GetAllIngredient()
  {
   _logger.LogInformation("GetAllIngredient of IngredientRepository");

   return await _db.Ingredients.Include(r => r.Recipe).ToListAsync();
  }

  public async Task<Ingredient?> GetIngredientByIngredientID(Guid ingredientId)
  {
   return await _db.Ingredients.Include(r => r.Recipe)
    .FirstOrDefaultAsync(temp => temp.IngredientID == ingredientId);
  }

  public async Task<Ingredient> UpdateIngredient(Ingredient ingredient)
  {
   Ingredient? matchingIngredient = await _db.Ingredients.FirstOrDefaultAsync(temp => temp.IngredientID == ingredient.IngredientID);

   if (matchingIngredient == null)
    return ingredient;

   matchingIngredient.IngredientID = ingredient.IngredientID;
   matchingIngredient.Quantity = ingredient.Quantity;
   matchingIngredient.Unit = ingredient.Unit;
   matchingIngredient.Description = ingredient.Description;
   matchingIngredient.RecipeId = ingredient.RecipeId;

   int countUpdated = await _db.SaveChangesAsync();

   return matchingIngredient;
  }
 }
}
