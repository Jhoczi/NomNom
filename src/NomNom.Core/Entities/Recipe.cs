namespace NomNom.Core.Entities;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Instructions { get; set; }
    public int PrepTimeMinutes { get; set; }
    public int CookTimeMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
}