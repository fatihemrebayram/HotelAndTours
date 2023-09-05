using AutoMapper;
using HotelAndTours.BusinessLayer.Abstract;
using HotelAndTours.DtoLayer.Dtos.HotelDto;
using HotelAndTours.EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace HotelAndTours.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IHotelService _hotelService;
    private readonly IMapper _mapper;

    public HotelController(IHotelService hotelService, IMapper mapper)
    {
        _hotelService = hotelService;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AddHotel(HotelDto p)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        p.AddedDate = DateTime.Now;
        var values = _mapper.Map<Hotel>(p);
        _hotelService.AddBL(values);
        return Ok("Başarıyla oda eklendi");
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteHotel(int id)
    {
        var roomNumber = _hotelService.TGetByID(id);
        _hotelService.RemoveBL(roomNumber);
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetHotelById(int id)
    {
        var values = _hotelService.TGetByID(id);
        return Ok(values);
    }

    [HttpGet]
    public IActionResult HotelList()
    {
        var values = _hotelService.GetListQueryableBL();
        return Ok(values);
    }

    [HttpGet("GetHotelsFiltered/{search}/{location}/{category}")]
    public IActionResult HotelListFiltered(string search, string location, string category)
    {
        var values = _hotelService.GetListQueryableBL();
        var valuesSent = new List<Hotel>();
        if (location != "Empty")
            values = values.Where(x =>
                x.Location.Contains(location));

        if (category != "Empty")
        {
            var categoryId =
                Convert.ToInt32(category); // Assuming 'category' is a string representation of the category ID
            values = values
                .Where(x => x.HotelCategories.Any(category => category.HotelCategory.HotelCategoryId == categoryId));
        }

        if (search != "Empty")
            values = values.Where(x =>
                x.HotelName.Contains(search) ||
                x.HotelCategories.Select(x => x.HotelCategory.HotelCategoryId).Equals(search) ||
                x.Location.Contains(search) ||
                x.HotelSpecs.Contains(search) ||
                x.Description.Contains(search));
        return Ok(values);
    }

    [HttpPut]
    public IActionResult UpdateHotel(HotelDto p)
    {
        if (!ModelState.IsValid)
            return BadRequest();


        p.AddedDate = DateTime.Now;
        var values = _mapper.Map<Hotel>(p);
        _hotelService.UpdateBL(values);
        return Ok("Başarıyla oda güncellendi");
    }
}