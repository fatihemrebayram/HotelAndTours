using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAndTours.EntityLayer.Concrete;

public partial class RoomNumbers
{
    [Key]
    public int RoomNumberId { get; set; }

    public DateTime AddedDate { get; set; }

    public string RoomName { get; set; }

    public int RoomId { get; set; }

    public virtual Room? Room { get; set; }
    public virtual List<Booking>? Booking { get; set; }

}
