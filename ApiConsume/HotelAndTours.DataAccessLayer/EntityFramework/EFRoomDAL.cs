using DataAccessLayer.Abstract;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelAndTours.DataAccessLayer.Abstract;
using HotelAndTours.EntityLayer.Concrete;
using HotelAndTours.DataAccessLayer.Concrete;

namespace HotelAndTours.DataAccessLayer.EntityFramework
{
    public class EFRoomDAL : GenericRepositories<Room>, IRoomDAL
    {
        public EFRoomDAL(Context context) : base(context)
        {

        }

        public void UpdateRoomSpecs(Room t)
        {
            var _dbContext = new Context();

            // Step 1: Delete the existing RRSpecs for the room
            var existingRRSpecs = _dbContext.RRSpecs.Where(rs => rs.RoomId == t.RoomId);
            _dbContext.RRSpecs.RemoveRange(existingRRSpecs);
            _dbContext.SaveChanges();

            // Step 2: Add new RRSpecs for the room
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
        }
    }
}