using System.Net;
using System.Text.Json;
using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using NomNom.Core.DTOs;
using NomNom.Core.Entities;
using NomNom.Infrastructure.Data;

namespace NomNomApi.Functions;

public class RecipeFunctions(IMapper mapper, RecipesDbContext context)
{
    [Function("GetRecipes")]
    public async Task<HttpResponseData> GetRecipes(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "recipes")] HttpRequestData req)
    {
        var recipes = await context.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .ToListAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(mapper.Map<List<RecipeDto>>(recipes));
        return response;
    }

    [Function("GetRecipe")]
    public async Task<HttpResponseData> GetRecipe(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "recipes/{id}")] HttpRequestData req,
        int id)
    {
        var recipe = await context.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null)
        {
            return req.CreateResponse(HttpStatusCode.NotFound);
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(mapper.Map<RecipeDto>(recipe));
        return response;
    }
    
    [Function("CreateRecipe")]
    public async Task<HttpResponseData> CreateRecipe(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "recipes")] HttpRequestData req)
    {
        var createDto = await JsonSerializer.DeserializeAsync<CreateRecipeDto>(req.Body);
        var recipe = mapper.Map<Recipe>(createDto);
        recipe.CreatedAt = DateTime.UtcNow;

        context.Recipes.Add(recipe);
        await context.SaveChangesAsync();

        var response = req.CreateResponse(HttpStatusCode.Created);
        await response.WriteAsJsonAsync(mapper.Map<RecipeDto>(recipe));
        return response;
    }
}