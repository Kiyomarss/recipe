using Entities;

namespace ContactsManager.Core.DTO;

public class IngredientAddRequest
{
    public string Quantity { get; set; }
    
    public string Unit { get; set; }
    
    public string Description { get; set; }

    public Ingredient ToIngredient()
    {
        return new Ingredient
        {
            Quantity = Quantity,
            Unit = Unit,
            Description = Description
        };
    }
    
}
