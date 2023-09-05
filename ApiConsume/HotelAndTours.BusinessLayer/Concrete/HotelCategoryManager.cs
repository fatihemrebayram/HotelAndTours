using HotelAndTours.BusinessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Abstract;
using HotelAndTours.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace HotelAndTours.BusinessLayer.Concrete;

public class HotelCategoryManager : IHotelCategoryService
{
    private readonly IHotelCategoryDAL _categoryDal;

    public HotelCategoryManager(IHotelCategoryDAL categoryDal)
    {
        _categoryDal = categoryDal;
    }

    public void AddBL(HotelCategory t)
    {
        _categoryDal.InsertDAL(t);
    }

    public void AddRangeBL(List<HotelCategory> p)
    {
        _categoryDal.AddRangeDAL(p);
    }

    public List<HotelCategory> GetListBL()
    {
        return _categoryDal.ListDAL();
    }

    public IQueryable<HotelCategory> GetListQueryableBL()
    {
        return _categoryDal.ListQueryableBL().Include(x=>x.Hotels).ThenInclude(x=>x.Hotel);
    }

    public List<HotelCategory> GetListFilteredBL(string filter)
    {
        return _categoryDal.ListQueryableBL(x => x.HotelCategoryName.Contains(filter)).ToList();
    }

    public void RemoveBL(HotelCategory t)
    {
        _categoryDal.DeleteDAL(t);
    }

    public HotelCategory TGetByID(int id)
    {
        return _categoryDal.GetByIdDAL(id);
    }

    public void UpdateBL(HotelCategory t)
    {
        _categoryDal.UpdateDAL(t);
    }
}