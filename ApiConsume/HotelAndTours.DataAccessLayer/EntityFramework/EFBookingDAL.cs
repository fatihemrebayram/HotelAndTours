using DataAccessLayer.Repositories;
using HotelAndTours.DataAccessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Concrete;
using HotelAndTours.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace HotelAndTours.DataAccessLayer.EntityFramework;

public class EFBookingDAL : GenericRepositories<Booking>, IBookingDAL
{
    public EFBookingDAL(Context context) : base(context)
    {
    }

    public Booking GetByIdIncluded(int id)
    {
        var context = new Context();
        var values = context.Bookings
            .Where(x => x.BookingId == id)
            .Include(x => x.RoomNumber).ThenInclude(x => x.Room)
            .FirstOrDefault();
        return values;
    }
}