using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace ContactsManager.UI.Controllers;

[Route("[controller]")]
[AllowAnonymous]
public class RecipesController  : Controller
{
    private readonly IRecipesGetterService _recipesGetterService;
    private readonly IRecipesAdderService _recipesAdderService;

    public RecipesController(IRecipesGetterService recipesGetterService, IRecipesAdderService recipesAdderService)
    {
        _recipesGetterService = recipesGetterService;
        _recipesAdderService = recipesAdderService;
    }
    
    [Route("[action]")]
    [Route("/")]
    public Task<IActionResult> Index()
    {
        return Task.FromResult<IActionResult>(PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "dist", "index.html"), "text/html"));
    }
    
    [Route("[action]")]
    public async Task<IActionResult> Search(string title)
    {
        //Search
        List<RecipeResponse> recipes = await _recipesGetterService.GetRecipeByRecipeName(title);

        return Json(recipes);
    }
    
    [Route("[action]")]
    public async Task<IActionResult> Load(Guid id)
    {
        //Search
        RecipeResponse? recipes = await _recipesGetterService.GetRecipeByRecipeID(id);

        return Json(new { recipes });
    }
    
    [HttpPost]
    //Url: Recipes/create
    [Route("[action]")]
    public async Task<IActionResult> Create([FromBody] RecipeAddRequest recipeAddRequest)
    {
        if (recipeAddRequest == null)
        {
            throw new ArgumentNullException(nameof(recipeAddRequest));
        }

        //call the service method
        RecipeResponse recipeResponse = await _recipesAdderService.AddRecipe(recipeAddRequest);

        //navigate to Index() action method (it makes another get request to "Recipes/index"
        return Json(recipeResponse);
    }
}