using AutoMapper;
using HotelAndTours.BusinessLayer.Abstract;
using HotelAndTours.DtoLayer.Dtos.BookingDto;
using HotelAndTours.EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace HotelAndTours.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IMapper _mapper;

    public BookingController(IMapper mapper, IBookingService bookingService)
    {
        _mapper = mapper;
        _bookingService = bookingService;
    }

    [HttpPost]
    public IActionResult AddBooking(BookingDto p)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        p.AddedDate = DateTime.Now;
        p.Status = "Onay Bekliyor";
        var values = _mapper.Map<Booking>(p);
        _bookingService.AddBL(values);
        return Ok("Başarıyla rezervasyon eklendi");
    }

    [HttpGet]
    public IActionResult BookingList()
    {
        var values = _bookingService.GetListQueryableBL();
        return Ok(values);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBooking(int id)
    {
        var roomNumber = _bookingService.TGetByID(id);
        _bookingService.RemoveBL(roomNumber);
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetBookingById(int id)
    {
        var values = _bookingService.TGetByID(id);
        return Ok(values);
    }

    [HttpPut]
    public IActionResult UpdateBooking(BookingDto p)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        p.AddedDate = DateTime.Now;
        var values = _mapper.Map<Booking>(p);
        _bookingService.UpdateBL(values);
        return Ok("Başarıyla rezervasyon güncellendi");
    }
}