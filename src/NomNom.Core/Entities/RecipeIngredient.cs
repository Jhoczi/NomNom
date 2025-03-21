namespace NomNom.Core.Entities;

public class RecipeIngredient
{
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; }
    public string Amount { get; set; }
}