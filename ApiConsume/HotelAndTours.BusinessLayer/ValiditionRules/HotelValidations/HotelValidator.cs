using FluentValidation;
using HotelAndTours.DtoLayer.Dtos.HotelDto;
using HotelAndTours.EntityLayer.Concrete;

namespace HotelAndTours.BusinessLayer.ValiditionRules.HotelValiditons;

public class HotelValidator : AbstractValidator<HotelDto>
{
    public HotelValidator()
    {
        RuleFor(x => x.HotelName).NotEmpty().WithMessage("Otel Adını Boş Geçemezsiniz");
        RuleFor(x => x.HotelName).MinimumLength(1).WithMessage("Otel Adı 1 Karakterden Aşağı Olamaz");
        RuleFor(x => x.HotelName).MaximumLength(100).WithMessage("Otel Adı 100 Karakterden Fazla Olamaz");

        RuleFor(x => x.Location).NotEmpty().WithMessage("Otel Konumunu Boş Geçemezsiniz");
        RuleFor(x => x.Location).MinimumLength(1).WithMessage("Otel Konumu 1 Karakterden Aşağı Olamaz");
        RuleFor(x => x.Location).MaximumLength(100).WithMessage("Otel Konumu 100 Karakterden Fazla Olamaz");

        RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklamayı Boş Geçemezsiniz");
        RuleFor(x => x.Description).MinimumLength(1).WithMessage("Açıklama 1 Karakterden Aşağı Olamaz");
        RuleFor(x => x.Description).MaximumLength(5000).WithMessage("Açıklama 5000 Karakterden Fazla Olamaz");

        RuleFor(x => x.HotelSpecs).NotEmpty().WithMessage("Otelin Özelliklerini Boş Geçemezsiniz");
    }
}