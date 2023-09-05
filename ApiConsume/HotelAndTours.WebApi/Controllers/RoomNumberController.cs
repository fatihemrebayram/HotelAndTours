using AutoMapper;
using HotelAndTours.BusinessLayer.Abstract;
using HotelAndTours.EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace HotelNumberAndTours.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomNumberController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRoomNumberService _numberService;

    public RoomNumberController(IRoomNumberService numberService, IMapper mapper)
    {
        _numberService = numberService;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AddRoomNumber(RoomNumbers p)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        p.AddedDate = DateTime.Now;
        var values = _mapper.Map<RoomNumbers>(p);
        _numberService.AddBL(values);
        return Ok("Başarıyla oda numarası eklendi");
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRoomNumber(int id)
    {
        var roomNumber = _numberService.TGetByID(id);
        _numberService.RemoveBL(roomNumber);
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetRoomNumberById(int id)
    {
        var values = _numberService.TGetByID(id);
        return Ok(values);
    }

    [HttpGet]
    public IActionResult RoomNumberList()
    {
        var values = _numberService.GetListQueryableBL();
        return Ok(values);
    }

    [HttpPut]
    public IActionResult UpdateRoomNumber(RoomNumbers p)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        p.AddedDate = DateTime.Now;
        var values = _mapper.Map<RoomNumbers>(p);
        _numberService.UpdateBL(values);
        return Ok("Başarıyla oda numarası güncellendi");
    }
}