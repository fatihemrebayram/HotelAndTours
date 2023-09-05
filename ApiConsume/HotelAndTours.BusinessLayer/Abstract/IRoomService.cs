using BusinessLayer.Abstract;
using HotelAndTours.EntityLayer.Concrete;

namespace HotelAndTours.BusinessLayer.Abstract;

public interface IRoomService : IGenericService<Room>
{
    List<Room> GetAllById(int id);
}