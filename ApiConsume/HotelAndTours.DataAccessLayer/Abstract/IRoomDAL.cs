using DataAccessLayer.Abstract;
using HotelAndTours.EntityLayer.Concrete;

namespace HotelAndTours.DataAccessLayer.Abstract;

public interface IRoomDAL : IGenericDAL<Room>
{
    void UpdateRoomSpecs(Room t);
}