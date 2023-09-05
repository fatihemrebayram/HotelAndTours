using HotelAndTours.BusinessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Concrete;
using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.ModelsLayer.Models.UpdateCountsClass;
using Microsoft.EntityFrameworkCore;

namespace HotelAndTours.BusinessLayer.Concrete;

public class HotelManager : IHotelService
{
    private readonly UpdateCounts _counts = new();
    private readonly IHotelDAL _hotelDAL;

    public HotelManager(IHotelDAL hotelDal)
    {
        _hotelDAL = hotelDal;
    }

    public void AddBL(Hotel t)
    {
        var _dbContext = new Context();
        _hotelDAL.InsertDAL(t);

        foreach (var id in t.HotelCategories)
        {
            var RRSpecs = new RSHotelCategory
            {
                HotelCategoryId = id.HotelCategoryId,
                HotelId = t.HotelId
            };

            _dbContext.RSHotelCategories.Add(RRSpecs);
        }

        _dbContext.SaveChanges();

        _counts.UpdateCount();
    }

    public void AddRangeBL(List<Hotel> p)
    {
        _hotelDAL.AddRangeDAL(p);
        _counts.UpdateCount();
    }

    public List<Hotel> GetListBL()
    {
        return _hotelDAL.ListDAL();
    }

    public List<Hotel> GetListFilteredBL(string filter)
    {
        return _hotelDAL.ListDAL(x => x.HotelName.Contains(filter));
    }

    public IQueryable<Hotel> GetListQueryableBL()
    {
        return _hotelDAL.ListQueryableBL()
                .Include(x => x.HotelComments)
                .Include(x => x.Rooms)
                .ThenInclude(x => x.RoomSpecs)
                .Include(x => x.HotelCategories)
                .ThenInclude(x => x.HotelCategory)
            ;
    }

    public void RemoveBL(Hotel t)
    {
        _hotelDAL.DeleteDAL(t);
        _counts.UpdateCount();
    }

    public Hotel TGetByID(int id)
    {
        var c = new Context();
        return c.Hotels
            .Include(h => h.Rooms)
            .Include(x => x.HotelCategories)
            .ThenInclude(x => x.HotelCategory)
            .Include(x => x.HotelComments) // Include the related Rooms entities
            .FirstOrDefault(h => h.HotelId == id);
        //   return _hotelDAL.GetByIdDAL(id, includeRelatedEntities: true);
    }

    public void UpdateBL(Hotel t)
    {
        var hotel = TGetByID(t.HotelId);
        var imagePath = hotel.HotelCoverImage.Split(new[] { "FileImage/" }, StringSplitOptions.None);
        var fileNameExist = imagePath[1];
        var fileExist = new FileInfo(fileNameExist);
        if (!string.IsNullOrEmpty(t.HotelCoverImage))
        {
            if (t.HotelCoverImage != hotel.HotelCoverImage)
            {
                if (File.Exists(fileNameExist))
                    fileExist.Delete();
            }
            else
            {
                t.HotelCoverImage = hotel.HotelCoverImage;
            }
        }
        else
        {
            t.HotelCoverImage = hotel.HotelCoverImage;
        }

        _hotelDAL.UpdateHotelCategory(t);
        _hotelDAL.UpdateDAL(t);
        _counts.UpdateCount();
    }
}