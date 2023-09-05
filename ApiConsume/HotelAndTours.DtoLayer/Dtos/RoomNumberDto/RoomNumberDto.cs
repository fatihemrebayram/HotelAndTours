using HotelAndTours.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAndTours.DtoLayer.Dtos.RoomNumberDto;

public record RoomNumberDto
{
    [Required(ErrorMessage = "Numaranın hangi odaya ait olduğunu seçiniz")]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "Odanın ismini giriniz")]
    public string RoomName { get; set; }

    public int RoomNumberId { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("RoomNumbers")]
    public virtual Room? Room { get; set; }
}
