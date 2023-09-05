using HotelAndTours.EntityLayer.Concrete;

namespace HotelAndTours.ModelsLayer.Models.RoomSpecs;

public class RoomSpecsCRUDViewModel
{
    public EntityLayer.Concrete.RoomSpecs _RoomSpecsAddViewModel { get; set; }
    public List<EntityLayer.Concrete.RoomSpecs> _RoomSpecsViewModel { get; set; }
}