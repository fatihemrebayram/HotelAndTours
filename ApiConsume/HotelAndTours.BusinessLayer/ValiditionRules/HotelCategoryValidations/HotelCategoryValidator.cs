using FluentValidation;
using HotelAndTours.DtoLayer.Dtos.HotelCategory;

namespace HotelAndTours.BusinessLayer.ValiditionRules.HotelCategoryValidations;

public class HotelCategoryValidator : AbstractValidator<HotelCategoryDto>
{
    public HotelCategoryValidator()
    {
        RuleFor(x => x.HotelCategoryName).NotEmpty().WithMessage("Kategori Adını Boş Geçemezsiniz");
        RuleFor(x => x.HotelCategoryName).MinimumLength(1).WithMessage("Kategori Adı 1 Karakterden Aşağı Olamaz");
        RuleFor(x => x.HotelCategoryName).MaximumLength(100).WithMessage("Kategori Adı 100 Karakterden Fazla Olamaz");
    }
}