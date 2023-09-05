using System.ComponentModel.DataAnnotations;

namespace HotelAndTours.EntityLayer.Concrete;

public class HotelCategory
{
    [Key]
    public int HotelCategoryId { get; set; }
    public string HotelCategoryName { get; set; }
    // Other category properties

    // Collection of hotels associated with the category
    public ICollection<RSHotelCategory> Hotels { get; set; }
}