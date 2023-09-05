using System.ComponentModel.DataAnnotations;

namespace HotelAndTours.EntityLayer.Concrete;

public class Booking
{
    [Key] public int BookingId { get; set; }
    public string Name { get; set; }
    public string Mail { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int AdultCount { get; set; }
    public int ChildCount { get; set; }
    public string Status { get; set; }
    public string BillingAddress { get; set; }
    public int PaidPrice { get; set; }
    public int RoomNumberId { get; set; }
    public virtual RoomNumbers? RoomNumber { get; set; }
    public DateTime AddedDate { get; set; }
}