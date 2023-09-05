using HotelAndTours.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAndTours.DtoLayer.Dtos.BookingDto
{
    public record BookingDto
    {
        [Key]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Giriş tarihini seçiniz")]
        public DateTime CheckIn { get; set; }

        [Required(ErrorMessage = "Çıkış tarihini seçiniz")]
        public DateTime CheckOut { get; set; }

        [Required(ErrorMessage = "Çocuk sayısı seçiniz")]
        public int ChildCount { get; set; }

        [Required(ErrorMessage = "Mail adresini doldurunuz")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Adı doldurunuz")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Telefon numarası giriniz")]
        public string PhoneNumber { get; set; }
        public int PaidPrice { get; set; }

        [Required(ErrorMessage = "Yetişkin sayısı seçiniz")]
        public int AdultCount { get; set; }

        [Required(ErrorMessage = "Oda seçiniz")]
        public int RoomNumberId { get; set; }
        public virtual RoomNumbers? RoomNumber { get; set; }

        [Required(ErrorMessage = "Fatura adresi boş geçilemez")]
        public string BillingAddress { get; set; }
        public string Status { get; set; }
        public DateTime AddedDate { get; set; }

    }
}