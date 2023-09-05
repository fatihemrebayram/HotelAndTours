namespace HotelAndTours.EntityLayer.Concrete;

public class RSHotelCategory
{
    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }

    public int HotelCategoryId { get; set; }
    public HotelCategory? HotelCategory { get; set; }
}