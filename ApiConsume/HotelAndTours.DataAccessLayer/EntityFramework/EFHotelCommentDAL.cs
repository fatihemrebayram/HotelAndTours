using DataAccessLayer.Repositories;
using HotelAndTours.DataAccessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Concrete;
using HotelAndTours.EntityLayer.Concrete;

namespace HotelAndTours.DataAccessLayer.EntityFramework;

public class EFHotelCommentDAL : GenericRepositories<HotelComment>, IHotelCommentDAL
{
    public EFHotelCommentDAL(Context context) : base(context)
    {
    }
}