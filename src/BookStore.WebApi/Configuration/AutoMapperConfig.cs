using AutoMapper;
using BookStore.Domain.Models;
using BookStore.WebApi.Dtos.Book;
using BookStore.WebApi.Dtos.Category;
using BookStore.WebApi.Dtos.Inventory;
using BookStore.WebApi.Dtos.Order;

namespace BookStore.WebApi.Configuration;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        this.CreateMap<Category, CategoryAddDto>().ReverseMap();
        this.CreateMap<Category, CategoryEditDto>().ReverseMap();
        this.CreateMap<Category, CategoryResultDto>().ReverseMap();

        this.CreateMap<Book, BookAddDto>().ReverseMap();
        this.CreateMap<Book, BookEditDto>().ReverseMap();
        this.CreateMap<Book, BookResultDto>().ReverseMap();

        this.CreateMap<InventoryResultDto, Inventory>()
            .ForMember(x => x.Id, opt => opt.MapFrom(m => m.BookId))
            .ForMember(x => x.Amount, opt => opt.MapFrom(m => m.Amount))
            .ForMember(x => x.Book, opt => opt.Ignore())
            .ReverseMap();
        this.CreateMap<InventoryEditDto, Inventory>()
            .ForMember(x => x.Id, opt => opt.MapFrom(m => m.BookId))
            .ForMember(x => x.Amount, opt => opt.MapFrom(m => m.Amount))
            .ForMember(x => x.Book, opt => opt.Ignore())
            .ReverseMap();
        this.CreateMap<InventoryAddDto, Inventory>()
            .ForMember(x => x.Id, opt => opt.MapFrom(m => m.BookId))
            .ForMember(x => x.Book, opt => opt.Ignore())
            .ForMember(x => x.Amount, opt => opt.MapFrom(m => m.Amount))
            .ReverseMap();

        this.CreateMap<int, Book>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src));
        this.CreateMap<OrderAddDto, Order>()
            .ForMember(x => x.Books, opt => opt.MapFrom(m => m.Books));

        this.CreateMap<Order, OrderResultDto>()
            .ForMember(x => x.Books, opt => opt.MapFrom(m => m.Books!.ConvertAll(x => x.Id)));
    }
}
