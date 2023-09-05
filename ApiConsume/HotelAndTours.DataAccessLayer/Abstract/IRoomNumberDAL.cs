using DataAccessLayer.Abstract;
using HotelAndTours.EntityLayer.Concrete;

namespace HotelAndTours.DataAccessLayer.Abstract;

public interface IRoomNumberDAL : IGenericDAL<RoomNumbers>
{
    RoomNumbers GetByIdIncluded(int id);
}