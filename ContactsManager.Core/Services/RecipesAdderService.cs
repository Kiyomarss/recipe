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
    public class RecipesAdderService : IRecipesAdderService
    {
        //private field
        private readonly IRecipesRepository _recipesRepository;
        private readonly IIngredientsRepository _ingredientsRepository;
        private readonly IDiagnosticContext _diagnosticContext;


        //constructor
        public RecipesAdderService(IRecipesRepository recipesRepository, IDiagnosticContext diagnosticContext, IIngredientsRepository ingredientsRepository)
        {
            _recipesRepository = recipesRepository;
            _diagnosticContext = diagnosticContext;
            _ingredientsRepository = ingredientsRepository;
        }


        public async Task<RecipeResponse> AddRecipe(RecipeAddRequest? recipeAddRequest)
        {
            if (recipeAddRequest == null)
            {
                throw new ArgumentNullException(nameof(recipeAddRequest));
            }

            ValidationHelper.ModelValidation(recipeAddRequest);

            Recipe recipe = recipeAddRequest.ToRecipe();
            recipe.RecipeID = Guid.NewGuid();

            if (recipe.Ingredients.Any())
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    ingredient.IngredientID = Guid.NewGuid();
                }
            }

            // استفاده از متد جدیدی که تراکنش را مدیریت می‌کند
            await _recipesRepository.AddRecipeWithIngredients(recipe, recipe.Ingredients.ToList());

            return recipe.ToRecipeResponse();
        }

    }
}