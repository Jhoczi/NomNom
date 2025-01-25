namespace NomNom.Core.DTOs;

public class CreateRecipeDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Instructions { get; set; }
    public int PrepTimeMinutes { get; set; }
    public int CookTimeMinutes { get; set; }
    public ICollection<CreateRecipeIngredientDto> Ingredients { get; set; }
}