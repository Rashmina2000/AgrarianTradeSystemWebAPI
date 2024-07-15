using AgrarianTradeSystemWebAPI.Dtos;
using AgrarianTradeSystemWebAPI.Models;
using AutoMapper;

namespace AgrarianTradeSystemWebAPI
{
    public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile() {
			//CreateMap<Product, ProductDto>();
			CreateMap<ProductDto, Product>();
			CreateMap<AddReviewDto, Review>();
		}
	}
}
