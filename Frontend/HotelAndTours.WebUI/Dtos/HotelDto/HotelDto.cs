using System.ComponentModel.DataAnnotations;

namespace HotelAndTours.WebUI.Dtos.HotelDto
{
    public class HotelDto
    {
        public DateTime AddedDate { get; set; }

        [Required(ErrorMessage = "Açıklamayı doldurunuz")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Kapak fotoğrafı seçiniz")]
        public string HotelCoverImage { get; set; }

        [Key]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Otelin en az bir fotoğrafını ekleyin")]
        public string HotelImages { get; set; }

        [Required(ErrorMessage = "Otele isim veriniz")]
        public string HotelName { get; set; }

        [Required(ErrorMessage = "Otelin özelliklerini giriniz")]
        public string HotelSpecs { get; set; }

        [Required(ErrorMessage = "Otelin konumunu giriniz")]
        public string Location { get; set; }
    }
}