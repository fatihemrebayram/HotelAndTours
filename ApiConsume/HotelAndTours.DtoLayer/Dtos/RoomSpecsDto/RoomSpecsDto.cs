using HotelAndTours.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAndTours.DtoLayer.Dtos.RoomSpecsDto
{
    public record RoomSpecsDto
    {
        public int Id { get; set; }
        public string RoomSpec { get; set; }
        public string Icon { get; set; }
        public int RoomId { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
