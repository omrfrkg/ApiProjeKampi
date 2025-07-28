using ApiProjeKampi.WebApi.Dtos.AboutDtos;
using ApiProjeKampi.WebApi.Dtos.CategoryDtos;
using ApiProjeKampi.WebApi.Dtos.ChefDtos;
using ApiProjeKampi.WebApi.Dtos.FeatureDtos;
using ApiProjeKampi.WebApi.Dtos.MessageDtos;
using ApiProjeKampi.WebApi.Dtos.NotificationDtos;
using ApiProjeKampi.WebApi.Dtos.ProductDtos;
using ApiProjeKampi.WebApi.Dtos.ServiceDtos;
using ApiProjeKampi.WebApi.Dtos.TestimonialDtos;
using ApiProjeKampi.WebApi.Dtos.YummyEventDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;

namespace ApiProjeKampi.WebApi.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Feature,ResultFeatureDto>().ReverseMap();
            CreateMap<Feature,CreateFeatureDto>().ReverseMap();
            CreateMap<Feature,GetByIdFeatureDto>().ReverseMap();
            CreateMap<Feature,UpdateFeatureDto>().ReverseMap();

            CreateMap<Message,CreateMessageDto>().ReverseMap();
            CreateMap<Message,GetByIdMessageDto>().ReverseMap();
            CreateMap<Message,UpdateMessageDto>().ReverseMap();
            CreateMap<Message,ResultMessageDto>().ReverseMap();

            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, GetProductByIdDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Product, ResultProductWithCategoryDto>().ForMember(x => x.CategoryName, y => y.MapFrom(z => z.Category.CategoryName)).ReverseMap();


            CreateMap<Service, CreateServiceDto>().ReverseMap();
            CreateMap<Service, GetByIdServiceDto>().ReverseMap();
            CreateMap<Service, UpdateFeatureDto>().ReverseMap();
            CreateMap<Service, ResultServiceDto>().ReverseMap();

            CreateMap<Notification, CreateNotificationDto>().ReverseMap();
            CreateMap<Notification, GetNotificationByIdDto>().ReverseMap();
            CreateMap<Notification, UpdateNotificationDto>().ReverseMap();
            CreateMap<Notification, ResultNotificationDto>().ReverseMap();

            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, GetCategoryByIdDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, ResultCategoryDto>().ReverseMap();

            CreateMap<About, CreateAboutDto>().ReverseMap();
            CreateMap<About, GetAboutByIdDto>().ReverseMap();
            CreateMap<About, UpdateAboutDto>().ReverseMap();
            CreateMap<About, ResultAboutDto>().ReverseMap();

            CreateMap<Testimonial, CreateTestimonialDto>().ReverseMap();
            CreateMap<Testimonial, GetByIdTestimonialDto>().ReverseMap();
            CreateMap<Testimonial, UpdateTestimonialDto>().ReverseMap();
            CreateMap<Testimonial, ResultTestimonialDto>().ReverseMap();

            CreateMap<YummyEvent, CreateYummyEventDto>().ForMember(dest => dest.ImageUrl, opt => opt.Ignore()).ReverseMap();
            CreateMap<YummyEvent, GetYummyEventByIdDto>().ReverseMap();
            CreateMap<YummyEvent, UpdateYummyEventDto>().ForMember(dest => dest.ImageUrl, opt => opt.Ignore()).ReverseMap();
            CreateMap<YummyEvent, ResultYummyEventDto>().ReverseMap();

            CreateMap<Chef, CreateChefDto>().ForMember(dest => dest.ImageUrl, opt => opt.Ignore()).ReverseMap().ReverseMap();
            CreateMap<Chef, GetChefByIdDto>().ReverseMap();
            CreateMap<Chef, UpdateChefDto>().ForMember(dest => dest.ImageUrl, opt => opt.Ignore()).ReverseMap().ReverseMap();
            CreateMap<Chef, ResultChefDto>().ReverseMap();

        }
    }
}
