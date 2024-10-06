using Entities;

namespace ContactsManager.Core.DTO;

public class IngredientResponse
{
    public Guid IngredientID { get; set; }

    public string Quantity { get; set; }
    
    public string Unit { get; set; }
    
    public string Description { get; set; }
}

public static class IngredientExtensions
{
    public static IngredientResponse ToIngredientResponse(this Ingredient ingredient)
    {
        return new IngredientResponse()
        {
            IngredientID = ingredient.IngredientID, 
            Quantity = ingredient.Quantity, 
            Unit = ingredient.Unit, 
            Description = ingredient.Description
        };
    }
}