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

public class IngredientFunctions(IMapper mapper, RecipesDbContext context)
    {
        [Function("GetIngredients")]
        public async Task<HttpResponseData> GetIngredients(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ingredients")]
            HttpRequestData req)
        {
            var query = context.Ingredients.AsQueryable();

            var category = req.Query["category"];
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(i => i.Category == category);
            }

            var ingredients = await query.ToListAsync();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(mapper.Map<List<IngredientDto>>(ingredients));
            return response;
        }

        [Function("GetIngredient")]
        public async Task<HttpResponseData> GetIngredient(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ingredients/{id}")]
            HttpRequestData req,
            int id)
        {
            var ingredient = await context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(mapper.Map<IngredientDto>(ingredient));
            return response;
        }
        
        [Function("CreateIngredient")]
        public async Task<HttpResponseData> CreateIngredient(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ingredients")] HttpRequestData req)
        {
            var ingredientDto = await JsonSerializer.DeserializeAsync<IngredientDto>(req.Body);
            var ingredient = mapper.Map<Ingredient>(ingredientDto);

            context.Ingredients.Add(ingredient);
            await context.SaveChangesAsync();

            var response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(mapper.Map<IngredientDto>(ingredient));
            return response;
        }

        [Function("DeleteIngredient")]
        public async Task<HttpResponseData> DeleteIngredient(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "ingredients/{id}")] HttpRequestData req,
            int id)
        {
            var ingredient = await context.Ingredients
                .Include(i => i.RecipeIngredients)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (ingredient == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            if (ingredient.RecipeIngredients.Count != 0)
            {
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteStringAsync("Cannot delete ingredient that is used in recipes");
                return response;
            }

            context.Ingredients.Remove(ingredient);
            await context.SaveChangesAsync();

            return req.CreateResponse(HttpStatusCode.NoContent);
        }
    }