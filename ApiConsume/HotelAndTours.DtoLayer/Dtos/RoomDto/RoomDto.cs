using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelAndTours.EntityLayer.Concrete;

namespace HotelAndTours.DtoLayer.Dtos.RoomDto
{
    public record RoomDto
    {
        public DateTime AddedDate { get; set; }

        [Required(ErrorMessage = "Açıklamayı doldurunuz")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Odanın hangi otele ait olduğunu seçiniz")]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Gecelik yetişkin fiyatını doldurunuz")]
        public int PriceForNightAdult { get; set; }

        [Required(ErrorMessage = "Gecelik çocuk fiyatını doldurunuz")]
        public int PriceForNightChildren { get; set; }

        public string? RoomCoverImage { get; set; }

        [Key]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Odanın özelliklerini seçiniz")]
        public string RoomSpecsId { get; set; }

        [Required(ErrorMessage = "Odaya isim veriniz")]
        public string Title { get; set; }

    public virtual Hotel? Hotel { get; set; }
    public virtual ICollection<RRSpecs>? RoomSpecs { get; set; }


    }
}