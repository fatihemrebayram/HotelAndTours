using DataAccessLayer.Repositories;
using HotelAndTours.DataAccessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Concrete;
using HotelAndTours.EntityLayer.Concrete;

namespace HotelAndTours.DataAccessLayer.EntityFramework;

public class EFHotelCategoryDAL : GenericRepositories<HotelCategory>, IHotelCategoryDAL
{
    public EFHotelCategoryDAL(Context context) : base(context)
    {
    }
}