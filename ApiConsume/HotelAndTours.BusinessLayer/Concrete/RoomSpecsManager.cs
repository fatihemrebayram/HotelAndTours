using HotelAndTours.BusinessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Abstract;
using HotelAndTours.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace HotelAndTours.BusinessLayer.Concrete;

public class RoomSpecsManager:IRoomSpecsService
{
    private readonly IRoomSpecsDAL _roomSpecDal;

    public RoomSpecsManager(IRoomSpecsDAL roomSpecDal)
    {
        _roomSpecDal = roomSpecDal;
    }


    public void AddBL(RoomSpecs t)
    {
       _roomSpecDal.InsertDAL(t);
    }

    public void AddRangeBL(List<RoomSpecs> p)
    {
        throw new NotImplementedException();
    }

    public List<RoomSpecs> GetListBL()
    {
        throw new NotImplementedException();
    }

    public IQueryable<RoomSpecs> GetListQueryableBL()
    {
        return _roomSpecDal.ListQueryableBL()
            .Include(x => x.RRSpecs)
            .ThenInclude(rs => rs.Room);

    }

    public List<RoomSpecs> GetListFilteredBL(string filter)
    {
        throw new NotImplementedException();
    }

    public void RemoveBL(RoomSpecs t)
    {
       _roomSpecDal.DeleteDAL(t);
    }

    public RoomSpecs TGetByID(int id)
    {
        return _roomSpecDal.GetByIdDAL(id);
    }

    public void UpdateBL(RoomSpecs t)
    {
       _roomSpecDal.UpdateDAL(t);
    }
}