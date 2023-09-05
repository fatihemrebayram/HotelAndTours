using HotelAndTours.DtoLayer.Dtos.HotelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelAndTours.DtoLayer.Dtos.BookingDto;

namespace HotelAndTours.ModelsLayer.Models.Booking
{
    public class BookingCRUDViewModel
    {
        public EntityLayer.Concrete.Booking _BookingAddViewModel { get; set; }
        public List<EntityLayer.Concrete.Booking> _BookingViewModel { get; set; }
    }
}