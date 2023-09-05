using HotelAndTours.BusinessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Abstract;
using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.ModelsLayer.Models.UpdateCountsClass;
using Microsoft.EntityFrameworkCore;

namespace HotelAndTours.BusinessLayer.Concrete;

public class BookingManager : IBookingService
{
    private readonly IBookingDAL _bookingDal;
    private readonly UpdateCounts _counts = new();

    public BookingManager(IBookingDAL bookingDal)
    {
        _bookingDal = bookingDal;
    }

    public void AddBL(Booking t)
    {
        _bookingDal.InsertDAL(t);
        _counts.UpdateCount();
    }

    public void AddRangeBL(List<Booking> p)
    {
        _bookingDal.AddRangeDAL(p);
        _counts.UpdateCount();
    }

    public List<Booking> GetListBL()
    {
        return _bookingDal.ListDAL();
    }

    public List<Booking> GetListFilteredBL(string filter)
    {
        return _bookingDal.ListDAL(x => x.Name.Contains(filter) ||
                                        x.CheckIn.ToString().Contains(filter) ||
                                        x.CheckOut.ToString().Contains(filter) ||
                                        x.Mail.Contains(filter) ||
                                        x.PhoneNumber.Contains(filter));
    }

    public IQueryable<Booking> GetListQueryableBL()
    {
        return _bookingDal.ListQueryableBL()
            .Include(x => x.RoomNumber)
            .Include(x => x.RoomNumber.Room)
            .Include(x => x.RoomNumber.Room.Hotel);
    }

    public void RemoveBL(Booking t)
    {
        _bookingDal.DeleteDAL(t);
        _counts.UpdateCount();
    }

    public Booking TGetByID(int id)
    {
        return _bookingDal.GetByIdIncluded(id);
    }

    public void UpdateBL(Booking t)
    {
        _bookingDal.UpdateDAL(t);
        _counts.UpdateCount();
    }
}