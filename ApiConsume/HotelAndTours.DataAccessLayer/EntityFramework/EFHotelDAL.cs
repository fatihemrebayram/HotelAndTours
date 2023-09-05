using DataAccessLayer.Repositories;
using HotelAndTours.DataAccessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Concrete;
using HotelAndTours.EntityLayer.Concrete;

namespace HotelAndTours.DataAccessLayer.EntityFramework;

public class EFHotelDAL : GenericRepositories<Hotel>, IHotelDAL
{
    public EFHotelDAL(Context context) : base(context)
    {
    }

    public void UpdateHotelCategory(Hotel t)
    {
        var _dbContext = new Context();

        // Step 1: Delete the existing RRSpecs for the room
        var existingRRSpecs = _dbContext.RSHotelCategories.Where(rs => rs.HotelId== t.HotelId);
        _dbContext.RSHotelCategories.RemoveRange(existingRRSpecs);
        _dbContext.SaveChanges();

        // Step 2: Add new RRSpecs for the room
        foreach (var id in t.HotelCategories)
        {
            var RRSpecs = new RSHotelCategory()
            {
                HotelId = t.HotelId,
                HotelCategoryId = id.HotelCategoryId
            };

            _dbContext.RSHotelCategories.Add(RRSpecs);
        }

        _dbContext.SaveChanges();
    }
}