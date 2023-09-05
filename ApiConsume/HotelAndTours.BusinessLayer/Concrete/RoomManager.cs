using HotelAndTours.BusinessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Concrete;
using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.ModelsLayer.Models.UpdateCountsClass;
using Microsoft.EntityFrameworkCore;

namespace HotelAndTours.BusinessLayer.Concrete;

public class RoomManager : IRoomService
{
    private readonly UpdateCounts _counts = new();
    private readonly IRoomDAL _roomDAL;

    public RoomManager(IRoomDAL roomDal)
    {
        _roomDAL = roomDal;
    }

    public void AddBL(Room t)
    {
        _roomDAL.InsertDAL(t);
        var _dbContext = new Context();

        var ids = t.RoomSpecsId.Split(',');
        foreach (var id in ids)
        {
            var RRSpecs = new RRSpecs
            {
                RoomId = t.RoomId,
                RoomSpecsId = int.Parse(id)
            };

            _dbContext.RRSpecs.Add(RRSpecs);
        }


        _dbContext.SaveChanges();
        _counts.UpdateCount();
    }

    public void AddRangeBL(List<Room> p)
    {
        _roomDAL.AddRangeDAL(p);
    }

    public List<Room> GetAllById(int id)
    {
        var c = new Context();
        return c.Rooms.Where(x => x.HotelId.Equals(id))
            .Include(x => x.Hotel)
            .Include(x => x.RoomSpecs)
            .ThenInclude(rs => rs.RoomSpecs).ToList();
    }

    public List<Room> GetListBL()
    {
        return _roomDAL.ListDAL();
    }

    public List<Room> GetListFilteredBL(string filter)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Room> GetListQueryableBL()
    {
        return _roomDAL.ListQueryableBL()
            .Include(x => x.Hotel)
            .Include(x => x.RoomSpecs)
            .ThenInclude(rs => rs.RoomSpecs)
            ;
        
    }

    public void RemoveBL(Room t)
    {
        _roomDAL.DeleteDAL(t);
        _counts.UpdateCount();
    }

    public Room TGetByID(int id)
    {
        return _roomDAL.GetByIdDAL(id);
    }

    public void UpdateBL(Room t)
    {
        var hotel = TGetByID(t.RoomId);
        var imagePath = hotel.RoomCoverImage.Split(new[] { "FileImage/" }, StringSplitOptions.None);
        var fileNameExist = imagePath[1];
        var fileExist = new FileInfo(fileNameExist);
        if (!string.IsNullOrEmpty(t.RoomCoverImage))
        {
            if (t.RoomCoverImage != hotel.RoomCoverImage)
            {
                if (File.Exists(fileNameExist)) 
                    fileExist.Delete();
            }
            else
                t.RoomCoverImage = hotel.RoomCoverImage;
        }
        else
        {
            t.RoomCoverImage = hotel.RoomCoverImage;
        }

        _roomDAL.UpdateRoomSpecs(t);
        _roomDAL.UpdateDAL(t);
        _counts.UpdateCount();
    }
}