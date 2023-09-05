using FluentValidation;
using HotelAndTours.DtoLayer.Dtos.RoomDto;

namespace HotelAndTours.BusinessLayer.ValiditionRules.RoomValidations;

public class RoomValidator : AbstractValidator<RoomDto>
{
    public RoomValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Oda Adını Boş Geçemezsiniz");
        RuleFor(x => x.Title).MinimumLength(1).WithMessage("Oda Adı 3 Karakterden Aşağı Olamaz");
        RuleFor(x => x.Title).MaximumLength(100).WithMessage("Oda Adı 100 Karakterden Fazla Olamaz");

        RuleFor(x => x.PriceForNightAdult).NotEmpty().WithMessage("Gecelik Yetişkin Fiyatını Boş Geçemezsiniz");
        RuleFor(x => x.PriceForNightChildren).NotEmpty().WithMessage("Gecelik Çocuk Fiyatını Boş Geçemezsiniz");

        RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklamayı Boş Geçemezsiniz");
        RuleFor(x => x.Description).MinimumLength(1).WithMessage("Açıklama 3 Karakterden Aşağı Olamaz");
        RuleFor(x => x.Description).MaximumLength(500).WithMessage("Açıklama 500 Karakterden Fazla Olamaz");
    }
}