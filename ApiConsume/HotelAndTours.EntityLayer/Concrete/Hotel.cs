using System.ComponentModel.DataAnnotations;

namespace HotelAndTours.EntityLayer.Concrete;

public class Hotel
{
    [Key] public int HotelId { get; set; }
    public string HotelName { get; set; }
    public string HotelImages { get; set; }
    public int HotelRoomCount { get; set; }
    public int HotelRoomTypeNumber { get; set; }
    public string HotelSpecs { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public int HotelAviableRoomCount { get; set; }
    public string HotelCoverImage { get; set; }
    public ICollection<Room>? Rooms { get; set; }
    public ICollection<RSHotelCategory> HotelCategories { get; set; }
    public ICollection<HotelComment> HotelComments { get; set; }
    public DateTime AddedDate { get; set; }
}