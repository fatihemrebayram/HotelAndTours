using HotelAndTours.BusinessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Abstract;
using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.ModelsLayer.Models.UpdateCountsClass;
using Microsoft.EntityFrameworkCore;

namespace HotelAndTours.BusinessLayer.Concrete;

public class RoomNumberManager : IRoomNumberService
{
    private readonly UpdateCounts _counts = new();
    private readonly IRoomNumberDAL _numberDAL;

    public RoomNumberManager(IRoomNumberDAL numberDal)
    {
        _numberDAL = numberDal;
    }

    public void AddBL(RoomNumbers t)
    {
        _numberDAL.InsertDAL(t);
        _counts.UpdateCount();
    }

    public void AddRangeBL(List<RoomNumbers> p)
    {
        _numberDAL.AddRangeDAL(p);
        _counts.UpdateCount();
    }


    public List<RoomNumbers> GetListBL()
    {
        return _numberDAL.ListDAL();
    }

    public List<RoomNumbers> GetListFilteredBL(string filter)
    {
        return _numberDAL.ListDAL(x => x.RoomName.Contains(filter));
    }

    public IQueryable<RoomNumbers> GetListQueryableBL()
    {
        return _numberDAL.ListQueryableBL()
            .Include(x => x.Room)
            .Include(x => x.Room.Hotel)
            .Include(x => x.Booking);
    }

    public void RemoveBL(RoomNumbers t)
    {
        _numberDAL.DeleteDAL(t);
        _counts.UpdateCount();
    }

    public RoomNumbers TGetByID(int id)
    {
        return _numberDAL.GetByIdIncluded(id);
    }

    public void UpdateBL(RoomNumbers t)
    {
        _numberDAL.UpdateDAL(t);
        _counts.UpdateCount();
    }
}