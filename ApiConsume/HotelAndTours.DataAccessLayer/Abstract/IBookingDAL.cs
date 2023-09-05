using DataAccessLayer.Abstract;
using HotelAndTours.EntityLayer.Concrete;

namespace HotelAndTours.DataAccessLayer.Abstract;

public interface IBookingDAL : IGenericDAL<Booking>
{
    Booking GetByIdIncluded(int id);
}