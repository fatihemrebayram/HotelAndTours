using HotelAndTours.EntityLayer.Concrete;
using System.ComponentModel.DataAnnotations;

namespace HotelAndTours.DtoLayer.Dtos.HotelComments;

public record HotelCommentsDto
{
    [Key] public int CommentId { get; set; }

    [Required(ErrorMessage = "Adınızı ve soyadınızı boş bırakmayınız")]
    public string Name { get; set; }

    [Required(ErrorMessage = "E-Posta adresinizi boş bırakmayınız")]
    public string Mail { get; set; }

    [Required(ErrorMessage = "Yorumunuzu boş bırakmayınız")]
    public string Text { get; set; }

    [Required(ErrorMessage = "Yıldız sayısını boş bırakmayınız")]
    public int Star { get; set; }
    public DateTime CommentDate { get; set; }
    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }
    public bool Status { get; set; }
}