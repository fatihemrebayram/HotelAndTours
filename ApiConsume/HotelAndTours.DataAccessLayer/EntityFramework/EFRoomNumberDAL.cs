using DataAccessLayer.Repositories;
using HotelAndTours.DataAccessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Concrete;
using HotelAndTours.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace HotelAndTours.DataAccessLayer.EntityFramework;

public class EFRoomNumberDAL : GenericRepositories<RoomNumbers>, IRoomNumberDAL
{
    public EFRoomNumberDAL(Context context) : base(context)
    {
    }

    public RoomNumbers GetByIdIncluded(int id)
    {
        using var _context = new Context();
        return _context.RoomNumbers
            .Where(x => x.RoomNumberId == id)
            .Include(x => x.Room)
            .ThenInclude(x => x.Hotel)
            .FirstOrDefault();
    }
}