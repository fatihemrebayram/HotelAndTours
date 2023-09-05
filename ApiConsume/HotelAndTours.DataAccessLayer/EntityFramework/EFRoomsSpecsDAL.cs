using DataAccessLayer.Repositories;
using HotelAndTours.DataAccessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Concrete;
using HotelAndTours.EntityLayer.Concrete;

namespace HotelAndTours.DataAccessLayer.EntityFramework;

public class EFRoomsSpecsDAL : GenericRepositories<RoomSpecs>, IRoomSpecsDAL
{
    public EFRoomsSpecsDAL(Context context) : base(context)
    {
    }
}