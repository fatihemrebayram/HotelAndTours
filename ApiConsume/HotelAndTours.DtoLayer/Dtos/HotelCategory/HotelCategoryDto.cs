using System.ComponentModel.DataAnnotations;

namespace HotelAndTours.DtoLayer.Dtos.HotelCategory;

public record HotelCategoryDto
{
    [Key] public int HotelCategoryId { get; set; }

    [Required(ErrorMessage = "Kategoriye İsim Veriniz")]
    public string HotelCategoryName { get; set; }
}