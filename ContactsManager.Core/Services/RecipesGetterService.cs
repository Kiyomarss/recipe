using System;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using Services.Helpers;
using ServiceContracts.Enums;
using CsvHelper;
using System.Globalization;
using System.IO;
using ContactsManager.Core.DTO;
using CsvHelper.Configuration;
using OfficeOpenXml;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTimings;
using Exceptions;

namespace Services
{
 public class RecipesGetterService : IRecipesGetterService
 {
  //private field
  private readonly IRecipesRepository _RecipesRepository;
  private readonly ILogger<RecipesGetterService> _logger;
  private readonly IDiagnosticContext _diagnosticContext;

  //constructor
  public RecipesGetterService(IRecipesRepository recipesRepository, ILogger<RecipesGetterService> logger, IDiagnosticContext diagnosticContext)
  {
   _RecipesRepository = recipesRepository;
   _logger = logger;
   _diagnosticContext = diagnosticContext;
  }
  
  public virtual async Task<RecipeResponse?> GetRecipeByRecipeID(Guid? recipeId)
  {
   if (recipeId == null)
    return null;

   var recipe = await _RecipesRepository.GetRecipeByRecipeId(recipeId.Value);

   return recipe?.ToRecipeResponse();
  }
  
  public virtual async Task<List<RecipeResponse>> GetAllRecipes()
  {
   var recipes = await _RecipesRepository.GetAllRecipes();

   return recipes.Select(recipe => recipe.ToRecipeResponse()).ToList();
  }
  
  public virtual async Task<List<RecipeResponse>> GetRecipeByRecipeName(string title)
  {
   var recipes = await _RecipesRepository.GetRecipeByRecipeName(title);

   return recipes.Select(recipe => recipe.ToRecipeResponse()).ToList();
  }
 }
}
