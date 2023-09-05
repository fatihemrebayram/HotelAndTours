using DataAccessLayer.Abstract;
using HotelAndTours.EntityLayer.Concrete;

namespace HotelAndTours.DataAccessLayer.Abstract;

public interface IHotelDAL : IGenericDAL<Hotel>
{
    void UpdateHotelCategory(Hotel t);
}