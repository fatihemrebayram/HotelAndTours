using AutoMapper;
using EntityLayer.Concrete;
using HotelAndTours.DtoLayer.Dtos.RoomDto;
using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.WebUI.Dtos.AppUserDto;
using HotelAndTours.WebUI.Dtos.HotelDto;

namespace HotelAndTours.WebUI.Mapping
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<HotelDto, Hotel>().ReverseMap();
            CreateMap<RoomDto, Room>().ReverseMap();

            CreateMap<AppUserUpdateDto, AppUser>().ReverseMap();
            CreateMap<AppUserUpdateDto, RegisterDto>().ReverseMap();
            CreateMap<AppUserUpdateDto, LoginDto>().ReverseMap();
        }
    }
}