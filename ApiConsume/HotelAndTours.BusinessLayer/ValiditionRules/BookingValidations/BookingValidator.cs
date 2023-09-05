using FluentValidation;
using HotelAndTours.DtoLayer.Dtos.BookingDto;
using HotelAndTours.DtoLayer.Dtos.HotelDto;

namespace HotelAndTours.BusinessLayer.ValiditionRules.BookingValidations;

public class BookingValidator : AbstractValidator<BookingDto>
{
    public BookingValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Müşteri Adını Boş Geçemezsiniz");
        RuleFor(x => x.Name).MinimumLength(3).WithMessage("Müşteri Adı 3 Karakterden Aşağı Olamaz");
        RuleFor(x => x.Name).MaximumLength(100).WithMessage("Müşteri Adı 100 Karakterden Fazla Olamaz");

        RuleFor(x => x.Mail).NotEmpty().WithMessage("E-Posta adresini boş geçemezsiniz");
        RuleFor(x => x.Mail).MinimumLength(3).WithMessage("E-Posta adresi 3 Karakterden Aşağı Olamaz");
        RuleFor(x => x.Mail).MaximumLength(100).WithMessage("E-Posta adresi 100 Karakterden Fazla Olamaz");

        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Telefon Numarasını Boş Geçemezsiniz");
        RuleFor(x => x.PhoneNumber).MinimumLength(3).WithMessage("Telefon Numarası 3 Karakterden Aşağı Olamaz");
        RuleFor(x => x.PhoneNumber).MaximumLength(15).WithMessage("Telefon Numarası 15 Karakterden Fazla Olamaz");

        RuleFor(x => x.Status).NotEmpty().WithMessage("Durumu Boş Geçemezsiniz");
        RuleFor(x => x.Status).MinimumLength(3).WithMessage("Durum 3 Karakterden Aşağı Olamaz");
        RuleFor(x => x.Status).MaximumLength(100).WithMessage("Durum 100 Karakterden Fazla Olamaz");

        RuleFor(x => x.CheckIn).NotEmpty().WithMessage("Giriş Tarihini Boş Geçemezsiniz");
        RuleFor(x => x.CheckOut).NotEmpty().WithMessage("Çıkış Tarihini Boş Geçemezsiniz");
        RuleFor(x => x.AdultCount).NotEmpty().WithMessage("Yetişkin Sayısını Boş Geçemezsiniz");

        RuleFor(x => x.RoomNumberId).NotEmpty().WithMessage("Oda Numarası Boş Geçemezsiniz");
    }
}