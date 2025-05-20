using AutoMapper;
using EshoppingZoneAPI.DTOs;
using EshoppingZoneAPI.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Define the mapping from ProductDTO to Product
        CreateMap<ProductDTO, Product>();
    }
}
