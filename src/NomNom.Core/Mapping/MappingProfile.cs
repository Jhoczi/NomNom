using AutoMapper;
using NomNom.Core.DTOs;
using NomNom.Core.Entities;

namespace NomNom.Core.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Recipe, RecipeDto>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.RecipeIngredients));
            
        CreateMap<CreateRecipeDto, Recipe>();
            
        CreateMap<RecipeIngredient, RecipeIngredientDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IngredientId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Ingredient.Name));

        CreateMap<Ingredient, IngredientDto>();
    }
}