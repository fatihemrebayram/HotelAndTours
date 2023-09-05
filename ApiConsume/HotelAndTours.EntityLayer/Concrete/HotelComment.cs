using System.ComponentModel.DataAnnotations;

namespace HotelAndTours.EntityLayer.Concrete;

public class HotelComment
{
    [Key] public int CommentId { get; set; }
    public string Name { get; set; }
    public string Mail { get; set; }
    public string Text { get; set; }
    public int Star { get; set; }
    public DateTime CommentDate { get; set; }
    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }
    public bool Status { get; set; }
}