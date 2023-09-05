using HotelAndTours.EntityLayer.Concrete;

namespace HotelAndTours.ModelsLayer.Models.Purchase;

public class BookingDetailsViewModel
{
    public int Price { get; set; }
    public string CheckIn { get; set; }
    public string CheckOut { get; set; }

    public RoomNumbers RoomNumber { get; set; }
    public int Adults { get; set; }
    public int Childs { get; set; }
    public string Mail { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string BillingAddress { get; set; }
}