using HotelAndTours.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAndTours.DtoLayer.Dtos.HotelDto;

    public record HotelDto
    {
        public DateTime AddedDate { get; set; }

        [Required(ErrorMessage = "Açıklamayı doldurunuz")]
        public string Description { get; set; }

        public int HotelAviableRoomCount { get; set; }

        public string? HotelCoverImage { get; set; }

        [Key]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Otelin en az bir fotoğrafını ekleyin")]
        public string HotelImages { get; set; }

        [Required(ErrorMessage = "Otele isim veriniz")]
        public string HotelName { get; set; }

        public int HotelRoomCount { get; set; }

        public int HotelRoomTypeNumber { get; set; }

        [Required(ErrorMessage = "Otelin özelliklerini giriniz")]
        public string HotelSpecs { get; set; }

        [Required(ErrorMessage = "Otelin konumunu giriniz")]
        public string Location { get; set; }
        public ICollection<Room>? Rooms { get; set; }
        public ICollection<RSHotelCategory>? HotelCategories { get; set; }



}
