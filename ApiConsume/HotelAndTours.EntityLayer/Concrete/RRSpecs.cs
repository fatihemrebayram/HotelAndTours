using System.ComponentModel.DataAnnotations;

namespace HotelAndTours.EntityLayer.Concrete;

public class RRSpecs
{
    [Key] public int RoomId { get; set; }

    public Room Room { get; set; }

    [Key] public int RoomSpecsId { get; set; }

    public RoomSpecs RoomSpecs { get; set; }
}