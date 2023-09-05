using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelAndTours.DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace HotelAndTours.ModelsLayer.Models.UpdateCountsClass
{
    public class UpdateCounts
    {
        public void UpdateCount()
        {
            using (var context = new Context())
            {
                var hotels = context.Hotels
                    .Include(h => h.Rooms)
                    .ThenInclude(r => r.RoomNumbers)
                    .ToList();

                foreach (var hotel in hotels)
                {
                    hotel.HotelRoomCount = hotel.Rooms.Sum(r => r.RoomNumbers.Count);
                    hotel.HotelRoomTypeNumber = hotel.Rooms.Select(r => r.RoomId).Distinct().Count();
                    hotel.HotelAviableRoomCount = hotel.Rooms
                        .SelectMany(r => r.RoomNumbers)
                        .GroupJoin(
                            context.Bookings,
                            rn => rn.RoomNumberId,
                            b => b.RoomNumberId,
                            (rn, b) => new { RoomNumber = rn, Bookings = b })
                        .Select(x => new { x.RoomNumber, Count = x.Bookings.Count() })
                        .Sum(x => x.RoomNumber != null ? 1 - x.Count : 1);

                    context.SaveChanges();
                }
            }
        }
    }
}