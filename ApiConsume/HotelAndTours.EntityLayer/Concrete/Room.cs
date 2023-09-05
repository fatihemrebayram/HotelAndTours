using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAndTours.EntityLayer.Concrete;

public partial class Room
{
    [Key]
    public int RoomId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public int HotelId { get; set; }
    public virtual Hotel? Hotel { get; set; }

    public int PriceForNightAdult { get; set; }

    public int PriceForNightChildren { get; set; }

    public string RoomCoverImage { get; set; }

    public string RoomSpecsId { get; set; }


    public virtual ICollection<RoomNumbers> RoomNumbers { get; set; } = new List<RoomNumbers>();
    public virtual ICollection<RRSpecs> RoomSpecs { get; set; } = new List<RRSpecs>();
    public DateTime AddedDate { get; set; }


}
