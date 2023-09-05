using HotelAndTours.DtoLayer.Dtos.HotelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelAndTours.DtoLayer.Dtos.RoomDto;

namespace HotelAndTours.ModelsLayer.Models.Hotel
{
    public class HotelDetailsViewModel
    {
        public EntityLayer.Concrete.Hotel HotelDetails { get; set; }
        public List<IGrouping<int, EntityLayer.Concrete.Room>> RoomGroups { get; set; }
    }
}