using AutoMapper;
using HotelAndTours.BusinessLayer.Abstract;
using HotelAndTours.DtoLayer.Dtos.RoomSpecsDto;
using HotelAndTours.EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace HotelAndTours.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomSpecsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRoomSpecsService _roomSpecsService;


    public RoomSpecsController(IMapper mapper, IRoomSpecsService roomSpecsService)
    {
        _mapper = mapper;
        _roomSpecsService = roomSpecsService;
    }

    [HttpPost]
    public IActionResult AddRoom(RoomSpecsDto p)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        p.AddedDate = DateTime.Now;
      
        var values = _mapper.Map<RoomSpecs>(p);
        _roomSpecsService.AddBL(values);
        return Ok("Başarıyla oda özelliği eklendi");
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRoom(int id)
    {
        var roomNumber = _roomSpecsService.TGetByID(id);
        _roomSpecsService.RemoveBL(roomNumber);
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetRoomById(int id)
    {
        var values = _roomSpecsService.TGetByID(id);
        return Ok(values);
    }

    [HttpGet]
    public IActionResult RoomList()
    {
        var values = _roomSpecsService.GetListQueryableBL();
        return Ok(values);
    }

    [HttpPut]
    public IActionResult UpdateRoom(RoomSpecsDto p)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        p.AddedDate = DateTime.Now;
        var values = _mapper.Map<RoomSpecs>(p);
        _roomSpecsService.UpdateBL(values);
        return Ok("Başarıyla oda özelliği güncellendi");
    }
}