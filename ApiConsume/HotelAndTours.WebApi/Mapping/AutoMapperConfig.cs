using AutoMapper;
using HotelAndTours.DtoLayer.Dtos.BookingDto;
using HotelAndTours.DtoLayer.Dtos.HotelCategory;
using HotelAndTours.DtoLayer.Dtos.HotelComments;
using HotelAndTours.DtoLayer.Dtos.HotelDto;
using HotelAndTours.DtoLayer.Dtos.RoomDto;
using HotelAndTours.DtoLayer.Dtos.RoomSpecsDto;
using HotelAndTours.EntityLayer.Concrete;

namespace HotelAndTours.WebApi.Mapping
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<RoomDto, Room>().ReverseMap();
            CreateMap<HotelDto, Hotel>().ReverseMap();
            CreateMap<BookingDto, Booking>().ReverseMap();
            CreateMap<RoomSpecsDto, RoomSpecs>().ReverseMap();
            CreateMap<HotelCategoryDto, HotelCategory>().ReverseMap();
            CreateMap<HotelCommentsDto, HotelComment>().ReverseMap();
        }
    }
}