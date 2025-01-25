namespace NomNom.Core.Entities;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
}