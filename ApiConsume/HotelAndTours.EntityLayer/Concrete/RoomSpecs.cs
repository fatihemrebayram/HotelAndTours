using System.ComponentModel.DataAnnotations;

namespace HotelAndTours.EntityLayer.Concrete;

public class RoomSpecs
{
    [Key]
    public int Id { get; set; }
    public string RoomSpec { get; set; }
    public string Icon { get; set; }
    public virtual ICollection<RRSpecs> RRSpecs { get; set; }
    public DateTime AddedDate { get; set; }

}